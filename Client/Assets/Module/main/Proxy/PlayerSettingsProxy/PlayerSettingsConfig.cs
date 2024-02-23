using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class PlayerSettingsConfig : IPlayerSettingsConfig
    {
        private readonly CommonSubscribe<PLAY_SETTINGS, string> commonSubscribe;
        CommonSubscribe<PLAY_SETTINGS, string> IPlayerSettingsConfig.CommonSubscribe => commonSubscribe;

        [Inject]
        public PlayerSettingsConfig()
        {
            {
                var build = commonSubscribe = new CommonSubscribe<PLAY_SETTINGS, string>();
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