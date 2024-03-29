namespace Ninth.Utility
{
    public interface IPathProxy
    {
        string Get(VERSION_PATH versionPath);
        (string assetServerVersionPath, VERSION_PATH versionPersistentDataTempPath) Get(ASSET_SERVER_VERSION_PATH versionPath);
        string Get(CONFIG_PATH configPath);
        (string assetServerConfigPath, CONFIG_PATH configPersistentDataTempPath) Get(ASSET_SERVER_CONFIG_PATH configPath, string version);
        string Get(BUNDLE_PATH bundlePath, string bundleName);
        (string assetServerBundlePath, BUNDLE_PATH bundlePersistentDataTempPath) Get(ASSET_SERVER_BUNDLE_PATH bundlePath, string version, string bundleName);
    }
}
