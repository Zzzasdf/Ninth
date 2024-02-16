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
        private readonly BaseSubscribe<CONFIG_PATH, string?> configPathSubscribe;
        private readonly BaseSubscribe<ASSET_SERVER_CONFIG_PATH, Func<string?, string?>?> assetServerConfigPathSubscribe;
        
        [Inject]
        public ConfigPathConfig(IAssetConfig assetConfig, IPlayerSettingsConfig playerSettingsConfig, INameConfig nameConfig)
        {
            var url = assetConfig.Url();

            var produceName = playerSettingsConfig.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettingsConfig.Get(PLAY_SETTINGS.PlatformName);
            
            var streamingAssetsPath = Application.streamingAssetsPath;
            var persistentDataPath = Application.persistentDataPath;
            var persistentData_produceName_platformName = $"{persistentDataPath}/{produceName}/{platformName}";
            var url_produceName_platformName = $"{url}/{produceName}/{platformName}";

            var loadConfigPathByLocalGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByLocalGroup()}/{nameConfig.LoadConfigNameByLocalGroup()}";
            var loadConfigPathByRemoteGroupByStreamingAssets = $"{streamingAssetsPath}/{nameConfig.DirectoryNameByRemoteGroup()}/{nameConfig.LoadConfigNameByRemoteGroup()}";
            
            var downloadConfigPathByRemoteGroupByPersistentData = $"{persistentData_produceName_platformName}/{nameConfig.DirectoryNameByRemoteGroup()}/{nameConfig.DownloadConfigNameByRemoteGroup()}";
            var downloadConfigTempPathByRemoteGroupByPersistentData = $"{persistentData_produceName_platformName}/{nameConfig.DirectoryNameByRemoteGroup()}/{nameConfig.DownloadConfigTempNameByRemoteGroup()}";
            var downloadConfigPathByDllGroupByPersistentData = $"{persistentData_produceName_platformName}/{nameConfig.DirectoryNameByDllGroup()}/{nameConfig.DownloadConfigNameByDllGroup()}";
            var downloadConfigTempPathByDllGroupByPersistentData = $"{persistentData_produceName_platformName}/{nameConfig.DirectoryNameByDllGroup()}/{nameConfig.DownloadConfigTempNameByDllGroup()}";
            var loadConfigPathByRemoteGroupByPersistentData = $"{persistentData_produceName_platformName}/{nameConfig.DirectoryNameByRemoteGroup()}/{nameConfig.LoadConfigNameByRemoteGroup()}";
            var loadConfigPathByDllGroupByPersistentData = $"{persistentData_produceName_platformName}/{nameConfig.DirectoryNameByDllGroup()}/{nameConfig.LoadConfigNameByDllGroup()}";

            configPathSubscribe = new BaseSubscribe<CONFIG_PATH, string?>
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

            assetServerConfigPathSubscribe = new BaseSubscribe<ASSET_SERVER_CONFIG_PATH, Func<string?, string?>?>
            {
                [ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByRemoteGroup] = version => $"{url_produceName_platformName}/{version}/{nameConfig.DirectoryNameByRemoteGroup()}",
                [ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByDllGroup] = version => $"{url_produceName_platformName}/{version}/{nameConfig.DirectoryNameByDllGroup()}/{nameConfig.DownloadConfigNameByDllGroup()}",
                [ASSET_SERVER_CONFIG_PATH.LoadConfigPathByRemoteGroup] = version => $"{url_produceName_platformName}/{version}/{nameConfig.DirectoryNameByRemoteGroup()}/{nameConfig.LoadConfigNameByRemoteGroup()}",
                [ASSET_SERVER_CONFIG_PATH.LoadConfigPathByDllGroup] = version => $"{url_produceName_platformName}/{version}/{nameConfig.DirectoryNameByDllGroup()}/{nameConfig.LoadConfigNameByDllGroup()}",
            };
        }
        
        string? IConfigPathConfig.Get(CONFIG_PATH configPath)
        {
            return configPathSubscribe.Get(configPath);
        }

        string? IConfigPathConfig.Get(ASSET_SERVER_CONFIG_PATH configPath, string version)
        {
            return assetServerConfigPathSubscribe.Get(configPath)?.Invoke(version);
        }
    }
}
