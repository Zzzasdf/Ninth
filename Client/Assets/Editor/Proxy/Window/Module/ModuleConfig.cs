using System;
using Ninth.Utility;
using VContainer;

namespace Ninth.Editor
{
    public class ModuleConfig : IModuleConfig
    {
        private MappingSelector<ModuleTab, Type> mappingSelector;
        MappingSelector<ModuleTab, Type> IModuleConfig.MappingSelector => mappingSelector;
        [Inject]
        public ModuleConfig()
        {
            mappingSelector = new MappingSelector<ModuleTab, Type>
                (ModuleTab.View)
                {
                    [ModuleTab.View] = typeof(ViewProxy),
                }.Build();
        }
    }
}
