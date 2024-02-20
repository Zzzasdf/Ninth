using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Ninth.Editor.Window
{
    [Serializable]
    public class WindowConfig: IWindowConfig
    {
        [SerializeField] private Tab currentTab;

        private readonly EnumTypeSubscribe<string?> enumTypeSubscribe;
        private readonly CommonSubscribe<Tab, (Type type, string path)?> commonSubscribe;

        [Inject]
        public WindowConfig()
        {
            enumTypeSubscribe = new EnumTypeSubscribe<string?>()
                .Subscribe<Tab>("Assets/Editor/Proxy/Window/WindowConfig.json");
            
            commonSubscribe = new CommonSubscribe<Tab, (Type type, string path)?>
            {
                [Tab.Build] = (typeof(IBuildProxy), "Assets/Editor/Proxy/Window/Build/BuildConfig.json"),
                [Tab.Excel] = (typeof(IExcelProxy), "Assets/Editor/Proxy/Window/Excel/ExcelConfig.json"),
                [Tab.Scan] = (typeof(IScanProxy), "Assets/Editor/Proxy/Window/Scan/ScanConfig.json"),
            };
        }
        
        public Tab CurrentTab
        {
            get => currentTab;
            set => currentTab = value;
        }

        string? IWindowConfig.GetEnumType<T>()
        {
            return enumTypeSubscribe.Get<T>();
        }

        (Type type, string path)? IWindowConfig.Get(Tab tab)
        {
            return commonSubscribe.Get(tab);
        }

        Dictionary<Type, string?>.KeyCollection IWindowConfig.EnumTypeKeys()
        {
            return enumTypeSubscribe.Keys;
        }

        Dictionary<Tab, (Type type, string path)?>.KeyCollection IWindowConfig.Keys()
        {
            return commonSubscribe.Keys;
        }
    }
}