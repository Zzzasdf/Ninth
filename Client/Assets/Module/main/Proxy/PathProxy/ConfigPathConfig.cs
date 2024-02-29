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
        private readonly SubscribeCollect<string, CONFIG_PATH> configPathSubscribe;
        private readonly SubscribeCollect<(Func<string, string> serverPath, CONFIG_PATH cachePath), ASSET_SERVER_CONFIG_PATH> assetServerConfigPathSubscribe;

        SubscribeCollect<string, CONFIG_PATH> IConfigPathConfig.ConfigPathSubscribe => configPathSubscribe;
        SubscribeCollect<(Func<string, string> serverPath, CONFIG_PATH cachePath), ASSET_SERVER_CONFIG_PATH> IConfigPathConfig.AssetServerConfigPathSubscribe => assetServerConfigPathSubscribe;

        [Inject]
        public ConfigPathConfig(IAssetConfig assetConfig, IPlayerSettingsConfig playerSettingsConfig, INameConfig nameConfig)
        {
            var url = assetConfig.Url();

            var produceName = playerSettingsConfig.StringSubscribe?.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettingsConfig.StringSubscribe?.Get(PLAY_SETTINGS.PlatformName);

            var streamingAssetsPath = Application.streamingAssetsPath;
            var persistentDataPath = Application.persistentDataPath;
            var persistentDataProduceNamePlatformName = $"{persistentDataPath}/{produceName}/{platformName}";
            var urlProduceNamePlatformName = $"{url}/{produceName}/{platformName}";

            var loadConfigPathByLocalGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByLocalGroup()}/{nameConfig.LoadConfigNameByLocalGroup()}";
            var loadConfigPathByRemoteGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByRemoteGroup()}/{nameConfig.LoadConfigNameByRemoteGroup()}";

            var downloadConfigPathByRemoteGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.DirectoryNameByRemoteGroup()}/{nameConfig.DownloadConfigNameByRemoteGroup()}";
            var downloadConfigTempPathByRemoteGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.DirectoryNameByRemoteGroup()}/{nameConfig.DownloadConfigTempNameByRemoteGroup()}";
            var downloadConfigPathByDllGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.DirectoryNameByDllGroup()}/{nameConfig.DownloadConfigNameByDllGroup()}";
            var downloadConfigTempPathByDllGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.DirectoryNameByDllGroup()}/{nameConfig.DownloadConfigTempNameByDllGroup()}";
            var loadConfigPathByRemoteGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.DirectoryNameByRemoteGroup()}/{nameConfig.LoadConfigNameByRemoteGroup()}";
            var loadConfigPathByDllGroupByPersistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.DirectoryNameByDllGroup()}/{nameConfig.LoadConfigNameByDllGroup()}";

            {
                var build = configPathSubscribe = new SubscribeCollect<string, CONFIG_PATH>();
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
                var build = assetServerConfigPathSubscribe = new SubscribeCollect<(Func<string, string> serverPath, CONFIG_PATH cachePath), ASSET_SERVER_CONFIG_PATH>();
                build.Subscribe(ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByRemoteGroup, (version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.DirectoryNameByRemoteGroup()}", CONFIG_PATH.DownloadConfigTempPathByRemoteGroupByPersistentData));
                build.Subscribe(ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByDllGroup,
                    (version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.DirectoryNameByDllGroup()}/{nameConfig.DownloadConfigNameByDllGroup()}", CONFIG_PATH.DownloadConfigTempPathByDllGroupByPersistentData));
                build.Subscribe(ASSET_SERVER_CONFIG_PATH.LoadConfigPathByRemoteGroup,
                    (version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.DirectoryNameByRemoteGroup()}/{nameConfig.LoadConfigNameByRemoteGroup()}", CONFIG_PATH.LoadConfigPathByRemoteGroupByPersistentData));
                build.Subscribe(ASSET_SERVER_CONFIG_PATH.LoadConfigPathByDllGroup, (version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.DirectoryNameByDllGroup()}/{nameConfig.LoadConfigNameByDllGroup()}", CONFIG_PATH.LoadConfigPathByDllGroupByPersistentData));
            }
        }
    }
}