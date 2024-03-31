using System;
using Ninth.HotUpdate;
using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "ViewAssetModuleSO", menuName = "EditorAssetModuleConfig/ViewAssetModuleSO")]
    public class ViewAssetModuleConfig : BaseAssetModuleConfig<BaseView, ViewParentConfig, BaseChildView, ViewChildConfig, ViewP2MAssetReference>
    {
        
    }

    [Serializable]
    public class ViewP2MAssetReference: P2MAssetReference<BaseView, ViewParentConfig, BaseChildView, ViewChildConfig>
    {
        public VIEW_HIERARCHY DefaultParentHierarchy;
        public ViewLayout ViewLayout;
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
