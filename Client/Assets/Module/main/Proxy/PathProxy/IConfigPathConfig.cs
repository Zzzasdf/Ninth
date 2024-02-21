using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public enum CONFIG_PATH
    {
        LoadConfigPathByLocalGroupByStreamingAssets,
        LoadConfigPathByRemoteGroupByStreamingAssets,
        
        DownloadConfigPathByRemoteGroupByPersistentData,
        DownloadConfigTempPathByRemoteGroupByPersistentData,
        DownloadConfigPathByDllGroupByPersistentData,
        DownloadConfigTempPathByDllGroupByPersistentData,
        LoadConfigPathByRemoteGroupByPersistentData,
        LoadConfigPathByDllGroupByPersistentData,
    }

    public enum ASSET_SERVER_CONFIG_PATH
    {
        DownloadConfigPathByRemoteGroup,
        DownloadConfigPathByDllGroup,
        LoadConfigPathByRemoteGroup,
        LoadConfigPathByDllGroup,
    }
    
    public interface IConfigPathConfig
    {
        CommonSubscribe<CONFIG_PATH, string> ConfigPathSubscribe { get; }
        CommonSubscribe<ASSET_SERVER_CONFIG_PATH, (Func<string, string> serverPath, CONFIG_PATH cachePath)> AssetServerConfigPathSubscribe { get; }
    }
}