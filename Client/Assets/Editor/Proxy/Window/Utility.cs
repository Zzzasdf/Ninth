// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
//
// namespace Ninth.Editor.Window
// {
//     public class Utility
//     {
//         public static string RenderDirectory(string directory, string containDirectory = null)
//         {
//             GUILayout.Label(CommonLanguage.SearchTargetFolderSettings.ToCurrLanguage(), EditorStyles.boldLabel);
//             GUI.enabled = false;
//             directory = EditorGUILayout.TextField(directory);
//             GUI.enabled = true;
//             if (GUILayout.Button(CommonLanguage.Browse.ToCurrLanguage()))
//             {
//                 string path = EditorUtility.OpenFolderPanel(CommonLanguage.SelectAFolderToSearch.ToCurrLanguage(), directory, "Excels");
//                 if (string.IsNullOrEmpty(path))
//                 {
//                     return directory;
//                 }
//                 if (string.IsNullOrEmpty(containDirectory))
//                 {
//                     directory = path;
//                 }
//                 else
//                 {
//                     // 指定包含目录
//                     if (path.Contains(containDirectory))
//                     {
//                         directory = path;
//                     }
//                     else
//                     {
//                         Debug.LogErrorFormat(LogFormat1.PathNotIncludedAppointFolder.ToCurrLanguage(), containDirectory);
//                     }
//                 }
//             }
//             return directory;
//         }
//     }
// }
