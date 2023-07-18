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
//     // 构造类表格
//     public partial class ExcelEncodeSettings
//     {
//         private readonly string TypeExcelName = "Type";

//         private void TypeDisplay(EncodeCompileTable table)
//         {
//             EditorGUILayout.LabelField(TypeExcelName);
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
//                     // GenerateType(table);
//                     // GenerateData(table);
//                     excelEncodeInfos.Save(table.FullName, table.LastWriteTimeUtc.ToString());
//                     AssetDatabase.Refresh();
//                 }
//             }
//             EditorGUILayout.EndHorizontal();
//         }

//         private class ClassInfo
//         {
//             public string ClassType { get; private set; }
//             public string AnalysisFormat { get; private set; }
//             public List<FileInfo> FileInfos { get; private set; }

//             public ClassInfo()
//             {
                
//             }
//         }

//         public class FileInfo
//         {
//             public string FileType { get; private set; }
//             public string FileName { get; private set; }
//             public string Describe { get; private set; }

//             public FileInfo(string fileType, string fileName, string describe)
//             {
//                 this.FileType = fileType;
//                 this.FileName = fileName;
//                 this.Describe = describe;
//             }
//         }
//     }
// }
