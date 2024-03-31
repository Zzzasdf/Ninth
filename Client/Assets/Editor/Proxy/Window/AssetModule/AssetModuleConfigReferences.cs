using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "AssetModuleConfigReferencesSO", menuName = "EditorAssetModuleConfig/AssetModuleConfigReferencesSO")]
    public class AssetModuleConfigReferences: ScriptableObject
    {
        public bool LockViewAssetModuleConfig;
        public ViewAssetModuleConfig ViewAssetModuleConfig;
        public bool LockRoleAssetModuleConfig;
        public RoleAssetModuleConfig RoleAssetModuleConfig;
    }
}
