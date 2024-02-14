using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth
{
    public enum PLAYERPREFS_STRING
    {
        DownloadBundleStartPosFromAssetVersion,
    }
    
    public interface IPlayerPrefsStringConfig
    {
        string? Get(PLAYERPREFS_STRING playerprefsString);
        bool ContainsKey(PLAYERPREFS_STRING playerprefsString);
    }
}
