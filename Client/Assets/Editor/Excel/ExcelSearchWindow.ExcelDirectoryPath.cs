using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class ExcelSearchWindow
    {
        private static void SetExcelPathDirectoryRoot()
        {
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Excel Directory Path Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            PlayerPrefsDefine.ExcelSearchPathDirectoryRoot = EditorGUILayout.TextField("DirectoryPath", PlayerPrefsDefine.ExcelSearchPathDirectoryRoot);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Select a folder to store resources", PlayerPrefsDefine.ExcelSearchPathDirectoryRoot, "Excels");
                if (!string.IsNullOrEmpty(path))
                {
                    PlayerPrefsDefine.ExcelSearchPathDirectoryRoot = path;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}