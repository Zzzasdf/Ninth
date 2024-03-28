using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ninth.Utility;
using UnityEngine;
using VContainer;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
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
                BuildSettingsMode.HotUpdate => 
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
                    BuildSettingsMode.HotUpdate => 1,
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
                        BuildSettingsMode.HotUpdate => 1,
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
                if(buildSettingsMode == BuildSettingsMode.HotUpdate)
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
                        Directory.Delete(Application.streamingAssetsPath, true);
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
                        Directory.Delete(Application.streamingAssetsPath, true);
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
                        var playerFolderName = GetPlayerFolderName(playerPlatformFolder, frameVersion.ToString());
                        var playerFolder = $"{buildFolders[BuildFolder.Players].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{platform}/{playerFolderName}";
                        var versionPath = $"{playerFolder}/{relativePath}/{nameConfig.FileNameByVersionConfig()}";
                        if (!string.IsNullOrEmpty(relativePath) && File.Exists(versionPath))
                        {
                            var jsonData = File.ReadAllText(versionPath, new UTF8Encoding(false));
                            var versionConfig = LitJson.JsonMapper.ToObject<PlayerVersionConfig>(jsonData);
                            if (versionConfig.Env == Environment.Local)
                            {
                                var versionFolder = $"{buildFolders[BuildFolder.Bundles].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{platform}/{platformVersions[platform].BundleByBuiltIn()}";
                                Utility.CopyDirectory($"{versionFolder}/{nameConfig.FolderByRemoteGroup()}", $"{playerFolder}/{relativePath}/{nameConfig.FolderByRemoteGroup()}");
                                Utility.CopyDirectory($"{versionFolder}/{nameConfig.FolderByDllGroup()}", $"{playerFolder}/{relativePath}/{nameConfig.FolderByDllGroup()}");
                            }
                        }
                    }
                }
                AssetDatabase.Refresh();
                UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
            }
        }

        private void RenderCopy()
        {
            var copySettings = buildSettings.CopySettings;
            var copyTargetPlatformIndex = new ReactiveProperty<int>(copySettings.TargetPlatformIndex).AsSetEvent(value => copySettings.TargetPlatformIndex = value);
            var copyLockModeIndex = new ReactiveProperty<int>(copySettings.LockModeIndex).AsSetEvent(value => copySettings.LockModeIndex = value);
            var copyBundleVersionIndex = new ReactiveProperty<int>(copySettings.BundleVersionIndex).AsSetEvent(value => copySettings.BundleVersionIndex = value);
            var copyPlayerVersionIndex = new ReactiveProperty<int>(copySettings.PlayerVersionIndex).AsSetEvent(value => copySettings.PlayerVersionIndex = value);
            var buildFolders = buildSettings.BuildFolders;
            var buildTargetPlatformSelector = buildSettings.BuildTargetPlatformSelector;
            var copyLockModes = buildSettings.CopyLockModes;
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("拷贝平台");
                if (EditorWindowUtility.Toolbar(copyTargetPlatformIndex, buildTargetPlatformSelector.Keys.ToArray().ToArrayString()))
                {
                    copyBundleVersionIndex.Value = 0;
                    copyPlayerVersionIndex.Value = 0;
                }
            }
            
            var currentTargetPlatform = buildTargetPlatformSelector.Keys.ToArray()[copyTargetPlatformIndex.Value];
            var bundleVersions = new List<string> { "nil" };
            var bundleFolderPath = $"{buildFolders[BuildFolder.Bundles].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{currentTargetPlatform}";
            if (Directory.Exists(bundleFolderPath))
            {
                foreach (var versionFolder in new DirectoryInfo(bundleFolderPath).GetDirectories().OrderByDescending(x => int.Parse(x.Name.Split('.')[2])))
                {
                    bundleVersions.Add(versionFolder.Name);
                }
            }
            var playerVersions = new List<string> { "nil" };
            var playerFolderPath = $"{buildFolders[BuildFolder.Players].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{currentTargetPlatform}";
            if (Directory.Exists(playerFolderPath))
            {
                foreach (var versionFolder in new DirectoryInfo(playerFolderPath).GetDirectories().OrderByDescending(x => int.Parse(x.Name.Split('.')[2])))
                {
                    playerVersions.Add(versionFolder.Name);
                }
            }
            if (copyBundleVersionIndex.Value >= bundleVersions.Count)
            {
                copyBundleVersionIndex.Value = 0;
            }
            if (copyPlayerVersionIndex.Value >= playerVersions.Count)
            {
                copyPlayerVersionIndex.Value = 0;
            }
            switch (copyBundleVersionIndex.Value == 0, copyPlayerVersionIndex.Value == 0)
            {
                case (false, false): SetPlayerVersionIndex(); break;
                case (true, false): SetBundleVersionIndex(); break;
                case (false, true): SetPlayerVersionIndex(); break;
            }
            var isModify = copyLockModes[copyLockModeIndex.Value] == CopyLockMode.None;
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("锁定模式");
                EditorWindowUtility.Toolbar(copyLockModeIndex, copyLockModes.ToArrayString());
            }
            if (!isModify)
            {
                copyBundleVersionIndex.Value = bundleVersions.Count > 1 ? 1 : 0;
                SetPlayerVersionIndex();
            }
            // Bundle
            using (new GUILayout.HorizontalScope())
            {
                if (EditorWindowUtility.Popup("Bundle 版本", copyBundleVersionIndex, bundleVersions.ToArray(), isModify))
                {
                    SetPlayerVersionIndex();
                }
                var currentBundleVersion = bundleVersions[copyBundleVersionIndex.Value];
                var bundleFolder = $"{bundleFolderPath}/{currentBundleVersion}";
                if (!Directory.Exists(bundleFolder))
                {
                    EditorGUILayout.LabelField("不存在对应的 Bundle 版本");
                }
                else
                {
                    if (GUILayout.Button("BundleFolder"))
                    {
                        Process.Start(bundleFolder);
                    }
                }
                var relativePath = buildTargetPlatformSelector[currentTargetPlatform].BundleCopy2PlayerRelativePath;
                var playerPlatformFolder = $"{buildFolders[BuildFolder.Players].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{currentTargetPlatform}";
                var frameVersion = currentBundleVersion.Split('.')[0];
                var playerFolderName = GetPlayerFolderName(playerPlatformFolder, frameVersion);
                var playerFolder = $"{buildFolders[BuildFolder.Players].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{currentTargetPlatform}/{playerFolderName}";
                var versionPath = $"{playerFolder}/{relativePath}/{nameConfig.FileNameByVersionConfig()}";
                if (!string.IsNullOrEmpty(relativePath) && File.Exists(versionPath))
                {
                    var jsonData = File.ReadAllText(versionPath, new UTF8Encoding(false));
                    var versionConfig = LitJson.JsonMapper.ToObject<PlayerVersionConfig>(jsonData);
                    if (versionConfig.Env == Environment.Local)
                    {
                        if (GUILayout.Button(" 拷贝到 Player "))
                        {
                            if (!string.IsNullOrEmpty(playerFolderName))
                            {
                                var versionFolder = $"{buildFolders[BuildFolder.Bundles].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{currentTargetPlatform}/{currentBundleVersion}";
                                Utility.CopyDirectory($"{versionFolder}/{nameConfig.FolderByRemoteGroup()}", $"{playerFolder}/{relativePath}/{nameConfig.FolderByRemoteGroup()}");
                                Utility.CopyDirectory($"{versionFolder}/{nameConfig.FolderByDllGroup()}", $"{playerFolder}/{relativePath}/{nameConfig.FolderByDllGroup()}");
                            }
                            AssetDatabase.Refresh();
                        }
                    }
                }
            }
            
            // Player
            using (new GUILayout.HorizontalScope())
            {
                if(EditorWindowUtility.Popup("Player 版本", copyPlayerVersionIndex, playerVersions.ToArray(), isModify))
                {
                    SetBundleVersionIndex();
                }
                var currentPlayerVersion = playerVersions[copyPlayerVersionIndex.Value];
                var playerPlatformFolder = $"{buildFolders[BuildFolder.Players].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{currentTargetPlatform}";
                var playerFolderName = GetPlayerFolderName(playerPlatformFolder, currentPlayerVersion.Split('.')[0]);
                var playerFolder = $"{playerPlatformFolder}/{playerFolderName}";
                if (string.IsNullOrEmpty(playerFolderName) || !Directory.Exists(playerFolder))
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
                var relativePath = buildTargetPlatformSelector[currentTargetPlatform].BundleCopy2PlayerRelativePath;
                var versionPath = $"{playerFolder}/{relativePath}/{nameConfig.FileNameByVersionConfig()}";
                if (File.Exists(versionPath))
                {
                    var jsonData = File.ReadAllText(versionPath, new UTF8Encoding(false));
                    var versionConfig = LitJson.JsonMapper.ToObject<PlayerVersionConfig>(jsonData);
                    switch (versionConfig.Env)
                    {
                        case Environment.Local:
                        {
                            if (GUILayout.Button("切换成 Remote"))
                            {
                                versionConfig.Env = Environment.Remote;
                                File.WriteAllText(versionPath, ConvertJsonString(LitJson.JsonMapper.ToJson(versionConfig)), new UTF8Encoding(false));
                                Directory.Delete($"{playerFolder}/{relativePath}/{nameConfig.FolderByRemoteGroup()}", true);
                                Directory.Delete($"{playerFolder}/{relativePath}/{nameConfig.FolderByDllGroup()}", true);
                                new DirectoryInfo(playerFolder).MoveTo(playerFolder.Replace($"{Environment.Local}", $"{Environment.Remote}"));
                                AssetDatabase.Refresh();
                            }
                            break;
                        }
                        case Environment.Remote:
                        {
                            if (GUILayout.Button("切换成 Local "))
                            {
                                versionConfig.Env = Environment.Local;
                                File.WriteAllText(versionPath, ConvertJsonString(LitJson.JsonMapper.ToJson(versionConfig)), new UTF8Encoding(false));
                                var bundleVersionFolder = $"{buildFolders[BuildFolder.Bundles].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{currentTargetPlatform}/{bundleVersions[copyBundleVersionIndex.Value]}";
                                Utility.CopyDirectory($"{bundleVersionFolder}/{nameConfig.FolderByRemoteGroup()}", $"{playerFolder}/{relativePath}/{nameConfig.FolderByRemoteGroup()}");
                                Utility.CopyDirectory($"{bundleVersionFolder}/{nameConfig.FolderByDllGroup()}", $"{playerFolder}/{relativePath}/{nameConfig.FolderByDllGroup()}");
                                new DirectoryInfo(playerFolder).MoveTo(playerFolder.Replace($"{Environment.Remote}", $"{Environment.Local}"));
                                AssetDatabase.Refresh();
                            }
                            break;
                        }
                    }
                }
            }
            return;
            
            void SetPlayerVersionIndex()
            {
                var relativePath = buildTargetPlatformSelector[currentTargetPlatform].BundleCopy2PlayerRelativePath;
                if (copyBundleVersionIndex.Value == 0 || string.IsNullOrEmpty(relativePath))
                {
                    copyPlayerVersionIndex.Value = 0;
                }
                else
                {
                    var targetBundleVersion = bundleVersions[copyBundleVersionIndex.Value];
                    var frameVersion = targetBundleVersion.Split('.')[0];
                    var playerPlatformFolder = $"{buildFolders[BuildFolder.Players].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{currentTargetPlatform}";
                    var folderName = GetPlayerFolderName(playerPlatformFolder, frameVersion);
                    copyPlayerVersionIndex.Value = folderName == null ? 0 : playerVersions.IndexOf(folderName);
                }
            }

            void SetBundleVersionIndex()
            {
                var relativePath = buildTargetPlatformSelector[currentTargetPlatform].BundleCopy2PlayerRelativePath;
                if (copyPlayerVersionIndex.Value == 0 || string.IsNullOrEmpty(relativePath))
                {
                    copyBundleVersionIndex.Value = 0;
                }
                else
                {
                    var targetPlayerVersion = playerVersions[copyPlayerVersionIndex.Value];
                    var frameVersion = targetPlayerVersion.Split('.')[0];
                    var bundlePlatformFolder = $"{buildFolders[BuildFolder.Bundles].Value}/{playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName)}/{currentTargetPlatform}";
                    var folderName = GetPlayerFolderName(bundlePlatformFolder, frameVersion);
                    copyBundleVersionIndex.Value = folderName == null ? 0 : bundleVersions.IndexOf(folderName);
                }
            }
        }

        private string? GetPlayerFolderName(string playerFolderPath, string frameVersion)
        {
            if (!Directory.Exists(playerFolderPath))
            {
                return null;
            }
            foreach (var folder in Directory.GetDirectories(playerFolderPath))
            {
                var folderName = Path.GetFileName(folder);
                var parts = folderName.Split('.');
                if (!parts[0].Equals(frameVersion))
                {
                    continue;
                }
                return folderName;
            }
            return null;
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
        
        private static string ConvertJsonString(string str)
        {
            var serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            var jtr = new JsonTextReader(tr);
            var obj = serializer.Deserialize(jtr);
            if (obj == null)
            {
                return str;
            }
            var textWriter = new StringWriter();
            var jsonWriter = new JsonTextWriter(textWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' '
            };
            serializer.Serialize(jsonWriter, obj);
            return textWriter.ToString();
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