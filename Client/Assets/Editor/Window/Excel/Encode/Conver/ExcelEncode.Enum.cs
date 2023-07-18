// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.IO;
// using System;
// using UnityEditor;
// using System.Diagnostics;
// using System.Linq;

// namespace Ninth.Editor
// {
//     // 枚举表格
//     public partial class ExcelEncodeSettings
//     {
//         private readonly string EnumExcelName = "Enum";

//         private void EnumDisplay(EncodeCompileTable table)
//         {
//             EditorGUILayout.LabelField(EnumExcelName);
//             EditorGUILayout.BeginHorizontal();
//             EditorGUILayout.TextField(table.FileName);
//             EditorGUILayout.TextField(string.Format("修改时间: {0}", table.LastWriteTimeUtc.ToString()));
//             EditorGUILayout.TextField(string.Format("Version: {0}", excelEncodeInfos.GetVersion(table.FullName).ToString()));
//             if (GUILayout.Button("Enter"))
//             {
//                 Process.Start(table.FullName);
//             }
//             if (enableEncodeTable.Contains(table))
//             {
//                 if (GUILayout.Button("Encode"))
//                 {
//                     GenerateCScript(table);
//                     excelEncodeInfos.Save(table.FullName, table.LastWriteTimeUtc.ToString());
//                     AssetDatabase.Refresh();
//                 }
//             }
//             EditorGUILayout.EndHorizontal();

//             void GenerateCScript(EncodeCompileTable table)
//             {
                
//             }
//         }
//     }
// }
