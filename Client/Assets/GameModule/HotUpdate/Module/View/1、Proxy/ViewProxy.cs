using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Ninth.HotUpdate
{
    public class ViewProxy : IViewProxy
    {
        private int uniqueId;
        
        private readonly IViewConfig viewConfig;
        private readonly IAssetProxy assetProxy;
        private readonly IObjectResolver resolver;
        private ViewLayout? viewLayout;

        private readonly IRecycleProxy<BaseView> recycleProxy;
        private readonly IStackProxy<VIEW_HIERARCHY, BaseView> stackProxy;
        private readonly ILinkedListProxy<VIEW_HIERARCHY, BaseView> displayProxy;

        [Inject]
        public ViewProxy(IViewConfig viewConfig, IAssetProxy assetProxy, IObjectResolver resolver)
        {
            this.viewConfig = viewConfig;
            this.assetProxy = assetProxy;
            this.resolver = resolver;
            recycleProxy = new RecycleProxy<BaseView>(single: (null, 10), total: (null, 100));
            stackProxy = new StackProxy<VIEW_HIERARCHY, BaseView>(single: (10, null), total: (10, null));
            displayProxy = new LinkedListProxy<VIEW_HIERARCHY, BaseView>(single: (1, null), total: (1, null));
        }

        T IViewProxy.Controller<T>(CancellationToken cancellationToken) where T : class
        {
            return resolver.Resolve<T>();
        }

        public async UniTask<T> ViewAsync<T>(CancellationToken cancellationToken) where T : BaseView
        {
            var assetParentConfig = viewConfig.ViewAssetConfig.GetAssetParentConfig<T>();
            var hierarchy = assetParentConfig.Hierarchy;
            var path = assetParentConfig.RelativePath;
            var weight = assetParentConfig.Weight;
            var currView = await recycleProxy.GetOneAsync(async () =>
            {
                var rectHierarchy = await GetHierarchy(hierarchy, cancellationToken);
                var obj = await assetProxy.CloneAsync(path, rectHierarchy, cancellationToken);
                obj.SetActive(false);
                var component = obj.GetComponent<T>();
                component.InitOnCreate(this, typeof(T), hierarchy, weight);
                return component;
            });
            if (currView == null)
            {
                $"无法找到在实例化的对象的根节点上找到 {nameof(T)} 组件, 预制体路径：{path}".FrameError();
                return null;
            }
            displayProxy.Enqueue(hierarchy, currView, true, removeDisplayView =>
            {
                removeDisplayView.gameObject.SetActive(false);
                stackProxy.Push(hierarchy, removeDisplayView, true, removeStackView =>
                {
                    recycleProxy.Recycle(removeStackView,true, Object.DestroyImmediate);
                });
            });
            currView.AddUniqueId(uniqueId++);
            currView.gameObject.SetActive(true);
            return (T)currView;
        }

        public async UniTask<TChild> ChildViewAsync<TParent, TChild>(RectTransform parentNode, CancellationToken cancellationToken) where TParent : BaseView where TChild : BaseChildView
        {
            var assetParentConfig = viewConfig.ViewAssetConfig.GetAssetChildConfig<TParent, TChild>();
            var path = assetParentConfig.RelativePath;
            var weight = assetParentConfig.Weight;
            var obj = await assetProxy.CloneAsync(path, parentNode, cancellationToken);
            obj.SetActive(false);
            var component = obj.GetComponent<TChild>();
            component.InitOnCreate(this, typeof(TChild), weight);
            var currView = component;
            if (currView == null)
            {
                $"无法找到在实例化的对象的根节点上找到 {nameof(TChild)} 组件, 预制体路径：{path}".FrameError();
                return null;
            }
            currView.gameObject.SetActive(true);
            return currView;
        }

        public async UniTaskVoid RecycleAsync(VIEW_HIERARCHY hierarchy, int uniqueId, CancellationToken cancellationToken)
        {
            var dequeueView = displayProxy.Dequeue(hierarchy, uniqueId);
            if (dequeueView == null)
            {
                $"无法找到 {hierarchy}, {uniqueId}".Error();
                return; 
            }
            dequeueView.gameObject.SetActive(false);
            recycleProxy.Recycle(dequeueView.Type, dequeueView, true, Object.DestroyImmediate);
            
            var pop = stackProxy.Pop(hierarchy);
            if (pop != null)
            {
                var rectHierarchy = await GetHierarchy(hierarchy, cancellationToken);
                displayProxy.Enqueue(hierarchy, pop, true, removeDisplayView =>
                {
                    removeDisplayView.gameObject.SetActive(false);
                    stackProxy.Push(hierarchy, removeDisplayView, true, removeStackView =>
                    {
                        recycleProxy.Recycle(dequeueView.Type, removeStackView,true, Object.DestroyImmediate);
                    });
                });
                pop.transform.SetParent(rectHierarchy);
                pop.AddUniqueId(this.uniqueId++);
                pop.gameObject.SetActive(true);
            }
        }
        
        private async UniTask<RectTransform> GetHierarchy(VIEW_HIERARCHY hierarchy, CancellationToken cancellationToken = default)
        {
            var viewLayoutPath = viewConfig.ViewAssetConfig.ViewLayout;
            if (viewLayout == null)
            {
                var viewLayoutObj = await assetProxy.CloneAsync(viewLayoutPath, cancellationToken);
                if (viewLayoutObj == null)
                {
                    $"无法实例化, 预制体路径: {viewLayoutPath}".FrameError();
                    return null;
                }
                viewLayout = viewLayoutObj.GetComponent<ViewLayout>();
                if (viewLayout == null)
                {
                    $"无法找到在实例化的对象的根节点上找到 {nameof(ViewLayout)} 组件，预制体路径：{viewLayoutPath}".FrameError();
                    return null;
                }
            }
            var rectHierarchy = viewLayout.GetViewHierarchy(hierarchy);
            if (rectHierarchy == null)
            {
                $"无法在 {nameof(ViewLayout)} 框架上找到层级对应的节点, 层级：{hierarchy}, 请检查路径 {viewLayoutPath} 预制体根节点上的 {nameof(ViewLayout)} 组件是否正常挂载和调用".FrameError();
            }
            return rectHierarchy;
        }
    }
}