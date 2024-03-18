using System;
using System.Collections;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEngine;

namespace Ninth.Utility
{
    public enum VERSION_PATH
    {
        StreamingAssets,
        PersistentData,
        PersistentDataTemp,
    }

    public enum ASSET_SERVER_VERSION_PATH
    {
        AssetServer,
    }
    
    public interface IVersionPathConfig
    {
        SubscriberCollect<string, VERSION_PATH> VersionPathSubscriber { get; }
        SubscriberCollect<(string serverPath, VERSION_PATH cachePath), ASSET_SERVER_VERSION_PATH> AssetServerVersionPathSubscriber { get; }
    }
}
