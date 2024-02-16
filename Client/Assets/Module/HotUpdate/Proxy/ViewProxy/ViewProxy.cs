using System;
using System.Collections.ObjectModel;
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
        
        private ViewLayout? viewLayout;
        
        [Inject]
        public ViewProxy(IViewConfig viewConfig, IAssetProxy assetProxy)
        {
            this.viewConfig = viewConfig;
            this.assetProxy = assetProxy;
        }

        async UniTask<T?> IViewProxy.Get<T>(CancellationToken cancellationToken) where T : class
        {
            (string? path, VIEW_HIERARCY? hierarchy)? tuple = viewConfig.Get<T>();
            var rectHierarchy = await GetHierarchy(hierarchy);
            var obj = await assetProxy.CloneAsync(path, rectHierarchy, cancellationToken);
            if (obj == null)
            {
                $"无法实例化, 预制体路径: {path}".FrameError();
                return null;
            }
            var component = obj.GetComponent<T>();
            if (component == null)
            {
                $"无法找到在实例化的对象的根节点上找到 {nameof(T)} 组件, 预制体路径：{path}".FrameError();
                return null;
            }
            return component;
        }
        
        async UniTask<T?> IViewProxy.Get<T>(VIEW view, CancellationToken cancellationToken) where T : class
        {
            var (path, hierarchy) = viewConfig.Get(view);
            var rectHierarchy = await GetHierarchy(hierarchy);
            var obj = await assetProxy.CloneAsync(path, rectHierarchy, cancellationToken);
            if (obj == null)
            {
                $"无法实例化, 预制体路径: {path}".FrameError();
                return null;
            }
            var component = obj.GetComponent<T>();
            if (component == null)
            {
                $"无法找到在实例化的对象的根节点上找到 {nameof(T)} 组件，预制体路径：{path}".FrameError();
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
            var viewLayoutPath = viewConfig.ViewLayoutPath();
            if (viewLayout == null)
            {
                var viewLayoutObj = await assetProxy.CloneAsync(viewLayoutPath);
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
            var hierarchy = viewLayout.GetViewHierarchy(viewHierarchy.Value);
            if (hierarchy == null)
            {
                $"无法在 {nameof(ViewLayout)} 框架上找到层级对应的节点, 层级：{viewHierarchy}, 请检查路径 {viewLayoutPath} 预制体根节点上的 {nameof(ViewLayout)} 组件是否正常挂载和调用".FrameError();
            }
            return hierarchy;
        }
    }
}