using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth
{
    public enum PLAYERPREFS_FLOAT
    {
        
    }
    
    public interface IPlayerPrefsFloatConfig
    {
        float? Get(PLAYERPREFS_FLOAT playerprefsFloat);
        bool ContainsKey(PLAYERPREFS_FLOAT playerprefsFloat);
    }
}
