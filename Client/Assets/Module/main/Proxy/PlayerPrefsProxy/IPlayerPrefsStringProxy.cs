using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public interface IPlayerPrefsStringProxy
    {
        string Get(PlayerPrefsString playerPrefsString);

        void Set(PlayerPrefsString playerPrefsString, string value);
    }
}
