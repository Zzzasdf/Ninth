using System.Collections.Generic;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;
using VContainer;
using System.Linq;
using Ninth.HotUpdate;

namespace Ninth.Editor
{
    public class BuildConfig : IBuildConfig
    {
        private readonly SubscribeCollect<List<string>> stringListSubscribe;
        private readonly SubscribeCollect<string> stringSubscribe;
        private readonly SubscribeCollect<int> intSubscribe;
        private readonly BuildSettings buildSettings;
            
        SubscribeCollect<List<string>> IBuildConfig.StringListSubscribe => stringListSubscribe;
        SubscribeCollect<string> IBuildConfig.StringSubscribe => stringSubscribe;
        SubscribeCollect<int> IBuildConfig.IntSubscribe => intSubscribe;
        BuildSettings IBuildConfig.BuildSettings => buildSettings;

        [Inject]
        public BuildConfig(BuildJson buildJson, IJsonProxy jsonProxy)
        {
            var common = buildJson.BuildCommon;
            var bundle = buildJson.BuildBundle;
            var player = buildJson.BuildPlayer;
            
            {
                var build = stringListSubscribe = new SubscribeCollect<List<string>>();
                build.Subscribe(AssetGroup.Local, common.LocalGroup).AsSetEvent(value => common.LocalGroup = value);
                build.Subscribe(AssetGroup.Remote, common.RemoteGroup).AsSetEvent(value => common.RemoteGroup = value);
            }

            {
                var build = stringSubscribe = new SubscribeCollect<string>();
                build.Subscribe<BuildFolder>(jsonProxy.GetPathByEnumType<BuildFolder>());
                build.Subscribe(BuildFolder.Bundles, bundle.ExportBundleFolder, 0).AsSetEvent(value => bundle.ExportBundleFolder = value);
                build.Subscribe(BuildFolder.Bundles, player.ExportBundleFolder, 1).AsSetEvent(value => player.ExportBundleFolder = value);
                build.Subscribe(BuildFolder.Players, player.ExportPlayFolder).AsSetEvent(value => player.ExportPlayFolder = value);
                build.Subscribe(BuildExportCopyFolderMode.StreamingAssets, Application.streamingAssetsPath);
                build.Subscribe(BuildExportCopyFolderMode.Remote, bundle.CopyBundleRemotePath, 0).AsSetEvent(value => bundle.CopyBundleRemotePath = value);
                build.Subscribe(BuildExportCopyFolderMode.Remote, player.CopyBundleRemotePath, 1).AsSetEvent(value => player.CopyBundleRemotePath = value);
                build.Subscribe(BuildVersion.Display, common.DisplayVersion).AsSetEvent(value => common.DisplayVersion = value);
            }

            {
                var build = intSubscribe = new SubscribeCollect<int>();
                build.Subscribe<BuildSettingsMode>(common.CurrentBuildModeIndex).AsSetEvent(value => common.CurrentBuildModeIndex = value);
                build.Subscribe<BuildBundleMode>(bundle.CurrentExportBundleModeIndex, 0).AsSetEvent(value => bundle.CurrentExportBundleModeIndex = value);
                build.Subscribe<BuildBundleMode>(player.CurrentExportBundleModeIndex, 1).AsSetEvent(value => player.CurrentExportBundleModeIndex = value);
                build.Subscribe<BuildExportCopyFolderMode>(bundle.CurrentCopyBundleModeIndex, 0).AsSetEvent(value => bundle.CurrentCopyBundleModeIndex = value);
                build.Subscribe<BuildExportCopyFolderMode>(player.CurrentCopyBundleModeIndex, 1).AsSetEvent(value => player.CurrentCopyBundleModeIndex = value);
                build.Subscribe<ActiveTargetMode>(bundle.CurrentActiveTargetModeIndex, 0).AsSetEvent(value => bundle.CurrentActiveTargetModeIndex = value);
                build.Subscribe<ActiveTargetMode>(player.CurrentActiveTargetModeIndex, 1).AsSetEvent(value => player.CurrentActiveTargetModeIndex = value);
                build.Subscribe(ActiveTargetMode.ActiveTarget, 0);
                build.Subscribe<BuildTarget>(bundle.InactiveBuildTargetIndex, 0).AsSetEvent(value => bundle.InactiveBuildTargetIndex = value);
                build.Subscribe<BuildTarget>(player.InactiveBuildTargetIndex, 1).AsSetEvent(value => player.InactiveBuildTargetIndex = value);
                build.Subscribe(BuildVersion.HotUpdate, common.HotUpdateVersion).AsSetEvent(value => common.HotUpdateVersion = value);
                build.Subscribe(BuildVersion.Iterate, common.IterateVersion).AsSetEvent(value => common.IterateVersion = value);
                build.Subscribe(BuildVersion.Frame, player.FrameVersion).AsSetEvent(value => player.FrameVersion = value);
            }

            {
                buildSettings = new BuildSettings(new MappingSelector<BuildSettingsMode, BuildSettings.BuildSettingsItem>
                {
                    [BuildSettingsMode.Bundle] = new BuildSettings.BuildSettingsItem(
                        new List<BuildFolder>
                        {
                            BuildFolder.Bundles
                        }, 
                        new CollectSelector<BuildBundleMode>(
                            intSubscribe.GetReactiveProperty<BuildBundleMode>().AsEnum<BuildBundleMode>() as ReactiveProperty<BuildBundleMode>)
                        {
                            BuildBundleMode.HotUpdateBundles,
                            BuildBundleMode.AllBundles,
                        }.Build(), 
                        new List<AssetGroup>
                        {
                            AssetGroup.Local,
                            AssetGroup.Remote,
                        }, 
                        new CollectSelector<BuildExportCopyFolderMode>(
                            intSubscribe.GetReactiveProperty<BuildExportCopyFolderMode>().AsEnum<BuildExportCopyFolderMode>())
                        {
                            BuildExportCopyFolderMode.StreamingAssets,
                            BuildExportCopyFolderMode.Remote,
                        }.Build(), 
                        new CollectSelector<ActiveTargetMode>(
                            intSubscribe.GetReactiveProperty<ActiveTargetMode>().AsEnum<ActiveTargetMode>())
                        {
                            ActiveTargetMode.ActiveTarget,
                            ActiveTargetMode.InactiveTarget,
                        }.Build()),
                    [BuildSettingsMode.Player] = new BuildSettings.BuildSettingsItem(
                        new List<BuildFolder>
                        {
                            BuildFolder.Bundles,
                            BuildFolder.Players,
                        }, 
                        new CollectSelector<BuildBundleMode>(
                            intSubscribe.GetReactiveProperty<BuildBundleMode>(1).AsEnum<BuildBundleMode>())
                        {
                            BuildBundleMode.AllBundles,
                        }.Build(),
                        new List<AssetGroup>
                        {
                            AssetGroup.Local,
                            AssetGroup.Remote,
                        }, 
                        new CollectSelector<BuildExportCopyFolderMode>(
                            intSubscribe.GetReactiveProperty<BuildExportCopyFolderMode>(1).AsEnum<BuildExportCopyFolderMode>())
                        {
                            BuildExportCopyFolderMode.StreamingAssets,
                            BuildExportCopyFolderMode.Remote,
                        }.Build(), 
                        new CollectSelector<ActiveTargetMode>(
                            intSubscribe.GetReactiveProperty<ActiveTargetMode>(1).AsEnum<ActiveTargetMode>())
                        {
                            ActiveTargetMode.ActiveTarget,
                            ActiveTargetMode.InactiveTarget
                        }.Build()),
                }.Build(),
                new MappingSelector<BuildTarget, BuildTargetGroup>(
                    intSubscribe.GetReactiveProperty<BuildTarget>().AsEnum<BuildTarget>())
                {
                    [BuildTarget.StandaloneWindows] = BuildTargetGroup.Standalone,
                    [BuildTarget.StandaloneWindows64] = BuildTargetGroup.Standalone,
                    [BuildTarget.Android] = BuildTargetGroup.Android,
                    [BuildTarget.iOS] = BuildTargetGroup.iOS,
                }.Build());
            }
        }

        public class BuildSettings
        {
            public readonly MappingSelector<BuildSettingsMode, BuildSettingsItem> BuildSettingsModes;
            public readonly MappingSelector<BuildTarget, BuildTargetGroup> BuildTargets;

            public BuildSettings(MappingSelector<BuildSettingsMode, BuildSettingsItem> buildSettingsModes, MappingSelector<BuildTarget, BuildTargetGroup> buildTargets)
            {
                this.BuildSettingsModes = buildSettingsModes;
                this.BuildTargets = buildTargets;
            }
            
            public class BuildSettingsItem
            {
                public readonly List<BuildFolder> BuildFolders;
                public readonly CollectSelector<BuildBundleMode> BuildBundleModes;
                public readonly List<AssetGroup> BuildAssetGroups;
                public readonly CollectSelector<BuildExportCopyFolderMode> BuildExportCopyFolderModes;
                public readonly CollectSelector<ActiveTargetMode> BuildActiveTargetModes;
                
                public BuildSettingsItem(List<BuildFolder> buildFolders, CollectSelector<BuildBundleMode> buildBundleModes,
                    List<AssetGroup> buildAssetGroups, CollectSelector<BuildExportCopyFolderMode> buildExportCopyFolderModes,
                    CollectSelector<ActiveTargetMode> buildActiveTargetModes)
                {
                    this.BuildFolders = buildFolders;
                    this.BuildBundleModes = buildBundleModes;
                    this.BuildAssetGroups = buildAssetGroups;
                    this.BuildExportCopyFolderModes = buildExportCopyFolderModes;
                    this.BuildActiveTargetModes = buildActiveTargetModes;
                }
            }
        }

        // public class BuildSettingssss
        // {
        //     public BuildSettingsPathss PathssInfo { get; }
        //     public BuildSettingsBundless BundlessInfo { get; }
        //     public BuildSettingsCopyss CopyssInfo { get; }
        //     public BuildSettingsBuildTargetss BuildTargetssInfo { get; }
        //     public BuildSettingsVersionss VersionssInfo { get; }
        //
        //     public BuildSettingssss(BuildSettingsPathss pathssInfo, BuildSettingsBundless bundlessInfo, BuildSettingsCopyss copyssInfo, BuildSettingsBuildTargetss buildTargetssInfo, BuildSettingsVersionss versionssInfo)
        //     {
        //         this.PathssInfo = pathssInfo;
        //         this.BundlessInfo = bundlessInfo;
        //         this.CopyssInfo = copyssInfo;
        //         this.BuildTargetssInfo = buildTargetssInfo;
        //         this.VersionssInfo = versionssInfo;
        //     }

        //     public class BuildSettingsPathss
        //     {
        //         public List<BuildSettingsPathItem> Items { get; private set; }
        //
        //         public BuildSettingsPathss(List<BuildSettingsPathItem> items)
        //         {
        //             this.Items = items;
        //         }
        //
        //         public class BuildSettingsPathItem
        //         {
        //             public string Label { get; }
        //             private readonly SubscribeCollect<string> stringSubscribe;
        //             private readonly PackMarkBit<BuildFolder> packMarkBit;
        //
        //             public string? Folder
        //             {
        //                 get => stringSubscribe.Get(packMarkBit.Key, packMarkBit.MarkBit);
        //                 set
        //                 {
        //                     if (string.IsNullOrEmpty(value))
        //                     {
        //                         return;
        //                     }
        //
        //                     if (value!.Contains(Application.dataPath))
        //                     {
        //                         $"打包路径: {value} 不能包含 {Application.dataPath}".FrameError();
        //                         return;
        //                     }
        //                     stringSubscribe.Set(packMarkBit.Key, value, packMarkBit.MarkBit);
        //                 }
        //             }
        //
        //             public string DefaultName { get; }
        //
        //             public BuildSettingsPathItem(string label, (SubscribeCollect<string> stringSubscribe, PackMarkBit<BuildFolder> packMarkBit) folder, string defaultName)
        //             {
        //                 this.Label = label;
        //                 this.stringSubscribe = folder.stringSubscribe;
        //                 this.packMarkBit = folder.packMarkBit;
        //                 this.DefaultName = defaultName;
        //             }
        //         }
        //     }
        //
        //     public class BuildSettingsBundless
        //     {
        //         private readonly SubscribeCollect<int> intSubscribe;
        //         private readonly int markBit;
        //         private readonly SubscribeCollect<List<string>> stringListSubscribe;
        //         private readonly Dictionary<BuildBundleMode, List<AssetGroup>> buildBundleModeDic;
        //         
        //         public string[] BuildBundleModeStrings => buildBundleModeDic.Keys.ToArrayString();
        //         public int Current
        //         {
        //             get => intSubscribe.Get<BuildBundleMode>(markBit);
        //             set
        //             {
        //                 // if (value >= buildBundleModeDic.Count)
        //                 // {
        //                 //     value = buildBundleModeDic.Count - 1;
        //                 // } 
        //                 intSubscribe.Set<BuildBundleMode>(value, markBit);
        //             }
        //         }
        //         private List<AssetGroup> currentAssetGroups => buildBundleModeDic.Values.ToList()[Current];
        //         public int[] CurrentAssetGroupsByIndex => currentAssetGroups.ToIndexArray();
        //         public List<string> Get(int groupIndex) => stringListSubscribe.Get(currentAssetGroups[groupIndex]);
        //         public AssetGroup GetAssetGroup(int groupIndex) => currentAssetGroups[groupIndex];
        //
        //         private void Set(int groupIndex, List<string> paths)
        //         {
        //             var assetGroups = currentAssetGroups;
        //             var assetGroup = assetGroups[groupIndex];
        //             stringListSubscribe.Set(assetGroup, paths);
        //         }
        //
        //         public void Set(int groupIndex, string path, int index)
        //         {
        //             if (string.IsNullOrEmpty(path))
        //             {
        //                 return;
        //             }
        //             if (!path.Contains(Application.dataPath))
        //             {
        //                 $"打包资源组路径: {path} 必须包含 {Application.dataPath}".FrameError();
        //                 return;
        //             }
        //             var paths = Get(groupIndex);
        //             if (index < 0 || index >= paths.Count) return;
        //             paths[index] = path;
        //             Set(groupIndex, paths);
        //         }
        //
        //         public void Remove(int groupIndex, int index)
        //         {
        //             var paths = Get(groupIndex);
        //             if (index < 0 || index >= paths.Count) return;
        //             var temp = new List<string>();
        //             for (int i = 0; i < paths.Count; i++)
        //             {
        //                 if (i == index) continue;
        //                 temp.Add(paths[i]);
        //             }
        //             Set(groupIndex, temp);
        //         }
        //
        //         public void Add(int groupIndex)
        //         {
        //             var paths = Get(groupIndex);
        //             paths.Add(string.Empty);
        //             Set(groupIndex, paths);
        //         }
        //
        //         public List<string> LocalGroupPaths => stringListSubscribe.Get(AssetGroup.Local);
        //         public List<string> RemoteGroupPaths => stringListSubscribe.Get(AssetGroup.Remote);
        //
        //         public BuildSettingsBundless(SubscribeCollect<int> intSubscribe, int markBit, SubscribeCollect<List<string>> stringListSubscribe, Dictionary<BuildBundleMode, List<AssetGroup>> buildBundleModeDic)
        //         {
        //             this.intSubscribe = intSubscribe;
        //             this.markBit = markBit;
        //             this.stringListSubscribe = stringListSubscribe;
        //             this.buildBundleModeDic = buildBundleModeDic;
        //         }
        //     }
        //
        //     public class BuildSettingsCopyss
        //     {
        //         private readonly SubscribeCollect<int> intSubscribe;
        //         private readonly int markBit;
        //         private readonly SubscribeCollect<string> stringSubscribe;
        //         private readonly List<PackMarkBit<BuildExportCopyFolderMode>> packBuildBundleCopyModes;
        //
        //         public string[] BuildBundleCopyModeStrings => packBuildBundleCopyModes.Select(x => x.Key).ToArray().ToArrayString();
        //
        //         public int Current
        //         {
        //             get
        //             {
        //                 return intSubscribe.Get<BuildExportCopyFolderMode>(markBit);
        //             }
        //             set
        //             {
        //                 // if (value >= packBuildBundleCopyModes.Count)
        //                 // {
        //                 //     value = packBuildBundleCopyModes.Count - 1;
        //                 // }
        //                 intSubscribe.Set<BuildExportCopyFolderMode>(value, markBit);
        //             }
        //         }
        //
        //         private PackMarkBit<BuildExportCopyFolderMode> CurrentPackBuildBundleCopyMode => packBuildBundleCopyModes[Current];
        //
        //         public string? Folder
        //         {
        //             get => stringSubscribe.Get(CurrentPackBuildBundleCopyMode.Key, CurrentPackBuildBundleCopyMode.MarkBit);
        //             set
        //             {
        //                 if (!IsModify) return;
        //                 if (string.IsNullOrEmpty(value))
        //                 {
        //                     return;
        //                 }
        //                 stringSubscribe.Set(CurrentPackBuildBundleCopyMode.Key, value, CurrentPackBuildBundleCopyMode.MarkBit);
        //             }
        //         }
        //
        //         public bool IsModify => CurrentPackBuildBundleCopyMode.IsModify;
        //
        //         public BuildSettingsCopyss(SubscribeCollect<int> intSubscribe, int markBit, SubscribeCollect<string> stringSubscribe, List<PackMarkBit<BuildExportCopyFolderMode>> packBuildBundleCopyModes)
        //         {
        //             this.intSubscribe = intSubscribe;
        //             this.markBit = markBit;
        //             this.stringSubscribe = stringSubscribe;
        //             this.packBuildBundleCopyModes = packBuildBundleCopyModes;
        //         }
        //     }
        //
        //     public class BuildSettingsBuildTargetss
        //     {
        //         private readonly SubscribeCollect<int> intSubscribe;
        //         private readonly int markBit;
        //         private readonly List<(PackMarkBit<ActiveTargetMode> packBuildTargetMode, BuildTarget? activeBuildTarget)> packBuildTargetModes;
        //         private readonly Dictionary<BuildTarget, BuildTargetGroup?> buildTargetDic;
        //
        //         public string[] BuildTargetModes => packBuildTargetModes.Select(x => x.packBuildTargetMode.Key).ToList().ToArrayString();
        //
        //         public int Current
        //         {
        //             get => intSubscribe.Get<ActiveTargetMode>(markBit);
        //             set
        //             {
        //                 // if (value >= packBuildTargetModes.Count)
        //                 // {
        //                 //     value = packBuildTargetModes.Count - 1;
        //                 // }
        //                 intSubscribe.Set<ActiveTargetMode>(value, markBit);
        //             }
        //         }
        //
        //         private PackMarkBit<ActiveTargetMode> CurrentPackBuildTargetMode => packBuildTargetModes[Current].packBuildTargetMode;
        //         public bool CurrentPackBuildTargetModeIsModify => CurrentPackBuildTargetMode.IsModify;
        //
        //         public int CurrentBuildTargetIndex
        //         {
        //             get
        //             {
        //                 if (packBuildTargetModes[Current].activeBuildTarget != null)
        //                 {
        //                     return buildTargetDic.Keys.ToList().IndexOf(packBuildTargetModes[Current].activeBuildTarget.Value);
        //                 }
        //                 return intSubscribe.Get(CurrentPackBuildTargetMode.Key, CurrentPackBuildTargetMode.MarkBit);
        //             }
        //             set
        //             {
        //                 if (!CurrentPackBuildTargetModeIsModify) return;
        //                 intSubscribe.Set(CurrentPackBuildTargetMode.Key, value, CurrentPackBuildTargetMode.MarkBit);
        //             }
        //         }
        //
        //         public BuildTarget CurrentBuildTarget => buildTargetDic.Keys.ToList()[CurrentBuildTargetIndex];
        //
        //         public string[] BuildTargetStrings => buildTargetDic.Keys.ToArrayString();
        //         public int[] BuildTargetIndex => buildTargetDic.Values.ToIndexArray();
        //
        //         public bool IsEnableCurrentBuildTargetGroup => buildTargetDic.Values.ToList()[CurrentBuildTargetIndex] != null;
        //         public int CurrentBuildTargetGroup => CurrentBuildTargetIndex;
        //         public string[] BuildTargetGroupStrings => buildTargetDic.Values.ToArrayString();
        //         public int[] BuildTargetGroupIndex => buildTargetDic.Values.ToIndexArray();
        //
        //         public BuildSettingsBuildTargetss(SubscribeCollect<int> intSubscribe, int markBit, List<(PackMarkBit<ActiveTargetMode> packBuildTargetMode, BuildTarget? activeBuildTarget)> packBuildTargetModes, Dictionary<BuildTarget,
        //             BuildTargetGroup?> buildTargetDic)
        //         {
        //             this.intSubscribe = intSubscribe;
        //             this.markBit = markBit;
        //             this.packBuildTargetModes = packBuildTargetModes;
        //             this.buildTargetDic = buildTargetDic;
        //         }
        //     }
        //
        //     public class BuildSettingsVersionss
        //     {
        //         private readonly SubscribeCollect<string> stringSubscribe;
        //         private readonly SubscribeCollect<int> intSubscribe;
        //         public bool EnableModifyFrame { get; }
        //         public bool EnableModifyHotUpdate { get; }
        //
        //         public int FrameTemp { get; set; }
        //         public int HotUpdateTemp { get; set; }
        //         public int IterateTemp { get; private set; }
        //         public bool IsModify { get; set; }
        //
        //         public string? Display
        //         {
        //             get => stringSubscribe.Get(BuildVersion.Display);
        //             set => stringSubscribe.Set(BuildVersion.Display, value);
        //         }
        //
        //         public int Frame
        //         {
        //             get => intSubscribe.Get(BuildVersion.Frame);
        //             private set => intSubscribe.Set(BuildVersion.Frame, value);
        //         }
        //
        //         public int HotUpdate
        //         {
        //             get => intSubscribe.Get(BuildVersion.HotUpdate);
        //             private set => intSubscribe.Set(BuildVersion.HotUpdate, value);
        //         }
        //
        //         public int Iterate
        //         {
        //             get => intSubscribe.Get(BuildVersion.Iterate);
        //             private set => intSubscribe.Set(BuildVersion.Iterate, value);
        //         }
        //
        //         public BuildSettingsVersionss(SubscribeCollect<string> stringSubscribe, SubscribeCollect<int> intSubscribe, bool enableModifyFrame, bool enableModifyHotUpdate)
        //         {
        //             this.stringSubscribe = stringSubscribe;
        //             this.intSubscribe = intSubscribe;
        //             this.EnableModifyFrame = enableModifyFrame;
        //             this.EnableModifyHotUpdate = enableModifyHotUpdate;
        //         }
        //
        //         public void Init()
        //         {
        //             if (IsModify)
        //             {
        //                 IterateTemp = Iterate + 1;
        //                 return;
        //             }
        //
        //             FrameTemp = Frame;
        //             HotUpdateTemp = HotUpdate;
        //             IterateTemp = Iterate;
        //         }
        //
        //         public void Reset()
        //         {
        //             IsModify = false;
        //         }
        //
        //         public void Save()
        //         {
        //             Frame = FrameTemp;
        //             HotUpdate = HotUpdateTemp;
        //             Iterate = IterateTemp;
        //             IsModify = false;
        //         }
        //     }
        // }
    }
}