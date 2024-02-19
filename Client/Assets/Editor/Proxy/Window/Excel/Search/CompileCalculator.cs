using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using System.IO;

namespace Ninth.Editor.Window
{
    using Table = TableCollect.Table;

    public class CompileCalculator
    {
        public static TableCollect Get(string compileDirectoryRoot)
        {
            TableCollect result = new TableCollect();
            DirectoryInfo directory = new DirectoryInfo(compileDirectoryRoot);
            FileInfo[] fileInfos = directory.GetFiles();
            for (int index = 0; index < fileInfos.Length; index++)
            {
                FileInfo fileInfo = fileInfos[index];
                try
                {
                    SearchXLS(fileInfo);
                }
                catch (OfficeXmlFileException)
                {
                    SearchXLSX(fileInfo);
                }
                catch (NotOLE2FileException)
                {
                    throw new NotOLE2FileException(nameof(fileInfo));
                }
            }
            return result;

            void SearchXLS(FileInfo fileInfo)
            {
                using (FileStream fsRead = File.OpenRead(fileInfo.FullName))
                {
                    string fullName = fileInfo.FullName;
                    HSSFWorkbook wk = new HSSFWorkbook(fsRead); // 抛出异常处
                    Table table = result.AddTable(fileInfo).Tables[fullName];
                    for (int sheetIndex = 0; sheetIndex < wk.NumberOfSheets; sheetIndex++)
                    {
                        Table.Sheet sheet = result.Tables[fullName].AddSheet(sheetIndex).Sheets[sheetIndex];
                        ISheet st = wk.GetSheetAt(sheetIndex);
                        int rowsCount = st.LastRowNum;
                        for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                        {
                            IRow row = st.GetRow(rowIndex);
                            int colsCount = row?.LastCellNum ?? 0;
                            for (int colIndex = 0; colIndex < colsCount; colIndex++)
                            {
                                string cellValue = st.GetRow(rowIndex).GetCell(colIndex)?.ToString();
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    sheet.AddCell(rowIndex + 1, colIndex + 1, cellValue);
                                }
                            }
                        }
                    }
                }
            }

            void SearchXLSX(FileInfo fileInfo)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(fileInfo)) // 抛出异常处
                {
                    string fullName = fileInfo.FullName;
                    ExcelWorkbook wk = excelPackage.Workbook;
                    Table table = result.AddTable(fileInfo).Tables[fullName];
                    for (int sheetIndex = 1; sheetIndex <= wk.Worksheets.Count; sheetIndex++)
                    {
                        ExcelWorksheet st = wk.Worksheets[sheetIndex];
                        if (st == null || st.Dimension == null)
                        {
                            continue;
                        }
                        Table.Sheet sheet = result.Tables[fullName].AddSheet(sheetIndex).Sheets[sheetIndex];
                        for (int rowIndex = st.Dimension.Start.Row; rowIndex <= st.Dimension.End.Row; rowIndex++)
                        {
                            for (int colIndex = st.Dimension.Start.Column; colIndex <= st.Dimension.End.Column; colIndex++)
                            {
                                string cellValue = st.GetValue(rowIndex, colIndex)?.ToString();
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    sheet.AddCell(rowIndex, colIndex, cellValue);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
