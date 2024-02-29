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
        
        int IPlayerPrefsIntProxy.Get(PLAYERPREFS_INT @int)
        {
            var defaultValue = intConfig.IntSubscribe.Get(@int);
            return PlayerPrefs.GetInt(((int)@int).ToString(), defaultValue);
        }

        void IPlayerPrefsIntProxy.Set(PLAYERPREFS_INT @int, int value)
        {
            if (!intConfig.IntSubscribe.ContainsKey(@int))
            {
                $"未订阅 {nameof(PLAYERPREFS_INT)}".FrameError();
                return;
            }
            PlayerPrefs.SetInt(((int)@int).ToString(), value);
        }
        
        float IPlayerPrefsFloatProxy.Get(PLAYERPREFS_FLOAT @float)
        {
            var defaultValue = floatConfig.FloatSubscribe.Get(@float);
            return PlayerPrefs.GetFloat(((int)@float).ToString(), defaultValue);
        }

        void IPlayerPrefsFloatProxy.Set(PLAYERPREFS_FLOAT @float, float value)
        {
            if (!floatConfig.FloatSubscribe.ContainsKey(@float))
            {
                $"未订阅 {nameof(PLAYERPREFS_FLOAT)}".FrameError();
                return;
            }
            PlayerPrefs.SetFloat(((int)@float).ToString(), value);
        }
        
        string IPlayerPrefsStringProxy.Get(PLAYERPREFS_STRING @string)
        {
            var defaultValue = stringConfig.StringSubscribe.Get(@string);
            return PlayerPrefs.GetString(((int)@string).ToString(), defaultValue);
        }

        void IPlayerPrefsStringProxy.Set(PLAYERPREFS_STRING @string, string value)
        {
            if (!stringConfig.StringSubscribe.ContainsKey(@string))
            {
                $"未订阅 {nameof(@string)}".FrameError();
                return;
            }
            PlayerPrefs.SetString(((int)@string).ToString(), value);
        }
    }
}
