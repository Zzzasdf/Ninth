using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public interface IPlayerPrefsIntProxy
    {
        int Get(PlayerPrefsInt playerPrefsInt);

        void Set(PlayerPrefsInt playerPrefsInt, int value);
    }
}
