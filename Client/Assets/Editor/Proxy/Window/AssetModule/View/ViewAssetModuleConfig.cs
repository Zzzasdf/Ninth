using System;
using Ninth.HotUpdate;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "ViewAssetModuleSO", menuName = "EditorAssetModuleConfig/ViewAssetModuleSO")]
    public class ViewAssetModuleConfig : BaseAssetModuleConfig
        <BaseView, ViewParentConfig, 
            BaseChildView, ViewChildConfig, 
            ViewP2MAssetReference,
            ViewAssetConfig, ViewAssetParentConfig, ViewAssetChildConfig>
    {
        
    }

    [Serializable]
    public class ViewP2MAssetReference: P2MAssetReference
        <BaseView, ViewParentConfig, 
            BaseChildView, ViewChildConfig,
            ViewAssetConfig, ViewAssetParentConfig, ViewAssetChildConfig>
    {
        public VIEW_HIERARCHY DefaultParentHierarchy;
        public ViewLayout ViewLayout;
        
        public override void WriteExtraParent(ViewAssetParentConfig assetParentConfig, ViewParentConfig parentConfig)
        {
            assetParentConfig.Hierarchy = parentConfig.Hierarchy;
        }
        
        public override void WriteExtra(ViewAssetConfig assetConfig)
        {
            assetConfig.ViewLayout = AssetDatabase.GetAssetPath(ViewLayout);
        }
    }

    [Serializable]
    public class ViewParentConfig: ParentConfig
    {
        public VIEW_HIERARCHY Hierarchy;
    }

    [Serializable]
    public class ViewChildConfig: ChildConfig
    {
    }
}