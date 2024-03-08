using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using LitJson;
using Ninth.Utility;

namespace Ninth.Editor
{
    public class WindowConfig : IWindowConfig
    {
        private MappingSelector<Tab, Type> mappingSelector;
        MappingSelector<Tab, Type> IWindowConfig.MappingSelector => mappingSelector;
        [Inject]
        public WindowConfig(WindowJson windowJson)
        {
            mappingSelector = new MappingSelector<Tab, Type>(new ReactiveProperty<Tab>(windowJson.CurrentTab).AsSetEvent(value => windowJson.CurrentTab = value))
            {
                [Tab.Build] = typeof(IBuildProxy),
                [Tab.Excel] = typeof(IExcelProxy),
                [Tab.Scan] = typeof(IScanProxy),
            }.Build();
        }
    }
}