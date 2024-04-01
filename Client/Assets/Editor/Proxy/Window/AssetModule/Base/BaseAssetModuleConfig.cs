using System;
using Ninth.HotUpdate;
using Ninth.Utility;
using UnityEngine;

namespace Ninth.Editor
{
    [Serializable]
    public class BaseAssetModuleConfig
        <TParent, TParentConfig, 
            TChild, TChildConfig, 
            TP2MAssetReference,
            TAssetConfig, TAssetParentConfig, TAssetChildConfig> : ScriptableObject
        where TParent: UnityEngine.Object
        where TParentConfig: ParentConfig, new()
        where TChild: UnityEngine.Object
        where TChildConfig: ChildConfig, new()
        where TP2MAssetReference: P2MAssetReference<TParent, TParentConfig, TChild, TChildConfig, TAssetConfig, TAssetParentConfig, TAssetChildConfig>, new()
        where TAssetConfig: BaseAssetConfig<TAssetParentConfig, TAssetChildConfig>, new()
        where TAssetParentConfig: BaseAssetParentConfig, new()
        where TAssetChildConfig: BaseAssetChildConfig, new()
    {
        public bool AssetFoldout;
        public AssetPathList AssetFolders = new();
        public TP2MAssetReference P2MAssetReference = new();
        public HierarchyInfo HierarchyInfo = new();
    }
    
    public class HierarchyInfo
    {
        public MappingSelector<FirstHierarchy, CollectSelector<SecondHierarchy>> Selector;

        public HierarchyInfo()
        {
            Selector = new MappingSelector<FirstHierarchy, CollectSelector<SecondHierarchy>>
            {
                [FirstHierarchy.Parent] = new CollectSelector<SecondHierarchy>
                {
                    SecondHierarchy.Appointed,
                    SecondHierarchy.UnAppoint
                }.Build(),
                [FirstHierarchy.Child] = new CollectSelector<SecondHierarchy>
                {
                    SecondHierarchy.Appointed,
                    SecondHierarchy.UnAppoint,
                    SecondHierarchy.RepeatAppoint
                }.Build()
            }.Build();
        }
    }

    public enum FirstHierarchy
    {
        Parent,
        Child
    }

    public enum SecondHierarchy
    {
        Appointed,
        UnAppoint,
        RepeatAppoint,
    }

    public enum Implement
    {
        Count,
        Render,
        AutoAppoint,

        MissCount,
        MissAutoRemove,
    }
}