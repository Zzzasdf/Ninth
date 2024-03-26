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
using Environment = Ninth.Utility.Environment;

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
                if (!RenderBuildAssetGroup()) verify = false;
            }
            using (new GUILayout.VerticalScope(GUI.skin.GetStyle("FrameBox")))
            {
                if (!RenderBuildMode()) verify = false;
                RenderBuildPlatform();
                if (!RenderTargetPlatformBuildInfo()) verify = false;
            }
            if (verify) RenderBuildExport();
            GUILayout.FlexibleSpace();
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
                BuildSettingsMode.Player =>
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
                    EditorGUILayout.LabelField("平台集");
                    selectedFlags.Value = (BuildTargetPlatform)EditorGUILayout.EnumFlagsField(selectedFlags.Value);
                }
            }
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
                EditorGUILayout.HelpBox("缺少平台", MessageType.Error);
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
                    if (!buildSettings.PlatformVersions.TryGetValue(item, out var info))
                    {
                        info = new BuildTargetPlatformInfo();
                        buildSettings.PlatformVersions.Add(item, info);
                    }
                    var displayVersion = new ReactiveProperty<string>(info.DisplayVersion).AsSetEvent(value => info.DisplayVersion = value);
                    if (!string.IsNullOrEmpty(displayVersion.Value))
                    {
                        continue;
                    }
                    EditorGUILayout.HelpBox($"{item} => 客户端显示版本不能为空", MessageType.Error);
                    result = false;
                }
            }
            using (new GUILayout.HorizontalScope())
            {
                GUI.enabled = false;
                buildTargetPlatformSelector.Current.Value = buildTargetPlatforms[buildTargetPlatformCurrentIndex.Value];
                EditorGUILayout.TextField("BuildTarget", buildTargetPlatformSelector.CurrentValue.BuildTarget.ToString());
                EditorGUILayout.TextField("BuildTargetGroup", buildTargetPlatformSelector.CurrentValue.BuildTargetGroup.ToString());
                GUI.enabled = true;
            }
            var buildTargetPlatform = buildTargetPlatformSelector.Current.Value;
            if (!buildSettings.PlatformVersions.TryGetValue(buildTargetPlatform, out var buildTargetPlatformInfo))
            {
                buildTargetPlatformInfo = new BuildTargetPlatformInfo();
                buildSettings.PlatformVersions.Add(buildTargetPlatform, buildTargetPlatformInfo);
            }
            using (new GUILayout.VerticalScope())
            {
                var displayVersion = new ReactiveProperty<string>(buildTargetPlatformInfo.DisplayVersion).AsSetEvent(value => buildTargetPlatformInfo.DisplayVersion = value);
                EditorWindowUtility.TextField("客户端显示版本", displayVersion);
            }
            var buildSettingsMode = buildSettings.BuildSettingsModes.Current.Value;
            using (new GUILayout.HorizontalScope())
            {
                GUI.enabled = false;
                EditorGUILayout.IntField("Frame 版本", buildTargetPlatformInfo.FrameVersion + buildSettingsMode switch
                {
                    BuildSettingsMode.Player => 1,
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
                {
                    buildTargetPlatformInfo.BuildOptions = (BuildOptions)EditorGUILayout.EnumPopup("player 压缩选项", buildTargetPlatformInfo.BuildOptions);
                }
            }
            if (buildSettings.BuildSettingsModes.Current.Value == BuildSettingsMode.Player)
            {
                using (new GUILayout.HorizontalScope())
                {
                    buildTargetPlatformInfo.Env = (Environment)EditorGUILayout.EnumPopup("运行环境选项", buildTargetPlatformInfo.Env);
                    if (buildTargetPlatformInfo.Env == Environment.Remote)
                    {
                        buildTargetPlatformInfo.Url = EditorGUILayout.TextField("Url 设置", buildTargetPlatformInfo.Url);
                    }
                }
            }
            return result;
        }
        
        private void RenderBuildExport()
        {
            if (GUILayout.Button("开始构建"))
            {
                VersionIncrement();
                Export();
            }
            return;

            void VersionIncrement()
            {
                var buildTargetPlatformSelector = buildSettings.BuildTargetPlatformSelector;
                var buildSettingsMode = buildSettings.BuildSettingsModes.Current.Value;
                foreach (var item in buildTargetPlatformSelector.Keys)
                {
                    if (!buildSettings.BuildTargetPlatform.Value.HasFlag(item))
                    {
                        continue;
                    }
                    var versionInfo = buildSettings.PlatformVersions[item];
                    versionInfo.FrameVersion += buildSettingsMode switch
                    {
                        BuildSettingsMode.Player => 1,
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
            
            void Export()
            {
                var buildSettingsItems = buildSettings.BuildSettingsItems;
                var barMenu = buildSettings.BuildSettingsModes.Collect.ToArray();
                var currentIndex = buildSettings.BuildSettingsModes.CurrentIndex;
                var buildSettingsMode = barMenu[currentIndex.Value];
                if(buildSettingsMode == BuildSettingsMode.HotUpdateBundle)
                {
                    buildSettingsItems.Remove(AssetGroup.Local);
                }
                var produceName = playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName);
                var buildTargetPlatformSelector = buildSettings.BuildTargetPlatformSelector;
                var buildFolders = buildSettings.BuildFolders;
                var platformVersions = buildSettings.PlatformVersions;
                foreach (var platform in buildTargetPlatformSelector.Keys)
                {
                    if (!buildSettings.BuildTargetPlatform.Value.HasFlag(platform))
                    {
                        continue;
                    }
                    platformVersions[platform].BuildSettingsMode = buildSettingsMode;
                    // 构建 bundle
                    var buildBundlesConfig = resolver.Resolve<BuildBundlesConfig>();
                    buildBundlesConfig.ProduceName = produceName;
                    buildBundlesConfig.BuildSettingsItems = buildSettingsItems;
                    buildBundlesConfig.BuildTarget = buildTargetPlatformSelector[platform].BuildTarget;
                    buildBundlesConfig.BuildFolder = buildFolders[BuildFolder.Bundles].Value;
                    buildBundlesConfig.BuildTargetPlatformInfo = platformVersions[platform];
                    BuildBundles(buildBundlesConfig);

                    // 构建 player
                    if (buildSettings.BuildSettingsModes.Current.Value == BuildSettingsMode.Player)
                    {
                        Utility.ClearFolderContents(Application.streamingAssetsPath);
                        // 拷贝 bundle 到 unity streamingAssets
                        if (platformVersions[platform].Env == Environment.Local)
                        {
                            var versionFolder = $"{buildFolders[BuildFolder.Bundles].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{platform}/{platformVersions[platform].BundleByBuiltIn()}";
                            Utility.CopyDirectory($"{versionFolder}/{nameConfig.FolderByRemoteGroup()}", $"{Application.streamingAssetsPath}/{nameConfig.FolderByRemoteGroup()}");
                            Utility.CopyDirectory($"{versionFolder}/{nameConfig.FolderByDllGroup()}", $"{Application.streamingAssetsPath}/{nameConfig.FolderByDllGroup()}");
                        }
                        var baseVersion = GetBaseVersion(platformVersions[platform].FrameVersion.ToString(), platform);
                        var baseVersionFolder = $"{buildFolders[BuildFolder.Bundles].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{platform}/{baseVersion}";
                        Utility.CopyDirectory($"{baseVersionFolder}/{nameConfig.FolderByLocalGroup()}", $"{Application.streamingAssetsPath}/{nameConfig.FolderByLocalGroup()}");
                        File.Copy($"{baseVersionFolder}/{nameConfig.FileNameByVersionConfig()}", $"{Application.streamingAssetsPath}/{nameConfig.FileNameByVersionConfig()}", true);

                        var buildPlayersConfig = resolver.Resolve<BuildPlayersConfig>();
                        buildPlayersConfig.ProduceName = produceName;
                        buildPlayersConfig.BuildTarget = buildTargetPlatformSelector[platform].BuildTarget;
                        buildPlayersConfig.BuildTargetGroup = buildTargetPlatformSelector[platform].BuildTargetGroup;
                        buildPlayersConfig.BuildFolder = buildFolders[BuildFolder.Players].Value;
                        buildPlayersConfig.BuildTargetPlatformInfo = platformVersions[platform];
                        BuildPlayer(buildPlayersConfig);
                        
                        Utility.ClearFolderContents(Application.streamingAssetsPath);
                    }
                    else
                    {
                        // 拷贝 bundle 到对应 player
                        var relativePath = buildTargetPlatformSelector[platform].BundleCopy2PlayerRelativePath;
                        if (string.IsNullOrEmpty(relativePath))
                        {
                            continue;
                        }
                        var frameVersion = platformVersions[platform].FrameVersion;
                        var playerPlatformFolder = $"{buildFolders[BuildFolder.Players].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{platform}";
                        var (folder, env) = GetPlayerFolderName(playerPlatformFolder, frameVersion.ToString());
                        if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(env))
                        {
                            continue;
                        }
                        if (env == Environment.Local.ToString())
                        {
                            var versionFolder = $"{buildFolders[BuildFolder.Bundles].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{platform}/{platformVersions[platform].BundleByBuiltIn()}";
                            Utility.CopyDirectory($"{versionFolder}/{nameConfig.FolderByRemoteGroup()}", $"{folder}/{relativePath}/{nameConfig.FolderByRemoteGroup()}");
                            Utility.CopyDirectory($"{versionFolder}/{nameConfig.FolderByDllGroup()}", $"{folder}/{relativePath}/{nameConfig.FolderByDllGroup()}");
                        }
                        // var baseVersion = GetBaseVersion(platformVersions[platform].FrameVersion.ToString(), platform);
                        // var baseVersionFolder = $"{buildFolders[BuildFolder.Bundles].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{platform}/{baseVersion}"; 
                        // Utility.CopyDirectory($"{baseVersionFolder}/{nameConfig.FolderByLocalGroup()}", $"{Application.streamingAssetsPath}/{nameConfig.FolderByLocalGroup()}");
                        // File.Copy($"{baseVersionFolder}/{nameConfig.FileNameByVersionConfig()}", $"{Application.streamingAssetsPath}/{nameConfig.FileNameByVersionConfig()}", true);
                    }
                }
                AssetDatabase.Refresh();
                UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
            }
        }

        private void RenderCopy()
        {
            var buildTargetPlatformSelector = buildSettings.BuildTargetPlatformSelector;
            var copySettings = buildSettings.CopySettings;
            var copyTargetPlatformIndex = new ReactiveProperty<int>(copySettings.CopyTargetPlatformIndex).AsSetEvent(value => copySettings.CopyTargetPlatformIndex = value);
            var copyLockModes = buildSettings.CopyLockModes;
            var copyLockModeIndex = new ReactiveProperty<int>(copySettings.CopyLockModeIndex).AsSetEvent(value => copySettings.CopyLockModeIndex = value);
            var copyVersionIndex = new ReactiveProperty<int>(copySettings.CopyVersionIndex).AsSetEvent(value => copySettings.CopyVersionIndex = value);
            var buildFolders = buildSettings.BuildFolders;
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("拷贝平台");
                EditorWindowUtility.Toolbar(copyTargetPlatformIndex, buildTargetPlatformSelector.Keys.ToArray().ToArrayString());
            }
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("锁定版本模式");
                EditorWindowUtility.Toolbar(copyLockModeIndex, copyLockModes.ToArrayString());
            }
            var isModify = copyLockModes[copyLockModeIndex.Value] == CopyLockMode.None;
            if (!isModify)
            {
                copyVersionIndex.Value = 0;
            }
            var currentTargetPlatform = buildTargetPlatformSelector.Keys.ToArray()[copyTargetPlatformIndex.Value];
            var currentVersion = string.Empty;
            using (new GUILayout.HorizontalScope())
            {
                var versions = new List<string>();
                var folderPath = $"{buildFolders[BuildFolder.Bundles].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{currentTargetPlatform}";
                if (!Directory.Exists(folderPath))
                {
                    return;
                }
                foreach (var versionFolder in new DirectoryInfo(folderPath).GetDirectories().OrderByDescending(x => int.Parse(x.Name.Split('.')[2])))
                {
                    versions.Add(versionFolder.Name);
                }
                EditorWindowUtility.Popup("拷贝版本", copyVersionIndex, versions.ToArray(), isModify);
                if (copyVersionIndex.Value >= versions.Count)
                {
                    copyVersionIndex.Value = versions.Count - 1;
                }
                currentVersion = versions[copyVersionIndex.Value];
                if (GUILayout.Button("BundleFolder"))
                {
                    var bundleFolder = $"{folderPath}/{currentVersion}";
                    Process.Start(bundleFolder);
                }
                var playerPlatformFolder = $"{buildFolders[BuildFolder.Players].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{currentTargetPlatform}";
                var (playerFolder, _) = GetPlayerFolderName(playerPlatformFolder, currentVersion.Split('.')[0]);
                if (string.IsNullOrEmpty(playerFolder))
                {
                    EditorGUILayout.LabelField("不存在对应的 Player 版本");
                }
                else
                {
                    if (GUILayout.Button("PlayerFolder"))
                    {
                        Process.Start(playerFolder);
                    }
                }
            }
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("拷贝目标");
                if (GUILayout.Button("拷贝到 player"))
                {
                    var relativePath = buildTargetPlatformSelector[currentTargetPlatform].BundleCopy2PlayerRelativePath;
                    if (!string.IsNullOrEmpty(relativePath))
                    {
                        var frameVersion = currentVersion.Split('.')[0];
                        var playerPlatformFolder = $"{buildFolders[BuildFolder.Players].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{currentTargetPlatform}";
                        var (folder, env) = GetPlayerFolderName(playerPlatformFolder, frameVersion);
                        if (!string.IsNullOrEmpty(folder))
                        {
                            var versionFolder = $"{buildFolders[BuildFolder.Bundles].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{currentTargetPlatform}/{currentVersion}";
                            Utility.CopyDirectory($"{versionFolder}/{nameConfig.FolderByRemoteGroup()}", $"{folder}/{relativePath}/{nameConfig.FolderByRemoteGroup()}");
                            Utility.CopyDirectory($"{versionFolder}/{nameConfig.FolderByDllGroup()}", $"{folder}/{relativePath}/{nameConfig.FolderByDllGroup()}");

                            // var baseVersion = GetBaseVersion(currentVersion.Split('.')[0], currentTargetPlatform);
                            // var baseVersionFolder = $"{buildFolders[BuildFolder.Bundles].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{currentTargetPlatform}/{baseVersion}".Log();
                            // Utility.CopyDirectory($"{baseVersionFolder}/{nameConfig.FolderByLocalGroup()}", $"{folder}/{relativePath}/{nameConfig.FolderByLocalGroup()}");
                            // File.Copy($"{baseVersionFolder}/{nameConfig.FileNameByVersionConfig()}", $"{folder}/{relativePath}/{nameConfig.FileNameByVersionConfig()}", true);
                        }
                        AssetDatabase.Refresh();
                    }
                }
            }
        }

        private (string? folder, string? env) GetPlayerFolderName(string playerFolderPath, string frameVersion)
        {
            if (!Directory.Exists(playerFolderPath))
            {
                return (null, null);
            }
            foreach (var folder in Directory.GetDirectories(playerFolderPath))
            {
                var folderName = Path.GetFileName(folder);
                var parts = folderName.Split('.');
                if (!parts[0].Equals(frameVersion))
                {
                    continue;
                }
                return (folder, parts[3]);
            }
            return (null, null);
        }

        private string? GetBaseVersion(string frameVersion, BuildTargetPlatform buildTargetPlatform)
        {
            var bundleFolderPath = $"{buildSettings.BuildFolders[BuildFolder.Bundles].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{buildTargetPlatform}";
            var baseVersion = string.Empty;
            var baseIterate = int.MaxValue;
            foreach (var folder in Directory.GetDirectories(bundleFolderPath))
            {
                var folderName = Path.GetFileName(folder);
                var parts = folderName.Split('.');
                if (!parts[0].Equals(frameVersion))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(baseVersion))
                {
                    baseVersion = folderName;
                    baseIterate = int.Parse(parts[2]);
                }
                else
                {
                    var iterate = int.Parse(parts[2]);
                    if (baseIterate < iterate)
                    {
                        continue;
                    }
                    baseVersion = folderName;
                    baseIterate = iterate;
                }
            }
            return baseVersion;
        }

        public class BuildBundlesConfig
        {
            public string ProduceName { get; set; }
            public string BuildFolder { get; set; }
            public string BundlePrefix() => $"{ProduceName}/{BuildTarget}/{BuildTargetPlatformInfo.BundleByBuiltIn()}";
            public Dictionary<AssetGroup, IBuildAssets> BuildSettingsItems { get; set; }
            public BuildTarget BuildTarget { get; set; }
            public BuildTargetPlatformInfo BuildTargetPlatformInfo { get; set; }
        }

        public class BuildPlayersConfig
        {
            public string ProduceName { get; set; }
            public string BuildFolder { get; set; }
            public string PlayerPrefix => $"{ProduceName}/{BuildTarget}/{BuildTargetPlatformInfo.PlayerByBuiltIn()}";
            public BuildTarget BuildTarget { get; set; }
            public BuildTargetGroup BuildTargetGroup { get; set; }
            public BuildTargetPlatformInfo BuildTargetPlatformInfo { get; set; }
        }
    }
}