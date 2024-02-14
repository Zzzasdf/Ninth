using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public interface IPlaySettingsProxy
    {
        string? Get(PLAY_SETTINGS playSettings);
    }
}
