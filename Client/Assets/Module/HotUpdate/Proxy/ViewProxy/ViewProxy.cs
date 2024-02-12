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
        private readonly string viewLayoutPath;
        private readonly ReadOnlyDictionary<Type, (string path, ViewHierarchy hierarchy)> mapContainer;
        private readonly IAssetProxy assetProxy;

        private ViewLayout? viewLayout;
        
        [Inject]
        public ViewProxy(IViewConfig viewConfig, IAssetProxy assetProxy)
        {
            this.assetProxy = assetProxy;
            this.viewLayoutPath = viewConfig.ViewLayoutPath();
            this.mapContainer = viewConfig.MapContainer();
        }
        
        public async UniTask<T?> Get<T>(CancellationToken cancellationToken = default) where T: IView
        {
            var type = typeof(T);
            if (!mapContainer.TryGetValue(type, out var value))
            {
                $"未注册 View: {nameof(T)}，请将该类型先注册到 {nameof(IViewConfig)} 上".FrameError();
                return default;
            }
            var hierarchy = await GetHierarchy(value.hierarchy);
            var view = await assetProxy.CloneAsync(value.path, hierarchy, cancellationToken);
            if (view == null)
            {
                value.path.FrameError("无法实例化预制体 在路径: {0}");
                return default;
            }
            return view.GetComponent<T>();
        }
        
        private async UniTask<RectTransform?> GetHierarchy(ViewHierarchy viewHierarchy)
        {
            if (viewLayout == null)
            {
                var viewLayoutObj = await assetProxy.CloneAsync(viewLayoutPath);
                if (viewLayoutObj == null)
                {
                    viewLayoutPath.FrameError("无法在找到 view 框架预制体 在路径：{0}");
                    return null;
                }
                viewLayout = viewLayoutObj.GetComponent<ViewLayout>();
                if (viewLayout == null)
                {
                    $"无法找到在实例化的对象的根节点上找到 {nameof(ViewLayout)} 组件，预制体路径：{viewLayoutPath}".FrameError();
                    return null;
                }
            }
            var hierarchy = viewLayout.GetViewHierarchy(viewHierarchy);
            if (hierarchy == null)
            {
                $"无法在 view 框架上找到层级对应的节点, 层级：{viewHierarchy}, 请检查路径 {viewLayoutPath} 预制体根节点上的 {nameof(ViewLayout)} 组件是否正常挂载和调用".FrameError();
            }
            return hierarchy;
        }
    }
}