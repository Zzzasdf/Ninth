using System.Collections;
using System.Collections.Generic;
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
        CommonSubscribe<VERSION_PATH, string> VersionPathSubscribe { get; }
        CommonSubscribe<ASSET_SERVER_VERSION_PATH, (string serverPath, VERSION_PATH cachePath)> AssetServerVersionPathSubscribe { get; }
    }
}
