using System;

namespace Ninth.Utility
{
    public enum BUNDLE_PATH
    {
        BundlePathByLocalGroupByStreamingAssets,
        BundlePathByRemoteGroupByStreamingAssets,
        BundlePathByDllGroupByStreamingAssets,
        
        BundlePathByRemoteGroupByPersistentData,
        BundlePathByDllGroupByPersistentData,
    }

    public enum ASSET_SERVER_BUNDLE_PATH
    {
        BundlePathByRemoteGroup,
        BundlePathByDllGroup,
    }
    
    public interface IBundlePathConfig
    {
        Subscriber<BUNDLE_PATH, Func<string, string>> BundlePathSubscriber { get; }
        Subscriber<ASSET_SERVER_BUNDLE_PATH, (Func<string, string, string> serverPath, BUNDLE_PATH cachePath)> AssetServerBundlePathSubscriber { get; }    
    }
}
