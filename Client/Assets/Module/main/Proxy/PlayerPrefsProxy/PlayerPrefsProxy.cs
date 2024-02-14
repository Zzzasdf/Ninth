using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Ninth
{
    public class PlayerPrefsProxy : IPlayerPrefsIntProxy, IPlayerPrefsFloatProxy, IPlayerPrefsStringProxy
    {
        private readonly IPlayerPrefsIntConfig intConfig;
        private readonly IPlayerPrefsFloatConfig floatConfig;
        private readonly IPlayerPrefsStringConfig stringConfig;

        [Inject]
        public PlayerPrefsProxy(IPlayerPrefsIntConfig intConfig,
            IPlayerPrefsFloatConfig floatConfig, IPlayerPrefsStringConfig stringConfig)
        {
            this.intConfig = intConfig;
            this.floatConfig = floatConfig;
            this.stringConfig = stringConfig;
        }
        
        int? IPlayerPrefsIntProxy.Get(PLAYERPREFS_INT playerprefsInt)
        {
            var defaultValue = intConfig.Get(playerprefsInt);
            if (!defaultValue.HasValue)
            {
                $"未订阅 {nameof(PLAYERPREFS_INT)}: {playerprefsInt}".FrameError();
                return null;
            }
            return PlayerPrefs.GetInt(((int)playerprefsInt).ToString(), defaultValue.Value);
        }

        void IPlayerPrefsIntProxy.Set(PLAYERPREFS_INT playerprefsInt, int value)
        {
            if (!intConfig.ContainsKey(playerprefsInt))
            {
                $"未订阅 {nameof(PLAYERPREFS_INT)}: {playerprefsInt}".FrameError();
                return;
            }
            PlayerPrefs.SetInt(((int)playerprefsInt).ToString(), value);
        }
        
        float? IPlayerPrefsFloatProxy.Get(PLAYERPREFS_FLOAT playerprefsFloat)
        {
            var defaultValue = floatConfig.Get(playerprefsFloat);
            if (!defaultValue.HasValue)
            {
                $"未订阅 {nameof(PLAYERPREFS_FLOAT)}: {playerprefsFloat}".FrameError();
                return null;
            }
            return PlayerPrefs.GetFloat(((int)playerprefsFloat).ToString(), defaultValue.Value);
        }

        void IPlayerPrefsFloatProxy.Set(PLAYERPREFS_FLOAT playerprefsFloat, float value)
        {
            if (!floatConfig.ContainsKey(playerprefsFloat))
            {
                $"未订阅 {nameof(PLAYERPREFS_FLOAT)}: {playerprefsFloat}".FrameError();
                return;
            }
            PlayerPrefs.SetFloat(((int)playerprefsFloat).ToString(), value);
        }
        
        string? IPlayerPrefsStringProxy.Get(PLAYERPREFS_STRING playerprefsString)
        {
            var defaultValue = stringConfig.Get(playerprefsString);
            if (defaultValue == null)
            {
                $"未订阅 {nameof(PLAYERPREFS_STRING)}: {playerprefsString}".FrameError();
                return null;
            }
            return PlayerPrefs.GetString(((int)playerprefsString).ToString(), defaultValue);
        }

        void IPlayerPrefsStringProxy.Set(PLAYERPREFS_STRING playerprefsString, string value)
        {
            if (!stringConfig.ContainsKey(playerprefsString))
            {
                $"未订阅 {nameof(PLAYERPREFS_STRING)}: {playerprefsString}".FrameError();
                return;
            }
            PlayerPrefs.SetString(((int)playerprefsString).ToString(), value);
        }
    }
}
