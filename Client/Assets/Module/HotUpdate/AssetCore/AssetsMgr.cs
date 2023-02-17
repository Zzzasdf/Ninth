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
        private Dictionary<string, AssetRef> m_AssetPath2AssetRef;

        private Dictionary<string, BundleRef> m_BundleName2BundleRef;

        private Func<string, string> m_LocalPathFunc;
        private Func<string, string> m_RemotePathFunc;

        public AssetsMgr()
        {
            m_AssetPath2AssetRef = new Dictionary<string, AssetRef>();
            m_BundleName2BundleRef = new Dictionary<string, BundleRef>();
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
                m_AssetPath2AssetRef.Add(key, value);
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
                        AssetRef assetRef = await LoadAssetRefAsync(assetPath);
                        cloneObj = UnityEngine.Object.Instantiate(assetRef.Asset);
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
                        AssetRef assetRef = await LoadAssetRefAsync(assetPath);
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

        private async UniTask<AssetRef> LoadAssetRefAsync(string assetPath)
        {
            if (!m_AssetPath2AssetRef.TryGetValue(assetPath, out AssetRef assetRef))
            {
                throw new InvalidOperationException("Asset not found");
            }
            List<BundleRef> bundleList = assetRef.Dependencies?.ToList() ?? new List<BundleRef>();

            bundleList.Add(assetRef.BundleRef);

            for (int index = 0; index < bundleList.Count; index++)
            {
                BundleRef bundleRef = bundleList[index];

                string bundleName = bundleRef.BundleName;

                if (!m_BundleName2BundleRef.ContainsKey(bundleName))
                {
                    AssetLocate assetLocate = assetRef.BundleRef.AssetLocate;

                    string bundlePath = string.Empty;
                    switch (assetLocate)
                    {
                        case AssetLocate.Local:
                            {
                                bundlePath = m_LocalPathFunc(bundleName);
                                break;
                            }
                        case AssetLocate.Remote:
                            {
                                bundlePath = m_RemotePathFunc(bundleName);
                                break;
                            }
                        case AssetLocate.Dll:
                            {
                                throw new InvalidOperationException("This loader cannot be used to load Dll. All Dlls have been loaded before the hotfix");
                            }
                    }
                    // 内存中只能存在一个，只能用AssetBundle.Unload(true)才能卸载掉
                    bundleRef.Bundle = await AssetBundle.LoadFromFileAsync(bundlePath);

                    m_BundleName2BundleRef.Add(bundleName, bundleRef);

                    bundleRef.BeAssetRefDependedList = new List<AssetRef>();
                }
                if(index < bundleList.Count - 1)
                {
                    bundleRef.BeAssetRefDependedList.Add(assetRef);
                }
            }
            bundleList.Remove(assetRef.BundleRef);

            assetRef.Dependencies = bundleList;

            AssetBundle bundle = assetRef.BundleRef.Bundle;

            string name = assetRef.AssetPath;

            assetRef.Asset = bundle.LoadAsset<GameObject>(name);

            return assetRef;
        }

        public async UniTask UnLoadAll()
        {
            foreach (AssetRef assetRef in m_AssetPath2AssetRef.Values)
            {
                if (assetRef.BeGameObjectDependedList == null || assetRef.BeGameObjectDependedList.Count == 0)
                {
                    continue;
                }
                for (int index = assetRef.BeGameObjectDependedList.Count - 1; index >= 0; index--)
                {
                    GameObject go = assetRef.BeGameObjectDependedList[index];

                    if (go == null)
                    {
                        assetRef.BeGameObjectDependedList.RemoveAt(index);
                    }
                }

                // 如果这个资源assetRef已经没有被任何GameObject所依赖了，那么此assetRef就可以卸载了
                if (assetRef.BeGameObjectDependedList.Count == 0)
                {
                    m_AssetPath2AssetRef.Remove(assetRef.AssetPath);

                    assetRef.Asset = null;

                    await Resources.UnloadUnusedAssets();

                    // 对于assetRef所属的这个bundle, 解除关系
                    assetRef.BundleRef.BeAssetRefDependedList.Remove(assetRef);

                    if (assetRef.BundleRef.BeAssetRefDependedList.Count == 0)
                    {
                        m_BundleName2BundleRef.Remove(assetRef.BundleRef.BundleName);

                        await assetRef.BundleRef.Bundle.UnloadAsync(true);
                    }

                    // 对于assetRef所依赖的那些bundle列表，解除关系
                    foreach (BundleRef bundleRef in assetRef.Dependencies)
                    {
                        if (bundleRef.BeAssetRefDependedList.Count == 0)
                        {
                            m_BundleName2BundleRef.Remove(bundleRef.BundleName);

                            await bundleRef.Bundle.UnloadAsync(true);
                        }
                    }

                }
            }
        }
    }
}