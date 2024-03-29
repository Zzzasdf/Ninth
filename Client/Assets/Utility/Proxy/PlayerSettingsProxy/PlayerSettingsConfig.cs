using System;
using UnityEngine;
using VContainer;

namespace Ninth.Utility
{
    public class PlayerSettingsConfig : IPlayerSettingsConfig
    {
        private readonly Subscriber<PLAY_SETTINGS, string> stringSubscriber;
        Subscriber<PLAY_SETTINGS, string> IPlayerSettingsConfig.StringSubscriber => stringSubscriber;

        [Inject]
        public PlayerSettingsConfig()
        {
            {
                var build = stringSubscriber = new Subscriber<PLAY_SETTINGS, string>();
                build.Subscribe(PLAY_SETTINGS.ProduceName, Application.productName);
                build.Subscribe(PLAY_SETTINGS.PlatformName, Application.platform switch
                {
                    RuntimePlatform.WindowsPlayer or RuntimePlatform.WindowsEditor => "StandaloneWindows64",
                    RuntimePlatform.Android => "Android",
                    RuntimePlatform.IPhonePlayer => "iOS",
                    _ => throw new ArgumentOutOfRangeException()
                });
            }
        }
    }
}