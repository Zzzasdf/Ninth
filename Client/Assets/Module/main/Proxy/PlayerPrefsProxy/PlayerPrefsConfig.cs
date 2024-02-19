using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class PlayerPrefsConfig: IPlayerPrefsIntConfig, IPlayerPrefsFloatConfig, IPlayerPrefsStringConfig
    {
        private readonly CommonSubscribe<PLAYERPREFS_INT, int?> intPlayerPrefsSubscribe;
        private readonly CommonSubscribe<PLAYERPREFS_FLOAT, float?> floatPlayerPrefsSubscribe;
        private readonly CommonSubscribe<PLAYERPREFS_STRING, string?> stringPlayerPrefsSubscribe;
        
        [Inject]
        public PlayerPrefsConfig()
        {
            intPlayerPrefsSubscribe = new CommonSubscribe<PLAYERPREFS_INT, int?>
            {
                [PLAYERPREFS_INT.DownloadBundleStartPos] = 0,
            };

            floatPlayerPrefsSubscribe = new CommonSubscribe<PLAYERPREFS_FLOAT, float?>
            {

            };

            stringPlayerPrefsSubscribe = new CommonSubscribe<PLAYERPREFS_STRING, string?>
            {
                [PLAYERPREFS_STRING.DownloadBundleStartPosFromAssetVersion] = "0.0.0.0",
            };
        }
        
        int? IPlayerPrefsIntConfig.Get(PLAYERPREFS_INT @int)
        {
            return intPlayerPrefsSubscribe.Get(@int);
        }

        bool IPlayerPrefsIntConfig.ContainsKey(PLAYERPREFS_INT @int)
        {
            return intPlayerPrefsSubscribe.ContainsKey(@int);
        }

        float? IPlayerPrefsFloatConfig.Get(PLAYERPREFS_FLOAT @float)
        {
            return floatPlayerPrefsSubscribe.Get(@float);
        }

        bool IPlayerPrefsFloatConfig.ContainsKey(PLAYERPREFS_FLOAT @float)
        {
            return floatPlayerPrefsSubscribe.ContainsKey(@float);
        }

        string? IPlayerPrefsStringConfig.Get(PLAYERPREFS_STRING @string)
        {
            return stringPlayerPrefsSubscribe.Get(@string);
        }

        bool IPlayerPrefsStringConfig.ContainsKey(PLAYERPREFS_STRING @string)
        {
            return stringPlayerPrefsSubscribe.ContainsKey(@string);
        }
    }
}
