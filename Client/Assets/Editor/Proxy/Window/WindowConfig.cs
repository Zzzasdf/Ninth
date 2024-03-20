using System;
using VContainer;
using Ninth.Utility;

namespace Ninth.Editor
{
    public class WindowConfig : IWindowConfig
    {
        private MappingSelector<Tab, Type> mappingSelector;
        MappingSelector<Tab, Type> IWindowConfig.MappingSelector => mappingSelector;
        [Inject]
        public WindowConfig()
        {
            mappingSelector = new MappingSelector<Tab, Type>
                (Tab.Build)
            {
                [Tab.Build] = typeof(IBuildProxy),
                [Tab.Excel] = typeof(IExcelProxy),
                [Tab.Scan] = typeof(IScanProxy),
            }.Build();
        }
    }
}