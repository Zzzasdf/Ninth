using System;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.IO;
using System.Linq;
using Ninth.HotUpdate;
using UnityEditor;

namespace Ninth.Editor
{
    public class BuildProxy : IBuildProxy
    {
        private readonly BuildConfig.BuildSettings buildSettings;

        [Inject]
        public BuildProxy(IBuildConfig buildConfig)
        {
            this.buildSettings = buildConfig.BuildSettings;
        }

        void IOnGUI.OnGUI()
        {
            var verify = true;
            using (new GUILayout.VerticalScope(GUI.skin.GetStyle("FrameBox")))
            {
                if (!RenderBuildFolder())
                {
                    verify = false;
                }
                GUILayout.Space(10);
                if (!RenderBuildAssetGroup())
                {
                    verify = false;
                }
            }
            GUILayout.Space(20);
            using (new GUILayout.VerticalScope(GUI.skin.GetStyle("FrameBox")))
            {
                RenderBuildMode();
                RenderBuildPlatform();
            }
            GUILayout.Space(20);
            using (new GUILayout.VerticalScope(GUI.skin.GetStyle("FrameBox")))
            {
                if (!RenderTargetPlatformBuildInfo())
                {
                    verify = false;
                }
            }
            GUILayout.Space(20);
            if (verify)
            {
                RenderBuildExport();
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
                    if (!x.Contains(Application.dataPath)) return null;
                    return $"该打包路径不能包含 {Application.dataPath}";
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
                    if (x.Contains(Application.dataPath)) return null;
                    return $"该打包路径必须包含 {Application.dataPath}";
                });
                if (!verify)
                {
                    result = false;
                }
            }
            return result;
        }
        
        // 打包模式
        private void RenderBuildMode()
        {
            var barMenu = buildSettings.BuildSettingsModes.Collect.ToArray().ToArrayString();
            var currentIndex = buildSettings.BuildSettingsModes.CurrentIndex;
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("打包模式");
                EditorWindowUtility.Toolbar(currentIndex, barMenu);
            }
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
        public bool RenderTargetPlatformBuildInfo()
        {
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
                return false;
            }
            if (buildTargetPlatformCurrentIndex.Value < 0)
            {
                buildTargetPlatformCurrentIndex.Value = 0;
            }
            else if(buildTargetPlatformCurrentIndex.Value >= buildTargetPlatforms.Count)
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
                GUI.enabled = false;
                buildTargetPlatformSelector.Current.Value = buildTargetPlatforms[buildTargetPlatformCurrentIndex.Value];
                EditorGUILayout.TextField("BuildTarget", buildTargetPlatformSelector.CurrentValue.Item1.ToString());
                EditorGUILayout.TextField("BuildTargetGroup", buildTargetPlatformSelector.CurrentValue.Item2.ToString());
                GUI.enabled = true;
            }
            if (!RenderBuildVersion(buildTargetPlatformSelector.Current.Value))
            {
                return false;
            }
            return true; 
            
            // 版本
            bool RenderBuildVersion(BuildTargetPlatform buildTargetPlatform)
            {
                var result = false;
                if(!buildSettings.PlatformVersions.TryGetValue(buildTargetPlatform, out var versionInfo))
                {
                    versionInfo = new VersionJson();
                    buildSettings.PlatformVersions.Add(buildTargetPlatform, versionInfo);
                }
                var buildSettingsMode = buildSettings.BuildSettingsModes.Current.Value;
                using (new GUILayout.VerticalScope())
                {
                    var displayVersion = new ReactiveProperty<string>(versionInfo.DisplayVersion).AsSetEvent(value => versionInfo.DisplayVersion = value);
                    EditorWindowUtility.TextField("客户端显示版本", displayVersion);
                    if (string.IsNullOrEmpty(displayVersion.Value))
                    {
                        EditorGUILayout.HelpBox("客户端显示版本不能为空", MessageType.Error);
                        result = false;
                    }
                }
                using (new GUILayout.HorizontalScope())
                {
                    GUI.enabled = false;
                    EditorGUILayout.IntField("Frame 版本", versionInfo.FrameVersion + buildSettingsMode switch
                    {
                        BuildSettingsMode.Player => 1,
                        BuildSettingsMode.AllBundle => 1,
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
                return result;
            }
        }

        private void RenderBuildExport()
        {
            if (GUILayout.Button("开始构建"))
            {
                // 检查配置完整性
                var isSuccess = CheckForCompleteness();
                if (!isSuccess)
                {
                    return;
                } 
                "构建成功..".Log();
            } 
        }

        // 检查配置完整性
        private bool CheckForCompleteness()
        {
            return true;
            // // bundle 打包临时缓存路径
            // var bundlesTemp = GetStringByEnumType<BuildFolder>();
            // if (string.IsNullOrEmpty(bundlesTemp))
            // {
            //     "bundle 打包临时缓存路径不能为空".FrameError();
            //     return false;
            // }
            // // 打包路径检查
            // var pathInfo = build.PathssInfo;
            // foreach (var path in pathInfo.Items)
            // {
            //     if (!Directory.Exists(path.Folder))
            //     {
            //         $"无法找到打包路径：{path.Folder}".FrameError();
            //         return false;
            //     }
            //     if (path.Folder.Contains(Application.dataPath))
            //     {
            //         $"打包路径: {path} 不能包含 {Application.dataPath}".FrameError();
            //         return false;
            //     }
            // }
            // // 打包资源组检查
            // var bundleInfo = build.BundlessInfo;
            // var localGroupPaths = bundleInfo.LocalGroupPaths;
            // foreach (var path in localGroupPaths)
            // {
            //     if (!Directory.Exists(path))
            //     {
            //         $"无法找到 Local 打包资源组路径: {path}".FrameError();
            //         return false;
            //     }
            //     if (!path.Contains(Application.dataPath))
            //     {
            //         $"Local 打包资源组路径: {path} 必须包含 {Application.dataPath}".FrameError();
            //         return false;
            //     }
            // }
            // var remoteGroupPaths = bundleInfo.RemoteGroupPaths;
            // if (remoteGroupPaths.Count == 0)
            // {
            //     "Remote 打包资源组至少为一个".FrameError();
            //     return false;
            // }
            // foreach (var path in remoteGroupPaths)
            // {
            //     if (!Directory.Exists(path))
            //     {
            //         $"无法找到 Remote 打包资源组路径: {path}".FrameError();
            //         return false;
            //     }
            //     if (!path.Contains(Application.dataPath))
            //     {
            //         $"Remote 打包资源组路径: {path} 必须包含 {Application.dataPath}".FrameError();
            //         return false;
            //     }
            // }
            // // 拷贝路径检查
            // var copyInfo = build.CopyssInfo;
            // if (!Directory.Exists(copyInfo.Folder))
            // {
            //     $"无法找到拷贝路径：{copyInfo.Folder}".FrameError();
            //     return false;
            // }
            // // 版本检查
            // var versionInfo = build.VersionssInfo;
            // if (string.IsNullOrEmpty(versionInfo.Display))
            // {
            //     "客户端显示的版本为空".FrameWarning();
            // }
            // if (!versionInfo.IsModify)
            // {
            //     "版本未更新".FrameError();
            //     return false;
            // }
            // return true;
        }

        private void Export(BuildConfig.BuildSettings build)
        {
            // 构建 bundle
            // BuildBundles(build);
            // 拷贝 bundle

            // 构建 player
        }

        private void CopyBundles()
        {
        }

        private void BuildPlayer()
        {
        }
    }
}