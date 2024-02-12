using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public interface IPlayerPrefsFloatProxy
    {
        float Get(PlayerPrefsFloat playerPrefsFloat);

        void Set(PlayerPrefsFloat playerPrefsFloat, float value);
    }
}
