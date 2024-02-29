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
        private readonly GenericsSubscribe<IJson, string> genericsSubscribe;
        private readonly EnumTypeSubscribe<string> enumTypeSubscribe;
        private readonly CommonSubscribe<Enum, string> commonSubscribe;

        GenericsSubscribe<IJson, string> IJsonConfig.GenericsSubscribe => genericsSubscribe;
        EnumTypeSubscribe<string> IJsonConfig.EnumTypeSubscribe => enumTypeSubscribe;
        CommonSubscribe<Enum, string> IJsonConfig.CommonSubscribe => commonSubscribe;

        [Inject]
        public JsonConfig(INameConfig nameConfig)
        {
            {
                var build = genericsSubscribe = new GenericsSubscribe<IJson, string>();
                build.Subscribe<VersionConfig>($"{Application.dataPath}/../../BundleTemp/{nameConfig.FileNameByVersionConfig()}");
                build.Subscribe<LoadConfig>($"{Application.dataPath}/../../BundleTemp/{nameConfig.LoadConfigNameByLocalGroup()}", (int)AssetGroup.Local);
                build.Subscribe<LoadConfig>($"{Application.dataPath}/../../BundleTemp/{nameConfig.LoadConfigNameByRemoteGroup()}", (int)AssetGroup.Remote);
                build.Subscribe<LoadConfig>($"{Application.dataPath}/../../BundleTemp/{nameConfig.LoadConfigNameByDllGroup()}", (int)AssetGroup.Dll);
                build.Subscribe<DownloadConfig>($"{Application.dataPath}/../../BundleTemp/{nameConfig.DownloadConfigNameByRemoteGroup()}", (int)AssetGroup.Remote);
                build.Subscribe<DownloadConfig>($"{Application.dataPath}/../../BundleTemp/{nameConfig.DownloadConfigNameByDllGroup()}", (int)AssetGroup.Dll);
            }

            {
                var build = enumTypeSubscribe = new EnumTypeSubscribe<string>();
                build.Subscribe<BuildFolder>($"{Application.dataPath}/../../BundleTemp");
                build.Subscribe<Tab>($"{Application.dataPath}/Editor/Proxy/Window/WindowJson.json");
            }

            {
                var build = commonSubscribe = new CommonSubscribe<Enum, string>();
                build.Subscribe(Tab.Build, $"{Application.dataPath}/Editor/Proxy/Window/Build/BuildJson.json");
                build.Subscribe(Tab.Excel, $"{Application.dataPath}/Editor/Proxy/Window/Excel/ExcelJson.json");
                build.Subscribe(Tab.Scan, $"{Application.dataPath}/Editor/Proxy/Window/Scan/ScanJson.json");
            }
        }
    }
}