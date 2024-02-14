using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class ConfigPath : IConfigPath
    {
        private readonly ReadOnlyDictionary<CONFIG_PATH, string> configPathContainer;
        private readonly ReadOnlyDictionary<ASSET_SERVER_CONFIG_PATH, Func<string, string>> assetServerConfigPathContainer;

        [Inject]
        public ConfigPath(IAssetConfig assetConfig, IPlayerSettings playerSettings, INameConfig nameConfig)
        {
            var url = assetConfig.Url();

            var produceName = playerSettings.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettings.Get(PLAY_SETTINGS.PlatformName);
            
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
            
            configPathContainer = new ReadOnlyDictionary<CONFIG_PATH, string>(new Dictionary<CONFIG_PATH, string>());
            assetServerConfigPathContainer = new ReadOnlyDictionary<ASSET_SERVER_CONFIG_PATH, Func<string, string>>(new Dictionary<ASSET_SERVER_CONFIG_PATH, Func<string, string>>());
            
            Subscribe(CONFIG_PATH.LoadConfigPathByLocalGroupByStreamingAssets, loadConfigPathByLocalGroupByStreamingAssets);
            Subscribe(CONFIG_PATH.LoadConfigPathByRemoteGroupByStreamingAssets, loadConfigPathByRemoteGroupByStreamingAssets);
            
            Subscribe(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData, downloadConfigPathByRemoteGroupByPersistentData);
            Subscribe(CONFIG_PATH.DownloadConfigTempPathByRemoteGroupByPersistentData, downloadConfigTempPathByRemoteGroupByPersistentData);
            Subscribe(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData, downloadConfigPathByDllGroupByPersistentData);
            Subscribe(CONFIG_PATH.DownloadConfigTempPathByDllGroupByPersistentData, downloadConfigTempPathByDllGroupByPersistentData);
            Subscribe(CONFIG_PATH.LoadConfigPathByRemoteGroupByPersistentData, loadConfigPathByRemoteGroupByPersistentData);
            Subscribe(CONFIG_PATH.LoadConfigPathByDllGroupByPersistentData, loadConfigPathByDllGroupByPersistentData);
            
            Subscribe(ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByRemoteGroup, version => $"{url_produceName_platformName}/{version}/{nameConfig.DirectoryNameByRemoteGroup()}");
            Subscribe(ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByDllGroup, version => $"{url_produceName_platformName}/{version}/{nameConfig.DirectoryNameByDllGroup()}/{nameConfig.DownloadConfigNameByDllGroup()}");
            Subscribe(ASSET_SERVER_CONFIG_PATH.LoadConfigPathByRemoteGroup, version => $"{url_produceName_platformName}/{version}/{nameConfig.DirectoryNameByRemoteGroup()}/{nameConfig.LoadConfigNameByRemoteGroup()}");
            Subscribe(ASSET_SERVER_CONFIG_PATH.LoadConfigPathByDllGroup, version => $"{url_produceName_platformName}/{version}/{nameConfig.DirectoryNameByDllGroup()}/{nameConfig.LoadConfigNameByDllGroup()}");
        }
        
        void Subscribe(CONFIG_PATH configPath, string path)
        {
            if (!configPathContainer.TryAdd(configPath, path))
            {
                $"重复订阅 {nameof(CONFIG_PATH)}: {configPath}".FrameError();
            }
        }
        
        void Subscribe(ASSET_SERVER_CONFIG_PATH configPath, Func<string, string> path)
        {
            if (!assetServerConfigPathContainer.TryAdd(configPath, path))
            {
                $"重复订阅 {nameof(ASSET_SERVER_CONFIG_PATH)}: {configPath}".FrameError();
            }
        }

        string? IConfigPath.Get(CONFIG_PATH configPath)
        {
            if (!configPathContainer.TryGetValue(configPath, out var result))
            {
                $"未订阅 {nameof(CONFIG_PATH)}: {configPath}".FrameError();
                return null;
            }
            return result;
        }

        string? IConfigPath.Get(ASSET_SERVER_CONFIG_PATH configPath, string version)
        {
            if (!assetServerConfigPathContainer.TryGetValue(configPath, out var result))
            {
                $"未订阅 {nameof(ASSET_SERVER_CONFIG_PATH)}: {configPath}".FrameError();
                return null;
            }
            return result?.Invoke(version);
        }
    }
}
