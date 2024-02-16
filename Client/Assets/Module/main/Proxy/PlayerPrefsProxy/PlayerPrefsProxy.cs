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
        
        int? IPlayerPrefsIntProxy.Get(PLAYERPREFS_INT @int)
        {
            var defaultValue = intConfig.Get(@int);
            if (!defaultValue.HasValue)
            {
                return null;
            }
            return PlayerPrefs.GetInt(((int)@int).ToString(), defaultValue.Value);
        }

        void IPlayerPrefsIntProxy.Set(PLAYERPREFS_INT @int, int value)
        {
            if (!intConfig.ContainsKey(@int))
            {
                $"未订阅 {nameof(PLAYERPREFS_INT)}: {@int}".FrameError();
                return;
            }
            PlayerPrefs.SetInt(((int)@int).ToString(), value);
        }
        
        float? IPlayerPrefsFloatProxy.Get(PLAYERPREFS_FLOAT @float)
        {
            var defaultValue = floatConfig.Get(@float);
            if (!defaultValue.HasValue)
            {
                return null;
            }
            return PlayerPrefs.GetFloat(((int)@float).ToString(), defaultValue.Value);
        }

        void IPlayerPrefsFloatProxy.Set(PLAYERPREFS_FLOAT @float, float value)
        {
            if (!floatConfig.ContainsKey(@float))
            {
                $"未订阅 {nameof(PLAYERPREFS_FLOAT)}: {@float}".FrameError();
                return;
            }
            PlayerPrefs.SetFloat(((int)@float).ToString(), value);
        }
        
        string? IPlayerPrefsStringProxy.Get(PLAYERPREFS_STRING @string)
        {
            var defaultValue = stringConfig.Get(@string);
            if (defaultValue == null)
            {
                return null;
            }
            return PlayerPrefs.GetString(((int)@string).ToString(), defaultValue);
        }

        void IPlayerPrefsStringProxy.Set(PLAYERPREFS_STRING @string, string value)
        {
            if (!stringConfig.ContainsKey(@string))
            {
                $"未订阅 {nameof(PLAYERPREFS_STRING)}: {@string}".FrameError();
                return;
            }
            PlayerPrefs.SetString(((int)@string).ToString(), value);
        }
    }
}
