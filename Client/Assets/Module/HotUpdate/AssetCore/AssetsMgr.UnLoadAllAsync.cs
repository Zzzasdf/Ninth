using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public partial class AssetsMgr
    {
        public void UnLoadAllAsync()
        {
            List<string> removeBundleRef = null;
            foreach(var item in m_BundlePath2BundleRef)
            {
                BundleRef bundleRef = item.Value;

                if(bundleRef == null)
                {
                    continue;
                }

                List<AssetRef> beAssetRefDependedList = bundleRef.BeAssetRefDependedList;

                if (bundleRef.BeAssetRefDependedList == null)
                {
                    continue;
                }
                for (int index = beAssetRefDependedList.Count - 1; index >= 0; index--)
                {
                    AssetRef assetRef = beAssetRefDependedList[index];

                    if (assetRef.AssetStatus != AssetStatus.Loaded)
                    {
                        continue;
                    }
                    List<GameObject> beGameObjectDependedList = assetRef.BeGameObjectDependedList;

                    if (beGameObjectDependedList != null)
                    {
                        for (int i = beGameObjectDependedList.Count - 1; i >= 0; i--)
                        {
                            GameObject go = beGameObjectDependedList[i];
                            if (go == null)
                            {
                                beGameObjectDependedList.RemoveAt(i);
                            }
                        }
                    }
                    if (beGameObjectDependedList == null || beGameObjectDependedList.Count == 0)
                    {
                        assetRef.Asset = null;
                        beAssetRefDependedList.RemoveAt(index);
                    }
                }
                Resources.UnloadUnusedAssets();

                if (beAssetRefDependedList.Count == 0)
                {
                    bundleRef.Bundle.Unload(true);

                    if (removeBundleRef == null)
                    {
                        removeBundleRef = new List<string>();
                    }
                    removeBundleRef.Add(item.Key);
                }
            }
            if (removeBundleRef != null)
            {
                for (int index = 0; index < removeBundleRef.Count; index++)
                {
                    m_BundlePath2BundleRef.Remove(removeBundleRef[index]);
                }
            }
        }
    }
}