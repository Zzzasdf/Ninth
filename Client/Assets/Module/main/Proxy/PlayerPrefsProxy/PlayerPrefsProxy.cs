using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class PlayerPrefsProxy : IPlayerPrefsIntProxy, IPlayerPrefsFloatProxy, IPlayerPrefsStringProxy
    {
        private readonly ReadOnlyDictionary<PlayerPrefsInt, int> intMapContainer;
        private readonly ReadOnlyDictionary<PlayerPrefsFloat, float> floatMapContainer;
        private readonly ReadOnlyDictionary<PlayerPrefsString, string> stringMapContainer;

        [Inject]
        public PlayerPrefsProxy(IPlayerPrefsIntConfig playerPrefsIntConfig,
            IPlayerPrefsFloatConfig playerPrefsFloatConfig, IPlayerPrefsStringConfig playerPrefsStringConfig)
        {
            this.intMapContainer = playerPrefsIntConfig.MapContainer();
            this.floatMapContainer = playerPrefsFloatConfig.MapContainer();
            this.stringMapContainer = playerPrefsStringConfig.MapContainer();
        }
        
        int IPlayerPrefsIntProxy.Get(PlayerPrefsInt playerPrefsInt)
        {
            if (!intMapContainer.TryGetValue(playerPrefsInt, out var result))
            {
                playerPrefsInt.FrameError("未注册, 请先注册该字段 {0}");
                return 0;
            }
            return PlayerPrefs.GetInt(((int)playerPrefsInt).ToString(), result);
        }

        void IPlayerPrefsIntProxy.Set(PlayerPrefsInt playerPrefsInt, int value)
        {
            if (!intMapContainer.ContainsKey(playerPrefsInt))
            {
                playerPrefsInt.FrameError("未注册, 请先注册该字段 {0}");
                return;
            }
            PlayerPrefs.SetInt(((int)playerPrefsInt).ToString(), value);
        }
        
        float IPlayerPrefsFloatProxy.Get(PlayerPrefsFloat playerPrefsFloat)
        {
            if (!floatMapContainer.TryGetValue(playerPrefsFloat, out var result))
            {
                playerPrefsFloat.FrameError("未注册, 请先注册该字段 {0}");
                return 0f;
            }
            return PlayerPrefs.GetFloat(((int)playerPrefsFloat).ToString(), result);
        }

        void IPlayerPrefsFloatProxy.Set(PlayerPrefsFloat playerPrefsFloat, float value)
        {
            if (!floatMapContainer.ContainsKey(playerPrefsFloat))
            {
                playerPrefsFloat.FrameError("未注册, 请先注册该字段 {0}");
                return;
            }
            PlayerPrefs.SetFloat(((int)playerPrefsFloat).ToString(), value);
        }
        
        string IPlayerPrefsStringProxy.Get(PlayerPrefsString playerPrefsString)
        {
            if (!stringMapContainer.TryGetValue(playerPrefsString, out var result))
            {
                playerPrefsString.FrameError("未注册, 请先注册该字段 {0}");
                return string.Empty;
            }
            return PlayerPrefs.GetString(((int)playerPrefsString).ToString(), result);
        }

        void IPlayerPrefsStringProxy.Set(PlayerPrefsString playerPrefsString, string value)
        {
            if (!stringMapContainer.ContainsKey(playerPrefsString))
            {
                playerPrefsString.FrameError("未注册, 请先注册该字段 {0}");
                return;
            }
            PlayerPrefs.SetString(((int)playerPrefsString).ToString(), value);
        }
    }
}
