using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ninth.Utility;
using UnityEngine;
using VContainer;
using System.IO;
using System.Linq;
using Ninth.HotUpdate;
using UnityEditor;

namespace Ninth.Editor
{
    public partial class BuildProxy : IBuildProxy
    {
        private readonly BuildConfig.BuildSettings buildSettings;
        private readonly IPlayerSettingsProxy playerSettingsProxy;
        private readonly INameConfig nameConfig;
        private readonly IJsonProxy jsonProxy;
        private readonly IObjectResolver resolver;

        [Inject]
        public BuildProxy(IBuildConfig buildConfig, IPlayerSettingsProxy playerSettingsProxy, INameConfig nameConfig, IJsonProxy jsonProxy, IObjectResolver resolver)
        {
            this.buildSettings = buildConfig.BuildSettings;
            this.playerSettingsProxy = playerSettingsProxy;
            this.nameConfig = nameConfig;
            this.jsonProxy = jsonProxy;
            this.resolver = resolver;
        }

        void IOnGUI.OnGUI()
        {
            var verify = true;
            using (new GUILayout.VerticalScope(GUI.skin.GetStyle("FrameBox")))
            {
                if (!RenderBuildFolder()) verify = false;
            }
            using (new GUILayout.VerticalScope(GUI.skin.GetStyle("FrameBox")))
            {
                if (!RenderBuildAssetGroup()) verify = false;
            }
            using (new GUILayout.VerticalScope(GUI.skin.GetStyle("FrameBox")))
            {
                if (!RenderBuildMode()) verify = false;
                RenderBuildPlatform();
            }
            using (new GUILayout.VerticalScope(GUI.skin.GetStyle("FrameBox")))
            {
                if (!RenderTargetPlatformBuildInfo()) verify = false;
            }
            if (verify) RenderBuildExport();
            using (new GUILayout.VerticalScope(GUI.skin.GetStyle("FrameBox")))
            {
                RenderCopy();
            }
        }

        // 打包路径
        private bool RenderBuildFolder()
        {
            var result = true;
            var buildFolders = buildSettings.BuildFolders;
            foreach (var buildFolder in buildFolders)
            {
                var (label, defaultName) = buildFolder.Key switch
                {
                    BuildFolder.Bundles => ("Bundle 打包路径", "Bundles"),
                    BuildFolder.Players => ("Player 打包路径", "Players"),
                    _ => throw new ArgumentOutOfRangeException()
                };
                var verify = EditorWindowUtility.SelectFolder(label, buildFolder.Value, defaultName, x =>
                {
                    if (x.Contains(Application.dataPath)) return $"该路径不能包含 {Application.dataPath}";
                    if (!Directory.Exists(x)) return "该路径不存在";
                    return null;
                });
                if (!verify) result = false;
            }
            return result;
        }

        // 打包资源组
        private bool RenderBuildAssetGroup()
        {
            var result = true;
            var buildSettingsItems = buildSettings.BuildSettingsItems;
            foreach (var (_, buildSettingsItem) in buildSettingsItems)
            {
                var assetGroupsInfo = buildSettingsItem.AssetGroupsPaths;
                if (assetGroupsInfo == null)
                {
                    continue;
                }
                var verify = EditorWindowUtility.SelectFolderCollect(assetGroupsInfo.AssetGroupLabel, assetGroupsInfo.AssetGroupPaths, assetGroupsInfo.AssetGroupDefaultName, x =>
                {
                    if (!x.Contains(Application.dataPath)) return $"该路径必须包含 {Application.dataPath}";
                    if (!Directory.Exists(x)) return "该路径不存在";
                    return null;
                });
                if (!verify) result = false;
                break;
            }
            return result;
        }

        // 打包模式
        private bool RenderBuildMode()
        {
            var barMenu = buildSettings.BuildSettingsModes.Collect.ToArray().ToArrayString();
            var currentIndex = buildSettings.BuildSettingsModes.CurrentIndex;
            var current = buildSettings.BuildSettingsModes.Current;
            var buildSettingsItems = buildSettings.BuildSettingsItems;
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("打包模式");
                EditorWindowUtility.Toolbar(currentIndex, barMenu);
            }
            var (result, message) = current.Value switch
            {
                BuildSettingsMode.HotUpdateBundle => 
                    (buildSettingsItems[AssetGroup.Remote].AssetGroupsPaths!.AssetGroupPaths.Value.Count > 0, 
                        "Remote 打包资源组至少有一个路径"),
                BuildSettingsMode.AllBundle or BuildSettingsMode.Player =>
                    (buildSettingsItems[AssetGroup.Local].AssetGroupsPaths!.AssetGroupPaths.Value.Count > 0
                     || buildSettingsItems[AssetGroup.Remote].AssetGroupsPaths!.AssetGroupPaths.Value.Count > 0,
                        "Local 或 Remote 打包资源组至少有一个路径"),
                _ => (true, null)
            };
            if (!result) EditorGUILayout.HelpBox(message, MessageType.Error);
            return result;
        }

        // 打包平台集
        private void RenderBuildPlatform()
        {
            var selectedFlags = buildSettings.BuildTargetPlatform;
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("打包平台集");
                    selectedFlags.Value = (BuildTargetPlatform)EditorGUILayout.EnumFlagsField(selectedFlags.Value);
                }
            }
        }

        public int buildBundleOperateIndex = 0;
        public enum BuildBundleOperate
        {
            None,
            Copy2PlayerStreamingAssets,
            ClearPlayerStreamingAssets
        }

        // 目标平台打包信息
        private bool RenderTargetPlatformBuildInfo()
        {
            var result = true;
            var buildTargetPlatformSelector = buildSettings.BuildTargetPlatformSelector;
            var buildTargetPlatformCurrentIndex = buildSettings.BuildTargetPlatformCurrentIndex;
            var buildTargetPlatforms = new List<BuildTargetPlatform>();
            foreach (var item in buildTargetPlatformSelector.Keys)
            {
                if (buildSettings.BuildTargetPlatform.Value.HasFlag(item))
                    buildTargetPlatforms.Add(item);
            }
            if (buildTargetPlatforms.Count == 0) 
            {
                EditorGUILayout.HelpBox("缺少打包平台", MessageType.Error);
                result = false;
                return result;
            }
            if (buildTargetPlatformCurrentIndex.Value < 0)
                buildTargetPlatformCurrentIndex.Value = 0;
            else if (buildTargetPlatformCurrentIndex.Value >= buildTargetPlatforms.Count)
                buildTargetPlatformCurrentIndex.Value = buildTargetPlatforms.Count - 1;
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("平台信息");
                EditorWindowUtility.Toolbar(buildTargetPlatformCurrentIndex, buildTargetPlatforms.ToArrayString());
            }
            using (new GUILayout.HorizontalScope())
            {
                for (var i = 0; i < buildTargetPlatforms.Count; i++)
                {
                    var item = buildTargetPlatforms[i];
                    if (DisplayVerifyCondition(item)) continue;
                    EditorGUILayout.HelpBox($"{item} => 客户端显示版本不能为空", MessageType.Error);
                    result = false;
                }
            }
            var buildTargetPlatform = buildTargetPlatformSelector.Current.Value;
            if (!buildSettings.PlatformVersions.TryGetValue(buildTargetPlatform, out var buildTargetPlatformInfo))
            {
                buildTargetPlatformInfo = new BuildTargetPlatformInfo();
                buildSettings.PlatformVersions.Add(buildTargetPlatform, buildTargetPlatformInfo);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUI.enabled = false;
                buildTargetPlatformSelector.Current.Value = buildTargetPlatforms[buildTargetPlatformCurrentIndex.Value];
                EditorGUILayout.TextField("BuildTarget", buildTargetPlatformSelector.CurrentValue.Item1.ToString());
                EditorGUILayout.TextField("BuildTargetGroup", buildTargetPlatformSelector.CurrentValue.Item2.ToString());
                GUI.enabled = true;
            }
            var buildSettingsMode = buildSettings.BuildSettingsModes.Current.Value;
            using (new GUILayout.VerticalScope())
            {
                var displayVersion = new ReactiveProperty<string>(buildTargetPlatformInfo.DisplayVersion).AsSetEvent(value => buildTargetPlatformInfo.DisplayVersion = value);
                EditorWindowUtility.TextField("客户端显示版本", displayVersion);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUI.enabled = false;
                EditorGUILayout.IntField("Frame 版本", buildTargetPlatformInfo.FrameVersion + buildSettingsMode switch
                {
                    BuildSettingsMode.Player or BuildSettingsMode.AllBundle => 1,
                    _ => 0,
                });
                EditorGUILayout.IntField("HotUpdate 版本", buildTargetPlatformInfo.HotUpdateVersion + buildSettingsMode switch
                {
                    BuildSettingsMode.HotUpdateBundle => 1,
                    _ => 0,
                });
                EditorGUILayout.IntField("迭代版本", buildTargetPlatformInfo.IterateVersion + 1);
                GUI.enabled = true;
            }

            using (new GUILayout.HorizontalScope())
            {
                buildTargetPlatformInfo.BuildAssetBundleOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumPopup("bundle 压缩选项", buildTargetPlatformInfo.BuildAssetBundleOptions);
                if (buildSettings.BuildSettingsModes.Current.Value == BuildSettingsMode.Player)
                    buildTargetPlatformInfo.BuildOptions = (BuildOptions)EditorGUILayout.EnumPopup("player 压缩选项", buildTargetPlatformInfo.BuildOptions);
            }

            using (new GUILayout.HorizontalScope())
            {
                buildBundleOperateIndex = EditorGUILayout.Popup("生成 bundle 后的操作", buildBundleOperateIndex, Enum.GetNames(typeof(BuildBundleOperate)));
            }
            return result;
            
            bool DisplayVerifyCondition(BuildTargetPlatform buildTargetPlatform)
            {
                if (!buildSettings.PlatformVersions.TryGetValue(buildTargetPlatform, out var info))
                {
                    info = new BuildTargetPlatformInfo();
                    buildSettings.PlatformVersions.Add(buildTargetPlatform, info);
                }

                using (new GUILayout.VerticalScope())
                {
                    var displayVersion = new ReactiveProperty<string>(info.DisplayVersion).AsSetEvent(value => info.DisplayVersion = value);
                    return !string.IsNullOrEmpty(displayVersion.Value);
                }
            }
        }

        private int versionIndex = 0;
        
        private int lockModeIndex = 0;
        public enum LockMode
        {
            None,
            Current,
        }

        private CopyTargetMode copyTargetMode;
        public enum CopyTargetMode
        {
            StreamingAssets = 1 << 0,
            Player = 1 << 1,
        }
        
        private void RenderCopy()
        {
            var buildTargetPlatformSelector = buildSettings.BuildTargetPlatformSelector;
            var buildTargetPlatformCurrentIndex = buildSettings.BuildTargetPlatformCurrentIndex;
            var buildTargetPlatforms = new List<BuildTargetPlatform>();
            foreach (var item in buildTargetPlatformSelector.Keys)
            {
                if (buildSettings.BuildTargetPlatform.Value.HasFlag(item))
                    buildTargetPlatforms.Add(item);
            }

            if (buildTargetPlatforms.Count == 0)
            {
                EditorGUILayout.HelpBox("缺少打包平台", MessageType.Error);
                return;
            }

            if (buildTargetPlatformCurrentIndex.Value < 0)
                buildTargetPlatformCurrentIndex.Value = 0;
            else if (buildTargetPlatformCurrentIndex.Value >= buildTargetPlatforms.Count)
                buildTargetPlatformCurrentIndex.Value = buildTargetPlatforms.Count - 1;
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("拷贝信息");
                EditorWindowUtility.Toolbar(buildTargetPlatformCurrentIndex, buildTargetPlatforms.ToArrayString());
            }

            var lockModes = new[] { LockMode.None, LockMode.Current };

            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("锁定版本模式");
                lockModeIndex = GUILayout.Toolbar(lockModeIndex, lockModes.ToArrayString());
            }

            var isLockCurrent = lockModes[lockModeIndex] == LockMode.Current;
            if (isLockCurrent)
            {
                versionIndex = 0;
            }

            using (new GUILayout.HorizontalScope())
            {
                var versions = new List<string>();
                var folderPath = $"{buildSettings.BuildFolders[BuildFolder.Bundles].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{buildTargetPlatforms[buildTargetPlatformCurrentIndex.Value]}";
                if (!Directory.Exists(folderPath))
                {
                    return;
                }

                foreach (var versionFolder in new DirectoryInfo(folderPath).GetDirectories().Reverse())
                {
                    versions.Add(versionFolder.Name);
                }

                if (isLockCurrent) GUI.enabled = false;
                versionIndex = EditorGUILayout.Popup("拷贝版本", versionIndex, versions.ToArray());
                if (isLockCurrent) GUI.enabled = true;
                if (GUILayout.Button("BundleFolder"))
                {
                    "TODO => OpenBundleFolder".Log();
                }
                if (GUILayout.Button("PlayerFolder"))
                {
                    "TODO => OpenPlayerFolder".Log();
                }
            }

            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("拷贝目标");
                copyTargetMode = (CopyTargetMode)EditorGUILayout.EnumFlagsField(copyTargetMode);
            }

            if (GUILayout.Button("拷贝"))
            {
                "TODO => 拷贝".Log();
            }
        }

        private void RenderBuildExport()
        {
            if (GUILayout.Button("开始构建"))
            {
                var buildTargetPlatformSelector = buildSettings.BuildTargetPlatformSelector;
                var buildSettingsMode = buildSettings.BuildSettingsModes.Current.Value;
                foreach (var item in buildTargetPlatformSelector.Keys)
                {
                    if (buildSettings.BuildTargetPlatform.Value.HasFlag(item))
                    {
                        var versionInfo = buildSettings.PlatformVersions[item];
                        versionInfo.FrameVersion += buildSettingsMode switch
                        {
                            BuildSettingsMode.Player or BuildSettingsMode.AllBundle => 1,
                            _ => 0,
                        };
                        versionInfo.HotUpdateVersion += buildSettingsMode switch
                        {
                            BuildSettingsMode.HotUpdateBundle => 1,
                            _ => 0,
                        };
                        versionInfo.IterateVersion += 1;
                    }
                }
                Export();
            }
            return;

            void Export()
            {
                var produceName = playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName);
                var buildSettingsItems = buildSettings.BuildSettingsItems;
                var buildTargetPlatformSelector = buildSettings.BuildTargetPlatformSelector;
                var buildFolders = buildSettings.BuildFolders;
                var platformVersions = buildSettings.PlatformVersions;
                foreach (var item in buildTargetPlatformSelector.Keys)
                {
                    if (buildSettings.BuildTargetPlatform.Value.HasFlag(item))
                    {
                        if (buildSettings.BuildSettingsModes.Current.Value == BuildSettingsMode.Player)
                        {
                            // 构建 player
                            var buildPlayersConfig = resolver.Resolve<BuildPlayersConfig>();
                            buildPlayersConfig.ProduceName = produceName;
                            buildPlayersConfig.BuildTarget = buildTargetPlatformSelector[item].Item1;
                            buildPlayersConfig.BuildTargetGroup = buildTargetPlatformSelector[item].Item2;
                            buildPlayersConfig.BuildFolder = buildFolders[BuildFolder.Players].Value;
                            buildPlayersConfig.BuildTargetPlatformInfo = platformVersions[item];
                            BuildPlayer(buildPlayersConfig);
                        }

                        // 构建 bundle
                        var buildBundlesConfig = resolver.Resolve<BuildBundlesConfig>();
                        buildBundlesConfig.ProduceName = produceName;
                        buildBundlesConfig.BuildSettingsItems = buildSettingsItems;
                        buildBundlesConfig.BuildTarget = buildTargetPlatformSelector[item].Item1;
                        buildBundlesConfig.BuildFolder = buildFolders[BuildFolder.Bundles].Value;
                        buildBundlesConfig.BuildTargetPlatformInfo = platformVersions[item];
                        BuildBundles(buildBundlesConfig);
                    }
                }

                "构建成功..".Log();
                if (buildSettings.BuildSettingsModes.Current.Value == BuildSettingsMode.Player)
                {
                    Process.Start($"{buildFolders[BuildFolder.Players].Value}/{produceName}");
                }
                else
                {
                    Process.Start($"{buildFolders[BuildFolder.Bundles].Value}/{produceName}");
                }
                UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
            }
        }

        public class BuildBundlesConfig
        {
            public string ProduceName { get; set; }
            public string BuildFolder { get; set; }
            public string BundlePrefix => $"{ProduceName}/{BuildTarget}/{BuildTargetPlatformInfo.BuiltIn()}";
            public Dictionary<AssetGroup, IBuildAssets> BuildSettingsItems { get; set; }
            public BuildTarget BuildTarget { get; set; }
            public BuildTargetPlatformInfo BuildTargetPlatformInfo { get; set; }
        }

        public class BuildPlayersConfig
        {
            public string ProduceName { get; set; }
            public string BuildFolder { get; set; }
            public string PlayerPrefix => $"{ProduceName}/{BuildTarget}/{BuildTargetPlatformInfo.BuiltIn()}";
            public BuildTarget BuildTarget { get; set; }
            public BuildTargetGroup BuildTargetGroup { get; set; }
            public BuildTargetPlatformInfo BuildTargetPlatformInfo { get; set; }
        }
    }
}