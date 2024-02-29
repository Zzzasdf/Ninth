using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class PlayerSettingsConfig : IPlayerSettingsConfig
    {
        private readonly SubscribeCollect<string, PLAY_SETTINGS> stringSubscribe;
        SubscribeCollect<string, PLAY_SETTINGS> IPlayerSettingsConfig.StringSubscribe => stringSubscribe;

        [Inject]
        public PlayerSettingsConfig()
        {
            {
                var build = stringSubscribe = new SubscribeCollect<string, PLAY_SETTINGS>();
                build.Subscribe(PLAY_SETTINGS.ProduceName, Application.productName);
                build.Subscribe(PLAY_SETTINGS.PlatformName, Application.platform switch
                {
                    RuntimePlatform.Android => "Android",
                    RuntimePlatform.IPhonePlayer => "iOS",
                    _ => "StandaloneWindows64"
                });
            }
        }
    }
}