using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth
{
    public enum PlayerPrefsInt
    {
        DownloadBundleStartPos,
    }
    
    public interface IPlayerPrefsIntConfig
    {
        public ReadOnlyDictionary<PlayerPrefsInt, int> MapContainer();
    }
}
