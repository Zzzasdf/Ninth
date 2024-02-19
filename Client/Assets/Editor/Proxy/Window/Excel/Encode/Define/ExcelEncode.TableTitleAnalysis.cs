//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using OfficeOpenXml;
//using System;

//namespace Ninth.Editor.Window
//{
//    public partial class ExcelEncodeSettings
//    {
//        // 获取表所有页的行表头解析字段
//        private List<(string rowTitleAnalysisField, int rowIndex)> IncreaseRowTableTitleAnalysis(ExcelWorksheet sheet)
//        {
//            List<(string rowTitleAnalysisField, int rowIndex)> list = new List<(string, int)>();
//            (int rowStart, int rowEnd) = GetRowRange(sheet);
//            for(int index = rowStart; index <= rowEnd; index++)
//            {
//                (string, int) item = (sheet.Rows[index, 1].Value?.ToString(), index);
//                list.Add(item);
//            }
//            return list;
//        }

//        // 获取所有行表头字段的范围
//        private (int rowStart, int rowEnd) GetRowRange(ExcelWorksheet sheet)
//        {
//            return (2, GetRowColEndNode(sheet).rowEnd);
//        }

//        // 获取表所有页的列表头解析字段
//        private List<(string colTitleAnalysisField, int colIndex)> IncreaseColTableTitleAnalysis(ExcelWorksheet sheet)
//        {
//            List<(string colTitleAnalysisField, int colIndex)> list = new List<(string, int)>();
//            (int colStart, int colEnd) = GetRowRange(sheet);
//            for(int index = colStart; index <= colEnd; index++)
//            {
//                (string, int) item = (sheet.Rows[index, 1].Value?.ToString(), index);
//                list.Add(item);
//            }
//            return list;
//        }

//        // 获取所有列表头字段的范围
//        private (int colStart, int colEnd) GetColRange(ExcelWorksheet sheet)
//        {
//            return (2, GetRowColEndNode(sheet).colEnd);
//        }

//        // 获取表格行列的终止节点索引
//        private (int rowEnd, int colEnd) GetRowColEndNode(ExcelWorksheet sheet)
//        {
//            int rowEnd = -1;
//            int colEnd = -1;
//            string cell = sheet.Rows[1, 1].Value?.ToString(); // Cell[1,1], 存放行列表头结束位置
//            if (!string.IsNullOrEmpty(cell))
//            {
//                string[] strArray = cell.Split(Separator);
//                if (strArray.Length != 2)
//                {
//                    Debug.LogError(string.Format("表格的[1,1]表格, 填写的数量必须为两个，并用{0}隔开！！", Separator));
//                }
//                else
//                {
//                    if (!int.TryParse(strArray[0], out rowEnd) || !int.TryParse(strArray[1], out colEnd))
//                    {
//                        Debug.LogError("表格的[1,1]表格, 填写必须为两个整型！！");
//                    }
//                    else
//                    {
//                        if (rowEnd < 2 || colEnd < 2)
//                        {
//                            Debug.LogError("表格的[1,1]表格, 填写的两个整型必须不小于2！！");
//                        }
//                    }
//                }
//            }
//            else
//            {
//                rowEnd = 2;
//                string curCell = sheet.Rows[rowEnd, 1].Value?.ToString();
//                while (!string.IsNullOrEmpty(curCell))
//                {
//                    rowEnd++;
//                    curCell = sheet.Rows[rowEnd, 1].Value?.ToString();
//                }
//                colEnd = 2;
//                curCell = sheet.Rows[1, colEnd].Value?.ToString();
//                while(!string.IsNullOrEmpty(curCell))
//                {
//                    colEnd++;
//                    curCell = sheet.Rows[1, colEnd].Value?.ToString();
//                }
//            }
//            return (rowEnd, colEnd);
//        }
//    }
//}
