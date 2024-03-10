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
        SubscriberCollect<Func<string, string>, BUNDLE_PATH> BundlePathSubscriber { get; }
        SubscriberCollect<(Func<string, string, string> serverPath, BUNDLE_PATH cachePath), ASSET_SERVER_BUNDLE_PATH> AssetServerBundlePathSubscriber { get; }    
    }
}
