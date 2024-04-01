using Ninth.Utility;
using VContainer;

namespace Ninth
{
    public class PlayerPrefsConfig : IPlayerPrefsIntConfig, IPlayerPrefsFloatConfig, IPlayerPrefsStringConfig
    {
        private readonly Subscriber<PLAYERPREFS_INT, int> intSubscriber;
        private readonly Subscriber<PLAYERPREFS_FLOAT, float> floatSubscriber;
        private readonly Subscriber<PLAYERPREFS_STRING, string> stringSubscriber;

        Subscriber<PLAYERPREFS_INT, int> IPlayerPrefsIntConfig.IntSubscriber => intSubscriber;
        Subscriber<PLAYERPREFS_FLOAT, float> IPlayerPrefsFloatConfig.FloatSubscriber => floatSubscriber;
        Subscriber<PLAYERPREFS_STRING, string> IPlayerPrefsStringConfig.StringSubscriber => stringSubscriber;

        [Inject]
        public PlayerPrefsConfig()
        {
            {
                var build = intSubscriber = new Subscriber<PLAYERPREFS_INT, int>();
                build.Subscribe(PLAYERPREFS_INT.DownloadBundleStartPos, 0);
            }

            {
                var build = floatSubscriber = new Subscriber<PLAYERPREFS_FLOAT, float>();
            }

            {
                var build = stringSubscriber = new Subscriber<PLAYERPREFS_STRING, string>();
                build.Subscribe(PLAYERPREFS_STRING.DownloadBundleStartPosFromAssetVersion, "0.0.0.0");
            }
        }
    }
}