using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class ExcelSearch
    {
        private static string m_ExcelSearchPathDirectoryRoot
        {
            get => PlayerPrefsDefine.ExcelSearchPathDirectoryRoot;
            set => PlayerPrefsDefine.ExcelSearchPathDirectoryRoot = value;
        }

        private static void SetSearchDirectory()
        {
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Search Target Directory Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            m_ExcelSearchPathDirectoryRoot = EditorGUILayout.TextField("Directory", m_ExcelSearchPathDirectoryRoot);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Select A Folder To Search", m_ExcelSearchPathDirectoryRoot, "Excels");
                if (!string.IsNullOrEmpty(path))
                {
                    m_ExcelSearchPathDirectoryRoot = path;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}