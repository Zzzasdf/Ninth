using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildSettings
    {
        private string BuildBundlesDirectoryRoot
        {
            get => EditorSOCore.GetBuildConfig().BuildBundlesDirectoryRoot;
            set => EditorSOCore.GetBuildConfig().BuildBundlesDirectoryRoot = value;
        }

        private string BuildPlayersDirectoryRoot
        {
            get => EditorSOCore.GetBuildConfig().BuildPlayersDirectoryRoot;
            set => EditorSOCore.GetBuildConfig().BuildPlayersDirectoryRoot = value;
        }

        private void SetBrowseBundlesTargetDirectory()
        {
            GUILayout.Space(10);
            GUILayout.Label("Export Target Directory Settings", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            BuildBundlesDirectoryRoot = EditorGUILayout.TextField("BundlesTargetDirectoryRoot", BuildBundlesDirectoryRoot);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Select A Folder To Export", BuildBundlesDirectoryRoot, "Bundles");
                if (!string.IsNullOrEmpty(path))
                {
                    BuildBundlesDirectoryRoot = path;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SetBrowsePlayersTargetDirectory()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            BuildPlayersDirectoryRoot = EditorGUILayout.TextField("PlayersDirectoryRoot", BuildPlayersDirectoryRoot);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFilePanel("Select A Folder To Export", BuildPlayersDirectoryRoot, "Players");
                if (!string.IsNullOrEmpty(path))
                {
                    BuildPlayersDirectoryRoot = path;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}