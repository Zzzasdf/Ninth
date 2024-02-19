using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class PlayerSettingsConfig: BaseSubscribe<PLAY_SETTINGS, string?>, IPlayerSettingsConfig
    {
        private readonly BaseSubscribe<PLAY_SETTINGS, string> playSettingSubscribe;
        
        [Inject]
        public PlayerSettingsConfig()
        {
            playSettingSubscribe = new BaseSubscribe<PLAY_SETTINGS, string>
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
        
        string? IPlayerSettingsConfig.Get(PLAY_SETTINGS playSettings)
        {
            return Get(playSettings);
        }
    }
}
