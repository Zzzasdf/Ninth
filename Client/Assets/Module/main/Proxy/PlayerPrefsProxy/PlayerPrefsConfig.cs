using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth
{
    public class PlayerPrefsConfig: IPlayerPrefsIntConfig, IPlayerPrefsFloatConfig, IPlayerPrefsStringConfig
    {
        private ReadOnlyDictionary<PlayerPrefsInt, int> intMapContainer;
        private ReadOnlyDictionary<PlayerPrefsFloat, float> floatMapContainer;
        private ReadOnlyDictionary<PlayerPrefsString, string> stringMapContainer;

        public PlayerPrefsConfig()
        {
            IntSubscribes();
            FloatSubscribes();
            StringSubscribes();
        }

        private void IntSubscribes()
        {
            var tempIntMapContainer = new Dictionary<PlayerPrefsInt, int>();
            intMapContainer = new ReadOnlyDictionary<PlayerPrefsInt, int>(tempIntMapContainer);
            
            Subscribe(PlayerPrefsInt.DownloadBundleStartPos, 0);
            
            void Subscribe(PlayerPrefsInt playerPrefsInt, int defaultValue)
            {
                if (!intMapContainer.TryAdd(playerPrefsInt, defaultValue))
                {
                    playerPrefsInt.FrameError("重复注册 View: {0}");
                }
            }
        }

        private void FloatSubscribes()
        {
            var tempFloatMapContainer = new Dictionary<PlayerPrefsFloat, float>();
            floatMapContainer = new ReadOnlyDictionary<PlayerPrefsFloat, float>(tempFloatMapContainer);
            
            void Subscribe(PlayerPrefsFloat playerPrefsFloat, float defaultValue)
            {
                if (!floatMapContainer.TryAdd(playerPrefsFloat, defaultValue))
                {
                    playerPrefsFloat.FrameError("重复注册 View: {0}");
                }
            }
        }

        private void StringSubscribes()
        {
            var tempStringMapContainer = new Dictionary<PlayerPrefsString, string>();
            stringMapContainer = new ReadOnlyDictionary<PlayerPrefsString, string>(tempStringMapContainer);
            
            Subscribe(PlayerPrefsString.DownloadBundleStartPosFromAssetVersion, "0.0.0.0");
            
            void Subscribe(PlayerPrefsString playerPrefsString, string defaultValue)
            {
                if (!stringMapContainer.TryAdd(playerPrefsString, defaultValue))
                {
                    playerPrefsString.FrameError("重复注册 View: {0}");
                }
            }
        }

        ReadOnlyDictionary<PlayerPrefsInt, int> IPlayerPrefsIntConfig.MapContainer() => intMapContainer;
        
        ReadOnlyDictionary<PlayerPrefsFloat, float> IPlayerPrefsFloatConfig.MapContainer() => floatMapContainer;

        ReadOnlyDictionary<PlayerPrefsString, string> IPlayerPrefsStringConfig.MapContainer() => stringMapContainer;
    }
}
