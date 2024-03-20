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
        SubscriberCollect<string, CONFIG_PATH> ConfigPathSubscriber { get; }
        SubscriberCollect<(Func<string, string> serverPath, CONFIG_PATH cachePath), ASSET_SERVER_CONFIG_PATH> AssetServerConfigPathSubscriber { get; }
    }
}