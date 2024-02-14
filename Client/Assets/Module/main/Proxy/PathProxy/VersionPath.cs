using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class VersionPath: IVersionPath
    {
        private readonly ReadOnlyDictionary<VERSION_PATH, string> versionPathContainer;
        private readonly ReadOnlyDictionary<ASSET_SERVER_VERSION_PATH, string> assetServerVersionPathContainer;

        [Inject]
        public VersionPath(IAssetConfig assetConfig, IPlayerSettings playerSettings, INameConfig nameConfig)
        {
            var url = assetConfig.Url();

            var produceName = playerSettings.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettings.Get(PLAY_SETTINGS.PlatformName);
            
            var streamingAssetsPath = Application.streamingAssetsPath;
            var persistentDataPath = Application.persistentDataPath;
            var persistentData_produceName_platformName = $"{persistentDataPath}/{produceName}/{platformName}";
            var url_produceName_platformName = $"{url}/{produceName}/{platformName}";

            var streamingAssets = $"{streamingAssetsPath}/{nameConfig.FileNameByVersionConfig()}";
            var persistentData = $"{persistentData_produceName_platformName}/{nameConfig.FileNameByVersionConfig()}";
            var persistentDataTemp = $"{persistentData_produceName_platformName}/{nameConfig.FileTempNameByVersionConfig()}";
            
            var assetServer = $"{url_produceName_platformName}/{nameConfig.FileNameByVersionConfig()}";
            
            versionPathContainer = new ReadOnlyDictionary<VERSION_PATH, string>(new Dictionary<VERSION_PATH, string>());
            assetServerVersionPathContainer = new ReadOnlyDictionary<ASSET_SERVER_VERSION_PATH, string>(new Dictionary<ASSET_SERVER_VERSION_PATH, string>());
            
            Subscribe(VERSION_PATH.StreamingAssets, streamingAssets);
            Subscribe(VERSION_PATH.PersistentData, persistentData);
            Subscribe(VERSION_PATH.PersistentDataTemp, persistentDataTemp);
            
            Subscribe(ASSET_SERVER_VERSION_PATH.AssetServer, assetServer);
        }
        
        void Subscribe(VERSION_PATH versionPath, string path)
        {
            if (!versionPathContainer.TryAdd(versionPath, path))
            {
                $"重复订阅 {nameof(VERSION_PATH)}: {versionPath}".FrameError();
            }
        }
        
        void Subscribe(ASSET_SERVER_VERSION_PATH versionPath, string path)
        {
            if (!assetServerVersionPathContainer.TryAdd(versionPath, path))
            {
                $"重复订阅 {nameof(ASSET_SERVER_VERSION_PATH)}: {versionPath}".FrameError();
            }
        }
        
        string? IVersionPath.Get(VERSION_PATH versionPath)
        {
            if (!versionPathContainer.TryGetValue(versionPath, out var result))
            {
                $"未订阅 {nameof(VERSION_PATH)}: {versionPath}".FrameError();
                return null;
            }
            return result;
        }
        
        string? IVersionPath.Get(ASSET_SERVER_VERSION_PATH versionPath)
        {
            if (!assetServerVersionPathContainer.TryGetValue(versionPath, out var result))
            {
                $"未订阅 {nameof(ASSET_SERVER_VERSION_PATH)}: {versionPath}".FrameError();
                return null;
            }
            return result;
        }
    }
}
