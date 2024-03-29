using System;

namespace Ninth.Utility
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
        Subscriber<CONFIG_PATH, string> ConfigPathSubscriber { get; }
        Subscriber<ASSET_SERVER_CONFIG_PATH, (Func<string, string> serverPath, CONFIG_PATH cachePath)> AssetServerConfigPathSubscriber { get; }
    }
}