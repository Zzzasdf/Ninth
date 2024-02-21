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
        private readonly CommonSubscribe<BUNDLE_PATH, Func<string, string>> bundlePathSubscribe;
        private readonly CommonSubscribe<ASSET_SERVER_BUNDLE_PATH, (Func<string, string, string> serverPath, BUNDLE_PATH cachePath)> assetServerBundlePathSubscribe;
        
        CommonSubscribe<BUNDLE_PATH, Func<string, string>> IBundlePathConfig.BundlePathSubscribe => bundlePathSubscribe;
        CommonSubscribe<ASSET_SERVER_BUNDLE_PATH, (Func<string, string, string> serverPath, BUNDLE_PATH cachePath)> IBundlePathConfig.AssetServerBundlePathSubscribe => assetServerBundlePathSubscribe;

        [Inject]
        public BundlePathConfig(IAssetConfig assetConfig, IPlayerSettingsConfig playerSettingsConfig, INameConfig nameConfig)
        {
            var url = assetConfig.Url();

            var produceName = playerSettingsConfig.CommonSubscribe?.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettingsConfig.CommonSubscribe?.Get(PLAY_SETTINGS.PlatformName);
            
            var streamingAssetsPath = Application.streamingAssetsPath;
            var persistentDataPath = Application.persistentDataPath;
            var persistentDataProduceNamePlatformName = $"{persistentDataPath}/{produceName}/{platformName}";
            var urlProduceNamePlatformName = $"{url}/{produceName}/{platformName}";
            
            var bundleRootPathByLocalGroupByStreamingAssets  = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByLocalGroup()}";
            var bundleRootPathByRemoteGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByRemoteGroup()}";
            var bundleRootPathByDllGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByDllGroup()}";

            var bundleRootPathByRemoteGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.DirectoryNameByRemoteGroup()}";
            var bundleRootPathByRemoteGroupPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.DirectoryNameByDllGroup()}";

            bundlePathSubscribe = new CommonSubscribe<BUNDLE_PATH, Func<string, string>>
            {
                [BUNDLE_PATH.BundlePathByLocalGroupByStreamingAssets] = bundleName => $"{bundleRootPathByLocalGroupByStreamingAssets}/{bundleName}",
                [BUNDLE_PATH.BundlePathByRemoteGroupByStreamingAssets] = bundleName => $"{bundleRootPathByRemoteGroupByStreamingAssets}/{bundleName}",
                [BUNDLE_PATH.BundlePathByDllGroupByStreamingAssets] = bundleName => $"{bundleRootPathByDllGroupByStreamingAssets}/{bundleName}",
                [BUNDLE_PATH.BundlePathByRemoteGroupByPersistentData] = bundleName => $"{bundleRootPathByRemoteGroupByPersistentData}/{bundleName}",
                [BUNDLE_PATH.BundlePathByDllGroupByPersistentData] = bundleName => $"{bundleRootPathByRemoteGroupPersistentData}/{bundleName}",
            };

            assetServerBundlePathSubscribe = new CommonSubscribe<ASSET_SERVER_BUNDLE_PATH, (Func<string, string, string> serverPath, BUNDLE_PATH cachePath)>
            {
                [ASSET_SERVER_BUNDLE_PATH.BundlePathByRemoteGroup] = ((version, bundleName) => $"{urlProduceNamePlatformName}/{version}/{nameConfig.DirectoryNameByRemoteGroup()}/{bundleName}", BUNDLE_PATH.BundlePathByRemoteGroupByPersistentData),
                [ASSET_SERVER_BUNDLE_PATH.BundlePathByDllGroup] = ((version, bundleName) => $"{urlProduceNamePlatformName}/{version}/{nameConfig.DirectoryNameByDllGroup()}/{bundleName}", BUNDLE_PATH.BundlePathByDllGroupByPersistentData),
            };
        }
    }
}
