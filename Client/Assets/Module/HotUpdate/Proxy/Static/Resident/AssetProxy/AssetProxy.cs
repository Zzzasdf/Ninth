using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Ninth.HotUpdate
{
    public static partial class AssetProxy
    {
        // 资源配置
        private static Dictionary<string, AssetRef> m_ConfigAssetPath2AssetRef;

        // 加载进内存的bundle, 确保只存在一个
        public static Dictionary<string, BundleRef> m_BundlePath2BundleRef;

        private static Func<string, string> m_LocalPathFunc;
        private static Func<string, string> m_RemotePathFunc;

        public static async UniTask Register()
        {
            m_ConfigAssetPath2AssetRef = new Dictionary<string, AssetRef>();
            m_BundlePath2BundleRef = new Dictionary<string, BundleRef>();
            LoadConfig localLoadConfig = await JsonProxy<LocalLoadConfig>.Get();
            LoadConfig remoteLoadConfig = await JsonProxy<RemoteLoadConfig>.Get();

            SetPath2BundleName(localLoadConfig);
            SetPath2BundleName(remoteLoadConfig);

            m_LocalPathFunc = (assetName) => PathConfig.BundleInLocalInStreamingAssetPath(assetName);
            m_RemotePathFunc = (assetName) => PathConfig.BundleInRemoteInPersistentDataPath(assetName);
            "AssetProxy 初始化成功".Log();
        }

        private static void SetPath2BundleName(LoadConfig loadConfig)
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