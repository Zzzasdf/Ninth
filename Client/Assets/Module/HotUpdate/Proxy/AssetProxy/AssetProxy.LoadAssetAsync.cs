using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public partial class AssetProxy
    {
        public async UniTask<GameObject> CloneAsync(string assetPath)
        {
            GameObject cloneObj = null;
            switch (assetConfig.AssetMode)
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
                        if (!m_ConfigAssetPath2AssetRef.TryGetValue(assetPath, out AssetRef assetRef))
                        {
                            throw new Exception("This path does not exist in the configuration");
                        }
                        assetRef.AssetStatus = AssetStatus.Loading;

                        assetRef = await LoadAssetRefAsync<GameObject>(assetPath);
                        cloneObj = UnityEngine.Object.Instantiate(assetRef.Asset) as GameObject;
                        if (assetRef.BeGameObjectDependedList == null)
                        {
                            assetRef.BeGameObjectDependedList = new List<GameObject>();
                        }
                        assetRef.BeGameObjectDependedList.Add(cloneObj);

                        assetRef.AssetStatus = AssetStatus.Loaded;
                        break;
                    }
            }
            return cloneObj;
        }

        public async UniTask<T> LoadAssetAsync<T>(string assetPath, GameObject mountObj) where T : UnityEngine.Object
        {
            switch (assetConfig.AssetMode)
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
            List<string> bundleNames = new List<string>();
            List<UniTask<AssetBundle>> waitTasks = new List<UniTask<AssetBundle>>();

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

                    bundleNames.Add(originBundleRef.BundleName);
                    waitTasks.Add(AssetBundle.LoadFromFileAsync(bundlePath).ToUniTask());
                }
                else
                {
                    BundleRef bundleRef = m_BundlePath2BundleRef[originBundleRef.BundleName];
                    if (bundleRef.BeAssetRefDependedList == null)
                    {
                        bundleRef.BeAssetRefDependedList = new List<AssetRef>();
                    }
                    bundleRef.BeAssetRefDependedList.Add(assetRef);
                }
            }

            var tasks = await UniTask.WhenAll(waitTasks);
            for (int index = 0; index < tasks.Length; index++)
            {
                m_BundlePath2BundleRef[bundleNames[index]].Bundle = tasks[index];
            }

            // 加载资源
            string bundleName = bundles[bundles.Count - 1].BundleName;
            assetRef.Asset = m_BundlePath2BundleRef[bundleName].Bundle.LoadAsset(assetRef.AssetPath);
            return assetRef;
        }
    }
}
