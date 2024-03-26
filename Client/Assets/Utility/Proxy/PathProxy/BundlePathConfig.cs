using System;
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
        public BundlePathConfig(PlayerVersionConfig playerVersionConfig, IPlayerSettingsConfig playerSettingsConfig, INameConfig nameConfig)
        {
            var url = playerVersionConfig.Url;
            var produceName = playerSettingsConfig.StringSubscriber.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettingsConfig.StringSubscriber.Get(PLAY_SETTINGS.PlatformName);

            var streamingAssetsPath = Application.streamingAssetsPath;
            var persistentDataPath = Application.persistentDataPath;
            Func<string, string> bundlePathByLocalGroupByStreamingAssets = bundleName => $"{streamingAssetsPath}/{nameConfig.FolderByLocalGroup()}/{bundleName}";
            Func<string, string> bundlePathByRemoteGroupByStreamingAssets = bundleName => $"{streamingAssetsPath}/{nameConfig.FolderByRemoteGroup()}/{bundleName}";
            Func<string, string> bundlePathByDllGroupByStreamingAssets = bundleName => $"{streamingAssetsPath}/{nameConfig.FolderByDllGroup()}/{bundleName}";
            Func<string, string> bundlePathByRemoteGroupByPersistentData = bundleName => $"{persistentDataPath}/{nameConfig.FolderByRemoteGroup()}/{bundleName}";
            Func<string, string> bundlePathByDllGroupByPersistentData = bundleName => $"{persistentDataPath}/{nameConfig.FolderByDllGroup()}/{bundleName}";

            var urlProduceNamePlatformName = $"{url}/{produceName}/{platformName}";
            Func<string, string, string> assetServerBundlePathByRemoteGroup = (version, bundleName) => $"{urlProduceNamePlatformName}/{version}/{nameConfig.FolderByRemoteGroup()}/{bundleName}";
            Func<string, string, string> assetServerBundlePathByDllGroup = (version, bundleName) => $"{urlProduceNamePlatformName}/{version}/{nameConfig.FolderByDllGroup()}/{bundleName}";
            {
                var build = bundlePathSubscriber = new SubscriberCollect<Func<string, string>, BUNDLE_PATH>();
                build.Subscribe(BUNDLE_PATH.BundlePathByLocalGroupByStreamingAssets, bundlePathByLocalGroupByStreamingAssets);
                build.Subscribe(BUNDLE_PATH.BundlePathByRemoteGroupByStreamingAssets, bundlePathByRemoteGroupByStreamingAssets);
                build.Subscribe(BUNDLE_PATH.BundlePathByDllGroupByStreamingAssets, bundlePathByDllGroupByStreamingAssets);
                build.Subscribe(BUNDLE_PATH.BundlePathByRemoteGroupByPersistentData, bundlePathByRemoteGroupByPersistentData);
                build.Subscribe(BUNDLE_PATH.BundlePathByDllGroupByPersistentData, bundlePathByDllGroupByPersistentData);
            }

            {
                var build = assetServerBundlePathSubscriber = new SubscriberCollect<(Func<string, string, string> serverPath, BUNDLE_PATH cachePath), ASSET_SERVER_BUNDLE_PATH>();
                build.Subscribe(ASSET_SERVER_BUNDLE_PATH.BundlePathByRemoteGroup, (assetServerBundlePathByRemoteGroup, BUNDLE_PATH.BundlePathByRemoteGroupByPersistentData));
                build.Subscribe(ASSET_SERVER_BUNDLE_PATH.BundlePathByDllGroup, (assetServerBundlePathByDllGroup, BUNDLE_PATH.BundlePathByDllGroupByPersistentData));
            }
        }
    }
}