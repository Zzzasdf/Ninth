using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public interface IPlayerPrefsStringProxy
    {
        string Get(PLAYERPREFS_STRING playerprefsString);

        void Set(PLAYERPREFS_STRING playerprefsString, string value);
    }
}
