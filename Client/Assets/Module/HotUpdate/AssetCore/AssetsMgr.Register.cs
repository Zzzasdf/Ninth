using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public partial class AssetsMgr
    {
        private async UniTask Register()
        {
            switch (GlobalConfig.AssetMode)
            {
                case AssetMode.NonAB:
                    {
                        break;
                    }
                case AssetMode.LocalAB:
                    {
                        LoadConfig localLoadConfig = await Utility.ToObjectWithLock<LoadConfig>(PathConfig.LoadConfigInLocalInStreamingAssetPath());
                        LoadConfig remoteLoadConfig = await Utility.ToObjectWithLock<LoadConfig>(PathConfig.LoadConfigInRemoteInStreamingAssetPath());
                        SetPath2BundleName(localLoadConfig).SetPath2BundleName(remoteLoadConfig);
                        m_LocalPathFunc = (assetName) => PathConfig.BundleInLocalInStreamingAssetPath(assetName);
                        m_RemotePathFunc = (assetName) => PathConfig.BunldeInRemoteInStreamingAssetPath(assetName);
                        break;
                    }
                case AssetMode.RemoteAB:
                    {
                        LoadConfig localLoadConfig = await Utility.ToObjectWithLock<LoadConfig>(PathConfig.LoadConfigInLocalInStreamingAssetPath());
                        LoadConfig remoteLoadConfig = await Utility.ToObjectWithLock<LoadConfig>(PathConfig.LoadConfigInRemoteInPersistentDataPath());
                        SetPath2BundleName(localLoadConfig).SetPath2BundleName(remoteLoadConfig);
                        m_LocalPathFunc = (assetName) => PathConfig.BundleInLocalInStreamingAssetPath(assetName);
                        m_RemotePathFunc = (assetName) => PathConfig.BundleInRemoteInPersistentDataPath(assetName);
                        break;
                    }
            }
            "注册成功".Log();
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
    }
}