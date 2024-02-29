using UnityEngine;
using VContainer;

namespace Ninth
{
    public class PlaySettingsProxy : IPlaySettingsProxy
    {
        private readonly IPlayerSettingsConfig playerSettingsConfig;
        
        [Inject]
        public PlaySettingsProxy(IPlayerSettingsConfig playerSettingsConfig)
        {
            this.playerSettingsConfig = playerSettingsConfig;
        }
        
        string IPlaySettingsProxy.Get(PLAY_SETTINGS playSettings)
        {
            var result = playerSettingsConfig.StringSubscribe.Get(playSettings);
            if (result == null)
            {
                return string.Empty;
            }
            return result;
        }
    }
}