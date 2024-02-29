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
        private readonly SubscribeCollect<int, PLAYERPREFS_INT> intSubscribe;
        private readonly SubscribeCollect<float, PLAYERPREFS_FLOAT> floatSubscribe;
        private readonly SubscribeCollect<string, PLAYERPREFS_STRING> stringSubscribe;

        SubscribeCollect<int, PLAYERPREFS_INT> IPlayerPrefsIntConfig.IntSubscribe => intSubscribe;
        SubscribeCollect<float, PLAYERPREFS_FLOAT> IPlayerPrefsFloatConfig.FloatSubscribe => floatSubscribe;
        SubscribeCollect<string, PLAYERPREFS_STRING> IPlayerPrefsStringConfig.StringSubscribe => stringSubscribe;

        [Inject]
        public PlayerPrefsConfig()
        {
            {
                var build = intSubscribe = new SubscribeCollect<int, PLAYERPREFS_INT>();
                build.Subscribe(PLAYERPREFS_INT.DownloadBundleStartPos, 0);
            }

            {
                var build = floatSubscribe = new SubscribeCollect<float, PLAYERPREFS_FLOAT>();
            }

            {
                var build = stringSubscribe = new SubscribeCollect<string, PLAYERPREFS_STRING>();
                build.Subscribe(PLAYERPREFS_STRING.DownloadBundleStartPosFromAssetVersion, "0.0.0.0");
            }
        }
    }
}