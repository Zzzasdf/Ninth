using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class ExcelSearch
    {
        private string ExcelSearchPathDirectoryRoot
        {
            get => EditorSOCore.GetExcelConfig().ExcelSearchPathDirectoryRoot;
            set => EditorSOCore.GetExcelConfig().ExcelSearchPathDirectoryRoot = value;
        }

        private void SetSearchDirectory()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Search Target Directory Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            ExcelSearchPathDirectoryRoot = EditorGUILayout.TextField("Directory", ExcelSearchPathDirectoryRoot);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Select A Folder To Search", ExcelSearchPathDirectoryRoot, "Excels");
                if (!string.IsNullOrEmpty(path))
                {
                    ExcelSearchPathDirectoryRoot = path;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}