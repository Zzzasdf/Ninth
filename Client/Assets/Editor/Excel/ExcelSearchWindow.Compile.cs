using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class ExcelSearchWindow
    {
        // SearchResults
        private static SearchCompile m_Compile;

        private static void SetExcelCompilePathDirectoryRoot()
        {
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Excel CompileDataFile Directory Path Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            PlayerPrefsDefine.ExcelSearchCompileDirectoryRoot = EditorGUILayout.TextField("DirectoryPath", PlayerPrefsDefine.ExcelSearchCompileDirectoryRoot);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Select a folder to store resources", PlayerPrefsDefine.ExcelSearchCompileDirectoryRoot, "Compile");
                if (!string.IsNullOrEmpty(path))
                {
                    PlayerPrefsDefine.ExcelSearchCompileDirectoryRoot = path;
                    SetCompileValue();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private static void SetCompileValue()
        {
            if(m_Compile == null)
            {
                m_Compile = new SearchCompile();
            }
            m_Compile.Tables.Clear();
            DirectoryInfo directory = new DirectoryInfo(PlayerPrefsDefine.ExcelSearchPathDirectoryRoot);
            FileInfo[] fileInfos = directory.GetFiles();
            for (int index = 0; index < fileInfos.Length; index++)
            {
                string tableName = fileInfos[index].Name + ".Json";
                CompileTable compileTableData = Utility.ToObject<CompileTable>(PlayerPrefsDefine.ExcelSearchCompileDirectoryRoot + "/" + tableName);
                m_Compile.Tables.Add(compileTableData);
            }
        }

        private static void SetCompile()
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Compile"))
            {
                if(m_Compile == null)
                {
                    m_Compile = new SearchCompile();
                }
                Stopwatch stopwatch = Stopwatch.StartNew();

                m_Compile.Tables.Clear();
                DirectoryInfo directory = new DirectoryInfo(PlayerPrefsDefine.ExcelSearchPathDirectoryRoot);
                FileInfo[] fileInfos = directory.GetFiles();
                for (int index = 0; index < fileInfos.Length; index++)
                {
                    if (!SearchXLS(fileInfos[index]))
                    {
                        SearchXLSX(fileInfos[index]);
                    }
                }
                //if (Directory.Exists(PlayerPrefsDefine.ExcelSearchCompileDirectoryRoot))
                //{
                //    Directory.Delete(PlayerPrefsDefine.ExcelSearchCompileDirectoryRoot, true);
                //}
                //Directory.CreateDirectory(PlayerPrefsDefine.ExcelSearchCompileDirectoryRoot);
                //for (int index = 0; index < m_Compile.Tables.Count; index++)
                //{
                //    string tableName = m_Compile.Tables[index].TableName + ".Json";
                //    Utility.ToJson(m_Compile.Tables[index], PlayerPrefsDefine.ExcelSearchCompileDirectoryRoot + "/" + tableName);
                //}
                m_SearchResultIntro = $"This search takes time,Milliseconds:{stopwatch.ElapsedMilliseconds},Ticks:{stopwatch.ElapsedTicks}";
            }
            EditorGUILayout.EndVertical();
        }

        private static bool SearchXLS(FileInfo fileInfo)
        {
            try
            {
                using (FileStream fsRead = File.OpenRead(fileInfo.FullName))
                {
                    CompileTable tableData = new CompileTable();
                    tableData.TableName = fileInfo.Name;
                    tableData.Path = fileInfo.FullName;
                    m_Compile.Tables.Add(tableData);

                    IWorkbook wk = new HSSFWorkbook(fsRead);
                    for (int sheetIndex = 0; sheetIndex < wk.NumberOfSheets; sheetIndex++)
                    {
                        CompileSheet compileSheet = new CompileSheet();
                        compileSheet.SheetIndex = sheetIndex + 1;
                        tableData.CompileSheets.Add(compileSheet);

                        ISheet sheet = wk.GetSheetAt(sheetIndex);
                        int maxCol = 0;
                        while (!string.IsNullOrEmpty(sheet.GetRow(0)?.GetCell(maxCol)?.ToString()))
                        {
                            maxCol++;
                        }
                        int row = 0;
                        while (!string.IsNullOrEmpty(sheet.GetRow(row)?.GetCell(0)?.ToString()))
                        {
                            for (int col = 0; col < maxCol; col++)
                            {
                                string value = sheet.GetRow(row)?.GetCell(col)?.ToString();
                                if(string.IsNullOrEmpty(value))
                                {
                                    continue;
                                }
                                CompileCell cell = new CompileCell()
                                {
                                    Row = row + 1,
                                    Col = col + 1,
                                    CellValue = value,
                                };
                                compileSheet.Cells.Add(cell);
                            }
                            row++;
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void SearchXLSX(FileInfo fileInfo)
        {
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                CompileTable tableData = new CompileTable();
                tableData.TableName = fileInfo.Name;
                tableData.Path = fileInfo.FullName;
                m_Compile.Tables.Add(tableData);

                for (int sheetIndex = 1; sheetIndex <= excelPackage.Workbook.Worksheets.Count; sheetIndex++)
                {
                    CompileSheet compileSheet = new CompileSheet();
                    compileSheet.SheetIndex = sheetIndex;
                    tableData.CompileSheets.Add(compileSheet);

                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[sheetIndex];
                    int maxCol = 1;
                    while (!string.IsNullOrEmpty(worksheet.Cells[1, maxCol].Value?.ToString()))
                    {
                        maxCol++;
                    }
                    int row = 1;
                    while (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Value?.ToString()))
                    {
                        for (int col = 1; col < maxCol; col++)
                        {
                            string value = worksheet.Cells[row, col].Value?.ToString();
                            if (string.IsNullOrEmpty(value))
                            {
                                continue;
                            }
                            CompileCell cell = new CompileCell()
                            {
                                Row = row,
                                Col = col,
                                CellValue = value,
                            };
                            compileSheet.Cells.Add(cell);
                        }
                        row++;
                    }
                }
            }
        }
    }
}
