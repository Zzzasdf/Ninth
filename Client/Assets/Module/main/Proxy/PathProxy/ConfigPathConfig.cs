using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class ConfigPathConfig : IConfigPathConfig
    {
        private readonly CommonSubscribe<CONFIG_PATH, string> configPathSubscribe;
        private readonly CommonSubscribe<ASSET_SERVER_CONFIG_PATH, (Func<string, string> serverPath, CONFIG_PATH cachePath)> assetServerConfigPathSubscribe;

        CommonSubscribe<CONFIG_PATH, string> IConfigPathConfig.ConfigPathSubscribe => configPathSubscribe;
        CommonSubscribe<ASSET_SERVER_CONFIG_PATH, (Func<string, string> serverPath, CONFIG_PATH cachePath)> IConfigPathConfig.AssetServerConfigPathSubscribe => assetServerConfigPathSubscribe;

        [Inject]
        public ConfigPathConfig(IAssetConfig assetConfig, IPlayerSettingsConfig playerSettingsConfig, INameConfig nameConfig)
        {
            var url = assetConfig.Url();

            var produceName = playerSettingsConfig.CommonSubscribe?.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettingsConfig.CommonSubscribe?.Get(PLAY_SETTINGS.PlatformName);
            
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

            configPathSubscribe = new CommonSubscribe<CONFIG_PATH, string>
            {
                [CONFIG_PATH.LoadConfigPathByLocalGroupByStreamingAssets] = loadConfigPathByLocalGroupByStreamingAssets,
                [CONFIG_PATH.LoadConfigPathByRemoteGroupByStreamingAssets] = loadConfigPathByRemoteGroupByStreamingAssets,
                [CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData] = downloadConfigPathByRemoteGroupByPersistentData,
                [CONFIG_PATH.DownloadConfigTempPathByRemoteGroupByPersistentData] = downloadConfigTempPathByRemoteGroupByPersistentData,
                [CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData] = downloadConfigPathByDllGroupByPersistentData,
                [CONFIG_PATH.DownloadConfigTempPathByDllGroupByPersistentData] = downloadConfigTempPathByDllGroupByPersistentData,
                [CONFIG_PATH.LoadConfigPathByRemoteGroupByPersistentData] = loadConfigPathByRemoteGroupByPersistentData,
                [CONFIG_PATH.LoadConfigPathByDllGroupByPersistentData] = loadConfigPathByDllGroupByPersistentData,
            };

            assetServerConfigPathSubscribe = new CommonSubscribe<ASSET_SERVER_CONFIG_PATH, (Func<string, string> serverPath, CONFIG_PATH cachePath)>
            {
                [ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByRemoteGroup] = (version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.DirectoryNameByRemoteGroup()}", CONFIG_PATH.DownloadConfigTempPathByRemoteGroupByPersistentData),
                [ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByDllGroup] = (version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.DirectoryNameByDllGroup()}/{nameConfig.DownloadConfigNameByDllGroup()}", CONFIG_PATH.DownloadConfigTempPathByDllGroupByPersistentData),
                [ASSET_SERVER_CONFIG_PATH.LoadConfigPathByRemoteGroup] = (version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.DirectoryNameByRemoteGroup()}/{nameConfig.LoadConfigNameByRemoteGroup()}", CONFIG_PATH.LoadConfigPathByRemoteGroupByPersistentData),
                [ASSET_SERVER_CONFIG_PATH.LoadConfigPathByDllGroup] = (version => $"{urlProduceNamePlatformName}/{version}/{nameConfig.DirectoryNameByDllGroup()}/{nameConfig.LoadConfigNameByDllGroup()}", CONFIG_PATH.LoadConfigPathByDllGroupByPersistentData),
            };
        }
    }
}
