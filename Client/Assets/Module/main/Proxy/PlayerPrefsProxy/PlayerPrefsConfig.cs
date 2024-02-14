using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class PlayerPrefsConfig: IPlayerPrefsIntConfig, IPlayerPrefsFloatConfig, IPlayerPrefsStringConfig
    {
        private readonly ReadOnlyDictionary<PLAYERPREFS_INT, int> intContainer;
        private readonly ReadOnlyDictionary<PLAYERPREFS_FLOAT, float> floatContainer;
        private readonly ReadOnlyDictionary<PLAYERPREFS_STRING, string> stringContainer;

        [Inject]
        public PlayerPrefsConfig()
        {
            intContainer = new ReadOnlyDictionary<PLAYERPREFS_INT, int>(new Dictionary<PLAYERPREFS_INT, int>());
            floatContainer = new ReadOnlyDictionary<PLAYERPREFS_FLOAT, float>(new Dictionary<PLAYERPREFS_FLOAT, float>());
            stringContainer = new ReadOnlyDictionary<PLAYERPREFS_STRING, string>(new Dictionary<PLAYERPREFS_STRING, string>());
            
            Subscribe(PLAYERPREFS_INT.DownloadBundleStartPos, 0);
            Subscribe(PLAYERPREFS_STRING.DownloadBundleStartPosFromAssetVersion, "0.0.0.0");
        }
        
        private void Subscribe(PLAYERPREFS_INT playerPrefsInt, int defaultValue)
        {
            if (!intContainer.TryAdd(playerPrefsInt, defaultValue))
            {
                $"重复订阅 {nameof(PLAYERPREFS_INT)}: {playerPrefsInt}".FrameError();
            }
        }
        private void Subscribe(PLAYERPREFS_FLOAT playerPrefsFloat, float defaultValue)
        {
            if (!floatContainer.TryAdd(playerPrefsFloat, defaultValue))
            {
                $"重复订阅 {nameof(PLAYERPREFS_FLOAT)}: {playerPrefsFloat}".FrameError();
            }
        }
        private void Subscribe(PLAYERPREFS_STRING playerPrefsString, string defaultValue)
        {
            if (!stringContainer.TryAdd(playerPrefsString, defaultValue))
            {
                $"重复订阅 {nameof(PLAYERPREFS_STRING)}: {playerPrefsString}".FrameError();
            }
        }

        int? IPlayerPrefsIntConfig.Get(PLAYERPREFS_INT playerprefsInt)
        {
            if (!intContainer.TryGetValue(playerprefsInt, out var result))
            {
                $"未订阅 {nameof(PLAYERPREFS_INT)}: {playerprefsInt}".FrameError();
                return null;
            }
            return result;
        }

        bool IPlayerPrefsIntConfig.ContainsKey(PLAYERPREFS_INT playerprefsInt)
        {
            return intContainer.ContainsKey(playerprefsInt);
        }

        float? IPlayerPrefsFloatConfig.Get(PLAYERPREFS_FLOAT playerprefsFloat)
        {
            if (!floatContainer.TryGetValue(playerprefsFloat, out var result))
            {
                $"未订阅 {nameof(PLAYERPREFS_FLOAT)}: {playerprefsFloat}".FrameError();
                return null;
            }
            return result;
        }

        bool IPlayerPrefsFloatConfig.ContainsKey(PLAYERPREFS_FLOAT playerprefsFloat)
        {
            return floatContainer.ContainsKey(playerprefsFloat);
        }

        string? IPlayerPrefsStringConfig.Get(PLAYERPREFS_STRING playerprefsString)
        {
            if (!stringContainer.TryGetValue(playerprefsString, out var result))
            {
                $"未订阅 {nameof(PLAYERPREFS_STRING)}: {playerprefsString}".FrameError();
                return null;
            }
            return result;
        }

        bool IPlayerPrefsStringConfig.ContainsKey(PLAYERPREFS_STRING playerprefsString)
        {
            return stringContainer.ContainsKey(playerprefsString);
        }
    }
}
