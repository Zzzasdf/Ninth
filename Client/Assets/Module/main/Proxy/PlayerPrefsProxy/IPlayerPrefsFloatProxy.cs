using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public interface IPlayerPrefsFloatProxy
    {
        float? Get(PLAYERPREFS_FLOAT playerprefsFloat);
        void Set(PLAYERPREFS_FLOAT playerprefsFloat, float value);
    }
}
