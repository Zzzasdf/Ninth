using System;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace Ninth.Editor
{
    public class BuildConfig : IBuildConfig
    {
        private readonly EnumTypeSubscribe<int> intEnumTypeSubscribe;
        private readonly CommonSubscribe<Enum, string> stringCommonSubscribe;
        private readonly CommonSubscribe<Enum, int> intCommonSubscribe;
        private readonly CommonSubscribe<BuildSettingsMode, BuildSettings> tabCommonSubscribe;

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
                var build = intEnumTypeSubscribe = new EnumTypeSubscribe<int>();
                build.Subscribe<BuildSettingsMode>((int)common.BuildSettingsType).AsSetEvent(value => common.BuildSettingsType = (BuildSettingsMode)value);

                build.Subscribe<BuildBundleMode>((int)bundle.BuildBundleBundleCurrentMode, 0).AsSetEvent(value => bundle.BuildBundleBundleCurrentMode = (BuildBundleMode)value);
                build.Subscribe<BuildBundleMode>((int)player.BuildPlayerBundleCurrentMode, 1).AsSetEvent(value => player.BuildPlayerBundleCurrentMode = (BuildBundleMode)value);
                
                // build.Subscribe<BuildExportCopyFolderMode>((int)common.BuildExportDirectoryType).AsSetEvent(value => common.BuildExportDirectoryType = (BuildExportCopyFolderMode)value);
                // build.Subscribe<ActiveTargetMode>((int)common.ActiveTargetMode).AsSetEvent(value => common.ActiveTargetMode = (ActiveTargetMode)value);
                // build.Subscribe<BuildTarget>((int)common.BuildTarget).AsSetEvent(value => common.BuildTarget = (BuildTarget)value);
                // build.Subscribe<BuildTargetGroup>((int)common.BuildTargetGroup).AsSetEvent(value => common.BuildTargetGroup = (BuildTargetGroup)value);
            }

            {
                var build = stringCommonSubscribe = new CommonSubscribe<Enum, string>(); 
                build.Subscribe(BuildFolder.DisaplayVerison, common.DisplayVersion).AsSetEvent(value => common.DisplayVersion = value);
                build.Subscribe(BuildFolder.Bundles, bundle.BuildBundlesDirectoryRoot, 0).AsSetEvent(value => bundle.BuildBundlesDirectoryRoot = value);
                build.Subscribe(BuildFolder.Bundles, player.BuildBundlesDirectoryRoot, 1).AsSetEvent(value => player.BuildBundlesDirectoryRoot = value);
                build.Subscribe(BuildFolder.Players, player.BuildPlayersDirectoryRoot).AsSetEvent(value => player.BuildPlayersDirectoryRoot = value);
            }

            {
                var build = intCommonSubscribe = new CommonSubscribe<Enum, int>();
                build.Subscribe(BuildVersion.HotUpdate, common.HotUpdateVersion).AsSetEvent(value => common.HotUpdateVersion = value);
                build.Subscribe(BuildVersion.Iterate, common.IterateVersion).AsSetEvent(value => common.IterateVersion = value);
                build.Subscribe(BuildVersion.Frame, player.FrameVersion).AsSetEvent(value => player.FrameVersion = value);
            }

            {
                var build = tabCommonSubscribe = new CommonSubscribe<BuildSettingsMode, BuildSettings>();
                build.Subscribe(BuildSettingsMode.Bundle, new BuildSettings(
                    new List<BuildSettings.BuildSettingsPath>
                    {
                        new("选择 bundle 打包的根节点", (stringCommonSubscribe, new PackMarkBit<BuildFolder>(BuildFolder.Bundles,0)), "选择目标文件夹", "Bundles"),
                    }, 
                    new BuildSettings.BuildSettingsBundle<BuildBundleMode>(
                        intEnumTypeSubscribe, 0,
                        new List<BuildBundleMode>
                        {
                            BuildBundleMode.HotUpdateBundles,
                            BuildBundleMode.AllBundles,
                        })
                    ));
                build.Subscribe(BuildSettingsMode.Player, new BuildSettings(
                    new List<BuildSettings.BuildSettingsPath>
                    {
                        new("选择 bundle 打包的根节点", (stringCommonSubscribe, new PackMarkBit<BuildFolder>(BuildFolder.Bundles, 1)), "选择目标文件夹", "Bundles"),
                        new("选择 player 打包的根节点", (stringCommonSubscribe, new PackMarkBit<BuildFolder>(BuildFolder.Players, 0)), "选择目标文件夹","Players"),
                    },
                    new BuildSettings.BuildSettingsBundle<BuildBundleMode>(
                        intEnumTypeSubscribe, 1,
                        new List<BuildBundleMode>
                        {
                            BuildBundleMode.AllBundles
                        })
                    ));
            }
        }

        public class BuildSettings
        {
            public List<BuildSettingsPath> Paths { get; }
            public BuildSettingsBundle<BuildBundleMode> BundleMode { get; }

            public BuildSettings(List<BuildSettingsPath> paths, BuildSettingsBundle<BuildBundleMode> bundleMode)
            {
                this.Paths = paths;
                this.BundleMode = bundleMode;
            }

            public class BuildSettingsPath
            {
                public string Label { get; }

                private readonly CommonSubscribe<Enum, string> stringCommonSubscribe;
                private readonly PackMarkBit<BuildFolder> packMarkBit;
                public string Folder
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

            public class BuildSettingsBundle<T>
                where T : Enum
            {
                private EnumTypeSubscribe<int> intEnumTypeSubscribe;
                private int markBit;
                public int Current
                {
                    get => intEnumTypeSubscribe.Get<T>(markBit);
                    set => intEnumTypeSubscribe.Set<T>(value, markBit);
                }
                public List<BuildBundleMode> BuildBundleModes { get; }
                
                public BuildSettingsBundle(EnumTypeSubscribe<int> intEnumTypeSubscribe, int markBit, List<BuildBundleMode> buildBundleModes)
                {
                    this.intEnumTypeSubscribe = intEnumTypeSubscribe;
                    this.markBit = markBit;
                    this.BuildBundleModes = buildBundleModes;
                }
            }
        }
    }
}