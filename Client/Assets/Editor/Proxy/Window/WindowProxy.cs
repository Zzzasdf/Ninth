using System;
using System.Collections.Generic;
using System.Linq;
using Ninth.Editor.Window;
using Ninth.HotUpdate;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.Editor
{
    public class WindowProxy: IWindowProxy
    {
        private readonly IWindowConfig windowConfig;
        private readonly IObjectResolver resolver;
        
        [Inject]
        public WindowProxy(IWindowConfig windowConfig, IObjectResolver resolver)
        {
            this.windowConfig = windowConfig;
            this.resolver = resolver;
        }

        void IWindowProxy.Tab()
        {
            var tabs = TabKeys().ToArrayString();
            var current = GetEnumType<Tab>();
            var temp = GUILayout.SelectionGrid(current, tabs, 1);
            if (temp != current)
            {
                SetEnumType<Tab>(temp);
            }
        }

        void IWindowProxy.Content()
        {
            var tab = GetEnumType<Tab>();
            var type = GetTab((Tab)tab);
            (resolver.Resolve(type) as IOnGUI)?.OnGUI();
        }
        
        private int GetEnumType<TKeyEnum>() where TKeyEnum: Enum
        {
            return windowConfig.EnumTypeSubscribe.Get<TKeyEnum>();
        }

        private void SetEnumType<TKeyEnum>(int value) where TKeyEnum: Enum
        {
            windowConfig.EnumTypeSubscribe.Set<TKeyEnum>(value);
        }

        private Type GetTab(Tab key)
        {
            return windowConfig.TabCommonSubscribe.Get(key);
        }
        
        private Dictionary<Tab, LinkedListReactiveProperty<Type>>.KeyCollection TabKeys()
        {
            return windowConfig.TabCommonSubscribe.Keys();
        }
    }
}