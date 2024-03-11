using System;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.IO;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Ninth.HotUpdate;
using UnityEditor;

namespace Ninth.Editor
{
    public partial class BuildProxy : IBuildProxy
    {
        private readonly BuildConfig.BuildSettings buildSettings;
        private readonly IObjectResolver resolver;
        
        [Inject]
        public BuildProxy(IBuildConfig buildConfig, IObjectResolver resolver)
        {
            this.buildSettings = buildConfig.BuildSettings;
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

            GUILayout.Space(10);
            using (new GUILayout.VerticalScope(GUI.skin.GetStyle("FrameBox")))
            {
                if (!RenderBuildMode()) verify = false;
                RenderBuildPlatform();
            }

            using (new GUILayout.VerticalScope(GUI.skin.GetStyle("FrameBox")))
            {
                if (!RenderTargetPlatformBuildInfo()) verify = false;
            }

            GUILayout.Space(10);
            if (verify) RenderBuildExport();
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
                    if (x.Contains(Application.dataPath))
                    {
                        return $"该路径不能包含 {Application.dataPath}";
                    }
                    if(!Directory.Exists(x))
                    {
                        return "该路径不存在";
                    }
                    return null;
                });
                if (!verify)
                {
                    result = false;
                }
            }
            return result;
        }

        // 打包资源组
        private bool RenderBuildAssetGroup()
        {
            var result = true;
            var buildAssetGroups = buildSettings.AssetGroups;
            foreach (var assetGroup in buildAssetGroups)
            {
                var (label, defaultName) = assetGroup.Key switch
                {
                    AssetGroup.Local => ("Local 打包资源组", "LocalGroup"),
                    AssetGroup.Remote => ("Remote 打包资源组", "RemoteGroup"),
                    _ => throw new ArgumentOutOfRangeException()
                };
                var verify = EditorWindowUtility.SelectFolderCollect(label, assetGroup.Value, defaultName, x =>
                {
                    if (!x.Contains(Application.dataPath))
                    {
                        return $"该路径必须包含 {Application.dataPath}";
                    }
                    if (!Directory.Exists(x))
                    {
                        return "该路径不存在";
                    }
                    return null;
                });
                if (!verify)
                {
                    result = false;
                }
            }
            return result;
        }

        // 打包模式
        private bool RenderBuildMode()
        {
            var barMenu = buildSettings.BuildSettingsModes.Collect.ToArray().ToArrayString();
            var currentIndex = buildSettings.BuildSettingsModes.CurrentIndex;
            var current = buildSettings.BuildSettingsModes.Current;
            var assetGroups = buildSettings.AssetGroups;
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("打包模式");
                EditorWindowUtility.Toolbar(currentIndex, barMenu);
            }
            var (result, message) = current.Value switch
            {
                BuildSettingsMode.HotUpdateBundle => (assetGroups[AssetGroup.Remote].Value.Count > 0, "Remote 打包资源组至少有一个路径"),
                BuildSettingsMode.AllBundle or BuildSettingsMode.Player => (assetGroups[AssetGroup.Local].Value.Count > 0 || assetGroups[AssetGroup.Remote].Value.Count > 0, "Local 或 Remote 打包资源组至少有一个路径"),
                _ => (true, null)
            };
            if (!result)
            {
                EditorGUILayout.HelpBox(message, MessageType.Error);
            }
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
                {
                    buildTargetPlatforms.Add(item);
                }
            }
            if (buildTargetPlatforms.Count == 0)
            {
                EditorGUILayout.HelpBox("缺少打包平台", MessageType.Error);
                result = false;
                return result;
            }
            if (buildTargetPlatformCurrentIndex.Value < 0)
            {
                buildTargetPlatformCurrentIndex.Value = 0;
            }
            else if (buildTargetPlatformCurrentIndex.Value >= buildTargetPlatforms.Count)
            {
                buildTargetPlatformCurrentIndex.Value = buildTargetPlatforms.Count - 1;
            }
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("目标平台打包信息");
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
            using (new GUILayout.HorizontalScope())
            {
                GUI.enabled = false;
                buildTargetPlatformSelector.Current.Value = buildTargetPlatforms[buildTargetPlatformCurrentIndex.Value];
                EditorGUILayout.TextField("BuildTarget", buildTargetPlatformSelector.CurrentValue.Item1.ToString());
                EditorGUILayout.TextField("BuildTargetGroup", buildTargetPlatformSelector.CurrentValue.Item2.ToString());
                GUI.enabled = true;
            }
            RenderBuildVersion(buildTargetPlatformSelector.Current.Value);
            return result;

            // 版本
            void RenderBuildVersion(BuildTargetPlatform buildTargetPlatform)
            {
                if (!buildSettings.PlatformVersions.TryGetValue(buildTargetPlatform, out var versionInfo))
                {
                    versionInfo = new VersionJson();
                    buildSettings.PlatformVersions.Add(buildTargetPlatform, versionInfo);
                }
                var buildSettingsMode = buildSettings.BuildSettingsModes.Current.Value;
                using (new GUILayout.VerticalScope())
                {
                    var displayVersion = new ReactiveProperty<string>(versionInfo.DisplayVersion).AsSetEvent(value => versionInfo.DisplayVersion = value);
                    EditorWindowUtility.TextField("客户端显示版本", displayVersion);
                }
                using (new GUILayout.HorizontalScope())
                {
                    GUI.enabled = false;
                    EditorGUILayout.IntField("Frame 版本", versionInfo.FrameVersion + buildSettingsMode switch
                    {
                        BuildSettingsMode.Player or BuildSettingsMode.AllBundle => 1,
                        _ => 0,
                    });
                    EditorGUILayout.IntField("HotUpdate 版本", versionInfo.HotUpdateVersion + buildSettingsMode switch
                    {
                        BuildSettingsMode.HotUpdateBundle => 1,
                        _ => 0,
                    });
                    EditorGUILayout.IntField("迭代版本", versionInfo.IterateVersion + 1);
                    GUI.enabled = true;
                }
            }
            bool DisplayVerifyCondition(BuildTargetPlatform buildTargetPlatform)
            {
                if (!buildSettings.PlatformVersions.TryGetValue(buildTargetPlatform, out var versionInfo))
                {
                    versionInfo = new VersionJson();
                    buildSettings.PlatformVersions.Add(buildTargetPlatform, versionInfo);
                }
                using (new GUILayout.VerticalScope())
                {
                    var displayVersion = new ReactiveProperty<string>(versionInfo.DisplayVersion).AsSetEvent(value => versionInfo.DisplayVersion = value);
                    return !string.IsNullOrEmpty(displayVersion.Value);
                }
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
        }
        
        private void Export()
        {
            var assetGroups = buildSettings.AssetGroups;
            var buildTargetPlatformSelector = buildSettings.BuildTargetPlatformSelector;
            var buildFolders = buildSettings.BuildFolders;
            var platformVersions = buildSettings.PlatformVersions;
            var buildSettingsMode = buildSettings.BuildSettingsModes.Current.Value;
            foreach (var item in buildTargetPlatformSelector.Keys)
            {
                if (buildSettings.BuildTargetPlatform.Value.HasFlag(item))
                {
                    // 构建 bundle
                    var buildBundlesConfig = resolver.Resolve<BuildBundlesConfig>();
                    buildBundlesConfig.AssetGroups = assetGroups;
                    buildBundlesConfig.BuildTarget = buildTargetPlatformSelector[item].Item1;
                    buildBundlesConfig.BuildFolder = buildFolders[BuildFolder.Bundles].Value;
                    buildBundlesConfig.VersionJson = platformVersions[item];
                    BuildBundles(buildBundlesConfig);
                    
                    // 构建 player
                    // TODO ..
                }
            }
            "构建成功..".Log();
        }

        public class BuildBundlesConfig
        {
            private readonly string produceName; 
            public string PrefixFolder => $"{produceName}/{BuildTarget}/{VersionJson.DisplayVersion}({VersionJson.BuiltIn})/";
            public Dictionary<AssetGroup, ReactiveProperty<List<string>>> AssetGroups { get; set; }
            public BuildTarget BuildTarget { get; set; }
            public string BuildFolder { get; set; }
            public VersionJson VersionJson { get; set; }
            
            [Inject]
            public BuildBundlesConfig(IPlayerSettingsProxy playerSettingsProxy)
            {
                produceName = playerSettingsProxy.Get(PLAY_SETTINGS.ProduceName);
            }
        }
    }
}