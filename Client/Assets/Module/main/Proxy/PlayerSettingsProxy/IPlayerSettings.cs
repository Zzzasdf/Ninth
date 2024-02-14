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

    public interface IPlayerSettings
    {
        string? Get(PLAY_SETTINGS playSettings);
    }
}