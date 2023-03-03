using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class ExcelSearchWindow
    {
        private static void SetSearchDirectory()
        {
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Search Target Directory Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            PlayerPrefsDefine.ExcelSearchPathDirectoryRoot = EditorGUILayout.TextField("Directory", PlayerPrefsDefine.ExcelSearchPathDirectoryRoot);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Select A Folder To Search", PlayerPrefsDefine.ExcelSearchPathDirectoryRoot, "Excels");
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