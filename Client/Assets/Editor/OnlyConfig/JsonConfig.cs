using System;
using Ninth.Editor.Window;
using Ninth.HotUpdate;
using Ninth.Utility;
using UnityEngine.Device;
using VContainer;

namespace Ninth.Editor
{
    public class JsonConfig : IJsonConfig
    {
        private readonly SubscribeCollect<string> stringSubscribe;
        SubscribeCollect<string> IJsonConfig.StringSubscribe => stringSubscribe;

        [Inject]
        public JsonConfig(INameConfig nameConfig)
        {
            {
                var build = stringSubscribe = new SubscribeCollect<string>();
                build.Subscribe<VersionConfig>($"{Application.dataPath}/../../BundleTemp/{nameConfig.FileNameByVersionConfig()}");
                build.Subscribe<LoadConfig>($"{Application.dataPath}/../../BundleTemp/{nameConfig.LoadConfigNameByLocalGroup()}", (int)AssetGroup.Local);
                build.Subscribe<LoadConfig>($"{Application.dataPath}/../../BundleTemp/{nameConfig.LoadConfigNameByRemoteGroup()}", (int)AssetGroup.Remote);
                build.Subscribe<LoadConfig>($"{Application.dataPath}/../../BundleTemp/{nameConfig.LoadConfigNameByDllGroup()}", (int)AssetGroup.Dll);
                build.Subscribe<DownloadConfig>($"{Application.dataPath}/../../BundleTemp/{nameConfig.DownloadConfigNameByRemoteGroup()}", (int)AssetGroup.Remote);
                build.Subscribe<DownloadConfig>($"{Application.dataPath}/../../BundleTemp/{nameConfig.DownloadConfigNameByDllGroup()}", (int)AssetGroup.Dll);
                build.Subscribe<BuildFolder>($"{Application.dataPath}/../../BundleTemp");
                build.Subscribe<Tab>($"{Application.dataPath}/Editor/Proxy/Window/WindowJson.json");
                build.Subscribe(Tab.Build, $"{Application.dataPath}/Editor/Proxy/Window/Build/BuildJson.json");
                build.Subscribe(Tab.Excel, $"{Application.dataPath}/Editor/Proxy/Window/Excel/ExcelJson.json");
                build.Subscribe(Tab.Scan, $"{Application.dataPath}/Editor/Proxy/Window/Scan/ScanJson.json");
            }
        }

    }
}