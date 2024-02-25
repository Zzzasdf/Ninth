using System;
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
        private readonly CommonSubscribe<Enum, List<string>> stringListCommonSubscribe;
        private readonly EnumTypeSubscribe<int> intEnumTypeSubscribe;
        private readonly CommonSubscribe<Enum, string> stringCommonSubscribe;
        private readonly CommonSubscribe<Enum, int> intCommonSubscribe;
        private readonly CommonSubscribe<BuildSettingsMode, BuildSettings> tabCommonSubscribe;

        CommonSubscribe<Enum, List<string>> IBuildConfig.StringListCommonSubscribe => stringListCommonSubscribe;
        EnumTypeSubscribe<int> IBuildConfig.IntEnumTypeSubscribe => intEnumTypeSubscribe;
        CommonSubscribe<Enum, string> IBuildConfig.StringCommonSubscribe => stringCommonSubscribe;
        CommonSubscribe<Enum, int> IBuildConfig.IntCommonSubscribe => intCommonSubscribe;
        CommonSubscribe<BuildSettingsMode, BuildSettings> IBuildConfig.TabCommonSubscribe => tabCommonSubscribe;

        [Inject]
        public BuildConfig(BuildJson buildJson)
        {
            var common = buildJson.BuildCommon;
            var bundle = buildJson.BuildBundle;
            var player = buildJson.BuildPlayer;
            {
                var build = stringListCommonSubscribe = new CommonSubscribe<Enum, List<string>>();
                build.Subscribe(AssetGroup.Local, common.LocalGroup).AsSetEvent(value => common.LocalGroup = value);
                build.Subscribe(AssetGroup.Remote, common.RemoteGroup).AsSetEvent(value => common.RemoteGroup = value);
            }
            
            {
                var build = intEnumTypeSubscribe = new EnumTypeSubscribe<int>();
                build.Subscribe<BuildSettingsMode>(common.CurrentBuildModeIndex).AsSetEvent(value => common.CurrentBuildModeIndex = value);
                build.Subscribe<BuildBundleMode>(bundle.CurrentExportBundleModeIndex, 0).AsSetEvent(value => bundle.CurrentExportBundleModeIndex = value);
                build.Subscribe<BuildBundleMode>(player.CurrentExportBundleModeIndex, 1).AsSetEvent(value => player.CurrentExportBundleModeIndex = value);
                build.Subscribe<BuildExportCopyFolderMode>(bundle.CurrentCopyBundleModeIndex, 0).AsSetEvent(value => bundle.CurrentCopyBundleModeIndex = value);
                build.Subscribe<BuildExportCopyFolderMode>(player.CurrentCopyBundleModeIndex, 1).AsSetEvent(value => player.CurrentCopyBundleModeIndex = value);
                build.Subscribe<ActiveTargetMode>(bundle.CurrentActiveTargetModeIndex, 0).AsSetEvent(value => bundle.CurrentActiveTargetModeIndex = value);
                build.Subscribe<ActiveTargetMode>(player.CurrentActiveTargetModeIndex, 1).AsSetEvent(value => player.CurrentActiveTargetModeIndex = value);
            }

            {
                var build = stringCommonSubscribe = new CommonSubscribe<Enum, string>();
                build.Subscribe(BuildFolder.Bundles, bundle.ExportBundleFolder, 0).AsSetEvent(value => bundle.ExportBundleFolder = value);
                build.Subscribe(BuildFolder.Bundles, player.ExportBundleFolder, 1).AsSetEvent(value => player.ExportBundleFolder = value);
                build.Subscribe(BuildFolder.Players, player.ExportPlayFolder).AsSetEvent(value => player.ExportPlayFolder = value);
                build.Subscribe(BuildExportCopyFolderMode.StreamingAssets, Application.streamingAssetsPath);
                build.Subscribe(BuildExportCopyFolderMode.Remote, bundle.CopyBundleRemotePath, 0).AsSetEvent(value => bundle.CopyBundleRemotePath = value);
                build.Subscribe(BuildExportCopyFolderMode.Remote, player.CopyBundleRemotePath, 1).AsSetEvent(value => player.CopyBundleRemotePath = value);
                build.Subscribe(BuildVersion.Display, common.DisplayVersion).AsSetEvent(value => common.DisplayVersion = value);
            }

            {
                var build = intCommonSubscribe = new CommonSubscribe<Enum, int>();
                build.Subscribe(ActiveTargetMode.ActiveTarget, 0);
                build.Subscribe(ActiveTargetMode.InactiveTarget, bundle.InactiveBuildTargetIndex, 0).AsSetEvent(value => bundle.InactiveBuildTargetIndex = value);
                build.Subscribe(ActiveTargetMode.InactiveTarget, player.InactiveBuildTargetIndex, 1).AsSetEvent(value => player.InactiveBuildTargetIndex = value);
                build.Subscribe(BuildVersion.HotUpdate, common.HotUpdateVersion).AsSetEvent(value => common.HotUpdateVersion = value);
                build.Subscribe(BuildVersion.Iterate, common.IterateVersion).AsSetEvent(value => common.IterateVersion = value);
                build.Subscribe(BuildVersion.Frame, player.FrameVersion).AsSetEvent(value => player.FrameVersion = value);
            }

            {
                var build = tabCommonSubscribe = new CommonSubscribe<BuildSettingsMode, BuildSettings>();
                build.Subscribe(BuildSettingsMode.Bundle, new BuildSettings(
                    new List<BuildSettings.BuildSettingsPath>
                    {
                        new("bundle 打包的路径", (stringCommonSubscribe, new PackMarkBit<BuildFolder>(BuildFolder.Bundles, 0)), "选择目标文件夹", "Bundles"),
                    },
                    new BuildSettings.BuildSettingsBundle(
                        intEnumTypeSubscribe, 0,
                        stringListCommonSubscribe, new Dictionary<BuildBundleMode, List<AssetGroup>>
                        {
                            [BuildBundleMode.HotUpdateBundles] = new() { AssetGroup.Remote },
                            [BuildBundleMode.AllBundles] = new() { AssetGroup.Local , AssetGroup.Remote },
                        }),
                    new BuildSettings.BuildSettingsCopy(
                        intEnumTypeSubscribe, 0,
                        stringCommonSubscribe, new List<PackMarkBit<BuildExportCopyFolderMode>>
                        {
                            new(BuildExportCopyFolderMode.StreamingAssets, isModify: false),
                            new(BuildExportCopyFolderMode.Remote, 0),
                        }),
                    new BuildSettings.BuildSettingsBuildTarget(
                        intEnumTypeSubscribe, 0,
                        intCommonSubscribe, new List<(PackMarkBit<ActiveTargetMode> packBuildTargetMode, BuildTarget? activeBuildTarget)>
                        {
                            (new(ActiveTargetMode.ActiveTarget, isModify: false), EditorUserBuildSettings.activeBuildTarget),
                            (new(ActiveTargetMode.InactiveTarget, 0), null),
                        },
                        new()
                        {
                            [BuildTarget.StandaloneWindows64] = null,
                            [BuildTarget.StandaloneWindows] = null,
                            [BuildTarget.Android] = null,
                            [BuildTarget.iOS] = null,
                        }),
                    new BuildSettings.BuildSettingsVersion(stringCommonSubscribe, intCommonSubscribe, false, true)
                ));
                build.Subscribe(BuildSettingsMode.Player, new BuildSettings(
                    new List<BuildSettings.BuildSettingsPath>
                    {
                        new("bundle 打包的路径", (stringCommonSubscribe, new PackMarkBit<BuildFolder>(BuildFolder.Bundles, 1)), "选择目标文件夹", "Bundles"),
                        new("player 打包的路径", (stringCommonSubscribe, new PackMarkBit<BuildFolder>(BuildFolder.Players, 0)), "选择目标文件夹", "Players"),
                    },
                    new BuildSettings.BuildSettingsBundle(
                        intEnumTypeSubscribe, 1,
                        stringListCommonSubscribe,new Dictionary<BuildBundleMode, List<AssetGroup>>
                        {
                            [BuildBundleMode.AllBundles] = new() { AssetGroup.Local, AssetGroup.Remote }
                        }),
                    new BuildSettings.BuildSettingsCopy(
                        intEnumTypeSubscribe, 1,
                        stringCommonSubscribe, new List<PackMarkBit<BuildExportCopyFolderMode>>
                        {
                            new(BuildExportCopyFolderMode.StreamingAssets, isModify: false),
                            new(BuildExportCopyFolderMode.Remote, 1),
                        }),
                    new BuildSettings.BuildSettingsBuildTarget(
                        intEnumTypeSubscribe, 1,
                        intCommonSubscribe, new List<(PackMarkBit<ActiveTargetMode> packBuildTargetMode, BuildTarget? activeBuildTarget)>
                        {
                            (new(ActiveTargetMode.ActiveTarget, isModify: false), EditorUserBuildSettings.activeBuildTarget),
                            (new(ActiveTargetMode.InactiveTarget, 1), null),
                        },
                        new()
                        {
                            [BuildTarget.StandaloneWindows64] = BuildTargetGroup.Standalone,
                            [BuildTarget.StandaloneWindows] = BuildTargetGroup.Standalone,
                            [BuildTarget.Android] = BuildTargetGroup.Android,
                            [BuildTarget.iOS] = BuildTargetGroup.iOS,
                        }),
                    new BuildSettings.BuildSettingsVersion(stringCommonSubscribe, intCommonSubscribe, true, false)
                ));
            }
        }

        public class BuildSettings
        {
            public List<BuildSettingsPath> Paths { get; }
            public BuildSettingsBundle BundleMode { get; }
            public BuildSettingsCopy CopyMode { get; }
            public BuildSettingsBuildTarget BuildTargetMode { get; }
            public BuildSettingsVersion Version { get; }

            public BuildSettings(List<BuildSettingsPath> paths, BuildSettingsBundle bundleMode, BuildSettingsCopy copyMode, BuildSettingsBuildTarget buildTargetMode, BuildSettingsVersion version)
            {
                this.Paths = paths;
                this.BundleMode = bundleMode;
                this.CopyMode = copyMode;
                this.BuildTargetMode = buildTargetMode;
                this.Version = version;
            }

            public class BuildSettingsPath
            {
                public string Label { get; }

                private readonly CommonSubscribe<Enum, string> stringCommonSubscribe;
                private readonly PackMarkBit<BuildFolder> packMarkBit;

                public string? Folder
                {
                    get => stringCommonSubscribe.Get(packMarkBit.Key, packMarkBit.MarkBit);
                    set => stringCommonSubscribe.Set(packMarkBit.Key, value, packMarkBit.MarkBit);
                }

                public string Title { get; }
                public string DefaultName { get; }

                public BuildSettingsPath(string label, (CommonSubscribe<Enum, string> stringCommonSubscribe, PackMarkBit<BuildFolder> packMarkBit) folder, string title, string defaultName)
                {
                    this.Label = label;
                    this.stringCommonSubscribe = folder.stringCommonSubscribe;
                    this.packMarkBit = folder.packMarkBit;
                    this.Title = title;
                    this.DefaultName = defaultName;
                }
            }

            public class BuildSettingsBundle
            {
                private readonly EnumTypeSubscribe<int> intEnumTypeSubscribe;
                private readonly int markBit;
                private readonly CommonSubscribe<Enum, List<string>> stringListCommonSubscribe;
                private readonly Dictionary<BuildBundleMode, List<AssetGroup>> buildBundleModeDic;
                
                public string[] BuildBundleModeStrings => buildBundleModeDic.Keys.ToArrayString();
                public int Current
                {
                    get => intEnumTypeSubscribe.Get<BuildBundleMode>(markBit);
                    set
                    {
                        // if (value >= buildBundleModeDic.Count)
                        // {
                        //     value = buildBundleModeDic.Count - 1;
                        // }
                        intEnumTypeSubscribe.Set<BuildBundleMode>(value, markBit);
                    }
                }
                private List<AssetGroup> currentAssetGroups => buildBundleModeDic.Values.ToList()[Current];
                public int[] CurrentAssetGroupsByIndex => currentAssetGroups.ToIndexArray();
                public List<string> Get(int groupIndex) =>  stringListCommonSubscribe.Get(currentAssetGroups[groupIndex]);
                public AssetGroup GetAssetGroup(int groupIndex) => currentAssetGroups[groupIndex];
                
                private void Set(int groupIndex, List<string> paths)
                {
                    var assetGroups = currentAssetGroups;
                    var assetGroup = assetGroups[groupIndex];
                    stringListCommonSubscribe.Set(assetGroup, paths);
                }
                
                public void Set(int groupIndex, string path, int index)
                {
                    var paths = Get(groupIndex);
                    if (index < 0 || index >= paths.Count) return;
                    paths[index] = path;
                    Set(groupIndex, paths);
                }
                
                public void Remove(int groupIndex, int index)
                {
                    var paths = Get(groupIndex);
                    if (index < 0 || index >= paths.Count) return;
                    var temp = new List<string>();
                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (i == index) continue;
                        temp.Add(paths[i]);
                    }
                    Set(groupIndex, temp);
                }

                public void Add(int groupIndex)
                {
                    var paths = Get(groupIndex);
                    paths.Add(string.Empty);
                    Set(groupIndex, paths);
                }

                public BuildSettingsBundle(EnumTypeSubscribe<int> intEnumTypeSubscribe, int markBit, CommonSubscribe<Enum, List<string>> stringListCommonSubscribe, Dictionary<BuildBundleMode, List<AssetGroup>> buildBundleModeDic)
                {
                    this.intEnumTypeSubscribe = intEnumTypeSubscribe;
                    this.markBit = markBit;
                    this.stringListCommonSubscribe = stringListCommonSubscribe;
                    this.buildBundleModeDic = buildBundleModeDic;
                }
            }

            public class BuildSettingsCopy
            {
                private readonly EnumTypeSubscribe<int> intEnumTypeSubscribe;
                private readonly int markBit;
                private readonly CommonSubscribe<Enum, string> stringCommonSubscribe;
                private readonly List<PackMarkBit<BuildExportCopyFolderMode>> packBuildBundleCopyModes;
                
                public string[] BuildBundleCopyModeStrings => packBuildBundleCopyModes.Select(x => x.Key).ToArray().ToArrayString();
                public int Current
                {
                    get => intEnumTypeSubscribe.Get<BuildExportCopyFolderMode>(markBit);
                    set
                    {
                        // if (value >= packBuildBundleCopyModes.Count)
                        // {
                        //     value = packBuildBundleCopyModes.Count - 1;
                        // }
                        intEnumTypeSubscribe.Set<BuildExportCopyFolderMode>(value, markBit);
                    }
                }
                private PackMarkBit<BuildExportCopyFolderMode> CurrentPackBuildBundleCopyMode => packBuildBundleCopyModes[Current];
                public string? Folder
                {
                    get => stringCommonSubscribe.Get(CurrentPackBuildBundleCopyMode.Key, CurrentPackBuildBundleCopyMode.MarkBit);
                    set
                    {
                        if (!IsModify) return;
                        stringCommonSubscribe.Set(CurrentPackBuildBundleCopyMode.Key, value, CurrentPackBuildBundleCopyMode.MarkBit);
                    }
                }
                public bool IsModify => CurrentPackBuildBundleCopyMode.IsModify;

                public BuildSettingsCopy(EnumTypeSubscribe<int> intEnumTypeSubscribe, int markBit, CommonSubscribe<Enum, string> stringCommonSubscribe, List<PackMarkBit<BuildExportCopyFolderMode>> packBuildBundleCopyModes)
                {
                    this.intEnumTypeSubscribe = intEnumTypeSubscribe;
                    this.markBit = markBit;

                    this.stringCommonSubscribe = stringCommonSubscribe;
                    this.packBuildBundleCopyModes = packBuildBundleCopyModes;
                }
            }

            public class BuildSettingsBuildTarget
            { 
                private readonly EnumTypeSubscribe<int> intEnumTypeSubscribe;
                private readonly int markBit;
                private readonly CommonSubscribe<Enum, int> intCommonSubscribe;
                private readonly List<(PackMarkBit<ActiveTargetMode> packBuildTargetMode, BuildTarget? activeBuildTarget)> packBuildTargetModes;
                private readonly Dictionary<BuildTarget, BuildTargetGroup?> buildTargetDic;

                public string[] BuildTargetModes => packBuildTargetModes.Select(x => x.packBuildTargetMode.Key).ToList().ToArrayString();
                public int Current
                {
                    get => intEnumTypeSubscribe.Get<ActiveTargetMode>(markBit);
                    set
                    {
                        // if (value >= packBuildTargetModes.Count)
                        // {
                        //     value = packBuildTargetModes.Count - 1;
                        // }
                        intEnumTypeSubscribe.Set<ActiveTargetMode>(value, markBit);
                    }
                }
                private PackMarkBit<ActiveTargetMode> CurrentPackBuildTargetMode => packBuildTargetModes[Current].packBuildTargetMode;
                public bool CurrentPackBuildTargetModeIsModify => CurrentPackBuildTargetMode.IsModify;
                 
                public int CurrentBuildTarget
                {
                    get
                    {
                        if (packBuildTargetModes[Current].activeBuildTarget != null)
                        {
                            return buildTargetDic.Keys.ToList().IndexOf(packBuildTargetModes[Current].activeBuildTarget.Value);
                        }
                        return intCommonSubscribe.Get(CurrentPackBuildTargetMode.Key, CurrentPackBuildTargetMode.MarkBit);
                    }
                    set
                    {
                        if (!CurrentPackBuildTargetModeIsModify) return; 
                        intCommonSubscribe.Set(CurrentPackBuildTargetMode.Key, value, CurrentPackBuildTargetMode.MarkBit);
                    }
                }

                public string[] BuildTargetStrings => buildTargetDic.Keys.ToArrayString();
                public int[] BuildTargetIndex => buildTargetDic.Values.ToIndexArray();
                
                public bool IsEnableCurrentBuildTargetGroup => buildTargetDic.Values.ToList()[CurrentBuildTarget] != null;
                public int CurrentBuildTargetGroup => CurrentBuildTarget;
                public string[] BuildTargetGroupStrings => buildTargetDic.Values.ToArrayString();
                public int[] BuildTargetGroupIndex => buildTargetDic.Values.ToIndexArray();

                public BuildSettingsBuildTarget(EnumTypeSubscribe<int> intEnumTypeSubscribe, int markBit, CommonSubscribe<Enum, int> intCommonSubscribe, List<(PackMarkBit<ActiveTargetMode> packBuildTargetMode, BuildTarget? activeBuildTarget)> packBuildTargetModes, Dictionary<BuildTarget, 
                        BuildTargetGroup?> buildTargetDic)
                {
                    this.intEnumTypeSubscribe = intEnumTypeSubscribe;
                    this.markBit = markBit;
                    this.intCommonSubscribe = intCommonSubscribe;
                    this.packBuildTargetModes = packBuildTargetModes;
                    this.buildTargetDic = buildTargetDic;
                }
            }

            public class BuildSettingsVersion
            {
                private readonly CommonSubscribe<Enum, string> stringEnumCommonSubscribe;
                private readonly CommonSubscribe<Enum, int> intEnumCommonSubscribe;
                public bool EnableModifyFrame { get; }
                public bool EnableModifyHotUpdate { get; }
                
                public int FrameTemp { get; set; }
                public int HotUpdateTemp { get; set; }
                public int IterateTemp { get; private set; }
                public bool IsModify { get; set; }
                public string? Display
                {
                    get => stringEnumCommonSubscribe.Get(BuildVersion.Display);
                    set => stringEnumCommonSubscribe.Set(BuildVersion.Display, value);
                }

                private  int Frame
                {
                    get => intEnumCommonSubscribe.Get(BuildVersion.Frame);
                    set => intEnumCommonSubscribe.Set(BuildVersion.Frame, value);
                }

                private int HotUpdate
                {
                    get => intEnumCommonSubscribe.Get(BuildVersion.HotUpdate);
                    set => intEnumCommonSubscribe.Set(BuildVersion.HotUpdate, value);
                }

                private int Iterate
                {
                    get => intEnumCommonSubscribe.Get(BuildVersion.Iterate);
                    set => intEnumCommonSubscribe.Set(BuildVersion.Iterate, value);
                }
                
                public BuildSettingsVersion(CommonSubscribe<Enum, string> stringEnumCommonSubscribe, CommonSubscribe<Enum, int> intEnumCommonSubscribe, bool enableModifyFrame, bool enableModifyHotUpdate)
                {
                    this.stringEnumCommonSubscribe = stringEnumCommonSubscribe;
                    this.intEnumCommonSubscribe = intEnumCommonSubscribe;
                    this.EnableModifyFrame = enableModifyFrame;
                    this.EnableModifyHotUpdate = enableModifyHotUpdate;
                }

                public void Init()
                {
                    if (IsModify)
                    {
                        IterateTemp = Iterate + 1;
                        return;
                    }
                    FrameTemp = Frame;
                    HotUpdateTemp = HotUpdate;
                    IterateTemp = Iterate;
                }

                public void Reset()
                {
                    IsModify = false;
                }

                public void Save()
                {
                    Frame = FrameTemp;
                    HotUpdate = HotUpdateTemp;
                    Iterate = IterateTemp;
                    IsModify = false;
                }
            }
        }
    }
}