using System;
using UnityEngine;
using VContainer;

namespace Ninth.Utility
{
    public class PlayerSettingsConfig : IPlayerSettingsConfig
    {
        private readonly SubscriberCollect<string, PLAY_SETTINGS> stringSubscriber;
        SubscriberCollect<string, PLAY_SETTINGS> IPlayerSettingsConfig.StringSubscriber => stringSubscriber;

        [Inject]
        public PlayerSettingsConfig()
        {
            {
                var build = stringSubscriber = new SubscriberCollect<string, PLAY_SETTINGS>();
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