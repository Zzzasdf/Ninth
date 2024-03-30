using System;
using Ninth.Utility;
using UnityEngine;

namespace Ninth.Editor
{
    [Serializable]
    public class BaseAssetModuleConfig<TParent, TChild> : ScriptableObject
    {
        public AssetPathList AssetFolders = new();

        public P2MAssetReference<TParent, TChild> P2MAssetReference = new();
        
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
}