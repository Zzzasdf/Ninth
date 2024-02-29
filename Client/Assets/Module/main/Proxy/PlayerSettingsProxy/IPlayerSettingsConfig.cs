using System.Collections;
using System.Collections.Generic;
using Ninth.Utility;
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
        SubscribeCollect<string, PLAY_SETTINGS> StringSubscribe { get; }
    }
}