using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class VersionPathConfig : IVersionPathConfig
    {
        private readonly CommonSubscribe<VERSION_PATH, string> versionPathSubscribe;
        private readonly CommonSubscribe<ASSET_SERVER_VERSION_PATH, (string serverPath, VERSION_PATH cachePath)> assetServerVersionPathSubscribe;

        CommonSubscribe<VERSION_PATH, string> IVersionPathConfig.VersionPathSubscribe => versionPathSubscribe;
        CommonSubscribe<ASSET_SERVER_VERSION_PATH, (string serverPath, VERSION_PATH cachePath)> IVersionPathConfig.AssetServerVersionPathSubscribe => assetServerVersionPathSubscribe;

        [Inject]
        public VersionPathConfig(IAssetConfig assetConfig, IPlayerSettingsConfig playerSettingsConfig, INameConfig nameConfig)
        {
            var url = assetConfig.Url();

            var produceName = playerSettingsConfig.CommonSubscribe?.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettingsConfig.CommonSubscribe?.Get(PLAY_SETTINGS.PlatformName);

            var streamingAssetsPath = Application.streamingAssetsPath;
            var persistentDataPath = Application.persistentDataPath;
            var persistentDataProduceNamePlatformName = $"{persistentDataPath}/{produceName}/{platformName}";
            var urlProduceNamePlatformName = $"{url}/{produceName}/{platformName}";

            var streamingAssets = $"{streamingAssetsPath}/{nameConfig.FileNameByVersionConfig()}";
            var persistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.FileNameByVersionConfig()}";
            var persistentDataTemp = $"{persistentDataProduceNamePlatformName}/{nameConfig.FileTempNameByVersionConfig()}";

            var assetServer = $"{urlProduceNamePlatformName}/{nameConfig.FileNameByVersionConfig()}";

            {
                var build = versionPathSubscribe = new CommonSubscribe<VERSION_PATH, string>();
                build.Subscribe(VERSION_PATH.StreamingAssets, streamingAssets);
                build.Subscribe(VERSION_PATH.PersistentData, persistentData);
                build.Subscribe(VERSION_PATH.PersistentDataTemp, persistentDataTemp);
            }

            {
                var build = assetServerVersionPathSubscribe = new CommonSubscribe<ASSET_SERVER_VERSION_PATH, (string serverPath, VERSION_PATH cachePath)>();
                build.Subscribe(ASSET_SERVER_VERSION_PATH.AssetServer, (assetServer, VERSION_PATH.PersistentDataTemp));
            }
        }
    }
}