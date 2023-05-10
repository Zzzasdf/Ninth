using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public sealed partial class ExcelEncode
    {
        private string ExcelEncodePathDirectoryRoot
        {
            get => EditorSOCore.GetExcelConfig().ExcelEncodePathDirectoryRoot;
            set => EditorSOCore.GetExcelConfig().ExcelEncodePathDirectoryRoot = value;
        }

        private void SetEncodeDirectory()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Encode Target Directory Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            ExcelEncodePathDirectoryRoot = EditorGUILayout.TextField("Directory", ExcelEncodePathDirectoryRoot);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Select A Folder To Encode", ExcelEncodePathDirectoryRoot, "Excels");
                if (!string.IsNullOrEmpty(path))
                {
                    ExcelEncodePathDirectoryRoot = path;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }

}

