using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

namespace Ninth.HotUpdate
{
    public class AssetProxy : IAssetProxy
    {
        private readonly IAssetProxyLoad assetProxyLoad;

        [Inject]
        public AssetProxy(IAssetProxyLoad assetProxyLoad)
        {
            this.assetProxyLoad = assetProxyLoad;
        }
        
        async UniTask<GameObject> IAssetProxy.CloneAsync(string assetPath, CancellationToken cancellationToken)
        {
            var (assetRef, asset) = await assetProxyLoad.Get<GameObject>(assetPath);
            if (asset == null)
            {
                $"无法加载, 预制体路径: {assetPath}".FrameError();
                return null;
            }
            var cloneObj = Object.Instantiate(asset);
            if (cloneObj == null)
            {
                $"无法实例化, 预制体路径: {assetPath}".FrameError();
                return null;
            }
            if (assetRef != null)
            {
                assetRef.BeGameObjectDependedList ??= new List<GameObject>();
                assetRef.BeGameObjectDependedList.Add(cloneObj);
            }
            return cloneObj;
        }

        async UniTask<GameObject> IAssetProxy.CloneAsync(string assetPath, Transform parent,
            CancellationToken cancellationToken)
        {
            var (assetRef, asset) = await assetProxyLoad.Get<GameObject>(assetPath);
            if (asset == null)
            {
                $"无法加载, 预制体路径: {assetPath}".FrameError();
                return null;
            }
            var cloneObj = Object.Instantiate(asset, parent);
            if (cloneObj == null)
            {
                $"无法实例化, 预制体路径: {assetPath}".FrameError();
                return null;
            }
            if (assetRef != null)
            {
                assetRef.BeGameObjectDependedList ??= new List<GameObject>();
                assetRef.BeGameObjectDependedList.Add(cloneObj);
            }
            return cloneObj;
        }

        async UniTask<T?> IAssetProxy.LoadAssetAsync<T>(string assetPath, GameObject mountObj) where T: class
        {
            var (assetRef, asset) = await assetProxyLoad.Get<T>(assetPath);
            if (asset == null)
            {
                $"无法加载, 预制体路径: {assetPath}".FrameError();
                return default;
            }
            if (assetRef != null)
            {
                assetRef.BeGameObjectDependedList ??= new List<GameObject>();
                assetRef.BeGameObjectDependedList.Add(mountObj);
            }
            return asset;
        }
        
        async UniTask IAssetProxy.UnLoadAllAsync()
        {
            await assetProxyLoad.UnLoadAllAsync();
        }
    }
}