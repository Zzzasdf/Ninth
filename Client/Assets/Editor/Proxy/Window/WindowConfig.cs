using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Ninth.Editor.Window
{
    [Serializable]
    public class WindowConfig: IWindowConfig
    {
        [SerializeField] private Tab currentTab;

        private readonly EnumTypeSubscribe<int> enumTypeSubscribe;
        private readonly CommonSubscribe<Tab, Type> commonSubscribe;

        [Inject]
        public WindowConfig()
        {
            enumTypeSubscribe = new EnumTypeSubscribe<int>()
                .Subscribe<Tab>((int)currentTab);

            commonSubscribe = new CommonSubscribe<Tab, Type>
            {
                [Tab.Build] = typeof(IBuildProxy),
                [Tab.Excel] = typeof(IExcelProxy),
                [Tab.Scan] = typeof(IScanProxy),
            };
        }
        
        // enumType
        T IWindowConfig.GetEnumType<T>()
        {
            return (T)Enum.ToObject(typeof(T),enumTypeSubscribe.Get<T>());
        }
        
        void IWindowConfig.SetEnumType<T>(int value)
        {
            enumTypeSubscribe.Set<T>(value);
        }

        Dictionary<Type, int>.KeyCollection IWindowConfig.EnumTypeKeys()
        {
            return enumTypeSubscribe.Keys;
        }
        
        // common
        Type? IWindowConfig.Get(Tab tab)
        {
            return commonSubscribe.Get(tab);
        }

        void IWindowConfig.Set(Tab key, Type value)
        {
            commonSubscribe.Set(key, value);
        }

        Dictionary<Tab, Type?>.KeyCollection IWindowConfig.Keys()
        {
            return commonSubscribe.Keys;
        }
    }
}