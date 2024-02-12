using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Ninth.HotUpdate
{
    public class AssetProxyLoadWithAB : IAssetProxyLoad
    {
        private readonly PathConfig pathConfig;

        private readonly Dictionary<string, AssetRef> configAssetPath2AssetRef;
        private readonly Dictionary<string, BundleRef> bundlePath2BundleRef;

        [Inject]
        public AssetProxyLoadWithAB(PathConfig pathConfig)
        {
            this.pathConfig = pathConfig;
            configAssetPath2AssetRef = new Dictionary<string, AssetRef>();
            bundlePath2BundleRef = new Dictionary<string, BundleRef>();
        }

        public async UniTask<(AssetRef?, T?)> Get<T>(string assetPath) where T : Object
        {
            if (!configAssetPath2AssetRef.TryGetValue(assetPath, out AssetRef assetRef))
            {
                return (null, null);
            }
            if (assetRef.Asset == null)
            {
                // 处理依赖
                var bundleNames = new List<string>();
                var waitTasks = new List<UniTask<AssetBundle>>();

                var bundles = new List<BundleRef>();
                if (assetRef.Dependencies != null)
                {
                    foreach (var t in assetRef.Dependencies)
                    {
                        bundles.Add(t);
                    }
                }
                bundles.Add(assetRef.BundleRef);
                foreach (var originBundleRef in bundles)
                {
                    if (!bundlePath2BundleRef.TryGetValue(originBundleRef.BundleName, out BundleRef bundleRef))
                    {
                        bundleRef = new BundleRef();
                        bundleRef.BundleName = originBundleRef.BundleName;
                        bundleRef.AssetLocate = originBundleRef.AssetLocate;

                        string bundlePath = bundleRef.AssetLocate switch
                        {
                            AssetLocate.Local => pathConfig.BundleInLocalInStreamingAssetPath(bundleRef.BundleName),
                            AssetLocate.Remote => pathConfig.BundleInRemoteInPersistentDataPath(bundleRef.BundleName),
                            _ => throw new Exception("Invalid resource location")
                        };
                        bundlePath2BundleRef.Add(bundleRef.BundleName, bundleRef);
                        bundleNames.Add(originBundleRef.BundleName);
                        waitTasks.Add(AssetBundle.LoadFromFileAsync(bundlePath).ToUniTask());
                    } 
                    bundleRef.BeAssetRefDependedList ??= new List<AssetRef>();
                    bundleRef.BeAssetRefDependedList.Add(assetRef);
                }
                var tasks = await UniTask.WhenAll(waitTasks);
                for (int index = 0; index < tasks.Length; index++)
                {
                    bundlePath2BundleRef[bundleNames[index]].Bundle = tasks[index];
                }
                // 加载资源
                string bundleName = bundles[bundles.Count - 1].BundleName;
                assetRef.Asset = bundlePath2BundleRef[bundleName].Bundle.LoadAsset(assetRef.AssetPath);
            }
            return (assetRef, assetRef.Asset as T);
        }

        public async UniTask UnLoadAllAsync()
        {
            List<string> removeBundleRef = null;
            foreach (var item in bundlePath2BundleRef)
            {
                BundleRef bundleRef = item.Value;

                if (bundleRef == null)
                {
                    continue;
                }

                List<AssetRef> beAssetRefDependedList = bundleRef.BeAssetRefDependedList;

                if (bundleRef.BeAssetRefDependedList == null)
                {
                    continue;
                }
                for (int index = beAssetRefDependedList.Count - 1; index >= 0; index--)
                {
                    AssetRef assetRef = beAssetRefDependedList[index];
                    List<GameObject> beGameObjectDependedList = assetRef.BeGameObjectDependedList;

                    if (beGameObjectDependedList != null)
                    {
                        for (int i = beGameObjectDependedList.Count - 1; i >= 0; i--)
                        {
                            GameObject go = beGameObjectDependedList[i];
                            if (go == null)
                            {
                                beGameObjectDependedList.RemoveAt(i);
                            }
                        }
                    }
                    if (beGameObjectDependedList == null || beGameObjectDependedList.Count == 0)
                    {
                        assetRef.Asset = null;
                        beAssetRefDependedList.RemoveAt(index);
                    }
                }
                await Resources.UnloadUnusedAssets();

                if (beAssetRefDependedList.Count == 0)
                {
                    await bundleRef.Bundle.UnloadAsync(true);

                    if (removeBundleRef == null)
                    {
                        removeBundleRef = new List<string>();
                    }
                    removeBundleRef.Add(item.Key);
                }
            }
            if (removeBundleRef != null)
            {
                for (int index = 0; index < removeBundleRef.Count; index++)
                {
                    bundlePath2BundleRef.Remove(removeBundleRef[index]);
                }
            }
        }
    }
}