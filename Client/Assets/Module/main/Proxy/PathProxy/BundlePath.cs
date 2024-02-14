using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class BundlePath : IBundlePath
    {
        private readonly ReadOnlyDictionary<BUNDLE_PATH, Func<string, string>> bundlePathContainer;
        private readonly ReadOnlyDictionary<ASSET_SERVER_BUNDLE_PATH, Func<string, string, string>> assetServerBundlePathContainer;
        
        [Inject]
        public BundlePath(IAssetConfig assetConfig, IPlayerSettings playerSettings, INameConfig nameConfig)
        {
            var url = assetConfig.Url();

            var produceName = playerSettings.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettings.Get(PLAY_SETTINGS.PlatformName);
            
            var streamingAssetsPath = Application.streamingAssetsPath;
            var persistentDataPath = Application.persistentDataPath;
            var persistentData_produceName_platformName = $"{persistentDataPath}/{produceName}/{platformName}";
            var url_produceName_platformName = $"{url}/{produceName}/{platformName}";
            
            var bundleRootPathByLocalGroupByStreamingAssets  = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByLocalGroup()}";
            var bundleRootPathByRemoteGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByRemoteGroup()}";
            var bundleRootPathByDllGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByDllGroup()}";

            var bundleRootPathByRemoteGroupByPersistentData = $"{persistentData_produceName_platformName}/{nameConfig.DirectoryNameByRemoteGroup()}";
            var bundleRootPathByRemoteGroupPersistentData = $"{persistentData_produceName_platformName}/{nameConfig.DirectoryNameByDllGroup()}";

            bundlePathContainer = new ReadOnlyDictionary<BUNDLE_PATH, Func<string, string>>(new Dictionary<BUNDLE_PATH, Func<string, string>>());
            assetServerBundlePathContainer = new ReadOnlyDictionary<ASSET_SERVER_BUNDLE_PATH, Func<string, string, string>>(new Dictionary<ASSET_SERVER_BUNDLE_PATH, Func<string, string, string>>());
            
            Subscribe(BUNDLE_PATH.BundlePathByLocalGroupByStreamingAssets, bundleName => $"{bundleRootPathByLocalGroupByStreamingAssets }/{bundleName}");
            Subscribe(BUNDLE_PATH.BundlePathByRemoteGroupByStreamingAssets, bundleName => $"{bundleRootPathByRemoteGroupByStreamingAssets}/{bundleName}");
            Subscribe(BUNDLE_PATH.BundlePathByDllGroupByStreamingAssets, bundleName => $"{bundleRootPathByDllGroupByStreamingAssets}/{bundleName}");
            Subscribe(BUNDLE_PATH.BundlePathByRemoteGroupByPersistentData, bundleName => $"{bundleRootPathByRemoteGroupByPersistentData}/{bundleName}");
            Subscribe(BUNDLE_PATH.BundlePathByDllGroupByPersistentData, bundleName => $"{bundleRootPathByRemoteGroupPersistentData}/{bundleName}");
            
            Subscribe(ASSET_SERVER_BUNDLE_PATH.BundlePathByRemoteGroup, (version, bundleName) => $"{url_produceName_platformName}/{version}/{nameConfig.DirectoryNameByRemoteGroup()}/{bundleName}");
            Subscribe(ASSET_SERVER_BUNDLE_PATH.BundlePathByDllGroup, (version, bundleName) => $"{url_produceName_platformName}/{version}/{nameConfig.DirectoryNameByDllGroup()}/{bundleName}");
        }
        
        void Subscribe(BUNDLE_PATH bundlePath, Func<string, string> path)
        {
            if (!bundlePathContainer.TryAdd(bundlePath, path))
            {
                $"重复订阅 {nameof(BUNDLE_PATH)}: {bundlePath}".FrameError();
            }
        }
        
        void Subscribe(ASSET_SERVER_BUNDLE_PATH bundlePath, Func<string, string, string> path)
        {
            if (!assetServerBundlePathContainer.TryAdd(bundlePath, path))
            {
                $"重复订阅 {nameof(ASSET_SERVER_BUNDLE_PATH)}: {bundlePath}".FrameError();
            }
        }
        
        string? IBundlePath.Get(BUNDLE_PATH bundlePath, string bundleName)
        {
            if (!bundlePathContainer.TryGetValue(bundlePath, out var result))
            {
                $"未订阅 {nameof(BUNDLE_PATH)}: {bundlePath}".FrameError();
                return null;
            }
            return result?.Invoke(bundleName);
        }

        string? IBundlePath.Get(ASSET_SERVER_BUNDLE_PATH bundlePath, string version, string bundleName)
        {
            if (!assetServerBundlePathContainer.TryGetValue(bundlePath, out var result))
            {
                $"未订阅 {nameof(ASSET_SERVER_BUNDLE_PATH)}: {bundlePath}".FrameError();
                return null;
            }
            return result?.Invoke(version, bundleName);
        }
    }
}
