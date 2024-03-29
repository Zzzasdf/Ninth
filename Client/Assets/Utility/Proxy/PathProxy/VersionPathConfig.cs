using UnityEngine;
using VContainer;

namespace Ninth.Utility
{
    public class VersionPathConfig : IVersionPathConfig
    {
        private readonly Subscriber<VERSION_PATH, string> versionPathSubscriber;
        private readonly Subscriber<ASSET_SERVER_VERSION_PATH, (string serverPath, VERSION_PATH cachePath)> assetServerVersionPathSubscriber;

        Subscriber<VERSION_PATH, string> IVersionPathConfig.VersionPathSubscriber => versionPathSubscriber;
        Subscriber<ASSET_SERVER_VERSION_PATH, (string serverPath, VERSION_PATH cachePath)> IVersionPathConfig.AssetServerVersionPathSubscriber => assetServerVersionPathSubscriber;

        [Inject]
        public VersionPathConfig(PlayerVersionConfig playerVersionConfig, IPlayerSettingsConfig playerSettingsConfig, INameConfig nameConfig)
        {
            var url = playerVersionConfig.Url;
            var produceName = playerSettingsConfig.StringSubscriber.GetValue(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettingsConfig.StringSubscriber.GetValue(PLAY_SETTINGS.PlatformName);

            var streamingAssetsPath = Application.streamingAssetsPath;
            var persistentDataPath = Application.persistentDataPath;
            var streamingAssets = $"{streamingAssetsPath}/{nameConfig.FileNameByVersionConfig()}";
            var persistentData = $"{persistentDataPath}/{nameConfig.FileNameByVersionConfig()}";
            var persistentDataTemp = $"{persistentDataPath}/{nameConfig.FileTempNameByVersionConfig()}";

            var urlProduceNamePlatformName = $"{url}/{produceName}/{platformName}";
            var assetServer = $"{urlProduceNamePlatformName}/{nameConfig.FileNameByVersionConfig()}";

            {
                var build = versionPathSubscriber = new Subscriber<VERSION_PATH, string>();
                build.Subscribe(VERSION_PATH.StreamingAssets, streamingAssets);
                build.Subscribe(VERSION_PATH.PersistentData, persistentData);
                build.Subscribe(VERSION_PATH.PersistentDataTemp, persistentDataTemp);
            }

            {
                var build = assetServerVersionPathSubscriber = new Subscriber<ASSET_SERVER_VERSION_PATH, (string serverPath, VERSION_PATH cachePath)>();
                build.Subscribe(ASSET_SERVER_VERSION_PATH.AssetServer, (assetServer, VERSION_PATH.PersistentDataTemp));
            }
        }
    }
}