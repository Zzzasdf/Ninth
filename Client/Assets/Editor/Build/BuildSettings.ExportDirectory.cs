using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildSettings
    {
        private string m_BuildBundlesDirectoryRoot
        {
            get => EditorSOCore.GetBuildConfig().BuildBundlesDirectoryRoot;
            set => EditorSOCore.GetBuildConfig().BuildBundlesDirectoryRoot = value;
        }

        private string m_BuildPlayersDirectoryRoot
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
            m_BuildBundlesDirectoryRoot = EditorGUILayout.TextField("BundlesTargetDirectoryRoot", m_BuildBundlesDirectoryRoot);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Select A Folder To Export", m_BuildBundlesDirectoryRoot, "Bundles");
                if (!string.IsNullOrEmpty(path))
                {
                    m_BuildBundlesDirectoryRoot = path;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SetBrowsePlayersTargetDirectory()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            m_BuildPlayersDirectoryRoot = EditorGUILayout.TextField("PlayersDirectoryRoot", m_BuildPlayersDirectoryRoot);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFilePanel("Select A Folder To Export", m_BuildPlayersDirectoryRoot, "Players");
                if (!string.IsNullOrEmpty(path))
                {
                    m_BuildPlayersDirectoryRoot = path;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}