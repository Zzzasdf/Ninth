using System.Collections.Generic;
using Ninth.Utility;
using UnityEditor;
using VContainer;

namespace Ninth.Editor
{
    public enum BuildTargetPlatform
    {
        StandaloneWindows64 = 1 << 0,
        Android = 1 << 1,
        iOS = 1 << 2
    }
    
    public class BuildConfig : IBuildConfig
    {
        private readonly SubscriberCollect<List<string>> stringListSubscriber;
        private readonly SubscriberCollect<string> stringSubscriber;
        private readonly SubscriberCollect<int> intSubscriber;
        private readonly BuildSettings buildSettings;
            
        SubscriberCollect<List<string>> IBuildConfig.StringListSubscriber => stringListSubscriber;
        SubscriberCollect<string> IBuildConfig.StringSubscriber => stringSubscriber;
        SubscriberCollect<int> IBuildConfig.IntSubscriber => intSubscriber;
        BuildSettings IBuildConfig.BuildSettings => buildSettings;

        [Inject]
        public BuildConfig(BuildJson buildJson, IJsonProxy jsonProxy, INameConfig nameConfig, IPlayerSettingsProxy playerSettingsProxy)
        {
            {
                var build = stringListSubscriber = new SubscriberCollect<List<string>>();
                build.Subscribe(AssetGroup.Local, buildJson.LocalGroup).AsSetEvent(value => buildJson.LocalGroup = value);
                build.Subscribe(AssetGroup.Remote, buildJson.RemoteGroup).AsSetEvent(value => buildJson.RemoteGroup = value);
            }

            {
                var build = stringSubscriber = new SubscriberCollect<string>();
                build.Subscribe(BuildFolder.Bundles, buildJson.ExportBundleFolder).AsSetEvent(value => buildJson.ExportBundleFolder = value);
                build.Subscribe(BuildFolder.Players, buildJson.ExportPlayFolder).AsSetEvent(value => buildJson.ExportPlayFolder = value);
            }

            {
                var build = intSubscriber = new SubscriberCollect<int>();
                build.Subscribe<BuildTargetPlatform>((int)buildJson.BuildTargetPlatforms).AsSetEvent(value => buildJson.BuildTargetPlatforms = (BuildTargetPlatform)value);
                build.Subscribe<BuildSettingsMode>((int)buildJson.BuildSettingsMode).AsSetEvent(value => buildJson.BuildSettingsMode = (BuildSettingsMode)value);
            }

            {
                buildSettings = new BuildSettings(
                    new Dictionary<BuildFolder, ReactiveProperty<string>>
                    {
                        [BuildFolder.Bundles] = stringSubscriber.GetReactiveProperty(BuildFolder.Bundles),
                        [BuildFolder.Players] = stringSubscriber.GetReactiveProperty(BuildFolder.Players),
                    },
                    new Dictionary<AssetGroup, IBuildAssets>
                    {
                        [AssetGroup.Local] = new BuildBundle(
                            AssetGroup.Local,
                            new AssetGroupsPaths(stringListSubscriber.GetReactiveProperty(AssetGroup.Local), "Local 打包资源组", "LocalGroup"),
                            nameConfig.FolderByLocalGroup()
                            ),
                        [AssetGroup.Remote] = new BuildBundle(
                            AssetGroup.Remote,
                            new AssetGroupsPaths(stringListSubscriber.GetReactiveProperty(AssetGroup.Remote), "Remote 打包资源组", "RemoteGroup"),
                            nameConfig.FolderByRemoteGroup()
                            ),
                        [AssetGroup.Dll] = new BuildDll(
                            AssetGroup.Dll,
                            nameConfig.FolderByDllGroup()
                            ),
                    },
                    new CollectSelector<BuildSettingsMode>
                        (intSubscriber.GetReactiveProperty<BuildSettingsMode>().AsEnum<BuildSettingsMode>())
                    {
                        BuildSettingsMode.HotUpdateBundle,
                        BuildSettingsMode.Player,
                    }.Build(),
                    intSubscriber.GetReactiveProperty<BuildTargetPlatform>().AsEnum<BuildTargetPlatform>(),
                    new MappingSelector<BuildTargetPlatform, BuildTargetPlatformSelectorItem>
                    {
                        [BuildTargetPlatform.StandaloneWindows64] = 
                            new (BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone, $"{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}_Data/StreamingAssets"),
                        [BuildTargetPlatform.Android] = 
                            new (BuildTarget.Android, BuildTargetGroup.Android),
                        [BuildTargetPlatform.iOS] = 
                            new (BuildTarget.iOS, BuildTargetGroup.iOS,$"{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}.exe/Data/Raw"),
                    }.Build(),
                    buildJson.PlatformVersions,
                    buildJson.CopySettings,
                    new List<CopyLockMode>
                    {
                        CopyLockMode.None, 
                        CopyLockMode.Latest,
                    });
            }
        }

        public class BuildSettings
        {
            public readonly Dictionary<BuildFolder, ReactiveProperty<string>> BuildFolders;
            public readonly Dictionary<AssetGroup, IBuildAssets> BuildSettingsItems;
            public readonly CollectSelector<BuildSettingsMode> BuildSettingsModes;
            public readonly ReactiveProperty<BuildTargetPlatform> BuildTargetPlatform;
            public readonly MappingSelector<BuildTargetPlatform, BuildTargetPlatformSelectorItem> BuildTargetPlatformSelector;
            public readonly ReactiveProperty<int> BuildTargetPlatformCurrentIndex;
            public readonly SerializableDictionary<BuildTargetPlatform, BuildTargetPlatformInfo> PlatformVersions;
            public readonly CopySettings CopySettings;
            public readonly List<CopyLockMode> CopyLockModes;
            
            public BuildSettings(Dictionary<BuildFolder, ReactiveProperty<string>> buildFolders, 
                Dictionary<AssetGroup, IBuildAssets> buildSettingsItems, 
                CollectSelector<BuildSettingsMode> buildSettingsModes,
                ReactiveProperty<BuildTargetPlatform> buildTargetPlatform,
                MappingSelector<BuildTargetPlatform, BuildTargetPlatformSelectorItem> buildTargetPlatformSelector,
                SerializableDictionary<BuildTargetPlatform, BuildTargetPlatformInfo> platformVersions,
                CopySettings copySettings,
                List<CopyLockMode> copyLockModes)
            {
                this.BuildFolders = buildFolders;
                this.BuildSettingsItems = buildSettingsItems;
                this.BuildSettingsModes = buildSettingsModes;
                this.BuildTargetPlatform = buildTargetPlatform;
                this.BuildTargetPlatformSelector = buildTargetPlatformSelector;
                this.BuildTargetPlatformCurrentIndex = new ReactiveProperty<int>(0);
                this.PlatformVersions = platformVersions;
                this.CopySettings = copySettings;
                this.CopyLockModes = copyLockModes;
            }
        }

        public class BuildTargetPlatformSelectorItem
        {
            public BuildTarget BuildTarget { get; }
            public BuildTargetGroup BuildTargetGroup { get; }
            public string? BundleCopy2PlayerRelativePath { get; }

            public BuildTargetPlatformSelectorItem(BuildTarget buildTarget, BuildTargetGroup buildTargetGroup, string? bundleCopy2PlayerRelativePath = null)
            {
                this.BuildTarget = buildTarget;
                this.BuildTargetGroup = buildTargetGroup;
                this.BundleCopy2PlayerRelativePath = bundleCopy2PlayerRelativePath;
            }
        }
    }
}