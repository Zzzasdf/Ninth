using UnityEngine;

namespace Ninth.Editor
{
    public class PackConfig
    {
        private static string m_BuildPlatform;
        public static string BuildPlatform
        {
            private get
            {
                return m_BuildPlatform;
            }
            set
            {
                m_BuildPlatform = value;

                AssetBundleSourceDataRoot =
                    string.Format("{0}/../../Bundles/AssetBundleSourceData/{1}/{2}", Application.dataPath, PlatformConfig.ProduceName, BuildPlatform);
            }
        }

        private static AssetMode m_PackAssetMode;

        public static AssetMode PackAssetMode
        {
            get
            {
                return m_PackAssetMode;
            }
            set
            {
                m_PackAssetMode = value;

                AssetBundleOutputRoot = m_PackAssetMode switch
                {
                    AssetMode.RemoteAB => string.Format("{0}/../../Bundles/AssetBundleOutput/{1}/{2}", Application.dataPath, PlatformConfig.ProduceName, BuildPlatform),
                    AssetMode.LocalAB => string.Format("{0}", Application.streamingAssetsPath),
                    _ => throw new System.NotImplementedException(),
                };
            }
        }

        // 打包资源根节点
        private static string m_GAssets { get; }
            = string.Format("{0}/GAssets", Application.dataPath);

        // 源数据根节点
        private static string AssetBundleSourceDataRoot { get; set; }

        // 完整数据根节点
        private static string AssetBundleOutputRoot { get; set; }

        // 源目录
        public static string SourceDataTempPathDirectory(long version)
        {
            return string.Format("{0}/{1}/{2}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.PackTempDirectory);
        }

        public static string SourceDataLocalPathDirectory(long version)
        {
            return string.Format("{0}/{1}/{2}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.LocalDirectory);
        }

        public static string SourceDataRemotePathDirectory(long version)
        {
            return string.Format("{0}/{1}/{2}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.RemoteDirectory);
        }

        public static string SourceDataRemoteExtensionsPathDirectory(long version)
        {
            return string.Format("{0}/{1}/{2}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.RemoteExtensionsDirectory);
        }

        public static string SourceDataDllPathDirectory(long version)
        {
            return string.Format("{0}/{1}/{2}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.DllDirectory);
        }

        // 输出目录
        public static string OutputLocalPathDirectory(long version)
        {
            return string.Format("{0}/{1}", Application.streamingAssetsPath, NameConfig.LocalDirectory);
        }

        public static string OutputRemotePathDirectory(long version)
        {
            return string.Format("{0}/{1}/{2}", AssetBundleOutputRoot, version.ToString(), NameConfig.RemoteDirectory);
        }

        public static string OutputRemoteExtensionsPathDirectory(long version)
        {
            return string.Format("{0}/{1}/{2}", AssetBundleOutputRoot, version.ToString(), NameConfig.RemoteExtensionsDirectory);
        }

        public static string OutputDllPathDirectory(long version)
        {
            return string.Format("{0}/{1}/{2}", AssetBundleOutputRoot, version.ToString(), NameConfig.DllDirectory);
        }

        // 打包资源根节点
        public static string GAssets()
        {
            return m_GAssets;
        }

        // 版本
        public static string VersionInSourceDataPath(long version)
        {
            return string.Format("{0}/{1}/{2}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.VersionConfigName);
        }

        public static string BaseVersion()
        {
            return PathConfig.BaseVersionPath();
        }

        public static string VersionInOutputPath()
        {
            return string.Format("{0}/{1}", AssetBundleOutputRoot, NameConfig.VersionConfigName);
        }

        // 下载配置
        public static string DownloadConfigInRemoteInSourceDataPath(long version)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.RemoteDirectory, NameConfig.DownloadConfigNameInRemote);
        }

        public static string DownloadConfigInRemoteExtensionsInSourceDataPath(long version)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.RemoteExtensionsDirectory, NameConfig.DownloadConfigNameInRemoteExtensions);
        }

        public static string DownloadConfigInDllInSourceDataPath(long version)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.DllDirectory, NameConfig.DownloadConfigNameInDll);
        }

        public static string DownloadConfigInRemoteInOutputPath(long version)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleOutputRoot, version.ToString(), NameConfig.RemoteDirectory, NameConfig.DownloadConfigNameInRemote);
        }

        public static string DownloadConfigInRemoteExtensionsInOutputPath(long version)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleOutputRoot, version.ToString(), NameConfig.RemoteExtensionsDirectory, NameConfig.DownloadConfigNameInRemoteExtensions);
        }

        public static string DownloadConfigInDllInOutputPath(long version)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleOutputRoot, version.ToString(), NameConfig.DllDirectory, NameConfig.DownloadConfigNameInDll);
        }

        // 加载配置
        public static string LoadConfigInLocalInSourceDataPath(long version)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.LocalDirectory, NameConfig.LoadConfigNameInLocal);
        }

        public static string LoadConfigInRemoteInSourceDataPath(long version)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.RemoteDirectory, NameConfig.LoadConfigNameInRemote);
        }

        public static string LoadConfigInRemoteExtensionsInSourceDataPath(long version)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.RemoteExtensionsDirectory, NameConfig.LoadConfigNameInRemoteExtensions);
        }

        public static string LoadConfigInDllInSourceDataPath(long version)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.DllDirectory, NameConfig.LoadConfigNameInDll);
        }

        public static string LoadConfigInLocalInOutputPath(long version)
        {
            return PathConfig.LoadConfigInLocalInStreamingAssetPath();
        }

        public static string LoadConfigInRemoteInOutputPath(long version)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleOutputRoot, version.ToString(), NameConfig.RemoteDirectory, NameConfig.LoadConfigNameInRemote);
        }

        public static string LoadConfigInRemoteExtensionsInOutputPath(long version)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleOutputRoot, version.ToString(), NameConfig.RemoteExtensionsDirectory, NameConfig.LoadConfigNameInRemoteExtensions);
        }

        public static string LoadConfigInDllInOutputPath(long version)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleOutputRoot, version.ToString(), NameConfig.DllDirectory, NameConfig.LoadConfigNameInDll);
        }

        // Bundle路径
        public static string BundleInTempInSourceDataPath(long version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.PackTempDirectory, bundleName);
        }

        public static string BundleInLocalInSourceDataPath(long version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.LocalDirectory, bundleName);
        }

        public static string BundleInRemoteInSourceDataPath(long version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.RemoteDirectory, bundleName);
        }

        public static string BundleInRemoteExtensionsInSourceDataPath(long version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.RemoteExtensionsDirectory, bundleName);
        }

        public static string BundleInDllInSourceDataPath(long version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleSourceDataRoot, version.ToString(), NameConfig.DllDirectory, bundleName);
        }

        public static string BundleInLocalInOutputPath(string bundleName)
        {
            return PathConfig.BundleInLocalInStreamingAssetPath(bundleName);
        }

        public static string BundleInRemoteInOutputPath(long version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleOutputRoot, version.ToString(), NameConfig.RemoteDirectory, bundleName);
        }

        public static string BundleInRemoteExtensionsInOutputPath(long version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleOutputRoot, version.ToString(), NameConfig.RemoteExtensionsDirectory, bundleName);
        }

        public static string BundleInDllInOutputPath(long version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", AssetBundleOutputRoot, version.ToString(), NameConfig.DllDirectory, bundleName);
        }
    }
}