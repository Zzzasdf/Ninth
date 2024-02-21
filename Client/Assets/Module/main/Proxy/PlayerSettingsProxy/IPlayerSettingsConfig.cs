using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public enum PLAY_SETTINGS
    {
        ProduceName,
        PlatformName,
    }

    public interface IPlayerSettingsConfig
    {
        CommonSubscribe<PLAY_SETTINGS, string> CommonSubscribe { get; }
    }
}