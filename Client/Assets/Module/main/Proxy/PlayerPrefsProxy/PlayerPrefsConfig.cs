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
        private readonly CommonSubscribe<PLAYERPREFS_INT, int> intCommonSubscribe;
        private readonly CommonSubscribe<PLAYERPREFS_FLOAT, float> floatCommonSubscribe;
        private readonly CommonSubscribe<PLAYERPREFS_STRING, string> stringCommonSubscribe;

        CommonSubscribe<PLAYERPREFS_INT, int> IPlayerPrefsIntConfig.CommonSubscribe => intCommonSubscribe;
        CommonSubscribe<PLAYERPREFS_FLOAT, float> IPlayerPrefsFloatConfig.CommonSubscribe => floatCommonSubscribe;
        CommonSubscribe<PLAYERPREFS_STRING, string> IPlayerPrefsStringConfig.CommonSubscribe => stringCommonSubscribe;

        [Inject]
        public PlayerPrefsConfig()
        {
            {
                var build = intCommonSubscribe = new CommonSubscribe<PLAYERPREFS_INT, int>();
                build.Subscribe(PLAYERPREFS_INT.DownloadBundleStartPos, 0);
            }

            {
                var build = floatCommonSubscribe = new CommonSubscribe<PLAYERPREFS_FLOAT, float>();
            }

            {
                var build = stringCommonSubscribe = new CommonSubscribe<PLAYERPREFS_STRING, string>();
                build.Subscribe(PLAYERPREFS_STRING.DownloadBundleStartPosFromAssetVersion, "0.0.0.0");
            }
        }
    }
}