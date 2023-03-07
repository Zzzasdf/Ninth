using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class ExcelSearch
    {
        private static SearchMode m_ExcelSearchMode
        {
            get => (SearchMode)PlayerPrefsDefine.ExcelSearchMode;
            set => PlayerPrefsDefine.ExcelSearchMode = (int)value;
        }

        private static Action<CompileCell, string, int> m_FilterFunc;
        private static List<string> m_SearchObjResultList;
        private static List<SearchResultCell> m_SearchResult;
        private static bool m_SearchResultInit;

        private static string m_SearchResultTypeInfo;
        private static string m_SearchResultIntro;

        #region SearchMode
        private static void ExchangeSearchMode()
        {
            GUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Search", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            string[] barMenu = new string[]
            {
                "ExactMode",
                "ExistMode"
            };
            SearchMode mode = (SearchMode)GUILayout.Toolbar((int)m_ExcelSearchMode, barMenu);
            if(mode != m_ExcelSearchMode)
            {
                m_ExcelSearchMode = mode;
                m_FilterFunc = m_ExcelSearchMode switch
                {
                    SearchMode.Exact => ExactFunc,
                    SearchMode.Exist => ExistFunc,
                    _ => throw new NotImplementedException(),
                };
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void ExactFunc(CompileCell compileCell, string path, int sheetIndex)
        {
            if (m_SearchObjResultList.Contains(compileCell.CellValue))
            {
                SearchResultCell searchResultCell = new SearchResultCell()
                {
                    SearchValue = compileCell.CellValue,
                    Path = path,
                    SheetIndex = sheetIndex,
                    Row = compileCell.Row,
                    Col = compileCell.Col,
                    CellValue = compileCell.CellValue,
                };
                m_SearchResult.Add(searchResultCell);
            }
        }

        private static void ExistFunc(CompileCell compileCell, string path, int sheetIndex)
        {
            for (int index = 0; index < m_SearchObjResultList.Count; index++)
            {
                if (compileCell.CellValue.Contains(m_SearchObjResultList[index]))
                {
                    SearchResultCell searchResultCell = new SearchResultCell()
                    {
                        SearchValue = m_SearchObjResultList[index],
                        Path = path,
                        SheetIndex = sheetIndex,
                        Row = compileCell.Row,
                        Col = compileCell.Col,
                        CellValue = compileCell.CellValue,
                    };
                    m_SearchResult.Add(searchResultCell);
                }
            }
        }
        #endregion

        #region Search
        private static void SetSearch()
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Search"))
            {
                List<string> NonEmptySearchObjectList = GetValidSearchObj();
                if (NonEmptySearchObjectList.Count == 0)
                {
                    m_SearchResultIntro = "Invalid Search Value!!";
                }
                else
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    if (m_SearchObjResultList == null)
                    {
                        m_SearchObjResultList = new List<string>();
                    }
                    else
                    {
                        m_SearchObjResultList.Clear();
                    }
                    for (int index = 0; index < NonEmptySearchObjectList.Count; index++)
                    {
                        m_SearchObjResultList.Add(NonEmptySearchObjectList[index]);
                    }
                    if (m_SearchResult == null)
                    {
                        m_SearchResult = new List<SearchResultCell>();
                    }
                    else
                    {
                        m_SearchResult.Clear();
                    }
                    m_SearchResultInit = false;

                    for (int tableIndex = 0; tableIndex < m_Compile.Tables.Count; tableIndex++)
                    {
                        CompileTable table = m_Compile.Tables[tableIndex];
                        for (int sheetIndex = 0; sheetIndex < table.CompileSheets.Count; sheetIndex++)
                        {
                            CompileSheet sheet = table.CompileSheets[sheetIndex];
                            for (int cellIndex = 0; cellIndex < sheet.Cells.Count; cellIndex++)
                            {
                                CompileCell cell = sheet.Cells[cellIndex];
                                m_FilterFunc(cell, table.Path, sheet.SheetIndex);
                            }
                        }
                    }
                    stopwatch.Stop();

                    m_SearchResultTypeInfo = m_ExcelSearchMode switch
                    {
                        SearchMode.Exact => "ExactSearchResult",
                        SearchMode.Exist => "ExistSearchResult",
                        _ => throw new System.NotImplementedException(),
                    };

                    m_SearchResultIntro = $"This Search Takes Time, Milliseconds:{stopwatch.ElapsedMilliseconds}, Ticks:{stopwatch.ElapsedTicks}";
                }
            }
            EditorGUILayout.EndVertical();
        }
        #endregion
    }
}
