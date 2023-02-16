using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Ninth.HotUpdate
{
    public class AssetsMgr: Singleton<AssetsMgr>
    {
        private Dictionary<string, AssetRef> m_Path2BundleName;

        public AssetsMgr()
        {
            m_Path2BundleName = new Dictionary<string, AssetRef>();
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
                        break;
                    }
                case AssetMode.RemoteAB:
                    {
                        LoadConfig localLoadConfig = Utility.ToObject<LoadConfig>(PathConfig.LoadConfigInLocalInStreamingAssetPath());
                        LoadConfig remoteLoadConfig = Utility.ToObject<LoadConfig>(PathConfig.LoadConfigInRemoteInPersistentDataPath());
                        SetPath2BundleName(localLoadConfig).SetPath2BundleName(remoteLoadConfig);
                        break;
                    }
            }
        }

        private AssetsMgr SetPath2BundleName(LoadConfig loadConfig)
        {
            for (int index = 0; index < loadConfig.AssetRefList.Count; index++)
            {
                string key = loadConfig.AssetRefList[index].AssetPath;
                AssetRef value = loadConfig.AssetRefList[index];
                m_Path2BundleName.Add(key, value);
            }
            return this;
        }

        //public async UniTask<bool> Load(string assetName)
        //{

        //}

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
    }
}