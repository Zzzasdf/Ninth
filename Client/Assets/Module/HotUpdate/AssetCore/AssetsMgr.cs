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
    public partial class AssetsMgr : Singleton<AssetsMgr>
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