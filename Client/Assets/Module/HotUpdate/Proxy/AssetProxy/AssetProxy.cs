using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

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
        
        public async UniTask<GameObject?> CloneAsync(string assetPath, CancellationToken cancellationToken = default)
        {
            var (assetRef, asset) = await assetProxyLoad.Get<GameObject>(assetPath);
            if (asset == null)
            {
                assetPath.FrameError("无法加载该预制体 在路径 {0}");
                return null;
            }
            var cloneObj = UnityEngine.Object.Instantiate(asset);
            if (cloneObj == null)
            {
                assetPath.FrameError("无法在实例化预制体 在路径 {0}");
                return null;
            }
            if (assetRef != null)
            {
                assetRef.BeGameObjectDependedList ??= new List<GameObject>();
                assetRef.BeGameObjectDependedList.Add(cloneObj);
            }
            return cloneObj;
        }

        public async UniTask<GameObject?> CloneAsync(string assetPath, Transform? parent,
            CancellationToken cancellationToken = default)
        {
            var (assetRef, asset) = await assetProxyLoad.Get<GameObject>(assetPath);
            if (asset == null)
            {
                assetPath.FrameError("无法加载预制体 在路径 {0}");
                return null;
            }
            var cloneObj = UnityEngine.Object.Instantiate(asset, parent);
            if (cloneObj == null)
            {
                assetPath.FrameError("无法在实例化预制体 在路径 {0}");
                return null;
            }
            if (assetRef != null)
            {
                assetRef.BeGameObjectDependedList ??= new List<GameObject>();
                assetRef.BeGameObjectDependedList.Add(cloneObj);
            }
            return cloneObj;
        }

        public async UniTask<T?> LoadAssetAsync<T>(string assetPath, GameObject mountObj) where T : UnityEngine.Object
        {
            var (assetRef, asset) = await assetProxyLoad.Get<T>(assetPath);
            if (asset == null)
            {
                assetPath.FrameError("无法加载预制体 在路径 {0}");
                return asset;
            }
            if (assetRef != null)
            {
                assetRef.BeGameObjectDependedList ??= new List<GameObject>();
                assetRef.BeGameObjectDependedList.Add(mountObj);
            }
            return asset;
        }
        
        public async UniTask UnLoadAllAsync()
        {
            await assetProxyLoad.UnLoadAllAsync();
        }
    }
}