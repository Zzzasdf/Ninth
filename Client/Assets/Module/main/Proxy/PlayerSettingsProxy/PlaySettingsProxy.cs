using VContainer;

namespace Ninth
{
    public class PlaySettingsProxy : IPlaySettingsProxy
    {
        private readonly IPlayerSettings playerSettings;
        
        [Inject]
        public PlaySettingsProxy(IPlayerSettings playerSettings)
        {
            this.playerSettings = playerSettings;
        }
        
        string? IPlaySettingsProxy.Get(PLAY_SETTINGS playSettings)
        {
            return playerSettings.Get(playSettings);
        }
    }
}