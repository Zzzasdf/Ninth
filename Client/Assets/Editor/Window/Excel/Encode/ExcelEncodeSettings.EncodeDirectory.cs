using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public sealed partial class ExcelEncodeSettings
    {
        private string ExcelEncodePathDirectoryRoot
        {
            get => WindowSOCore.Get<WindowExcelConfig>().ExcelEncodePathDirectoryRoot;
            set => WindowSOCore.Get<WindowExcelConfig>().ExcelEncodePathDirectoryRoot = value;
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
            if(GUILayout.Button("Open"))
            {
                Application.OpenURL(ExcelEncodePathDirectoryRoot);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }

}

