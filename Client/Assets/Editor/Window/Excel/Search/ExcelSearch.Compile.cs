using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class ExcelSearch
    {
        // SearchResults
        private static SearchCompile m_Compile;

        private static void SetCompile()
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Compile"))
            {
                if (m_Compile == null)
                {
                    m_Compile = new SearchCompile();
                }

                Stopwatch stopwatch = Stopwatch.StartNew();
                m_Compile.Tables.Clear();
                DirectoryInfo directory = new DirectoryInfo(ExcelSearchPathDirectoryRoot);
                FileInfo[] fileInfos = directory.GetFiles();
                for (int index = 0; index < fileInfos.Length; index++)
                {
                    try
                    {
                        SearchXLS(fileInfos[index]);
                    }
                    catch (OfficeXmlFileException)
                    {
                        SearchXLSX(fileInfos[index]);
                    }
                    catch (NotOLE2FileException)
                    {
                        UnityEngine.Debug.LogError($"Unable to resolve this file {fileInfos[index].Name}");
                    }
                }
                m_SearchResultIntro = $"This Compile Takes Time, Milliseconds:{stopwatch.ElapsedMilliseconds}, Ticks:{stopwatch.ElapsedTicks}";
            }
            EditorGUILayout.EndVertical();
        }

        private static void SearchXLS(FileInfo fileInfo)
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
                            if (string.IsNullOrEmpty(value))
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
