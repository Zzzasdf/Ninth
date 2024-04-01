using System;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEngine;

namespace Ninth.HotUpdate
{
    [Serializable]
    public abstract class BaseAssetConfig<TBaseAssetParentConfig, TBaseAssetChildConfig> : IJson
        where TBaseAssetParentConfig : BaseAssetParentConfig
        where TBaseAssetChildConfig : BaseAssetChildConfig
    {
        public List<TBaseAssetParentConfig> AssetParentConfigs = new();
        public List<TBaseAssetChildConfig> AssetChildConfigs = new();
        
        public List<string> Keys = new();
        public List<AssetItemConfig> ParentValueIndexs = new();
    }
        
    [Serializable]
    public class AssetItemConfig
    {
        public int ValueIndex;
        public List<int> ChildIndex;
    }

    [Serializable]
    public abstract class BaseAssetParentConfig
    {
        public string Key;
        public string RelativePath;
        public int Weight;
    }

    [Serializable]
    public class BaseAssetChildConfig
    {
        public string Key;
        public string RelativePath;
        public int Weight;
    }
}
