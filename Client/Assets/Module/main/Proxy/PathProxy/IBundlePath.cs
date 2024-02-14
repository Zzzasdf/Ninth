using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
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
    
    public interface IBundlePath
    {
        string? Get(BUNDLE_PATH bundlePath, string bundleName);

        string? Get(ASSET_SERVER_BUNDLE_PATH bundlePath, string version, string bundleName);
    }
}
