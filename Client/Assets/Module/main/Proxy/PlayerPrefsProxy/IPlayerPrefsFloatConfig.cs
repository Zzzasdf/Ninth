using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;

namespace Ninth
{
    public enum PLAYERPREFS_FLOAT
    {
        
    }
    
    public interface IPlayerPrefsFloatConfig
    {
        CommonSubscribe<PLAYERPREFS_FLOAT, float> CommonSubscribe { get; }
    }
}
