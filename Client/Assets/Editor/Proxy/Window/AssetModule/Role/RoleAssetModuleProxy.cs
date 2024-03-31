using Ninth.HotUpdate;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace Ninth.Editor
{
    public class RoleAssetModuleProxy: BaseAssetModuleProxy<BaseRole, RoleParentConfig, BaseChildRole, RoleChildConfig, RoleP2MAssetReference, RoleAssetModuleConfig>
    {
        private readonly AssetModuleConfigReferences assetModuleConfigReferences;
        
        [Inject]
        public RoleAssetModuleProxy(AssetModuleConfigReferences assetModuleConfigReferences)
        {
            this.assetModuleConfigReferences = assetModuleConfigReferences;
            assetModuleConfig = assetModuleConfigReferences.RoleAssetModuleConfig;
        }

        protected override bool RenderLockSOStatus()
        {
            return assetModuleConfigReferences.LockRoleAssetModuleConfig;
        }

        protected override void ModifyLockStatus()
        {
            assetModuleConfigReferences.LockRoleAssetModuleConfig = !assetModuleConfigReferences.LockRoleAssetModuleConfig;
            if (assetModuleConfigReferences)
            {
                assetModuleConfigReferences.RoleAssetModuleConfig = assetModuleConfig;
            }
            EditorUtility.SetDirty(assetModuleConfigReferences);
            AssetDatabase.SaveAssetIfDirty(assetModuleConfigReferences);
        }
    }
}
