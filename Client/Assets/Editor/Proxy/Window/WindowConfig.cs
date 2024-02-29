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
        private readonly SubscribeCollect<int> intSubscribe;
        private readonly SubscribeCollect<Type, Tab> typeSubscribe;

        SubscribeCollect<int> IWindowConfig.IntSubscribe => intSubscribe;
        SubscribeCollect<Type, Tab> IWindowConfig.TypeSubscribe => typeSubscribe;
        [Inject]
        public WindowConfig(WindowJson windowJson)
        {
            {
                var build = intSubscribe = new SubscribeCollect<int>();
                build.Subscribe<Tab>((int)windowJson.CurrentTab).AsSetEvent(value => windowJson.CurrentTab = (Tab)value);
            }

            {
                var build = typeSubscribe = new SubscribeCollect<Type, Tab>();
                build.Subscribe(Tab.Build, typeof(IBuildProxy));
                build.Subscribe(Tab.Excel, typeof(IExcelProxy));
                build.Subscribe(Tab.Scan, typeof(IScanProxy));
            }
        }
    }
}