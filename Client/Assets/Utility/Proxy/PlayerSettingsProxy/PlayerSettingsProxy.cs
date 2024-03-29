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
            return playerSettingsConfig.StringSubscriber.GetValue(playSettings);
        }
    }
}