using System;
using Ninth.Utility;
using UnityEngine.Device;
using VContainer;

namespace Ninth.Editor
{
    public class JsonConfig : IJsonConfig
    {
        private readonly TypeSubscriber<string> typeSubscriber;
        private readonly Subscriber<Enum, string> subscriber;
        TypeSubscriber<string> IJsonConfig.TypeSubscriber => typeSubscriber;
        Subscriber<Enum, string> IJsonConfig.Subscriber => subscriber;

        [Inject]
        public JsonConfig(INameConfig nameConfig)
        {
            {
                var build = typeSubscriber = new TypeSubscriber<string>();
                build.Subscribe<Tab>($"{Application.dataPath}/Editor/Proxy/Window/WindowJson.json");
            }
            {
                var build = subscriber = new Subscriber<Enum, string>();
                build.Subscribe(Tab.Build, $"{Application.dataPath}/Editor/Proxy/Window/Build/BuildJson.json");
                build.Subscribe(Tab.Excel, $"{Application.dataPath}/Editor/Proxy/Window/Excel/ExcelJson.json");
                build.Subscribe(Tab.Scan, $"{Application.dataPath}/Editor/Proxy/Window/Scan/ScanJson.json");
            }
        }
    }
}