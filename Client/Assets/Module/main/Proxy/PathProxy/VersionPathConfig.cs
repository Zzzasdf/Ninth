using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class VersionPathConfig: IVersionPathConfig
    {
        private readonly BaseSubscribe<VERSION_PATH, string?> versionPathSubscribe;
        private readonly BaseSubscribe<ASSET_SERVER_VERSION_PATH, string?> assetServerVersionPathSubscribe;
        
        [Inject]
        public VersionPathConfig(IAssetConfig assetConfig, IPlayerSettingsConfig playerSettingsConfig, INameConfig nameConfig)
        {
            var url = assetConfig.Url();

            var produceName = playerSettingsConfig.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettingsConfig.Get(PLAY_SETTINGS.PlatformName);
            
            var streamingAssetsPath = Application.streamingAssetsPath;
            var persistentDataPath = Application.persistentDataPath;
            var persistentData_produceName_platformName = $"{persistentDataPath}/{produceName}/{platformName}";
            var url_produceName_platformName = $"{url}/{produceName}/{platformName}";

            var streamingAssets = $"{streamingAssetsPath}/{nameConfig.FileNameByVersionConfig()}";
            var persistentData = $"{persistentData_produceName_platformName}/{nameConfig.FileNameByVersionConfig()}";
            var persistentDataTemp = $"{persistentData_produceName_platformName}/{nameConfig.FileTempNameByVersionConfig()}";
            
            var assetServer = $"{url_produceName_platformName}/{nameConfig.FileNameByVersionConfig()}";

            versionPathSubscribe = new BaseSubscribe<VERSION_PATH, string?>
            {
                [VERSION_PATH.StreamingAssets] = streamingAssets,
                [VERSION_PATH.PersistentData] = persistentData,
                [VERSION_PATH.PersistentDataTemp] = persistentDataTemp,
            };

            assetServerVersionPathSubscribe = new BaseSubscribe<ASSET_SERVER_VERSION_PATH, string?>
            {
                [ASSET_SERVER_VERSION_PATH.AssetServer] = assetServer,
            };
        }
        
        string? IVersionPathConfig.Get(VERSION_PATH versionPath)
        {
            return versionPathSubscribe.Get(versionPath);
        }

        string? IVersionPathConfig.Get(ASSET_SERVER_VERSION_PATH versionPath)
        {
            return assetServerVersionPathSubscribe.Get(versionPath);
        }
    }
}
