using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class PlayerPrefsConfig : IPlayerPrefsIntConfig, IPlayerPrefsFloatConfig, IPlayerPrefsStringConfig
    {
        private readonly SubscriberCollect<int, PLAYERPREFS_INT> intSubscriber;
        private readonly SubscriberCollect<float, PLAYERPREFS_FLOAT> floatSubscriber;
        private readonly SubscriberCollect<string, PLAYERPREFS_STRING> stringSubscriber;

        SubscriberCollect<int, PLAYERPREFS_INT> IPlayerPrefsIntConfig.IntSubscriber => intSubscriber;
        SubscriberCollect<float, PLAYERPREFS_FLOAT> IPlayerPrefsFloatConfig.FloatSubscriber => floatSubscriber;
        SubscriberCollect<string, PLAYERPREFS_STRING> IPlayerPrefsStringConfig.StringSubscriber => stringSubscriber;

        [Inject]
        public PlayerPrefsConfig()
        {
            {
                var build = intSubscriber = new SubscriberCollect<int, PLAYERPREFS_INT>();
                build.Subscribe(PLAYERPREFS_INT.DownloadBundleStartPos, 0);
            }

            {
                var build = floatSubscriber = new SubscriberCollect<float, PLAYERPREFS_FLOAT>();
            }

            {
                var build = stringSubscriber = new SubscriberCollect<string, PLAYERPREFS_STRING>();
                build.Subscribe(PLAYERPREFS_STRING.DownloadBundleStartPosFromAssetVersion, "0.0.0.0");
            }
        }
    }
}