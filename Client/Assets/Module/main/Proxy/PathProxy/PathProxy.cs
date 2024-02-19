using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;
using Ninth;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class PathProxy: IPathProxy
    {
        private readonly CommonSubscribe<ASSET_SERVER_VERSION_PATH, VERSION_PATH?> versionPathCommonSubscribe;
        private readonly CommonSubscribe<ASSET_SERVER_CONFIG_PATH, CONFIG_PATH?> configPathCommonSubscribe;
        private readonly CommonSubscribe<ASSET_SERVER_BUNDLE_PATH, BUNDLE_PATH?> bundlePathCommonSubscribe;
        
        private readonly IVersionPathConfig versionPathConfig;
        private readonly IConfigPathConfig configPathConfig;
        private readonly IBundlePathConfig bundlePathConfig;

        [Inject]
        public PathProxy(IVersionPathConfig versionPathConfig, IConfigPathConfig configPathConfig, IBundlePathConfig bundlePathConfig)
        {
            this.versionPathConfig = versionPathConfig;
            this.configPathConfig = configPathConfig;
            this.bundlePathConfig = bundlePathConfig;

            versionPathCommonSubscribe = new CommonSubscribe<ASSET_SERVER_VERSION_PATH, VERSION_PATH?>
            {
                [ASSET_SERVER_VERSION_PATH.AssetServer] = VERSION_PATH.PersistentDataTemp,
            };

            configPathCommonSubscribe = new CommonSubscribe<ASSET_SERVER_CONFIG_PATH, CONFIG_PATH?>
            {
                [ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByRemoteGroup] = CONFIG_PATH.DownloadConfigTempPathByRemoteGroupByPersistentData,
                [ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByDllGroup] = CONFIG_PATH.DownloadConfigTempPathByDllGroupByPersistentData,
                [ASSET_SERVER_CONFIG_PATH.LoadConfigPathByRemoteGroup] = CONFIG_PATH.LoadConfigPathByRemoteGroupByPersistentData,
                [ASSET_SERVER_CONFIG_PATH.LoadConfigPathByDllGroup] = CONFIG_PATH.LoadConfigPathByDllGroupByPersistentData,
            };

            bundlePathCommonSubscribe = new CommonSubscribe<ASSET_SERVER_BUNDLE_PATH, BUNDLE_PATH?>
            {

            };
        }
        
        string? IPathProxy.Get(VERSION_PATH versionPath)
        {
            return versionPathConfig.Get(versionPath);
        }

        (string? assetServerVersionPath, VERSION_PATH? versionPersistentDataTempPath) IPathProxy.Get(ASSET_SERVER_VERSION_PATH versionPath)
        {
            return (versionPathConfig.Get(versionPath), versionPathCommonSubscribe.Get(versionPath));
        }

        string? IPathProxy.Get(CONFIG_PATH configPath)
        {
            return configPathConfig.Get(configPath);
        }

        (string? assetServerConfigPath, CONFIG_PATH? configPersistentDataTempPath) IPathProxy.Get(ASSET_SERVER_CONFIG_PATH configPath, string version)
        {
            return (configPathConfig.Get(configPath, version), configPathCommonSubscribe.Get(configPath));
        }

        string? IPathProxy.Get(BUNDLE_PATH bundlePath, string bundleName)
        {
            return bundlePathConfig.Get(bundlePath, bundleName);
        }

        (string? assetServerBundlePath, BUNDLE_PATH? bundlePersistentDataTempPath) IPathProxy.Get(ASSET_SERVER_BUNDLE_PATH bundlePath, string version, string bundleName)
        {
            return (bundlePathConfig.Get(bundlePath, version, bundleName), bundlePathCommonSubscribe.Get(bundlePath));
        }
    }
}