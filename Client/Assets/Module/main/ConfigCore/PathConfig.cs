using UnityEngine;

namespace Ninth
{
    public sealed class PathConfig
    {
        private AssetConfig assetConfig;
        private PlatformConfig platformConfig;
        private NameConfig nameConfig;
        public PathConfig(AssetConfig assetConfig, PlatformConfig platformConfig, NameConfig nameConfig)
        {
            this.assetConfig = assetConfig;
            this.platformConfig = platformConfig;
            this.nameConfig = nameConfig;

            // 版本
            baseVersionPath = string.Format("{0}/{1}", Application.streamingAssetsPath, nameConfig.VersionConfigName);
            versionInServerPath = string.Format("{0}/{1}/{2}/{3}", assetConfig.Url, platformConfig.ProduceName, platformConfig.PlatformName, nameConfig.VersionConfigName);
            versionInPersistentDataPath = string.Format("{0}/{1}/{2}/{3}", Application.persistentDataPath, platformConfig.ProduceName, platformConfig.PlatformName, nameConfig.VersionConfigName);
            tempVersionInPersistentDataPath = string.Format("{0}/{1}/{2}/{3}", Application.persistentDataPath, platformConfig.ProduceName, platformConfig.PlatformName, nameConfig.TempVersionConfigName);

            // 下载配置
            downloadConfigRootInServerPath = string.Format("{0}/{1}/{2}", assetConfig.Url, platformConfig.ProduceName, platformConfig.PlatformName);
            downloadConfigInRemoteInPersistentDataPath = string.Format("{0}/{1}/{2}/{3}/{4}", Application.persistentDataPath, platformConfig.ProduceName, platformConfig.PlatformName, nameConfig.RemoteDirectory, nameConfig.DownloadConfigNameInRemote);
            tempDownloadConfigInRemoteInPersistentDataPath = string.Format("{0}/{1}/{2}/{3}/{4}", Application.persistentDataPath, platformConfig.ProduceName, platformConfig.PlatformName, nameConfig.RemoteDirectory, nameConfig.TempDownloadConfigNameInRemote);
            downloadConfigInDllInPersistentDataPath =  string.Format("{0}/{1}/{2}/{3}/{4}", Application.persistentDataPath, platformConfig.ProduceName, platformConfig.PlatformName, nameConfig.DllDirectory, nameConfig.DownloadConfigNameInDll);
            tempDownloadConfigInDllInPersistentDataPath = string.Format("{0}/{1}/{2}/{3}/{4}", Application.persistentDataPath, platformConfig.ProduceName, platformConfig.PlatformName, nameConfig.DllDirectory, nameConfig.TempDownloadConfigNameInDll);

            // 加载配置
            loadConfigRootInServerPath = string.Format("{0}/{1}/{2}",assetConfig.Url, platformConfig.ProduceName, platformConfig.PlatformName);
            loadConfigInLocalInStreamingAssetPath = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, nameConfig.LocalDirectory, nameConfig.LoadConfigNameInLocal);
            loadConfigInRemoteInStreamingAssetPath = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, nameConfig.RemoteDirectory, nameConfig.LoadConfigNameInRemote);
            loadConfigInRemoteInPersistentDataPath = string.Format("{0}/{1}/{2}/{3}/{4}", Application.persistentDataPath, platformConfig.ProduceName, platformConfig.PlatformName, nameConfig.RemoteDirectory, nameConfig.LoadConfigNameInRemote);
            loadConfigInDllInPersistentDataPath = string.Format("{0}/{1}/{2}/{3}/{4}", Application.persistentDataPath, platformConfig.ProduceName, platformConfig.PlatformName, nameConfig.DllDirectory, nameConfig.LoadConfigNameInDll);

            // 资源
            bundleRootInServerPath = string.Format("{0}/{1}/{2}", assetConfig.Url, platformConfig.ProduceName, platformConfig.PlatformName);
            bundleRootInLocalInStreamingAssetPath = string.Format("{0}/{1}", Application.streamingAssetsPath, nameConfig.LocalDirectory);
            bundleRootInRemoteInStreamingAssetPath = string.Format("{0}/{1}", Application.streamingAssetsPath, nameConfig.RemoteDirectory);
            bundleRootInRemoteInPersistentDataPath = string.Format("{0}/{1}/{2}/{3}", Application.persistentDataPath, platformConfig.ProduceName, platformConfig.PlatformName, nameConfig.RemoteDirectory);
            bundleRootInDllInStreamingAssetPath = string.Format("{0}/{1}", Application.streamingAssetsPath, nameConfig.DllDirectory);
            bundleRootInDllInPersistentDataPath = string.Format("{0}/{1}/{2}/{3}", Application.persistentDataPath, platformConfig.ProduceName, platformConfig.PlatformName, nameConfig.DllDirectory);
        }

        // 版本
        private string baseVersionPath { get; }
        private string versionInServerPath { get; }
        private string versionInPersistentDataPath { get; }
        private string tempVersionInPersistentDataPath { get; }

        // 下载配置
        private string downloadConfigRootInServerPath { get; }
        private string downloadConfigInRemoteInPersistentDataPath { get; }
        private string tempDownloadConfigInRemoteInPersistentDataPath { get; }
        private string downloadConfigInDllInPersistentDataPath { get; }
        private string tempDownloadConfigInDllInPersistentDataPath { get; }

        // 加载配置
        private string loadConfigRootInServerPath { get; }
        private string loadConfigInLocalInStreamingAssetPath { get; }
        private string loadConfigInRemoteInStreamingAssetPath { get; }
        private string loadConfigInRemoteInPersistentDataPath { get; }
        private string loadConfigInDllInPersistentDataPath { get; }

        // 资源
        private string bundleRootInServerPath { get; }
        private string bundleRootInLocalInStreamingAssetPath { get; }
        public string bundleRootInRemoteInStreamingAssetPath { get; }
        public string bundleRootInRemoteInPersistentDataPath { get; }
        private string bundleRootInDllInStreamingAssetPath { get; }
        public string bundleRootInDllInPersistentDataPath { get; }

        // 版本在服务器的路径
        public string VersionInServerPath()
        {
            return versionInServerPath;
        }

        // 版本在持久化目录的路径
        public string VersionInPersistentDataPath()
        {
            return versionInPersistentDataPath;
        }

        // 版本在持久化目录的临时路径
        public string TempVersionInPersistentDataPath()
        {
            return tempVersionInPersistentDataPath;
        }

        // 基础版本的版本路径
        public string BaseVersionPath()
        {
            return baseVersionPath;
        }

        // 下载配置在服务器的路径
        public string DownloadConfigInRemoteInServerPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", downloadConfigRootInServerPath, version.ToString(), nameConfig.RemoteDirectory, nameConfig.DownloadConfigNameInRemote);
        }

        public string DownloadConfigInDllInServerPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", downloadConfigRootInServerPath, version.ToString(), nameConfig.DllDirectory, nameConfig.DownloadConfigNameInDll);
        }

        // 下载配置在持久化目录的路径
        public string DownloadConfigInRemoteInPersistentDataPath()
        {
            return downloadConfigInRemoteInPersistentDataPath;
        }

        public string TempDownloadConfigInRemoteInPersistentDataPath()
        {
            return tempDownloadConfigInRemoteInPersistentDataPath;
        }

        public string DownloadConfigInDllInPersistentDataPath()
        {
            return downloadConfigInDllInPersistentDataPath;
        }

        public string TempDownloadConfigInDllInPersistentDataPath()
        {
            return tempDownloadConfigInDllInPersistentDataPath;
        }

        // 加载配置在服务器的路径
        public string LoadConfigInRemoteInServerPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", loadConfigRootInServerPath, version.ToString(), nameConfig.RemoteDirectory, nameConfig.LoadConfigNameInRemote);
        }

        public string LoadConfigInDllInServerPath(string version)
        {
            return string.Format("{0}/{1}/{2}/{3}", loadConfigRootInServerPath, version.ToString(), nameConfig.DllDirectory, nameConfig.LoadConfigNameInDll);
        }

        // 加载配置在本地的路径
        public string LoadConfigInLocalInStreamingAssetPath()
        {
            return loadConfigInLocalInStreamingAssetPath;
        }

        public string LoadConfigInRemoteInStreamingAssetPath()
        {
            return loadConfigInRemoteInStreamingAssetPath;
        }

        public string LoadConfigInRemoteInPersistentDataPath()
        {
            return loadConfigInRemoteInPersistentDataPath;
        }

        public string LoadConfigInDllInPersistentDataPath()
        {
            return loadConfigInDllInPersistentDataPath;
        }

        // Bundle在服务器的路径
        public string BundleInRemoteInServerPath(string version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", bundleRootInServerPath, version.ToString(), nameConfig.RemoteDirectory, bundleName);
        }

        public string BundleInDllInServerPath(string version, string bundleName)
        {
            return string.Format("{0}/{1}/{2}/{3}", bundleRootInServerPath, version.ToString(), nameConfig.DllDirectory, bundleName);
        }

        // Bundle在本地的路径
        public string BundleInLocalInStreamingAssetPath(string bundleName)
        {
            return string.Format("{0}/{1}", bundleRootInLocalInStreamingAssetPath, bundleName);
        }

        public string BunldeInRemoteInStreamingAssetPath(string bundleName)
        {
            return string.Format("{0}/{1}", bundleRootInRemoteInStreamingAssetPath, bundleName);
        }

        public string BundleInRemoteInPersistentDataPath(string bundleName)
        {
            return string.Format("{0}/{1}", bundleRootInRemoteInPersistentDataPath, bundleName);
        }
        
        public string BunldeInDllInStreamingAssetPath(string bundleName)
        {
            return string.Format("{0}/{1}", bundleRootInDllInStreamingAssetPath, bundleName);
        }
        public string BundleInDllInPersistentDataPath(string bundleName)
        {
            return string.Format("{0}/{1}", bundleRootInDllInPersistentDataPath, bundleName);
        }
    }
}