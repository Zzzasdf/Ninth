using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public partial class AssetsMgr
    {
        public async UniTask<GameObject> CloneAsync(string assetPath)
        {
            GameObject cloneObj = null;
            switch (GlobalConfig.AssetMode)
            {
                case AssetMode.NonAB:
                    {
#if UNITY_EDITOR
                        GameObject asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                        cloneObj = UnityEngine.Object.Instantiate(asset);
#else
                        throw new Exception("Loading resources using this mode in non-editor mode is not supported");
#endif
                        break;
                    }
                case AssetMode.LocalAB:
                case AssetMode.RemoteAB:
                    {
                        AssetRef assetRef = await LoadAssetRefAsync<GameObject>(assetPath);
                        cloneObj = UnityEngine.Object.Instantiate(assetRef.Asset) as GameObject;
                        if (assetRef.BeGameObjectDependedList == null)
                        {
                            assetRef.BeGameObjectDependedList = new List<GameObject>();
                        }
                        assetRef.BeGameObjectDependedList.Add(cloneObj);
                        break;
                    }
            }
            return cloneObj;
        }

        public async UniTask<T> LoadAssetAsync<T>(string assetPath, GameObject mountObj) where T : UnityEngine.Object
        {
            switch (GlobalConfig.AssetMode)
            {
                case AssetMode.NonAB:
                    {
#if UNITY_EDITOR
                        T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
                        return asset;
#else
                        throw new Exception("Loading resources using this mode in non-editor mode is not supported");
#endif
                    }
                case AssetMode.LocalAB:
                case AssetMode.RemoteAB:
                    {
                        AssetRef assetRef = await LoadAssetRefAsync<T>(assetPath);
                        if (assetRef.BeGameObjectDependedList == null)
                        {
                            assetRef.BeGameObjectDependedList = new List<GameObject>();
                        }
                        assetRef.BeGameObjectDependedList.Add(mountObj);
                        return assetRef.Asset as T;
                    }
            }
            throw new Exception("This mode is not defined");
        }

        private async UniTask<AssetRef> LoadAssetRefAsync<T>(string assetPath) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return null;
            }
            if (!m_ConfigAssetPath2AssetRef.TryGetValue(assetPath, out AssetRef assetRef))
            {
                throw new Exception("This path does not exist in the configuration");
            }
            if (assetRef.Asset != null)
            {
                return assetRef;
            }

            // 处理依赖
            List<BundleRef> bundles = new List<BundleRef>();
            if (assetRef.Dependencies != null)
            {
                for (int index = 0; index < assetRef.Dependencies.Count; index++)
                {
                    bundles.Add(assetRef.Dependencies[index]);
                }
            }
            bundles.Add(assetRef.BundleRef);
            for (int index = 0; index < bundles.Count; index++)
            {
                BundleRef originBundleRef = bundles[index];

                if (!m_BundlePath2BundleRef.ContainsKey(originBundleRef.BundleName))
                {
                    BundleRef bundleRef = new BundleRef();
                    bundleRef.BundleName = originBundleRef.BundleName;
                    bundleRef.AssetLocate = originBundleRef.AssetLocate;

                    string bundlePath = string.Empty;
                    switch (bundleRef.AssetLocate)
                    {
                        case AssetLocate.Local:
                            {
                                bundlePath = m_LocalPathFunc.Invoke(bundleRef.BundleName);
                                break;
                            }
                        case AssetLocate.Remote:
                            {
                                bundlePath = m_RemotePathFunc.Invoke(bundleRef.BundleName);
                                break;
                            }
                        default:
                            {
                                throw new Exception("Invalid resource location");
                            }
                    }
                    if (bundleRef.BeAssetRefDependedList == null)
                    {
                        bundleRef.BeAssetRefDependedList = new List<AssetRef>();
                    }
                    bundleRef.BeAssetRefDependedList.Add(assetRef);

                    m_BundlePath2BundleRef.Add(bundleRef.BundleName, bundleRef);

                    // 异步加载一定要放在添加依赖之后, 否则有可能被卸载掉!!
                    bundleRef.Bundle = await AssetBundle.LoadFromFileAsync(bundlePath);
                }
                else
                {
                    BundleRef bundleRef = m_BundlePath2BundleRef[originBundleRef.BundleName];
                    if (bundleRef.BeAssetRefDependedList == null)
                    {
                        bundleRef.BeAssetRefDependedList = new List<AssetRef>();
                    }
                    bundleRef.BeAssetRefDependedList.Add(assetRef);

                    if (bundleRef.Bundle != null)
                    {
                        continue;
                    }
                    else
                    {
                        throw new Exception("Bundle is null");
                    }
                }
            }

            // 加载资源
            string bundleName = bundles[bundles.Count - 1].BundleName;
            assetRef.Asset = await m_BundlePath2BundleRef[bundleName].Bundle.LoadAssetAsync(assetRef.AssetPath);
            return assetRef;
        }
    }
}
