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
        SubscriberCollect<string, PLAYERPREFS_STRING> StringSubscriber { get; }
    }
}
