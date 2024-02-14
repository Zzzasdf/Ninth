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
    
    public interface IConfigPath
    {
        string? Get(CONFIG_PATH configPath);

        string? Get(ASSET_SERVER_CONFIG_PATH configPath, string version);
    }
}