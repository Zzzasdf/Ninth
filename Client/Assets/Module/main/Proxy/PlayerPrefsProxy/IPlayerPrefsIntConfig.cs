using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;

namespace Ninth
{
    public enum PLAYERPREFS_INT
    {
        DownloadBundleStartPos,
    }
    
    public interface IPlayerPrefsIntConfig
    {
        SubscribeCollect<int, PLAYERPREFS_INT> IntSubscribe { get; }
    }
}
