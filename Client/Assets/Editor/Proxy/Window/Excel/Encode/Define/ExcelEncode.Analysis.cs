using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System;

namespace Ninth.Editor.Window
{
    public partial class ExcelEncodeSettings
    {
        // private TableAnalysis GetCommonTableDefineAnalysic(ExcelPackage excelPackage)
        // {
        //     TableAnalysis tableAnalysis = new TableAnalysis();

        //     for(int index = 1; index < excelPackage.Workbook.Worksheets.Count; index++)
        //     {
        //         ExcelWorksheet sheet =  excelPackage.Workbook.Worksheets[index];
        //         // 添加获取所有行表头解析字段
        //         List<(string rowTitleAnalysisField, int rowIndex)> rowTuples = IncreaseRowTableTitleAnalysis(sheet);
        //         for(int i = 0; i < rowTuples.Count; i++)
        //         {
        //             tableAnalysis.SetRowTableTitleAnalysisDic(index, rowTuples[i].rowTitleAnalysisField, rowTuples[i].rowIndex);
        //         }
        //         // 获取所有列表头解析字段
        //         List<(string colTitleAnalysisField, int colIndex)> colTuples = IncreaseColTableTitleAnalysis(sheet);
        //         for(int i = 0; i < colTuples.Count; i++)
        //         {
        //             tableAnalysis.SetColTableTitleAnalysisDic(index, colTuples[i].colTitleAnalysisField, colTuples[i].colIndex);
        //         }
        //         // 获取所有字段
                
        //         // 设置所有字段格式
        //     }
            
        // }

        public class TableAnalysis
        {
            // key => Name
            private Dictionary<string, Name2Infos> analysisDic { get; set; }

            private TableAnalysis()
            {
                analysisDic = new Dictionary<string, Name2Infos>();
            }

            public bool TryAdd2Analysis(string name, List<string> rowTableTitleDefines, List<string> colTableTitleLimits)
            {
                if(!analysisDic.TryGetValue(name, out Name2Infos value))
                {
                    value = new Name2Infos();
                    analysisDic.Add(name, value);
                }
                // value.RowTableTitleDefines


                return true;
            }
        }

        private class Name2Infos
        {
            // RowFieldDic
            public List<string> RowTableTitleDefines { get; private set; }

            // ColFieldDic
            public List<string> ColTableTitleLimits { get; private set; }

            public Name2Infos()
            {
                RowTableTitleDefines = new List<string>();
                ColTableTitleLimits = new List<string>();
            }
        }
    }
}
