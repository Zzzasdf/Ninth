using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;
using VContainer;

namespace Ninth
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
            var persistentDataProduceNamePlatformName = $"{persistentDataPath}/{produceName}/{platformName}";
            var urlProduceNamePlatformName = $"{url}/{produceName}/{platformName}";

            var loadConfigPathByLocalGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.FolderByLocalGroup()}/{nameConfig.LoadConfigNameByLocalGroup()}";
            var loadConfigPathByRemoteGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.FolderByRemoteGroup()}/{nameConfig.LoadConfigNameByRemoteGroup()}";

            var downloadConfigPathByRemoteGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.FolderByRemoteGroup()}/{nameConfig.DownloadConfigNameByRemoteGroup()}";
            var downloadConfigTempPathByRemoteGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.FolderByRemoteGroup()}/{nameConfig.DownloadConfigTempNameByRemoteGroup()}";
            var downloadConfigPathByDllGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.FolderByDllGroup()}/{nameConfig.DownloadConfigNameByDllGroup()}";
            var downloadConfigTempPathByDllGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.FolderByDllGroup()}/{nameConfig.DownloadConfigTempNameByDllGroup()}";
            var loadConfigPathByRemoteGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.FolderByRemoteGroup()}/{nameConfig.LoadConfigNameByRemoteGroup()}";
            var loadConfigPathByDllGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.FolderByDllGroup()}/{nameConfig.LoadConfigNameByDllGroup()}";

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
                build.Subscribe(ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByRemoteGroup, (version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.FolderByRemoteGroup()}", CONFIG_PATH.DownloadConfigTempPathByRemoteGroupByPersistentData));
                build.Subscribe(ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByDllGroup,
                    (version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.FolderByDllGroup()}/{nameConfig.DownloadConfigNameByDllGroup()}", CONFIG_PATH.DownloadConfigTempPathByDllGroupByPersistentData));
                build.Subscribe(ASSET_SERVER_CONFIG_PATH.LoadConfigPathByRemoteGroup,
                    (version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.FolderByRemoteGroup()}/{nameConfig.LoadConfigNameByRemoteGroup()}", CONFIG_PATH.LoadConfigPathByRemoteGroupByPersistentData));
                build.Subscribe(ASSET_SERVER_CONFIG_PATH.LoadConfigPathByDllGroup, (version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.FolderByDllGroup()}/{nameConfig.LoadConfigNameByDllGroup()}", CONFIG_PATH.LoadConfigPathByDllGroupByPersistentData));
            }
        }
    }
}