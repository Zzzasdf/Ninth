using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class PlayerSettingsConfig: CommonSubscribe<PLAY_SETTINGS, string?>, IPlayerSettingsConfig
    {
        private readonly CommonSubscribe<PLAY_SETTINGS, string> commonSubscribe;
        CommonSubscribe<PLAY_SETTINGS, string> IPlayerSettingsConfig.CommonSubscribe => commonSubscribe;
        
        [Inject]
        public PlayerSettingsConfig()
        {
            commonSubscribe = new CommonSubscribe<PLAY_SETTINGS, string>
            {
                [PLAY_SETTINGS.ProduceName] = Application.productName,
                [PLAY_SETTINGS.PlatformName] = Application.platform switch
                {
                    RuntimePlatform.Android => "Android",
                    RuntimePlatform.IPhonePlayer => "iOS",
                    _ => "StandaloneWindows64"
                },
            };
        }
    }
}
