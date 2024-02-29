using System;
using System.Collections;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEngine;

namespace Ninth
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
        SubscribeCollect<string, VERSION_PATH> VersionPathSubscribe { get; }
        SubscribeCollect<(string serverPath, VERSION_PATH cachePath), ASSET_SERVER_VERSION_PATH> AssetServerVersionPathSubscribe { get; }
    }
}
