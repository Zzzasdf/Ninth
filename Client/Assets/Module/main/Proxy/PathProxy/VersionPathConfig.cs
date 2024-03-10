using System;
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
        private readonly SubscriberCollect<string, VERSION_PATH> versionPathSubscriber;
        private readonly SubscriberCollect<(string serverPath, VERSION_PATH cachePath), ASSET_SERVER_VERSION_PATH> assetServerVersionPathSubscriber;

        SubscriberCollect<string, VERSION_PATH> IVersionPathConfig.VersionPathSubscriber => versionPathSubscriber;
        SubscriberCollect<(string serverPath, VERSION_PATH cachePath), ASSET_SERVER_VERSION_PATH> IVersionPathConfig.AssetServerVersionPathSubscriber => assetServerVersionPathSubscriber;

        [Inject]
        public VersionPathConfig(IAssetConfig assetConfig, IPlayerSettingsConfig playerSettingsConfig, INameConfig nameConfig)
        {
            var url = assetConfig.Url();

            var produceName = playerSettingsConfig.StringSubscriber?.Get(PLAY_SETTINGS.ProduceName);
            var platformName = playerSettingsConfig.StringSubscriber?.Get(PLAY_SETTINGS.PlatformName);

            var streamingAssetsPath = Application.streamingAssetsPath;
            var persistentDataPath = Application.persistentDataPath;
            var persistentDataProduceNamePlatformName = $"{persistentDataPath}/{produceName}/{platformName}";
            var urlProduceNamePlatformName = $"{url}/{produceName}/{platformName}";

            var streamingAssets = $"{streamingAssetsPath}/{nameConfig.FileNameByVersionConfig()}";
            var persistentData = $"{persistentDataProduceNamePlatformName}/{nameConfig.FileNameByVersionConfig()}";
            var persistentDataTemp = $"{persistentDataProduceNamePlatformName}/{nameConfig.FileTempNameByVersionConfig()}";

            var assetServer = $"{urlProduceNamePlatformName}/{nameConfig.FileNameByVersionConfig()}";

            {
                var build = versionPathSubscriber = new SubscriberCollect<string, VERSION_PATH>();
                build.Subscribe(VERSION_PATH.StreamingAssets, streamingAssets);
                build.Subscribe(VERSION_PATH.PersistentData, persistentData);
                build.Subscribe(VERSION_PATH.PersistentDataTemp, persistentDataTemp);
            }

            {
                var build = assetServerVersionPathSubscriber = new SubscriberCollect<(string serverPath, VERSION_PATH cachePath), ASSET_SERVER_VERSION_PATH>();
                build.Subscribe(ASSET_SERVER_VERSION_PATH.AssetServer, (assetServer, VERSION_PATH.PersistentDataTemp));
            }
        }
    }
}