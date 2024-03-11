using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public interface IPlayerSettingsProxy
    {
        string Get(PLAY_SETTINGS playSettings);
    }
}
