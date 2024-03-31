using System;
using Ninth.HotUpdate;
using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "RoleAssetModuleSO", menuName = "EditorAssetModuleConfig/RoleAssetModuleSO")]
    public class RoleAssetModuleConfig : BaseAssetModuleConfig<BaseRole, RoleParentConfig, BaseChildRole, RoleChildConfig, RoleP2MAssetReference>
    {
        
    }

    [Serializable]
    public class RoleP2MAssetReference: P2MAssetReference<BaseRole, RoleParentConfig, BaseChildRole, RoleChildConfig>
    {
        
    }

    [Serializable]
    public class RoleParentConfig: ParentConfig
    {
        
    }

    [Serializable]
    public class RoleChildConfig: ChildConfig
    {
    }
}
