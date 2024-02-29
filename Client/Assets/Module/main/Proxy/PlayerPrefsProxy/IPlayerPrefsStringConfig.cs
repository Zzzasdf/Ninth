using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ninth.Utility;
using UnityEngine;

namespace Ninth
{
    public enum PLAYERPREFS_STRING
    {
        DownloadBundleStartPosFromAssetVersion,
    }
    
    public interface IPlayerPrefsStringConfig
    {
        SubscribeCollect<string, PLAYERPREFS_STRING> StringSubscribe { get; }
    }
}
