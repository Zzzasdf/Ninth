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
        Subscriber<VERSION_PATH, string> VersionPathSubscriber { get; }
        Subscriber<ASSET_SERVER_VERSION_PATH, (string serverPath, VERSION_PATH cachePath)> AssetServerVersionPathSubscriber { get; }
    }
}
