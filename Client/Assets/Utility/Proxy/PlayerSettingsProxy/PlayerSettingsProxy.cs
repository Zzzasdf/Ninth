using UnityEngine;
using VContainer;

namespace Ninth.Utility
{
    public class PlayerSettingsProxy : IPlayerSettingsProxy
    {
        private readonly IPlayerSettingsConfig playerSettingsConfig;
        
        [Inject]
        public PlayerSettingsProxy(IPlayerSettingsConfig playerSettingsConfig)
        {
            this.playerSettingsConfig = playerSettingsConfig;
        }
        
        string IPlayerSettingsProxy.Get(PLAY_SETTINGS playSettings)
        {
            var result = playerSettingsConfig.StringSubscriber.Get(playSettings);
            if (result == null)
            {
                return string.Empty;
            }
            return result;
        }
    }
}