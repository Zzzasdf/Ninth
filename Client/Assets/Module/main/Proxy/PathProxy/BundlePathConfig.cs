using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class BundlePathConfig : IBundlePathConfig
    {
        private readonly SubscribeCollect<Func<string, string>, BUNDLE_PATH> bundlePathSubscribe;
        private readonly SubscribeCollect<(Func<string, string, string> serverPath, BUNDLE_PATH cachePath), ASSET_SERVER_BUNDLE_PATH> assetServerBundlePathSubscribe;

        SubscribeCollect<Func<string, string>, BUNDLE_PATH> IBundlePathConfig.BundlePathSubscribe => bundlePathSubscribe;
        SubscribeCollect<(Func<string, string, string> serverPath, BUNDLE_PATH cachePath), ASSET_SERVER_BUNDLE_PATH> IBundlePathConfig.AssetServerBundlePathSubscribe => assetServerBundlePathSubscribe;

        [Inject]
        public BundlePathConfig(IAssetConfig assetConfig, IPlayerSettingsConfig playerSettingsConfig, INameConfig nameConfig)
        {
            var url = assetConfig.Url();

            var produceName = playerSettingsConfig.StringSubscribe.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettingsConfig.StringSubscribe.Get(PLAY_SETTINGS.PlatformName);

            var streamingAssetsPath = Application.streamingAssetsPath;
            var persistentDataPath = Application.persistentDataPath;
            var persistentDataProduceNamePlatformName = $"{persistentDataPath}/{produceName}/{platformName}";
            var urlProduceNamePlatformName = $"{url}/{produceName}/{platformName}";

            var bundleRootPathByLocalGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByLocalGroup()}";
            var bundleRootPathByRemoteGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByRemoteGroup()}";
            var bundleRootPathByDllGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByDllGroup()}";

            var bundleRootPathByRemoteGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.DirectoryNameByRemoteGroup()}";
            var bundleRootPathByRemoteGroupPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.DirectoryNameByDllGroup()}";

            {
                var build = bundlePathSubscribe = new SubscribeCollect<Func<string, string>, BUNDLE_PATH>();
                build.Subscribe(BUNDLE_PATH.BundlePathByRemoteGroupByStreamingAssets, bundleName => $"{bundleRootPathByRemoteGroupByStreamingAssets}/{bundleName}");
                build.Subscribe(BUNDLE_PATH.BundlePathByDllGroupByStreamingAssets, bundleName => $"{bundleRootPathByDllGroupByStreamingAssets}/{bundleName}");
                build.Subscribe(BUNDLE_PATH.BundlePathByRemoteGroupByPersistentData, bundleName => $"{bundleRootPathByRemoteGroupByPersistentData}/{bundleName}");
                build.Subscribe(BUNDLE_PATH.BundlePathByDllGroupByPersistentData, bundleName => $"{bundleRootPathByRemoteGroupPersistentData}/{bundleName}");
            }

            {
                var build = assetServerBundlePathSubscribe = new SubscribeCollect<(Func<string, string, string> serverPath, BUNDLE_PATH cachePath), ASSET_SERVER_BUNDLE_PATH>();
                build.Subscribe(ASSET_SERVER_BUNDLE_PATH.BundlePathByRemoteGroup, ((version, bundleName) => $"{urlProduceNamePlatformName}/{version}/{nameConfig.DirectoryNameByRemoteGroup()}/{bundleName}", BUNDLE_PATH.BundlePathByRemoteGroupByPersistentData));
                build.Subscribe(ASSET_SERVER_BUNDLE_PATH.BundlePathByDllGroup, ((version, bundleName) => $"{urlProduceNamePlatformName}/{version}/{nameConfig.DirectoryNameByDllGroup()}/{bundleName}", BUNDLE_PATH.BundlePathByDllGroupByPersistentData));
            }
        }
    }
}