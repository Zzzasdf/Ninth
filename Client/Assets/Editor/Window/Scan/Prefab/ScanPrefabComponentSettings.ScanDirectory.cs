using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class ScanPrefabComponentSettings<TEnum, T>
    {
        private string PathDirectoryRoot
        {
            get => GetPathDirectoryRoot();
            set => SetPathDirectoryRoot(value);
        }

        protected abstract string GetPathDirectoryRoot();

        protected abstract void SetPathDirectoryRoot(string value);

        private void SetScanDirectory()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Scan Target Directory Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            EditorGUILayout.TextField("Directory", PathDirectoryRoot);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Select A Folder To Scanf", PathDirectoryRoot, "Assets");
                if (!path.Contains(Application.dataPath))
                {
                    Debug.LogError("请选择项目里的一个文件夹");
                }
                else
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        PathDirectoryRoot = path;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}