using System;
using VContainer;
using Ninth.Utility;
using UnityEngine;

namespace Ninth.Editor
{
    public class WindowConfig : IWindowConfig
    {
        private MappingSelector<Tab, Type> mappingSelector;
        MappingSelector<Tab, Type> IWindowConfig.MappingSelector => mappingSelector;
        
        [Inject]
        public WindowConfig(WindowJson windowJson)
        {
            mappingSelector = new MappingSelector<Tab, Type>
                (new ReactiveProperty<Tab>(windowJson.Tab).AsSetEvent(value => windowJson.Tab = value))
            {
                [Tab.AssetModule] = typeof(IAssetModuleProxy),
                [Tab.Build] = typeof(IBuildProxy),
                [Tab.Excel] = typeof(IExcelProxy),
                [Tab.Scan] = typeof(IScanProxy),
            }.Build();
        }
    }

    public class WindowJson: IJson
    {
        public Tab Tab;
    }
}