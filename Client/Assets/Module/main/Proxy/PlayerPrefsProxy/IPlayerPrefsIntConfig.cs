using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth
{
    public enum PLAYERPREFS_INT
    {
        DownloadBundleStartPos,
    }
    
    public interface IPlayerPrefsIntConfig
    {
        int? Get(PLAYERPREFS_INT playerprefsInt);
        
        bool ContainsKey(PLAYERPREFS_INT playerprefsInt);
    }
}
