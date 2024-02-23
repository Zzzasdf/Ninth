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
        public Tab CurrentTab { get; private set; }

        private readonly EnumTypeSubscribe<int> enumTypeSubscribe;
        private readonly CommonSubscribe<Tab, Type> commonSubscribe;

        EnumTypeSubscribe<int> IWindowConfig.EnumTypeSubscribe => enumTypeSubscribe;
        CommonSubscribe<Tab, Type> IWindowConfig.CommonSubscribe => commonSubscribe;

        [Inject]
        public WindowConfig()
        {
            {
                var build = enumTypeSubscribe = new EnumTypeSubscribe<int>();
                build.Subscribe<Tab>((int)CurrentTab).AsSetEvent(value => CurrentTab = (Tab)value);
            }

            {
                var build = commonSubscribe = new CommonSubscribe<Tab, Type>();
                build.Subscribe(Tab.Build, typeof(IBuildProxy));
                build.Subscribe(Tab.Excel, typeof(IExcelProxy));
                build.Subscribe(Tab.Scan, typeof(IScanProxy));
            }
        }
    }
}