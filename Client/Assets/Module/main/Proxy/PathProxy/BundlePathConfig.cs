using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class BundlePathConfig : IBundlePathConfig
    {
        private readonly BaseSubscribe<BUNDLE_PATH, Func<string?, string?>?> bundlePathSubscribe;
        private readonly BaseSubscribe<ASSET_SERVER_BUNDLE_PATH, Func<string?, string?, string?>?> assetServerBundlePathSubscribe;
        
        
        [Inject]
        public BundlePathConfig(IAssetConfig assetConfig, IPlayerSettingsConfig playerSettingsConfig, INameConfig nameConfig)
        {
            var url = assetConfig.Url();

            var produceName = playerSettingsConfig.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettingsConfig.Get(PLAY_SETTINGS.PlatformName);
            
            var streamingAssetsPath = Application.streamingAssetsPath;
            var persistentDataPath = Application.persistentDataPath;
            var persistentData_produceName_platformName = $"{persistentDataPath}/{produceName}/{platformName}";
            var url_produceName_platformName = $"{url}/{produceName}/{platformName}";
            
            var bundleRootPathByLocalGroupByStreamingAssets  = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByLocalGroup()}";
            var bundleRootPathByRemoteGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByRemoteGroup()}";
            var bundleRootPathByDllGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByDllGroup()}";

            var bundleRootPathByRemoteGroupByPersistentData = $"{persistentData_produceName_platformName}/{nameConfig.DirectoryNameByRemoteGroup()}";
            var bundleRootPathByRemoteGroupPersistentData = $"{persistentData_produceName_platformName}/{nameConfig.DirectoryNameByDllGroup()}";

            bundlePathSubscribe = new BaseSubscribe<BUNDLE_PATH, Func<string?, string?>?>
            {
                [BUNDLE_PATH.BundlePathByLocalGroupByStreamingAssets] = bundleName => $"{bundleRootPathByLocalGroupByStreamingAssets}/{bundleName}",
                [BUNDLE_PATH.BundlePathByRemoteGroupByStreamingAssets] = bundleName => $"{bundleRootPathByRemoteGroupByStreamingAssets}/{bundleName}",
                [BUNDLE_PATH.BundlePathByDllGroupByStreamingAssets] = bundleName => $"{bundleRootPathByDllGroupByStreamingAssets}/{bundleName}",
                [BUNDLE_PATH.BundlePathByRemoteGroupByPersistentData] = bundleName => $"{bundleRootPathByRemoteGroupByPersistentData}/{bundleName}",
                [BUNDLE_PATH.BundlePathByDllGroupByPersistentData] = bundleName => $"{bundleRootPathByRemoteGroupPersistentData}/{bundleName}",
            };

            assetServerBundlePathSubscribe = new BaseSubscribe<ASSET_SERVER_BUNDLE_PATH, Func<string?, string?, string?>?>
            {
                [ASSET_SERVER_BUNDLE_PATH.BundlePathByRemoteGroup] = (version, bundleName) => $"{url_produceName_platformName}/{version}/{nameConfig.DirectoryNameByRemoteGroup()}/{bundleName}",
                [ASSET_SERVER_BUNDLE_PATH.BundlePathByDllGroup] = (version, bundleName) => $"{url_produceName_platformName}/{version}/{nameConfig.DirectoryNameByDllGroup()}/{bundleName}",
            };
        }

        string? IBundlePathConfig.Get(BUNDLE_PATH bundlePath, string? bundleName)
        {
            return bundlePathSubscribe.Get(bundlePath)?.Invoke(bundleName);
        }

        string? IBundlePathConfig.Get(ASSET_SERVER_BUNDLE_PATH bundlePath, string? version, string? bundleName)
        {
            return assetServerBundlePathSubscribe.Get(bundlePath)?.Invoke(version, bundleName);
        }
    }
}
