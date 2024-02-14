using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public interface IPlayerPrefsIntProxy
    {
        int? Get(PLAYERPREFS_INT playerprefsINT);

        void Set(PLAYERPREFS_INT playerprefsINT, int value);
    }
}
