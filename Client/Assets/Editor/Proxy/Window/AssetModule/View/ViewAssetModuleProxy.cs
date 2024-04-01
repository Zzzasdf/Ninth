using Ninth.HotUpdate;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace Ninth.Editor
{
    public class ViewAssetModuleProxy: BaseAssetModuleProxy
        <BaseView, ViewParentConfig, 
            BaseChildView, ViewChildConfig, 
            ViewP2MAssetReference, ViewAssetModuleConfig, ViewAssetConfig, ViewAssetParentConfig, ViewAssetChildConfig>
    {
        private readonly AssetModuleConfigReferences assetModuleConfigReferences;
        
        [Inject]
        public ViewAssetModuleProxy(AssetModuleConfigReferences assetModuleConfigReferences)
        {
            this.assetModuleConfigReferences = assetModuleConfigReferences;
            assetModuleConfig = assetModuleConfigReferences.ViewAssetModuleConfig;
            textAsset = assetModuleConfigReferences.PreLoadAssets.ViewAssetConfig;
        }

        protected override bool RenderLockSOStatus()
        {
            return assetModuleConfigReferences.LockViewAssetModuleConfig;
        }

        protected override void ModifyLockStatus()
        {
            assetModuleConfigReferences.LockViewAssetModuleConfig = !assetModuleConfigReferences.LockViewAssetModuleConfig;
            if (assetModuleConfigReferences)
            {
                assetModuleConfigReferences.ViewAssetModuleConfig = assetModuleConfig;
            }
            EditorUtility.SetDirty(assetModuleConfigReferences);
            AssetDatabase.SaveAssetIfDirty(assetModuleConfigReferences);
        }
        
        protected override void RenderDefaultParentExtraConfig(ViewP2MAssetReference p2MAssetReference)
        {
            GUILayout.Label("层级");
            p2MAssetReference.DefaultParentHierarchy = (VIEW_HIERARCHY)EditorGUILayout.EnumPopup(p2MAssetReference.DefaultParentHierarchy); 
        }

        protected override void ParentExtraInit(ViewParentConfig parentConfig)
        {
            parentConfig.Hierarchy = assetModuleConfig.P2MAssetReference.DefaultParentHierarchy;
        }
        
        protected override void RenderParentExtraConfig(ViewParentConfig parentConfig)
        {
            parentConfig.Hierarchy = (VIEW_HIERARCHY)EditorGUILayout.EnumPopup(parentConfig.Hierarchy); 
        }

        protected override void RenderExtraConfig(ViewP2MAssetReference p2MAssetReference)
        {
            p2MAssetReference.ViewLayout = (ViewLayout)EditorGUILayout.ObjectField(p2MAssetReference.ViewLayout, typeof(ViewLayout));
        }
    }
}