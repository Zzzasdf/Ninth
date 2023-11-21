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
//     public partial class ExcelEncodeSettings
//     {
//         private ExcelAllInDirInfos excelAllInDirInfos; // 在目录下的所有表格信息
//         private List<EncodeCompileTable> enableEncodeTable; // 需要编码的表格 (修改过)
//         private ExcelEncodeInfos excelEncodeInfos; // 所有编译过的表格信息

//         private void SetDisplay()
//         {
//             if(!Directory.Exists(EncodePathDirectoryRoot))
//             {
//                 return;
//             }
//             if (excelAllInDirInfos == null)
//             {
//                 excelAllInDirInfos = new ExcelAllInDirInfos();
//                 excelEncodeInfos = Utility.Get<ExcelEncodeInfos>(Application.dataPath + "/Editor/Window/Excel/Encode/ExcelEncodeInfo.Json");
//                 if (excelEncodeInfos == null)
//                 {
//                     excelEncodeInfos = new ExcelEncodeInfos();
//                 }
//             }
//             else
//             {
//                 excelAllInDirInfos.Tables.Clear();
//             }

//             DirectoryInfo directory = new DirectoryInfo(EncodePathDirectoryRoot);
//             List<FileInfo> fileInfos = directory.GetFiles().Where(x => x.Name.StartsWith("~$")).ToList();
//             fileInfos.ForEach(x => excelAllInDirInfos.Tables.Add(new EncodeCompileTable(x)));
//             EncodeCompileTable table = null;
            
//             // 添加需要编码的表格
//             if(enableEncodeTable == null)
//             {
//                 enableEncodeTable = new List<EncodeCompileTable>();
//             }
//             else
//             {
//                 enableEncodeTable.Clear();
//             }
//             for(int index = 0; index < excelAllInDirInfos.Tables.Count; index++)
//             {
//                 table = excelAllInDirInfos.Tables[index];
//                 if (!excelEncodeInfos.TryGetValue(table.FullName, out string LastWriteTimeUtc)
//                     || LastWriteTimeUtc != table.LastWriteTimeUtc.ToString())
//                 {
//                     enableEncodeTable.Add(table);
//                 }
//             }

//             // 枚举表格
//             table = excelAllInDirInfos.Tables.Where(x => x.FullName == EnumExcelName).ToList().First();
//             excelAllInDirInfos.Tables.Remove(table);
//             EnumDisplay(table);
//             // 结构表格
//             table = excelAllInDirInfos.Tables.Where(x => x.FullName == TypeExcelName).ToList().First();
//             excelAllInDirInfos.Tables.Remove(table);
//             TypeDisplay(table);
//             // 本地化表格
//             table = excelAllInDirInfos.Tables.Where(x => x.FullName == LocalizationExcelName).ToList().First();
//             excelAllInDirInfos.Tables.Remove(table);
//             LocalizationDisplay(table);
//             // 通用表格
//             for (int index = 0; index < excelAllInDirInfos.Tables.Count; index++)
//             {
//                 table = excelAllInDirInfos.Tables[index];
//                 CommonDisplay(table);
//             }
//         }

//         public class ExcelAllInDirInfos
//         {
//             public List<EncodeCompileTable> Tables;

//             public ExcelAllInDirInfos()
//             {
//                 Tables = new List<EncodeCompileTable>();
//             }
//         }

//         public class EncodeCompileTable
//         {
//             public string FullName { get; private set; }
//             public string FileName { get; private set; }

//             public DateTime LastWriteTimeUtc { get; private set; }

//             public EncodeCompileTable(FileInfo fileInfo)
//             {
//                 FullName = fileInfo.FullName;
//                 FileName = fileInfo.Name;
//                 LastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
//             }
//         }

//         public class ExcelEncodeInfos
//         {
//             public Dictionary<string, ExcelEncodeInfo> TabDic { get; private set; }

//             public ExcelEncodeInfos()
//             {
//                 TabDic = new Dictionary<string, ExcelEncodeInfo>();
//             }

//             public int GetVersion(string key)
//             {
//                 if (TabDic.TryGetValue(key, out ExcelEncodeInfo info))
//                 {
//                     return info.Version;
//                 }
//                 return 0;
//             }

//             public bool TryGetValue(string key, out string LastWriteTimeUtc)
//             {
//                 if (TabDic.TryGetValue(key, out ExcelEncodeInfo info))
//                 {
//                     LastWriteTimeUtc = info.LastWriteTimeUtc;
//                     return true;
//                 }
//                 LastWriteTimeUtc = null;
//                 return false;
//             }

//             public void Save(string key, string LastWriteTimeUtc)
//             {
//                 if (!TabDic.TryGetValue(key, out ExcelEncodeInfo info))
//                 {
//                     info = new ExcelEncodeInfo();
//                     TabDic.Add(key, info);
//                 }
//                 info.LastWriteTimeUtc = LastWriteTimeUtc;
//                 info.Version++;
//                 // 保存文件
//                 Utility.Store(this, Application.dataPath + "/Editor/Window/Excel/Encode/ExcelEncodeInfo.Json");
//             }

//             public class ExcelEncodeInfo
//             {
//                 public string LastWriteTimeUtc;
//                 public int Version;
//             }
//         }
//     }
// }


