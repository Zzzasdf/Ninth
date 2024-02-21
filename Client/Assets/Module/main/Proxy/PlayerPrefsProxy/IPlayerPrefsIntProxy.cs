using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public interface IPlayerPrefsIntProxy
    {
        int Get(PLAYERPREFS_INT playerprefsInt);

        void Set(PLAYERPREFS_INT playerprefsInt, int value);
    }
}
