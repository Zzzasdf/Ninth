using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;
using VContainer;

namespace Ninth.Utility
{
    public class BundlePathConfig : IBundlePathConfig
    {
        private readonly SubscriberCollect<Func<string, string>, BUNDLE_PATH> bundlePathSubscriber;
        private readonly SubscriberCollect<(Func<string, string, string> serverPath, BUNDLE_PATH cachePath), ASSET_SERVER_BUNDLE_PATH> assetServerBundlePathSubscriber;

        SubscriberCollect<Func<string, string>, BUNDLE_PATH> IBundlePathConfig.BundlePathSubscriber => bundlePathSubscriber;
        SubscriberCollect<(Func<string, string, string> serverPath, BUNDLE_PATH cachePath), ASSET_SERVER_BUNDLE_PATH> IBundlePathConfig.AssetServerBundlePathSubscriber => assetServerBundlePathSubscriber;

        [Inject]
        public BundlePathConfig(IAssetConfig assetConfig, IPlayerSettingsConfig playerSettingsConfig, INameConfig nameConfig)
        {
            var url = assetConfig.Url();

            var produceName = playerSettingsConfig.StringSubscriber.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettingsConfig.StringSubscriber.Get(PLAY_SETTINGS.PlatformName);

            var streamingAssetsPath = Application.streamingAssetsPath;
            var persistentDataPath = Application.persistentDataPath;
            var persistentDataProduceNamePlatformName = $"{persistentDataPath}/{produceName}/{platformName}";
            var urlProduceNamePlatformName = $"{url}/{produceName}/{platformName}";

            var bundleRootPathByLocalGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.FolderByLocalGroup()}";
            var bundleRootPathByRemoteGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.FolderByRemoteGroup()}";
            var bundleRootPathByDllGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.FolderByDllGroup()}";

            var bundleRootPathByRemoteGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.FolderByRemoteGroup()}";
            var bundleRootPathByRemoteGroupPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.FolderByDllGroup()}";

            {
                var build = bundlePathSubscriber = new SubscriberCollect<Func<string, string>, BUNDLE_PATH>();
                build.Subscribe(BUNDLE_PATH.BundlePathByRemoteGroupByStreamingAssets, bundleName => $"{bundleRootPathByRemoteGroupByStreamingAssets}/{bundleName}");
                build.Subscribe(BUNDLE_PATH.BundlePathByDllGroupByStreamingAssets, bundleName => $"{bundleRootPathByDllGroupByStreamingAssets}/{bundleName}");
                build.Subscribe(BUNDLE_PATH.BundlePathByRemoteGroupByPersistentData, bundleName => $"{bundleRootPathByRemoteGroupByPersistentData}/{bundleName}");
                build.Subscribe(BUNDLE_PATH.BundlePathByDllGroupByPersistentData, bundleName => $"{bundleRootPathByRemoteGroupPersistentData}/{bundleName}");
            }

            {
                var build = assetServerBundlePathSubscriber = new SubscriberCollect<(Func<string, string, string> serverPath, BUNDLE_PATH cachePath), ASSET_SERVER_BUNDLE_PATH>();
                build.Subscribe(ASSET_SERVER_BUNDLE_PATH.BundlePathByRemoteGroup, ((version, bundleName) => $"{urlProduceNamePlatformName}/{version}/{nameConfig.FolderByRemoteGroup()}/{bundleName}", BUNDLE_PATH.BundlePathByRemoteGroupByPersistentData));
                build.Subscribe(ASSET_SERVER_BUNDLE_PATH.BundlePathByDllGroup, ((version, bundleName) => $"{urlProduceNamePlatformName}/{version}/{nameConfig.FolderByDllGroup()}/{bundleName}", BUNDLE_PATH.BundlePathByDllGroupByPersistentData));
            }
        }
    }
}