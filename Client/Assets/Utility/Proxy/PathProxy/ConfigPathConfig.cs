using System;
using UnityEngine;
using VContainer;

namespace Ninth.Utility
{
    public class ConfigPathConfig : IConfigPathConfig
    {
        private readonly SubscriberCollect<string, CONFIG_PATH> configPathSubscriber;
        private readonly SubscriberCollect<(Func<string, string> serverPath, CONFIG_PATH cachePath), ASSET_SERVER_CONFIG_PATH> assetServerConfigPathSubscriber;

        SubscriberCollect<string, CONFIG_PATH> IConfigPathConfig.ConfigPathSubscriber => configPathSubscriber;
        SubscriberCollect<(Func<string, string> serverPath, CONFIG_PATH cachePath), ASSET_SERVER_CONFIG_PATH> IConfigPathConfig.AssetServerConfigPathSubscriber => assetServerConfigPathSubscriber;

        [Inject]
        public ConfigPathConfig(IAssetConfig assetConfig, IPlayerSettingsConfig playerSettingsConfig, INameConfig nameConfig)
        {
            var url = assetConfig.Url();
            var produceName = playerSettingsConfig.StringSubscriber?.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettingsConfig.StringSubscriber?.Get(PLAY_SETTINGS.PlatformName);

            var streamingAssetsPath = Application.streamingAssetsPath;
            var persistentDataPath = Application.persistentDataPath;
            var loadConfigPathByLocalGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.FolderByLocalGroup()}/{nameConfig.LoadConfigNameByLocalGroup()}";
            var loadConfigPathByRemoteGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.FolderByRemoteGroup()}/{nameConfig.LoadConfigNameByRemoteGroup()}";
            var downloadConfigPathByRemoteGroupByPersistentData = $"{persistentDataPath}/{nameConfig.FolderByRemoteGroup()}/{nameConfig.DownloadConfigNameByRemoteGroup()}";
            var downloadConfigTempPathByRemoteGroupByPersistentData = $"{persistentDataPath}/{nameConfig.FolderByRemoteGroup()}/{nameConfig.DownloadConfigTempNameByRemoteGroup()}";
            var downloadConfigPathByDllGroupByPersistentData = $"{persistentDataPath}/{nameConfig.FolderByDllGroup()}/{nameConfig.DownloadConfigNameByDllGroup()}";
            var downloadConfigTempPathByDllGroupByPersistentData = $"{persistentDataPath}/{nameConfig.FolderByDllGroup()}/{nameConfig.DownloadConfigTempNameByDllGroup()}";
            var loadConfigPathByRemoteGroupByPersistentData = $"{persistentDataPath}/{nameConfig.FolderByRemoteGroup()}/{nameConfig.LoadConfigNameByRemoteGroup()}";
            var loadConfigPathByDllGroupByPersistentData = $"{persistentDataPath}/{nameConfig.FolderByDllGroup()}/{nameConfig.LoadConfigNameByDllGroup()}";

            var urlProduceNamePlatformName = $"{url}/{produceName}/{platformName}";
            Func<string, string> assetServerDownloadConfigPathByRemoteGroup = version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.FolderByRemoteGroup()}/{nameConfig.DownloadConfigNameByRemoteGroup()}";
            Func<string, string> assetServerDownloadConfigPathByDllGroup = version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.FolderByDllGroup()}/{nameConfig.DownloadConfigNameByDllGroup()}";
            Func<string, string> assetServerLoadConfigPathByRemoteGroup = version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.FolderByRemoteGroup()}/{nameConfig.LoadConfigNameByRemoteGroup()}";
            Func<string, string> assetServerLoadConfigPathByDllGroup = version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.FolderByDllGroup()}/{nameConfig.LoadConfigNameByDllGroup()}";
            
            {
                var build = configPathSubscriber = new SubscriberCollect<string, CONFIG_PATH>();
                build.Subscribe(CONFIG_PATH.LoadConfigPathByLocalGroupByStreamingAssets, loadConfigPathByLocalGroupByStreamingAssets);
                build.Subscribe(CONFIG_PATH.LoadConfigPathByRemoteGroupByStreamingAssets, loadConfigPathByRemoteGroupByStreamingAssets);
                build.Subscribe(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData, downloadConfigPathByRemoteGroupByPersistentData);
                build.Subscribe(CONFIG_PATH.DownloadConfigTempPathByRemoteGroupByPersistentData, downloadConfigTempPathByRemoteGroupByPersistentData);
                build.Subscribe(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData, downloadConfigPathByDllGroupByPersistentData);
                build.Subscribe(CONFIG_PATH.DownloadConfigTempPathByDllGroupByPersistentData, downloadConfigTempPathByDllGroupByPersistentData);
                build.Subscribe(CONFIG_PATH.LoadConfigPathByRemoteGroupByPersistentData, loadConfigPathByRemoteGroupByPersistentData);
                build.Subscribe(CONFIG_PATH.LoadConfigPathByDllGroupByPersistentData, loadConfigPathByDllGroupByPersistentData);
            }

            {
                var build = assetServerConfigPathSubscriber = new SubscriberCollect<(Func<string, string> serverPath, CONFIG_PATH cachePath), ASSET_SERVER_CONFIG_PATH>();
                build.Subscribe(ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByRemoteGroup, (assetServerDownloadConfigPathByRemoteGroup, CONFIG_PATH.DownloadConfigTempPathByRemoteGroupByPersistentData));
                build.Subscribe(ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByDllGroup, (assetServerDownloadConfigPathByDllGroup, CONFIG_PATH.DownloadConfigTempPathByDllGroupByPersistentData));
                build.Subscribe(ASSET_SERVER_CONFIG_PATH.LoadConfigPathByRemoteGroup, (assetServerLoadConfigPathByRemoteGroup, CONFIG_PATH.LoadConfigPathByRemoteGroupByPersistentData));
                build.Subscribe(ASSET_SERVER_CONFIG_PATH.LoadConfigPathByDllGroup, (assetServerLoadConfigPathByDllGroup, CONFIG_PATH.LoadConfigPathByDllGroupByPersistentData));
            }
        }
    }
}