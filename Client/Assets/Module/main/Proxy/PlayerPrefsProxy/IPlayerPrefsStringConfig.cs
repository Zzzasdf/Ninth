using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth
{
    public enum PlayerPrefsString
    {
        DownloadBundleStartPosFromAssetVersion,
    }
    
    public interface IPlayerPrefsStringConfig
    {
        ReadOnlyDictionary<PlayerPrefsString, string> MapContainer();
    }
}
