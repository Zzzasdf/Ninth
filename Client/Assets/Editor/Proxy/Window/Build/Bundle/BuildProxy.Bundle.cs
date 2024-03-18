using System.Collections.Generic;
using System.IO;
using Ninth.Utility;

namespace Ninth.Editor
{
    public partial class BuildProxy
    {
        private void BuildBundles(BuildBundlesConfig buildBundlesConfig)
        {
            var buildFullFolder = $"{buildBundlesConfig.BuildFolder}/{buildBundlesConfig.BundlePrefix}";
            var buildSettingsItems = buildBundlesConfig.BuildSettingsItems;
            var buildTarget = buildBundlesConfig.BuildTarget;
            var buildTargetPlatformInfo = buildBundlesConfig.BuildTargetPlatformInfo;
            var buildAssetBundleOptions = buildTargetPlatformInfo.BuildAssetBundleOptions;

            var groupFullFolders = new Dictionary<AssetGroup, string>
            {
                [AssetGroup.Local] = $"{buildFullFolder}/{nameConfig.FolderByLocalGroup()}",
                [AssetGroup.Remote] = $"{buildFullFolder}/{nameConfig.FolderByRemoteGroup()}",
                [AssetGroup.Dll] = $"{buildFullFolder}/{nameConfig.FolderByDllGroup()}",
            };
            var loadConfigNames = new Dictionary<AssetGroup, string>
            {
                [AssetGroup.Local] = nameConfig.LoadConfigNameByLocalGroup(),
                [AssetGroup.Remote] = nameConfig.LoadConfigNameByRemoteGroup(),
                [AssetGroup.Dll] = nameConfig.LoadConfigNameByDllGroup(),
            };
            var downloadConfigNames = new Dictionary<AssetGroup, string?>
            {
                [AssetGroup.Local] = null,
                [AssetGroup.Remote] = nameConfig.DownloadConfigNameByRemoteGroup(),
                [AssetGroup.Dll] = nameConfig.DownloadConfigNameByDllGroup(),
            };
            foreach (var (assetGroup, buildSettingsItem) in buildSettingsItems)
            { 
                var groupFullFolder = groupFullFolders[assetGroup];
                var loadConfigName = loadConfigNames[assetGroup];
                var downloadConfigName = downloadConfigNames[assetGroup];
                
                buildSettingsItem.ScanAssets(buildFullFolder, buildTarget);
                buildSettingsItem.Build(groupFullFolder, buildAssetBundleOptions, buildTarget);
                buildSettingsItem.CalculateDependencies();
                buildSettingsItem.SaveConfig(groupFullFolder, jsonProxy, loadConfigName, downloadConfigName);
            }
            // 保存版本号
            var versionSourceFileName = $"{buildFullFolder}/{nameConfig.FileNameByVersionConfig()}";
            var versionDestFileName = $"{buildFullFolder}/../{nameConfig.FileNameByVersionConfig()}";
            jsonProxy.ToJson(new VersionConfig 
            {
                DisplayVersion = buildBundlesConfig.BuildTargetPlatformInfo.DisplayVersion,
                FrameVersion = buildBundlesConfig.BuildTargetPlatformInfo.FrameVersion,
                HotUpdateVersion = buildBundlesConfig.BuildTargetPlatformInfo.HotUpdateVersion,
                IterateVersion = buildBundlesConfig.BuildTargetPlatformInfo.IterateVersion,
            }, versionSourceFileName);
            // 拷贝版本号到外部
            File.Copy(versionSourceFileName, versionDestFileName, true);
        }
    }
}