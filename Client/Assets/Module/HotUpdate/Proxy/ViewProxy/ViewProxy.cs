using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Ninth.HotUpdate
{
    public class ViewProxy: IViewProxy
    {
        private readonly IViewConfig viewConfig;
        private readonly IAssetProxy assetProxy;
        private readonly IObjectResolver resolver;
        
        private ViewLayout? viewLayout;
        
        [Inject]
        public ViewProxy(IViewConfig viewConfig, IAssetProxy assetProxy, IObjectResolver resolver)
        {
            this.viewConfig = viewConfig;
            this.assetProxy = assetProxy;
            this.resolver = resolver;
        }

        T IViewProxy.Controller<T>(CancellationToken cancellationToken) where T : class
        {
            return resolver.Resolve<T>();
        }

        async UniTask<T> IViewProxy.View<T>(CancellationToken cancellationToken)
        {
            var tuple = viewConfig.TupleSubscriber.Get<T>();
            var rectHierarchy = await GetHierarchy(tuple.hierarcy);
            var obj = await assetProxy.CloneAsync(tuple.path, rectHierarchy, cancellationToken);
            if (obj == null)
            {
                $"无法实例化, 预制体路径: {tuple.path}".FrameError();
                return null;
            }

            T component = null;
            await UniTask.WaitUntil(() => (component = obj.GetComponent<T>()) != null, cancellationToken: cancellationToken);
            if (component == null)
            {
                $"无法找到在实例化的对象的根节点上找到 {nameof(T)} 组件, 预制体路径：{tuple.path}".FrameError();
                return null;
            }
            return component;
        }
        
        async UniTask<T> IViewProxy.View<T>(VIEW view, CancellationToken cancellationToken)
        {
            var tuple = viewConfig.TupleSubscriber.Get(view);
            var rectHierarchy = await GetHierarchy(tuple.hierarcy);
            var obj = await assetProxy.CloneAsync(tuple.path, rectHierarchy, cancellationToken);
            if (obj == null)
            {
                $"无法实例化, 预制体路径: {tuple.path}".FrameError();
                return null;
            }
            await UniTask.WaitUntil(() => obj.GetComponent<T>() != null, cancellationToken: cancellationToken);
            var component = obj.GetComponent<T>();
            if (component == null)
            {
                $"无法找到在实例化的对象的根节点上找到 {nameof(T)} 组件，预制体路径：{tuple.path}".FrameError();
                return null;
            }
            return component;
        }

        private async UniTask<RectTransform?> GetHierarchy(VIEW_HIERARCY? viewHierarchy)
        {
            if (!viewHierarchy.HasValue)
            {
                $"{nameof(VIEW_HIERARCY)} 为空".FrameError();
                return null;
            }
            var viewLayoutPath = viewConfig.StringSubscriber.Get<VIEW_HIERARCY>();
            if (viewLayout == null)
            {
                var viewLayoutObj = await assetProxy.CloneAsync(viewLayoutPath);
                if (viewLayoutObj == null)
                {
                    $"无法实例化, 预制体路径: {viewLayoutPath}".FrameError();
                    return null;
                }
                viewLayout = viewLayoutObj.AddComponent<ViewLayout>();
                if (viewLayout == null)
                {
                    $"无法找到在实例化的对象的根节点上找到 {nameof(ViewLayout)} 组件，预制体路径：{viewLayoutPath}".FrameError();
                    return null;
                }
            }
            var hierarchy = viewLayout.GetViewHierarchy(viewHierarchy.Value);
            if (hierarchy == null)
            {
                $"无法在 {nameof(ViewLayout)} 框架上找到层级对应的节点, 层级：{viewHierarchy}, 请检查路径 {viewLayoutPath} 预制体根节点上的 {nameof(ViewLayout)} 组件是否正常挂载和调用".FrameError();
            }
            return hierarchy;
        }
    }
}