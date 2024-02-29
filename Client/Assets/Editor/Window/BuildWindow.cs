using System;
using System.Linq;
using Ninth.HotUpdate;
using UnityEditor;
using UnityEngine;
using VContainer.Unity;

namespace Ninth.Editor
{
    public class BuildWindow : IStartable
    {
        private Action? tabFunc;
        private Func<BuildConfig.BuildSettings>? contentFunc;
        private Func<BuildConfig.BuildSettings, bool>? checkForCompletenessFunc;
        private Action<BuildConfig.BuildSettings>? exportFunc;

        public BuildWindow Subscribe(Action tab, Func<BuildConfig.BuildSettings> content, Func<BuildConfig.BuildSettings, bool> checkForCompleteness, Action<BuildConfig.BuildSettings> export)
        {
            this.tabFunc = tab;
            this.contentFunc = content;
            this.checkForCompletenessFunc = checkForCompleteness;
            this.exportFunc = export;
            return this;
        }

        void IStartable.Start()
        {
            using (new GUILayout.VerticalScope())
            {
                RenderTabs();
                RenderContent();
            }
        }

        private void RenderTabs()
        {
            tabFunc?.Invoke();
        }

        private void RenderContent()
        {
            GUILayout.Space(10);
            RenderBuildPath(); 
            GUILayout.Space(30);
            RenderBuildBundleMode();
            GUILayout.Space(30);
            RenderBuildCopyPath();
            GUILayout.Space(30);
            RenderBuildTargetMode();
            RenderBuildVersion();
            RenderBuildExport();
        }

        private void RenderBuildPath()
        {
            if (contentFunc == null) return;
            var pathInfo = contentFunc.Invoke().PathInfo;
            foreach (var path in pathInfo.Items)
            {
                using (new GUILayout.HorizontalScope())
                {
                    var label = path.Label;
                    var folder = path.Folder;
                    var title = "选择目标文件夹";
                    var defaultName = path.DefaultName;
                    GUI.enabled = false;
                    EditorGUILayout.TextField(label, folder);
                    GUI.enabled = true;
                    if (GUILayout.Button("浏览"))
                    {
                        path.Folder = EditorUtility.OpenFolderPanel(title, folder, defaultName);
                    }
                }
            }
        }
        
         private void RenderBuildBundleMode()
         {
             if (contentFunc == null) return;
             var bundleMode = contentFunc.Invoke().BundleInfo;
             using (new GUILayout.HorizontalScope())
             {
                 var barMenu = bundleMode.BuildBundleModeStrings;
                 var label = "选择 bundle 的打包模式 => ";
                 var current = bundleMode.Current;
                 GUILayout.FlexibleSpace();
                 GUILayout.Label(label, EditorStyles.boldLabel);
                 bundleMode.Current = GUILayout.Toolbar(current, barMenu);
             }
             var title = "选择目标文件夹";
             foreach (var groupIndex in bundleMode.CurrentAssetGroupsByIndex)
             {
                 var folders = bundleMode.Get(groupIndex);
                 EditorGUILayout.LabelField($"{bundleMode.GetAssetGroup(groupIndex)} 打包资源组");
                 for (int i = 0; i < folders.Count; i++)
                 {
                     using (new GUILayout.HorizontalScope())
                     {
                         var folder = folders[i];
                         GUI.enabled = false;
                         EditorGUILayout.TextField($"[{i}] =>", folder);
                         GUI.enabled = true;
                         if (GUILayout.Button("浏览"))
                         {
                             var temp = EditorUtility.OpenFolderPanel(title, folder, groupIndex.ToString());
                             bundleMode.Set(groupIndex, temp, i);
                         }
                         if (GUILayout.Button("移除"))
                         {
                             bundleMode.Remove(groupIndex, i);
                         }
                     }
                 }
                 if (GUILayout.Button("++++++新增资源组++++++"))
                 {
                     bundleMode.Add(groupIndex);
                 }
             }
         }
         
         private void RenderBuildCopyPath()
         {
             if (contentFunc == null) return;
             var copyMode = contentFunc.Invoke().CopyInfo;
             using (new GUILayout.HorizontalScope())
             {
                 var barMenu = copyMode.BuildBundleCopyModeStrings;
                 var label = "拷贝 bundle 资源到路径 => ";
                 var current = copyMode.Current;
                 GUILayout.FlexibleSpace();
                 GUILayout.Label(label, EditorStyles.boldLabel);
                 copyMode.Current = GUILayout.Toolbar(current, barMenu);
             }
             using (new GUILayout.HorizontalScope())
             {
                 var label = "bundle 拷贝的路径";
                 var folder = copyMode.Folder;
                 var isModify = copyMode.IsModify;
                 var title = "选择目标文件夹";
                 var defaultName = "Remote";
                 GUI.enabled = false;
                 EditorGUILayout.TextField(label, folder);
                 GUI.enabled = true;
                 if (!isModify)
                 {
                     return;
                 }
                 if (GUILayout.Button("浏览"))
                 {
                     var temp = EditorUtility.OpenFolderPanel(title, folder, defaultName);
                     copyMode.Folder = temp;
                 }
             }
         }
         
         private void RenderBuildTargetMode()
         {
             if (contentFunc == null) return;
             var buildTargetMode = contentFunc.Invoke().BuildTargetInfo;
             using (new GUILayout.HorizontalScope())
             {
                 var barMenu = buildTargetMode.BuildTargetModes;
                 var current = buildTargetMode.Current;
                 GUILayout.FlexibleSpace();
                 buildTargetMode.Current = GUILayout.Toolbar(current, barMenu);
             }
             using (new GUILayout.HorizontalScope())
             {
                 var label = "bundle 打包的平台";
                 var isModify = buildTargetMode.CurrentPackBuildTargetModeIsModify;
                 var currentBuildTarget = buildTargetMode.CurrentBuildTargetIndex;
                 var displayedOptions = buildTargetMode.BuildTargetStrings;
                 var optionValues = buildTargetMode.BuildTargetIndex;
                 if (!isModify)
                 {
                     GUI.enabled = false;
                 }
                 buildTargetMode.CurrentBuildTargetIndex = EditorGUILayout.IntPopup(label, currentBuildTarget, displayedOptions, optionValues);
                 if (!isModify)
                 {
                     GUI.enabled = true;
                 }
             }

             using (new GUILayout.HorizontalScope())
             {
                 if (!buildTargetMode.IsEnableCurrentBuildTargetGroup)
                 {
                     return;
                 }
                 var currentTargetGroup = buildTargetMode.CurrentBuildTargetGroup;
                 var label = "player 打包的平台";
                 var displayedOptions = buildTargetMode.BuildTargetGroupStrings;
                 var optionValues = buildTargetMode.BuildTargetGroupIndex;
                 GUI.enabled = false;
                 EditorGUILayout.IntPopup(label, currentTargetGroup, displayedOptions, optionValues);
                 GUI.enabled = true;
             }
         }

         private void RenderBuildVersion()
         {
             if (contentFunc == null) return;
             var version = contentFunc.Invoke().VersionInfo;
             version.Init();
             using (new GUILayout.HorizontalScope())
             {
                 version.Display = EditorGUILayout.TextField("客户端显示版本", version.Display);
             }
             using (new GUILayout.HorizontalScope())
             {
                 GUI.enabled = false;
                 EditorGUILayout.IntField("Frame 版本", version.FrameTemp);
                 GUI.enabled = true;
                 if (version is { IsModify: false, EnableModifyFrame: true } && GUILayout.Button("+1"))
                 {
                     version.FrameTemp++;
                     version.IsModify = true;
                 }
             }
             using (new GUILayout.HorizontalScope())
             {
                 GUI.enabled = false;
                 EditorGUILayout.IntField("HotUpdate 版本", version.HotUpdateTemp);
                 GUI.enabled = true;
                 if (!version.IsModify && version.EnableModifyHotUpdate && GUILayout.Button("+1"))
                 {
                     version.HotUpdateTemp++;
                     version.IsModify = true;
                 }
             }
             using (new GUILayout.HorizontalScope())
             {
                 GUI.enabled = false;
                 EditorGUILayout.IntField("迭代版本", version.IterateTemp);
                 GUI.enabled = true;
             }
             using (new GUILayout.HorizontalScope())
             {
                 if (version.IsModify && GUILayout.Button("还原版本"))
                 {
                     version.Reset();
                 }
             }
         }

         private void RenderBuildExport()
         {
             if (contentFunc == null
                 || checkForCompletenessFunc == null
                 || exportFunc == null)
             {
                 return;
             }
             var build = contentFunc.Invoke();
             var version = build.VersionInfo;
             if (GUILayout.Button("开始构建"))
             {
                 var isSuccess = checkForCompletenessFunc.Invoke(build);
                 if (!isSuccess)
                 {
                     return;
                 }
                 version.Save();
                 exportFunc.Invoke(build);
                 "构建成功..".Log();
             }
         }
    }
}