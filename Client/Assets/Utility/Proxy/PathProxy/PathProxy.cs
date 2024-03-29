using UnityEngine;
using VContainer;

namespace Ninth.Utility
{
    public class PathProxy: IPathProxy
    {
        private readonly IVersionPathConfig versionPathConfig;
        private readonly IConfigPathConfig configPathConfig;
        private readonly IBundlePathConfig bundlePathConfig;

        [Inject]
        public PathProxy(IVersionPathConfig versionPathConfig, IConfigPathConfig configPathConfig, IBundlePathConfig bundlePathConfig)
        {
            this.versionPathConfig = versionPathConfig;
            this.configPathConfig = configPathConfig;
            this.bundlePathConfig = bundlePathConfig;
        }
        
        string IPathProxy.Get(VERSION_PATH versionPath)
        {
            return versionPathConfig.VersionPathSubscriber.GetValue(versionPath);
        }

        (string assetServerVersionPath, VERSION_PATH versionPersistentDataTempPath) IPathProxy.Get(ASSET_SERVER_VERSION_PATH versionPath)
        {
            return versionPathConfig.AssetServerVersionPathSubscriber.GetValue(versionPath);
        }

        string IPathProxy.Get(CONFIG_PATH configPath)
        {
            return configPathConfig.ConfigPathSubscriber.GetValue(configPath);
        }

        (string assetServerConfigPath, CONFIG_PATH configPersistentDataTempPath) IPathProxy.Get(ASSET_SERVER_CONFIG_PATH configPath, string version)
        {
            var item = configPathConfig.AssetServerConfigPathSubscriber.GetValue(configPath);
            return (item.serverPath.Invoke(version), item.cachePath);
        }

        string IPathProxy.Get(BUNDLE_PATH bundlePath, string bundleName)
        {
            return bundlePathConfig.BundlePathSubscriber.GetValue(bundlePath).Invoke( bundleName);
        }

        (string assetServerBundlePath, BUNDLE_PATH bundlePersistentDataTempPath) IPathProxy.Get(ASSET_SERVER_BUNDLE_PATH bundlePath, string version, string bundleName)
        {
            var item = bundlePathConfig.AssetServerBundlePathSubscriber.GetValue(bundlePath);
            return (item.serverPath.Invoke(version, bundleName), item.cachePath);
        }
    }
}