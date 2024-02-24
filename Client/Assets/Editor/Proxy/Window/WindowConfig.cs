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
        private readonly EnumTypeSubscribe<int> enumTypeSubscribe;
        private readonly CommonSubscribe<Tab, Type> tabCommonSubscribe;

        EnumTypeSubscribe<int> IWindowConfig.EnumTypeSubscribe => enumTypeSubscribe;
        CommonSubscribe<Tab, Type> IWindowConfig.TabCommonSubscribe => tabCommonSubscribe;
        [Inject]
        public WindowConfig(WindowJson windowJson)
        {
            {
                var build = enumTypeSubscribe = new EnumTypeSubscribe<int>();
                build.Subscribe<Tab>((int)windowJson.CurrentTab).AsSetEvent(value => windowJson.CurrentTab = (Tab)value);
            }

            {
                var build = tabCommonSubscribe = new CommonSubscribe<Tab, Type>();
                build.Subscribe(Tab.Build, typeof(IBuildProxy));
                build.Subscribe(Tab.Excel, typeof(IExcelProxy));
                build.Subscribe(Tab.Scan, typeof(IScanProxy));
            }
        }
    }
}