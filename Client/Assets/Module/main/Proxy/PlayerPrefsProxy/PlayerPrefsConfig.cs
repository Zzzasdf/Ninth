using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class PlayerPrefsConfig: IPlayerPrefsIntConfig, IPlayerPrefsFloatConfig, IPlayerPrefsStringConfig
    {
        private readonly CommonSubscribe<PLAYERPREFS_INT, int> intCommonSubscribe;
        private readonly CommonSubscribe<PLAYERPREFS_FLOAT, float> floatCommonSubscribe;
        private readonly CommonSubscribe<PLAYERPREFS_STRING, string> stringCommonSubscribe;
        
        CommonSubscribe<PLAYERPREFS_INT, int> IPlayerPrefsIntConfig.CommonSubscribe => intCommonSubscribe;
        CommonSubscribe<PLAYERPREFS_FLOAT, float> IPlayerPrefsFloatConfig.CommonSubscribe => floatCommonSubscribe;
        CommonSubscribe<PLAYERPREFS_STRING, string> IPlayerPrefsStringConfig.CommonSubscribe =>stringCommonSubscribe;

        [Inject]
        public PlayerPrefsConfig()
        {
            intCommonSubscribe = new CommonSubscribe<PLAYERPREFS_INT, int>
            {
                [PLAYERPREFS_INT.DownloadBundleStartPos] = 0,
            };

            floatCommonSubscribe = new CommonSubscribe<PLAYERPREFS_FLOAT, float>
            {

            };

            stringCommonSubscribe = new CommonSubscribe<PLAYERPREFS_STRING, string>
            {
                [PLAYERPREFS_STRING.DownloadBundleStartPosFromAssetVersion] = "0.0.0.0",
            };
        }
    }
}
