using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Ninth.HotUpdate
{
    public partial class AssetProxy
    {
        // 资源配置
        private Dictionary<string, AssetRef> m_ConfigAssetPath2AssetRef;

        // 加载进内存的bundle, 确保只存在一个
        public Dictionary<string, BundleRef> m_BundlePath2BundleRef;

        private Func<string, string> m_LocalPathFunc;
        private Func<string, string> m_RemotePathFunc;

        public AssetProxy()
        {
            m_ConfigAssetPath2AssetRef = new Dictionary<string, AssetRef>();
            m_BundlePath2BundleRef = new Dictionary<string, BundleRef>();

            // LoadConfig localLoadConfig = await ProxyCtrl.ModelProxy.Get<LocalLoadConfig>();
            // LoadConfig remoteLoadConfig = await ProxyCtrl.ModelProxy.Get<RemoteLoadConfig>();

            // SetPath2BundleName(localLoadConfig);
            // SetPath2BundleName(remoteLoadConfig);

            m_LocalPathFunc = (assetName) => PathConfig.BundleInLocalInStreamingAssetPath(assetName);
            m_RemotePathFunc = (assetName) => PathConfig.BundleInRemoteInPersistentDataPath(assetName);
            "AssetProxy 初始化成功".Log();
        }

        private void SetPath2BundleName(LoadConfig loadConfig)
        {
            if (loadConfig.AssetRefList == null)
            {
                return;
            }
            for (int index = 0; index < loadConfig.AssetRefList.Count; index++)
            {
                string key = loadConfig.AssetRefList[index].AssetPath;
                AssetRef value = loadConfig.AssetRefList[index];
                m_ConfigAssetPath2AssetRef.Add(key, value);
            }
        }
    }
}