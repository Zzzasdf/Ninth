using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Ninth.HotUpdate;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using VContainer;
using VContainer.Unity;

namespace Ninth.Editor
{
    public class BuildWindow : IStartable
    {
        private Action? tab;
        private Func<BuildConfig.BuildSettings>? content;

        public BuildWindow Subscribe(Action tab, Func<BuildConfig.BuildSettings> content)
        {
            this.tab = tab;
            this.content = content;
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
            tab?.Invoke();
        }

        private void RenderContent()
        {
            GUILayout.Space(10);
            RenderBuildFolderAndBrowse(); 
            RenderBuildBundleMode();
        }

        private void RenderBuildFolderAndBrowse()
        {
            if (content == null) return;
            var paths = content.Invoke().Paths;
            foreach (var path in paths)
            {
                var label = path.Label;
                var folder = path.Folder;
                var title = path.Title;
                var defaultName = path.DefaultName;
                using (new GUILayout.HorizontalScope())
                {
                    GUI.enabled = false;
                    EditorGUILayout.TextField(label, folder);
                    GUI.enabled = true;
                    if (GUILayout.Button("浏览"))
                    {
                        var temp = EditorUtility.OpenFolderPanel(title, folder, defaultName);
                        if (string.IsNullOrEmpty(temp))
                        {
                            return;
                        }
                        path.Folder = temp;
                    }
                }
            }
        }
        
         private void RenderBuildBundleMode()
         {
             if (content == null) return;
             var bundleMode = content.Invoke().BundleMode;
             var barMenuBundle = bundleMode.BuildBundleModes.ToArrayString();
             var current = bundleMode.Current;
             using (new GUILayout.HorizontalScope())
             {
                 GUILayout.FlexibleSpace();
                 if (current >= barMenuBundle.Length)
                 {
                     current = barMenuBundle.Length - 1;
                 }
                 bundleMode.Current = GUILayout.Toolbar(current, barMenuBundle);
             }
         }
        
        // #region Bundle
        // private void OnBundleGUI()
        // {
        //     RenderBuildBundlesTargetFolderRootAndBrowse();
        //
        // RenderBuildBundleMode();
        //     RenderToolbarByBuildExportCopyFolderMode();
        //     RenderToolbarByActiveTarget();
        //     RenderPopupByBuildTarget();
        // 
        //     RenderVersion();
        //     RenderBtnResetByVersion();
        //     RenderExport();
        // }
        // #endregion
        //
        // #region Player
        // private void OnPlayerGUI()
        // {
        //     RenderBuildBundlesTargetFolderRootAndBrowse();
        //     RenderBuildPlayersTargetFolderRootAndBrowse();
        //
        // RenderBuildAllBundles();
        //     RenderToolbarByBuildExportCopyFolderMode();
        //     RenderToolbarByActiveTarget();
        //     RenderPopupByBuildTarget();
        //
        // RenderPopupByBuildTargetGroup();
        //     RenderVersion();
        //     RenderBtnResetByVersion();
        //     RenderExport();
        // }
        // #endregion
    }
}