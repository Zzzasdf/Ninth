using System;

namespace Ninth.HotUpdate
{
    [Serializable]
    public class ViewAssetConfig : BaseAssetConfig<ViewAssetParentConfig, ViewAssetChildConfig>
    {
        public string ViewLayout;
    }

    [Serializable]
    public class ViewAssetParentConfig : BaseAssetParentConfig
    {
        public VIEW_HIERARCHY Hierarchy;
    }

    [Serializable]
    public class ViewAssetChildConfig : BaseAssetChildConfig
    {
        
    }
}