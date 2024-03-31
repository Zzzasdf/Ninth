using System;
using Ninth.Utility;
using VContainer;

namespace Ninth.Editor
{
    public class AssetAssetModuleConfig : IAssetModuleConfig
    {
        private MappingSelector<AssetModuleTab, Type> mappingSelector;
        MappingSelector<AssetModuleTab, Type> IAssetModuleConfig.MappingSelector => mappingSelector;
        [Inject]
        public AssetAssetModuleConfig()
        {
            mappingSelector = new MappingSelector<AssetModuleTab, Type>
                (AssetModuleTab.View)
                {
                    [AssetModuleTab.View] = typeof(ViewAssetModuleProxy),
                    [AssetModuleTab.Role] = typeof(RoleAssetModuleProxy),
                }.Build();
        }
    }
}
