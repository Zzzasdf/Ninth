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
    public class BuildProxy: IBuildProxy
    {
        private readonly IBuildConfig buildConfig;
        private readonly BuildConfig.BuildSettings buildSettings;
        private readonly VersionConfig versionConfig;
        
        [Inject]
        public BuildProxy(IBuildConfig buildConfig, VersionConfig versionConfig)
        {
            this.buildConfig = buildConfig;
            this.buildSettings = buildConfig.BuildSettings;
            this.versionConfig = versionConfig;
        }
        
        void IOnGUI.OnGUI()
        {
            using (new GUILayout.VerticalScope())
            {
                RenderTabs();
                GUILayout.Space(10);
                RenderBuildFolder();
                GUILayout.Space(30);
                RenderBuildBundleMode();
                RenderBuildAssetGroups();
                GUILayout.Space(30);
                RenderBuildCopyPath();
                GUILayout.Space(30);
                RenderBuildTargetMode();
                // RenderBuildVersion();
                // RenderBuildExport();
            }
        }
        
        private void RenderTabs()
        {
            var barMenu = buildSettings.BuildSettingsModes.Keys.ToArray().ToArrayString();
            var currentIndex = buildSettings.BuildSettingsModes.CurrentIndex;
            EditorWindowUtility.ToolBar(currentIndex, barMenu);
        }

        private void RenderBuildFolder()
        {
            var buildFolders = buildSettings.BuildSettingsModes.CurrentValue.BuildFolders;
            var buildSettingsModeCurrent = buildSettings.BuildSettingsModes.Current;
            var folderSubscribe = buildConfig.StringSubscribe;
            Func<string, bool> condition = (folder) =>
            {
                if (!folder.Contains(Application.dataPath)) return true;
                $"打包路径: {folder} 不能包含 {Application.dataPath}".FrameError();
                return false;
            };
            foreach (var buildFolder in buildFolders)
            {
                var (label, folder, defaultName)
                    = (buildSettingsModeCurrent.Value, buildFolder) switch
                    {
                        (BuildSettingsMode.Bundle, BuildFolder.Bundles) =>
                            ("bundle 打包路径", folderSubscribe.GetReactiveProperty(BuildFolder.Bundles), "Bundles"),
                        (BuildSettingsMode.Player, BuildFolder.Bundles) =>
                            ("bundle 打包路径", folderSubscribe.GetReactiveProperty(BuildFolder.Bundles, 1), "Bundles"),
                        (BuildSettingsMode.Player, BuildFolder.Players) =>
                            ("player 打包路径", folderSubscribe.GetReactiveProperty(BuildFolder.Players), "Players"),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                EditorWindowUtility.SelectFolder(label, (folder as ReactiveProperty<string>)!, defaultName, condition);
            }
        }

        private void RenderBuildBundleMode()
        {
            var barMenu = buildSettings.BuildSettingsModes.CurrentValue.BuildBundleModes.ToArray().ToArrayString();
            var currentIndex = buildSettings.BuildSettingsModes.CurrentValue.BuildBundleModes.CurrentIndex;
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("选择 bundle 的打包模式 => ", EditorStyles.boldLabel);
                EditorWindowUtility.ToolBar(currentIndex, barMenu);
            }
        }

        private void RenderBuildAssetGroups()
        {
            var buildAssetGroups = buildSettings.BuildSettingsModes.CurrentValue.BuildAssetGroups;
            var folderListSubscribe = buildConfig.StringListSubscribe;
            foreach (var assetGroup in buildAssetGroups)
            {
                var (label,  folders, defaultName) = assetGroup switch
                {
                    AssetGroup.Local => ("Local 打包资源组", folderListSubscribe.GetReactiveProperty(AssetGroup.Local), "LocalGroup"),
                    AssetGroup.Remote => ("Remote 打包资源组", folderListSubscribe.GetReactiveProperty(AssetGroup.Remote), "RemoteGroup"),
                    _ => throw new ArgumentOutOfRangeException()
                };
                EditorWindowUtility.SelectFolderCollect(label, (folders! as ReactiveProperty<List<string>>)!, defaultName);
            }
        }

        private void RenderBuildCopyPath()
        {
            var barMenu = buildSettings.BuildSettingsModes.CurrentValue.BuildExportCopyFolderModes.Collect.ToArray().ToArrayString();
            var currentIndex = buildSettings.BuildSettingsModes.CurrentValue.BuildExportCopyFolderModes.CurrentIndex;
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("选择 bundle 的打包模式 => ", EditorStyles.boldLabel);
                EditorWindowUtility.ToolBar(currentIndex, barMenu);
            }

            var buildSettingsModeCurrent = buildSettings.BuildSettingsModes.Current;
            var current = buildSettings.BuildSettingsModes.CurrentValue.BuildExportCopyFolderModes.Current;
            var folderSubscribe = buildConfig.StringSubscribe;
            var (label, folder, defaultName, isModify) = (buildSettingsModeCurrent.Value, current.Value) switch
            {
                (_, BuildExportCopyFolderMode.StreamingAssets)
                    => ("拷贝 bundle 资源到路径 => ", folderSubscribe.GetReactiveProperty(BuildExportCopyFolderMode.StreamingAssets), "Remote", false),
                (BuildSettingsMode.Bundle, BuildExportCopyFolderMode.Remote)
                    => ("拷贝 bundle 资源到路径 => ", folderSubscribe.GetReactiveProperty(BuildExportCopyFolderMode.Remote), "Remote", true),
                (BuildSettingsMode.Player, BuildExportCopyFolderMode.Remote)
                    => ("拷贝 bundle 资源到路径 => ", folderSubscribe.GetReactiveProperty(BuildExportCopyFolderMode.Remote, 1), "Remote", true),
                _ => throw new ArgumentOutOfRangeException()
            };
            EditorWindowUtility.SelectFolder(label, (folder as ReactiveProperty<string>)!, defaultName, null, isModify);
        }

        private void RenderBuildTargetMode()
        {
            {
                var buildActiveTargetModes = buildSettings.BuildSettingsModes.CurrentValue.BuildActiveTargetModes;
                var barMenu = buildActiveTargetModes.Collect.ToArray().ToArrayString();
                var currentIndex = buildActiveTargetModes.CurrentIndex;
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    EditorWindowUtility.ToolBar(currentIndex, barMenu);
                }
            }

            {
                var buildTargetsCurrentMode = buildSettings.BuildSettingsModes.CurrentValue.BuildActiveTargetModes.Current;
                var buildTargetsCurrent = buildSettings.BuildTargets.Current;
                var buildTargetsBarMenus = buildSettings.BuildTargets.Keys.ToArray();
                var (selectedValue, displayedOptions, optionValues, isModify) = buildTargetsCurrentMode.Value switch
                {
                    ActiveTargetMode.ActiveTarget =>
                        (new ReactiveProperty<BuildTarget>(EditorUserBuildSettings.activeBuildTarget), new[] { EditorUserBuildSettings.activeBuildTarget.ToString() }, new[] { (int)EditorUserBuildSettings.activeBuildTarget }, false),
                    ActiveTargetMode.InactiveTarget =>
                        (buildTargetsCurrent, buildTargetsBarMenus.ToArrayString(), buildTargetsBarMenus.Cast<int>().ToArray(), true),
                    _ => throw new ArgumentOutOfRangeException()
                };
                EditorWindowUtility.IntPopup("bundle 打包的平台", selectedValue, displayedOptions, optionValues, isModify);
            }


            // using (new GUILayout.HorizontalScope())
            // {
            //     if (!buildTargetMode.IsEnableCurrentBuildTargetGroup)
            //     {
            //         return;
            //     }
            //     var currentTargetGroup = buildTargetMode.CurrentBuildTargetGroup;
            //     var label = "player 打包的平台";
            //     var displayedOptions = buildTargetMode.BuildTargetGroupStrings;
            //     var optionValues = buildTargetMode.BuildTargetGroupIndex;
            //     GUI.enabled = false;
            //     EditorGUILayout.IntPopup(label, currentTargetGroup, displayedOptions, optionValues);
            //     GUI.enabled = true;
            // }
        }

        // private void RenderBuildVersion()
        // {
        //     if (contentFunc == null) return;
        //     var version = contentFunc.Invoke().VersionssInfo;
        //     version.Init();
        //     using (new GUILayout.HorizontalScope())
        //     {
        //         version.Display = EditorGUILayout.TextField("客户端显示版本", version.Display);
        //     }
        //     using (new GUILayout.HorizontalScope())
        //     {
        //         GUI.enabled = false;
        //         EditorGUILayout.IntField("Frame 版本", version.FrameTemp);
        //         GUI.enabled = true;
        //         if (version is { IsModify: false, EnableModifyFrame: true } && GUILayout.Button("+1"))
        //         {
        //             version.FrameTemp++;
        //             version.IsModify = true;
        //         }
        //     }
        //     using (new GUILayout.HorizontalScope())
        //     {
        //         GUI.enabled = false;
        //         EditorGUILayout.IntField("HotUpdate 版本", version.HotUpdateTemp);
        //         GUI.enabled = true;
        //         if (!version.IsModify && version.EnableModifyHotUpdate && GUILayout.Button("+1"))
        //         {
        //             version.HotUpdateTemp++;
        //             version.IsModify = true;
        //         }
        //     }
        //     using (new GUILayout.HorizontalScope())
        //     {
        //         GUI.enabled = false;
        //         EditorGUILayout.IntField("迭代版本", version.IterateTemp);
        //         GUI.enabled = true;
        //     }
        //     using (new GUILayout.HorizontalScope())
        //     {
        //         if (version.IsModify && GUILayout.Button("还原版本"))
        //         {
        //             version.Reset();
        //         }
        //     }
        // }
        //
        // private void RenderBuildExport()
        // {
        //     if (contentFunc == null
        //         || checkForCompletenessFunc == null
        //         || exportFunc == null)
        //     {
        //         return;
        //     }
        //     var build = contentFunc.Invoke();
        //     var version = build.VersionssInfo;
        //     if (GUILayout.Button("开始构建"))
        //     {
        //         var isSuccess = checkForCompletenessFunc.Invoke(build);
        //         if (!isSuccess)
        //         {
        //             return;
        //         }
        //         version.Save();
        //         exportFunc.Invoke(build);
        //         "构建成功..".Log();
        //     }
        // }
        
        // // 检查配置完整性
        // private bool CheckForCompleteness(BuildConfig.BuildSettings build)
        // {
        //     // bundle 打包临时缓存路径
        //     var bundlesTemp = GetStringByEnumType<BuildFolder>();
        //     if (string.IsNullOrEmpty(bundlesTemp))
        //     {
        //         "bundle 打包临时缓存路径不能为空".FrameError();
        //         return false;
        //     }
        //     // 打包路径检查
        //     var pathInfo = build.PathssInfo;
        //     foreach (var path in pathInfo.Items)
        //     {
        //         if (!Directory.Exists(path.Folder))
        //         {
        //             $"无法找到打包路径：{path.Folder}".FrameError();
        //             return false;
        //         }
        //         if (path.Folder.Contains(Application.dataPath))
        //         {
        //             $"打包路径: {path} 不能包含 {Application.dataPath}".FrameError();
        //             return false;
        //         }
        //     }
        //     // 打包资源组检查
        //     var bundleInfo = build.BundlessInfo;
        //     var localGroupPaths = bundleInfo.LocalGroupPaths;
        //     foreach (var path in localGroupPaths)
        //     {
        //         if (!Directory.Exists(path))
        //         {
        //             $"无法找到 Local 打包资源组路径: {path}".FrameError();
        //             return false;
        //         }
        //         if (!path.Contains(Application.dataPath))
        //         {
        //             $"Local 打包资源组路径: {path} 必须包含 {Application.dataPath}".FrameError();
        //             return false;
        //         }
        //     }
        //     var remoteGroupPaths = bundleInfo.RemoteGroupPaths;
        //     if (remoteGroupPaths.Count == 0)
        //     {
        //         "Remote 打包资源组至少为一个".FrameError();
        //         return false;
        //     }
        //     foreach (var path in remoteGroupPaths)
        //     {
        //         if (!Directory.Exists(path))
        //         {
        //             $"无法找到 Remote 打包资源组路径: {path}".FrameError();
        //             return false;
        //         }
        //         if (!path.Contains(Application.dataPath))
        //         {
        //             $"Remote 打包资源组路径: {path} 必须包含 {Application.dataPath}".FrameError();
        //             return false;
        //         }
        //     }
        //     // 拷贝路径检查
        //     var copyInfo = build.CopyssInfo;
        //     if (!Directory.Exists(copyInfo.Folder))
        //     {
        //         $"无法找到拷贝路径：{copyInfo.Folder}".FrameError();
        //         return false;
        //     }
        //     // 版本检查
        //     var versionInfo = build.VersionssInfo;
        //     if (string.IsNullOrEmpty(versionInfo.Display))
        //     {
        //         "客户端显示的版本为空".FrameWarning();
        //     }
        //     if (!versionInfo.IsModify)
        //     {
        //         "版本未更新".FrameError();
        //         return false;
        //     }
        //     return true;
        // }

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
