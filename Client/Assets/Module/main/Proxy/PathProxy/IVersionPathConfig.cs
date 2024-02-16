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

    public interface a
    {
        
    }

    public enum ASSET_SERVER_VERSION_PATH
    {
        AssetServer,
    }
    
    
    public interface IVersionPathConfig
    {
        string? Get(VERSION_PATH versionPath);
        string? Get(ASSET_SERVER_VERSION_PATH assetServerVersionPath);
    }
}
