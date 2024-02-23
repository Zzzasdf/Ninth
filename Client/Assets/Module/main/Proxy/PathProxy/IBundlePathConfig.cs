using System;
using System.Collections;
using System.Collections.Generic;
using Ninth.Utility;
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
    
    public interface IBundlePathConfig
    {
        CommonSubscribe<BUNDLE_PATH, Func<string, string>> BundlePathSubscribe { get; }
        CommonSubscribe<ASSET_SERVER_BUNDLE_PATH, (Func<string, string, string> serverPath, BUNDLE_PATH cachePath)> AssetServerBundlePathSubscribe { get; }    
    }
}
