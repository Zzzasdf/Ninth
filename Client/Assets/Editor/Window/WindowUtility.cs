using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ninth.Editor
{
    public class WindowUtility
    {
        public static string RenderDirectory(string directory, string containDirectory = null)
        {
            GUILayout.Label("RenderBtnSearch Target Directory Settings", EditorStyles.boldLabel);
            GUI.enabled = false;
            directory = EditorGUILayout.TextField(directory);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Select A Folder To RenderBtnSearch", directory, "Excels");
                if (string.IsNullOrEmpty(path))
                {
                    return directory;
                }
                if (string.IsNullOrEmpty(containDirectory))
                {
                    directory = path;
                }
                else
                {
                    // 指定包含目录
                    if (path.Contains(containDirectory))
                    {
                        directory = path;
                    }
                    else
                    {
                        Debug.LogErrorFormat("该路径未包含指定的目录{0}", containDirectory);
                    }
                }
            }
            return directory;
        }
    }
}
