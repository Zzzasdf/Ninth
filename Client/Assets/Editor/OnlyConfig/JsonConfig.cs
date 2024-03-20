using Ninth.Utility;
using UnityEngine.Device;
using VContainer;

namespace Ninth.Editor
{
    public class JsonConfig : IJsonConfig
    {
        private readonly SubscriberCollect<string> stringSubscriber;
        SubscriberCollect<string> IJsonConfig.StringSubscriber => stringSubscriber;

        [Inject]
        public JsonConfig(INameConfig nameConfig)
        {
            {
                var build = stringSubscriber = new SubscriberCollect<string>();
                build.Subscribe<Tab>($"{Application.dataPath}/Editor/Proxy/Window/WindowJson.json");
                build.Subscribe(Tab.Build, $"{Application.dataPath}/Editor/Proxy/Window/Build/BuildJson.json");
                build.Subscribe(Tab.Excel, $"{Application.dataPath}/Editor/Proxy/Window/Excel/ExcelJson.json");
                build.Subscribe(Tab.Scan, $"{Application.dataPath}/Editor/Proxy/Window/Scan/ScanJson.json");
            }
        }

    }
}