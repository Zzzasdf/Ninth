using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class PlayerSettings: IPlayerSettings
    {
        private readonly ReadOnlyDictionary<PLAY_SETTINGS, string> playerSettingsContainer;

        [Inject]
        public PlayerSettings()
        {
            playerSettingsContainer = new ReadOnlyDictionary<PLAY_SETTINGS, string>(new Dictionary<PLAY_SETTINGS, string>());
            
            Subscribe(PLAY_SETTINGS.ProduceName, Application.productName);
            Subscribe(PLAY_SETTINGS.PlatformName, Application.platform switch
            {
                RuntimePlatform.Android => "Android",
                RuntimePlatform.IPhonePlayer => "iOS",
                _ => "StandaloneWindows64"
            });
        }
        
        private void Subscribe(PLAY_SETTINGS playSettingsInfo, string info)
        {
            if (!playerSettingsContainer.TryAdd(playSettingsInfo, info))
            {
                $"重复订阅 {nameof(PLAY_SETTINGS)}: {playSettingsInfo}".FrameError();
            }
        }
        
        string? IPlayerSettings.Get(PLAY_SETTINGS playSettings)
        {
            if (!playerSettingsContainer.TryGetValue(playSettings, out var result))
            {
                $"未订阅 {nameof(PLAY_SETTINGS)} {playSettings}".FrameError();
                return null;
            }
            return result;
        }
    }
}
