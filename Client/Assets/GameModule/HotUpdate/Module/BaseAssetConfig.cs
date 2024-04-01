using System;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ninth.HotUpdate
{
    [Serializable]
    public abstract class BaseAssetConfig<TBaseAssetParentConfig, TBaseAssetChildConfig> : IJson
        where TBaseAssetParentConfig : BaseAssetParentConfig
        where TBaseAssetChildConfig : BaseAssetChildConfig
    {
        public List<TBaseAssetParentConfig> AssetParentConfigs = new();
        public List<TBaseAssetChildConfig> AssetChildConfigs = new();

        public Dictionary<string, TBaseAssetParentConfig> ParentAssembler;
        public Dictionary<string, TBaseAssetChildConfig> ChildAssembler;
        
        public BaseAssetConfig<TBaseAssetParentConfig, TBaseAssetChildConfig> Build()
        {
            ParentAssembler = new Dictionary<string, TBaseAssetParentConfig>();
            for (var i = 0; i < AssetParentConfigs.Count; i++)
            {
                ParentAssembler.Add(AssetParentConfigs[i].Key, AssetParentConfigs[i]);
            }
            ChildAssembler = new Dictionary<string, TBaseAssetChildConfig>();
            for (var i = 0; i < AssetChildConfigs.Count; i++)
            {
                ChildAssembler.Add(AssetChildConfigs[i].Key, AssetChildConfigs[i]);
            }
            return this;
        }

        public TBaseAssetParentConfig GetAssetParentConfig<T>()
        {
            var key = typeof(T).Name;
             return ParentAssembler[key];
        }

        public TBaseAssetChildConfig GetAssetChildConfig<TParent, TChild>()
        {
            var key = typeof(TParent).Name;
            var childKey = typeof(TChild).Name;
            if (!ParentAssembler[key].ChildKeys.Contains(childKey))
            {
                $"界面 {key} 下未引用 {childKey}".FrameError();
                return null;
            }
            return ChildAssembler[childKey];
        }
    }

    [Serializable]
    public abstract class BaseAssetParentConfig
    {
        public string Key;
        public string RelativePath;
        public int Weight;
        public List<string> ChildKeys;
    }

    [Serializable]
    public class BaseAssetChildConfig
    {
        public string Key;
        public string RelativePath;
        public int Weight;
    }
}
