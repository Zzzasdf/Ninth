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
            var tabs = TabKeys().Select(x => x.key.ToString()).ToArray();
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
            return windowConfig.IntSubscribe.Get<TKeyEnum>();
        }

        private void SetEnumType<TKeyEnum>(int value) where TKeyEnum: Enum
        {
            windowConfig.IntSubscribe.Set<TKeyEnum>(value);
        }

        private Type GetTab(Tab key)
        {
            return windowConfig.TypeSubscribe.Get(key);
        }
        
        private Dictionary<(Tab key, int markBit), IReactiveProperty<Type>>.KeyCollection TabKeys()
        {
            return windowConfig.TypeSubscribe.KeysByCommon();
        }
    }
}