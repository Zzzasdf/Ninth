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
        
        string? IPlaySettingsProxy.Get(PLAY_SETTINGS playSettings)
        {
            return playerSettingsConfig.Get(playSettings);
        }
    }
}