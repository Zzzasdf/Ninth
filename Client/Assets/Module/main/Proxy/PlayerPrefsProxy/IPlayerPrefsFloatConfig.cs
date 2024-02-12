using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth
{
    public enum PlayerPrefsFloat
    {
        
    }
    
    public interface IPlayerPrefsFloatConfig
    {
        ReadOnlyDictionary<PlayerPrefsFloat, float> MapContainer();
    }
}
