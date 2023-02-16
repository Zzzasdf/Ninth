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
    public class AssetsMgr: Singleton<AssetsMgr>
    {
        private static Dictionary<string, AssetRef> m_AssetPath2AssetRef;

        private static Dictionary<string, BundleRef> m_BundleName2BundleRef;

        private static Func<string, string> m_LocalPathFunc;
        private static Func<string, string> m_RemotePathFunc;

        public AssetsMgr()
        {
            m_AssetPath2AssetRef = new Dictionary<string, AssetRef>();
            m_BundleName2BundleRef = new Dictionary<string, BundleRef>();
            Register();
        }

        private void Register()
        {
            switch(GlobalConfig.AssetMode)
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
            if(loadConfig.AssetRefList == null)
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

        public static async UniTask<T> Load<T>(string assetName) where T : UnityEngine.Object
        {
            T asset = null;
            switch(GlobalConfig.AssetMode)
            {
                case AssetMode.NonAB:
                    {
#if UNITY_EDITOR
                        asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetName);
#else
                        "不支持在非编辑器模式下使用此模式加载资源".Error();
#endif
                        break;
                    }
                case AssetMode.LocalAB:
                case AssetMode.RemoteAB:
                    {
                        AssetRef assetRef = await LoadAssetRef(assetName);

                        asset = assetRef.Asset as T;
                        break;
                    }
            }
            return asset;
        }

        private static async UniTask<AssetRef> LoadAssetRef(string assetPath)
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
                bundleRef.BeAssetRefDependedList.Add(assetRef);
            }
            AssetBundle bundle = assetRef.BundleRef.Bundle;

            string name = assetRef.AssetPath;

            assetRef.Asset = bundle.LoadAsset<GameObject>(name);

            return assetRef;
        }

        public async UniTask UnLoadAll()
        {
            "尝试卸载".Warning();
            await Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        public async UniTask AssetsClearAndUnLoadAll()
        {
            "清空资源并卸载".Warning();
            m_AssetPath2AssetRef.Clear();
            m_BundleName2BundleRef.Clear();
            Register();
            await UnLoadAll();
        }
    }
}