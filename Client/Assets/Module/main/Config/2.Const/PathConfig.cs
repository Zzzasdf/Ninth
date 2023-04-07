using UnityEngine;

namespace Ninth
{
    public sealed class PathConfig
    {
        public static NameConfig NameConfig
        {
            get => SOCore.GetNameConfig();
        }

        // 版本
        private static string m_BaseVersionPath { get; }
            = string.Format("{0}/{1}", Application.streamingAssetsPath, NameConfig.VersionConfigName);

        private static string m_VersionInServerPath { get; }
            = string.Format("{0}/{1}/{2}/{3}", SOCore.GetGlobalConfig().Url, PlatformConfig.ProduceName, PlatformConfig.PlatformName, NameConfig.VersionConfigName);

        private static string m_VersionInPersistentDataPath { get; }
            = string.Format("{0}/{1}/{2}/{3}", Application.persistentDataPath, PlatformConfig.ProduceName, PlatformConfig.PlatformName, NameConfig.VersionConfigName);

        private static string m_TempVersionInPersistentDataPath { get; }
            = string.Format("{0}/{1}/{2}/{3}", Application.persistentDataPath, PlatformConfig.ProduceName, PlatformConfig.PlatformName, NameConfig.TempVersionConfigName);

        // 下载配置
        private static string m_DownloadConfigRootInServerPath { get; }
            = string.Format("{0}/{1}/{2}", SOCore.GetGlobalConfig().Url, PlatformConfig.ProduceName, PlatformConfig.PlatformName);

        private static string m_DownloadConfigInRemoteInPersistentDataPath { get; }
            = string.Format("{0}/{1}/{2}/{3}/{4}", Application.persistentDataPath, PlatformConfig.ProduceName, PlatformConfig.PlatformName, NameConfig.RemoteDirectory, NameConfig.DownloadConfigNameInRemote);

        private static string m_TempDownloadConfigInRemoteInPersistentDataPath { get; }
          = string.Format("{0}/{1}/{2}/{3}/{4}", Application.persistentDataPath, PlatformConfig.ProduceName, PlatformConfig.PlatformName, NameConfig.RemoteDirectory, NameConfig.TempDownloadConfigNameInRemote);

        private static string m_DownloadConfigInDllInPersistentDataPath { get; }
           = string.Format("{0}/{1}/{2}/{3}/{4}", Application.persistentDataPath, PlatformConfig.ProduceName, PlatformConfig.PlatformName, NameConfig.DllDirectory, NameConfig.DownloadConfigNameInDll);

        private static string m_TempDownloadConfigInDllInPersistentDataPath { get; }
            = string.Format("{0}/{1}/{2}/{3}/{4}", Application.persistentDataPath, PlatformConfig.ProduceName, PlatformConfig.PlatformName, NameConfig.DllDirectory, NameConfig.TempDownloadConfigNameInDll);

        // 加载配置
        private static string m_LoadConfigRootInServerPath { get; }
            = string.Format("{0}/{1}/{2}", SOCore.GetGlobalConfig().Url, PlatformConfig.ProduceName, PlatformConfig.PlatformName);

        private static string m_LoadConfigInLocalInStreamingAssetPath { get; }
            = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, NameConfig.LocalDirectory, NameConfig.LoadConfigNameInLocal);

        private static string m_LoadConfigInRemoteInStreamingAssetPath { get; }
            = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, NameConfig.RemoteDirectory, NameConfig.LoadConfigNameInRemote);

        private static string m_LoadConfigInRemoteInPersistentDataPath { get; }
            = string.Format("{0}/{1}/{2}/{3}/{4}", Application.persistentDataPath, PlatformConfig.ProduceName, PlatformConfig.PlatformName, NameConfig.RemoteDirectory, NameConfig.LoadConfigNameInRemote);

        private static string m_LoadConfigInDllInPersistentDataPath { get; }
            = string.Format("{0}/{1}/{2}/{3}/{4}", Application.persistentDataPath, PlatformConfig.ProduceName, PlatformConfig.PlatformName, NameConfig.DllDirectory, NameConfig.LoadConfigNameInDll);

        // 资源
        private static string m_BundleRootInServerPath { get; }
            = string.Format("{0}/{1}/{2}", SOCore.GetGlobalConfig().Url, PlatformConfig.ProduceName, PlatformConfig.PlatformName);

        private static string m_BundleRootInLocalInStreamingAssetPath { get; }
            = string.Format("{0}/{1}", Application.streamingAssetsPath, NameConfig.LocalDirectory);

        public static string m_BundleRootInRemoteInStreamingAssetPath { get; }
            = string.Format("{0}/{1}", Application.streamingAssetsPath, NameConfig.RemoteDirectory);

        public static string BundleRootInRemoteInPersistentDataPath { get; }
            = string.Format("{0}/{1}/{2}/{3}", Application.persistentDataPath, PlatformConfig.ProduceName, PlatformConfig.PlatformName, NameConfig.RemoteDirectory);

        private static string m_BundleRootInDllInStreamingAssetPath { get; }
            = string.Format("{0}/{1}", Application.streamingAssetsPath, NameConfig.DllDirectory);

        public static string BundleRootInDllInPersistentDataPath { get; }
            = string.Format("{0}/{1}/{2}/{3}", Application.persistentDataPath, PlatformConfig.ProduceName, PlatformConfig.PlatformName, NameConfig.DllDirectory);

        // 版本在服务器的路径
        public static string VersionInServerPath()
        {
            return m_VersionInServerPath;
        }

        // 版本在持久化目录的路径
        public static string VersionInPersistentDataPath()
        {
            return m_VersionInPersistentDataPath;
        }

        // 版本在持久化目录的临时路径
        public static string TempVersionInPersistentDataPath()
        {
            return m_TempVersionInPersistentDataPath;
        }

        // 基础版本的版本路径
        public static string BaseVersionPath()
        {
            return m_BaseVersionPath;
        }

        // 下载配置在服务器的路径
        public static string DownloadConfigInRemoteInServerPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", m_DownloadConfigRootInServerPath, version.ToString(), NameConfig.RemoteDirectory, NameConfig.DownloadConfigNameInRemote);
        }

        public static string DownloadConfigInDllInServerPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", m_DownloadConfigRootInServerPath, version.ToString(), NameConfig.DllDirectory, NameConfig.DownloadConfigNameInDll);
        }

        // 下载配置在持久化目录的路径
        public static string DownloadConfigInRemoteInPersistentDataPath()
        {
            return m_DownloadConfigInRemoteInPersistentDataPath;
        }

        public static string TempDownloadConfigInRemoteInPersistentDataPath()
        {
            return m_TempDownloadConfigInRemoteInPersistentDataPath;
        }

        public static string DownloadConfigInDllInPersistentDataPath()
        {
            return m_DownloadConfigInDllInPersistentDataPath;
        }

        public static string TempDownloadConfigInDllInPersistentDataPath()
        {
            return m_TempDownloadConfigInDllInPersistentDataPath;
        }

        // 加载配置在服务器的路径
        public static string LoadConfigInRemoteInServerPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", m_LoadConfigRootInServerPath, version.ToString(), NameConfig.RemoteDirectory, NameConfig.LoadConfigNameInRemote);
        }

        public static string LoadConfigInDllInServerPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", m_LoadConfigRootInServerPath, version.ToString(), NameConfig.DllDirectory, NameConfig.LoadConfigNameInDll);
        }

        // 加载配置在本地的路径
        public static string LoadConfigInLocalInStreamingAssetPath()
        {
            return m_LoadConfigInLocalInStreamingAssetPath;
        }

        public static string LoadConfigInRemoteInStreamingAssetPath()
        {
            return m_LoadConfigInRemoteInStreamingAssetPath;
        }

        public static string LoadConfigInRemoteInPersistentDataPath()
        {
            return m_LoadConfigInRemoteInPersistentDataPath;
        }

        public static string LoadConfigInDllInPersistentDataPath()
        {
            return m_LoadConfigInDllInPersistentDataPath;
        }

        // Bundle在服务器的路径
        public static string BundleInRemoteInServerPath(string version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", m_BundleRootInServerPath, version.ToString(), NameConfig.RemoteDirectory, bundleName);
        }

        public static string BundleInDllInServerPath(string version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", m_BundleRootInServerPath, version.ToString(), NameConfig.DllDirectory, bundleName);
        }

        // Bundle在本地的路径
        public static string BundleInLocalInStreamingAssetPath(string bundleName)
        {
            return string.Format("{0}/{1}", m_BundleRootInLocalInStreamingAssetPath, bundleName);
        }

        public static string BunldeInRemoteInStreamingAssetPath(string bundleName)
        {
            return string.Format("{0}/{1}", m_BundleRootInRemoteInStreamingAssetPath, bundleName);
        }

        public static string BundleInRemoteInPersistentDataPath(string bundleName)
        {
            return string.Format("{0}/{1}", BundleRootInRemoteInPersistentDataPath, bundleName);
        }
        
        public static string BunldeInDllInStreamingAssetPath(string bundleName)
        {
            return string.Format("{0}/{1}", m_BundleRootInDllInStreamingAssetPath, bundleName);
        }
        public static string BundleInDllInPersistentDataPath(string bundleName)
        {
            return string.Format("{0}/{1}", BundleRootInDllInPersistentDataPath, bundleName);
        }
    }
}