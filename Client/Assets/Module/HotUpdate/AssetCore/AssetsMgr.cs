using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Ninth.HotUpdate
{
    public class AssetsMgr : Singleton<AssetsMgr>
    {
        // 资源配置
        private Dictionary<string, AssetRef> m_ConfigAssetPath2AssetRef;

        // 加载进内存的bundle, 确保只存在一个
        private Dictionary<string, BundleRef> m_BundlePath2BundleRef;

        private Func<string, string> m_LocalPathFunc;
        private Func<string, string> m_RemotePathFunc;

        public AssetsMgr()
        {
            m_ConfigAssetPath2AssetRef = new Dictionary<string, AssetRef>();

            m_BundlePath2BundleRef = new Dictionary<string, BundleRef>();
            Register();
        }

        private void Register()
        {
            switch (GlobalConfig.AssetMode)
            {
                case AssetMode.NonAB:
                    {
                        break;
                    }
                case AssetMode.LocalAB:
                    {
                        LoadConfig localLoadConfig = Utility.ToObject<LoadConfig>(PathConfig.LoadConfigInLocalInStreamingAssetPath());
                        LoadConfig remoteLoadConfig = Utility.ToObject<LoadConfig>(PathConfig.LoadConfigInRemoteInStreamingAssetPath());
                        SetPath2BundleName(localLoadConfig).SetPath2BundleName(remoteLoadConfig);
                        m_LocalPathFunc = (assetName) => PathConfig.BundleInLocalInStreamingAssetPath(assetName);
                        m_RemotePathFunc = (assetName) => PathConfig.BunldeInRemoteInStreamingAssetPath(assetName);
                        break;
                    }
                case AssetMode.RemoteAB:
                    {
                        LoadConfig localLoadConfig = Utility.ToObject<LoadConfig>(PathConfig.LoadConfigInLocalInStreamingAssetPath());
                        LoadConfig remoteLoadConfig = Utility.ToObject<LoadConfig>(PathConfig.LoadConfigInRemoteInPersistentDataPath());
                        SetPath2BundleName(localLoadConfig).SetPath2BundleName(remoteLoadConfig);
                        m_LocalPathFunc = (assetName) => PathConfig.BundleInLocalInStreamingAssetPath(assetName);
                        m_RemotePathFunc = (assetName) => PathConfig.BundleInRemoteInPersistentDataPath(assetName);
                        break;
                    }
            }
        }

        private AssetsMgr SetPath2BundleName(LoadConfig loadConfig)
        {
            if (loadConfig.AssetRefList == null)
            {
                return this;
            }
            for (int index = 0; index < loadConfig.AssetRefList.Count; index++)
            {
                string key = loadConfig.AssetRefList[index].AssetPath;
                AssetRef value = loadConfig.AssetRefList[index];
                m_ConfigAssetPath2AssetRef.Add(key, value);
            }
            return this;
        }

        public async UniTask<bool> Download(string srcPath, string dstPath)
        {
            UnityWebRequest request = UnityWebRequest.Get(srcPath);

            request.downloadHandler = new DownloadHandlerFile(dstPath);

            ("原路径:" + srcPath + "请求下载到本地路径: " + dstPath).Log();

            await request.SendWebRequest();

            if (string.IsNullOrEmpty(request.error) == false)
            {
                ($"下载文件：{request.error}").Log();

                return false;
            }
            return true;
        }

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
            if(string.IsNullOrEmpty(assetPath))
            {
                return null; 
            }
            if(!m_ConfigAssetPath2AssetRef.TryGetValue(assetPath, out AssetRef assetRef))
            {
                throw new Exception("This path does not exist in the configuration");
            }
            if(assetRef.Asset != null)
            {
                return assetRef;
            }

            // 处理依赖
            List<BundleRef> bundles = assetRef.Dependencies;
            if(bundles == null)
            {
                bundles = new List<BundleRef>();
            }
            bundles.Add(assetRef.BundleRef);
            for (int index = 0; index < bundles.Count; index++)
            {
                BundleRef originBundleRef = bundles[index];

                if(!m_BundlePath2BundleRef.ContainsKey(originBundleRef.BundleName))
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

        public async UniTask UnLoadAllAsync()
        {
            List<string> removeBundleRef = null;
            foreach(var item in m_BundlePath2BundleRef)
            {
                BundleRef bundleRef = item.Value;
                
                List<AssetRef> beAssetRefDependedList = bundleRef.BeAssetRefDependedList;

                for (int index = beAssetRefDependedList.Count - 1; index >= 0; index--)
                {
                    AssetRef assetRef = beAssetRefDependedList[index];

                    List<GameObject> beGameObjectDependedList = assetRef.BeGameObjectDependedList;
                    
                    if(beGameObjectDependedList != null)
                    {
                        for (int i = beGameObjectDependedList.Count - 1; i >=  0; i--)
                        {
                            GameObject go = beGameObjectDependedList[i];
                            if(go == null)
                            {
                                beGameObjectDependedList.RemoveAt(i);
                            }
                        }
                    }
                    if(beGameObjectDependedList == null || beGameObjectDependedList.Count == 0)
                    {
                        assetRef.Asset = null;
                        beAssetRefDependedList.RemoveAt(index);
                    }
                }
                await Resources.UnloadUnusedAssets();
                
                if (beAssetRefDependedList.Count == 0)
                {
                    await bundleRef.Bundle.UnloadAsync(true);

                    if(removeBundleRef == null)
                    {
                        removeBundleRef = new List<string>();
                    }
                    removeBundleRef.Add(item.Key);
                }
            }
            if(removeBundleRef != null)
            {
                for (int index = 0; index < removeBundleRef.Count; index++)
                {
                    m_BundlePath2BundleRef.Remove(removeBundleRef[index]);
                }
            }
        }
    }
}