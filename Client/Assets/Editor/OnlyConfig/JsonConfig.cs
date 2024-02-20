using System;
using Ninth.Editor.Window;
using Ninth.Utility;
using VContainer;
using NullReferenceException = System.NullReferenceException;

namespace Ninth.Editor
{
    public class JsonConfig: BaseJsonConfig
    {
        [Inject]
        public JsonConfig()
        {
            enumTypeSubscribe = new EnumTypeSubscribe<string?>()
                .Subscribe<Tab>("Assets/Editor/Proxy/Window/WindowConfig.json");

            commonSubscribe = new CommonSubscribe<Enum, string?>()
                .Subscribe(Tab.Build, "Assets/Editor/Proxy/Window/Build/BuildConfig.json")
                .Subscribe(Tab.Excel, "Assets/Editor/Proxy/Window/Excel/ExcelConfig.json")
                .Subscribe(Tab.Scan, "Assets/Editor/Proxy/Window/Scan/ScanConfig.json");
        }
    }
}