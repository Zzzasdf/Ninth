using System;
using Ninth.Editor.Window;
using Ninth.Utility;
using UnityEngine.Device;
using VContainer;

namespace Ninth.Editor
{
    public class JsonConfig: BaseJsonConfig
    {
        [Inject]
        public JsonConfig()
        {
            genericsSubscribe = new GenericsSubscribe<IJson, string>();
            
            enumTypeSubscribe = new EnumTypeSubscribe<string>()
                .Subscribe<Tab>(Application.dataPath + "/Editor/Proxy/Window/WindowConfig.json");

            commonSubscribe = new CommonSubscribe<Enum, string>
            {
                [Tab.Build] = Application.dataPath + "/Editor/Proxy/Window/Build/BuildConfig.json",
                [Tab.Excel] = Application.dataPath + "/Editor/Proxy/Window/Excel/ExcelConfig.json",
                [Tab.Scan] = Application.dataPath + "/Editor/Proxy/Window/Scan/ScanConfig.json",
            };
        }
    }
}