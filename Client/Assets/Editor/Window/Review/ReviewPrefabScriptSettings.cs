using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Ninth.Editor
{
    public class ReviewPrefabScriptSettings
    {
        public void OnDraw()
        {
            SetScanDirectory();
        }

        private string TxtFiledPath
        {
            get => WindowSOCore.Get<WindowReviewConfig>().TxtFiledPath;
            set => WindowSOCore.Get<WindowReviewConfig>().TxtFiledPath = value;
        }

        private void SetScanDirectory()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Scan Target File Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            EditorGUILayout.TextField("txt", TxtFiledPath);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFilePanel("Select A Txt To Check", TxtFiledPath, "txt");
                if (!string.IsNullOrEmpty(path))
                {
                    TxtFiledPath = path;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void Print()
        {
            if (GUILayout.Button("PrintTxt"))
            {
                // using(File file = System.IO.File.Open(TxtFiledPath, FileMode.Open))
                // {

                // }
            }
        }
    }
}
