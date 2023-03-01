using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;

namespace Ninth.Editor
{
    public enum SearchResultMode
    {
        Table,
        Value
    }
    
    public class SearchResultOne
    {
        public string Path;
        public string Value;

        public int Sheet;
        public int Row;
        public int Col;
        public string CellValue;
    }

    public class ExcelSearchWindow : EditorWindow
    {
        // Input
        private static List<string> m_SearchObjList;
        private static int m_Count;

        // Result
        private static List<string> m_SearchObjResultList;
        private static List<SearchResultOne> m_SearchResult;
        private static Dictionary<string, Dictionary<string, LinkedList<SearchResultOne>>> m_SearchResultDic;
        private static Dictionary<string, bool> m_FoldOutDic;
        private static Dictionary<string, Dictionary<string, bool>> m_FoldOutDic2;
        private static bool m_SearchResultInit;
        private static string m_SearchResultIntro;

        private static Vector2 m_SearchResultSV_V2;

        private void Awake()
        {
            m_SearchObjList = new List<string>();
            m_Count = 1;
            m_SearchObjResultList = new List<string>();
            m_SearchResult = new List<SearchResultOne>();
            m_SearchResultDic = new Dictionary<string, Dictionary<string, LinkedList<SearchResultOne>>>();
            m_FoldOutDic = new Dictionary<string, bool>();
            m_FoldOutDic2 = new Dictionary<string, Dictionary<string, bool>>();
        }

        private void OnGUI()
        {
            GUILayout.Space(20);
            GUILayout.Label("Excel Path Settings", EditorStyles.boldLabel);

            SetExcelPathDirectoryRoot();
            SetSearchObj();
            SetSearch();
            ExchangeMode();
            SetSearchResult();
            GUILayout.Label(m_SearchResultIntro);
        }

        private static void SetExcelPathDirectoryRoot()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            PlayerPrefsDefine.ExcelSearchPathDirectoryRoot = EditorGUILayout.TextField("ExcelPathDirectoryRoot", PlayerPrefsDefine.ExcelSearchPathDirectoryRoot);
            GUI.enabled = true;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Select a folder to store resources", PlayerPrefsDefine.ExcelSearchPathDirectoryRoot, "Excels");
                if (!string.IsNullOrEmpty(path))
                {
                    PlayerPrefsDefine.ExcelSearchPathDirectoryRoot = path;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void SetSearchObj()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Search Obj", EditorStyles.boldLabel);
            if (GUILayout.Button("Remove"))
            {
                if (m_Count > 1)
                {
                    m_Count--;
                }
            }
            if (GUILayout.Button("Add"))
            {
                m_Count++;
            }
            EditorGUILayout.EndHorizontal();
            for (int index = m_SearchObjList.Count; index < m_Count; index++)
            {
                m_SearchObjList.Add(string.Empty);
            }
            for (int index = m_Count; index < m_SearchObjList.Count; index++)
            {
                m_SearchObjList.RemoveAt(m_SearchObjList.Count - 1);
            }
            EditorGUI.indentLevel++;
            for (int index = 0; index < m_SearchObjList.Count; index++)
            {
                m_SearchObjList[index] = EditorGUILayout.TextField($"Value{index + 1}", m_SearchObjList[index]);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private static void SetSearch()
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Search"))
            {
                Search();
            }
            EditorGUILayout.EndVertical();
        }

        private static void Search()
        {
            List<string> NonEmptySearchObjectList = new List<string>();
            for(int index = 0; index < m_SearchObjList.Count; index++)
            {
                if (!string.IsNullOrEmpty(m_SearchObjList[index]))
                {
                    NonEmptySearchObjectList.Add(m_SearchObjList[index]);
                }
            }
            if(NonEmptySearchObjectList.Count == 0)
            {
                return;
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            m_SearchObjResultList.Clear();
            for(int index = 0; index < NonEmptySearchObjectList.Count; index++)
            {
                m_SearchObjResultList.Add(NonEmptySearchObjectList[index]);
            }
            m_SearchResult.Clear();
            m_SearchResultInit = false;
            DirectoryInfo directory = new DirectoryInfo(PlayerPrefsDefine.ExcelSearchPathDirectoryRoot);
            FileInfo[] fileInfos = directory.GetFiles();
            for (int index = 0; index < fileInfos.Length; index++)
            {
                if (!SearchXLS(fileInfos[index]))
                {
                    SearchXLSX(fileInfos[index]);
                }
            }
            stopwatch.Stop();
            m_SearchResultIntro = $"This search takes time,Milliseconds:{stopwatch.ElapsedMilliseconds},Ticks:{stopwatch.ElapsedTicks}";
        }

        private static bool SearchXLS(FileInfo fileInfo)
        {
            try
            {
                using (FileStream fsRead = File.OpenRead(fileInfo.FullName))
                {
                    IWorkbook wk = new HSSFWorkbook(fsRead);
                    for (int i = 0; i < wk.NumberOfSheets; i++)
                    {
                        ISheet sheet = wk.GetSheetAt(i);

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
                                for (int index = 0; index < m_SearchObjResultList.Count; index++)
                                {
                                    if (value.IndexOf(m_SearchObjResultList[index]) > -1)
                                    {
                                        SearchResultOne searchResultOne = new SearchResultOne()
                                        {
                                            Path = fileInfo.FullName,
                                            Value = m_SearchObjResultList[index],
                                            Sheet = i + 1,
                                            Row = row + 1,
                                            Col = col + 1,
                                            CellValue = value
                                        };
                                        m_SearchResult.Add(searchResultOne);
                                    }
                                }
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
                for (int i = 1; i <= excelPackage.Workbook.Worksheets.Count; i++)
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[i];

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
                            if(string.IsNullOrEmpty(value))
                            {
                                continue;
                            }
                            for (int index = 0; index < m_SearchObjResultList.Count; index++)
                            {
                                UnityEngine.Debug.Log("value: " + value + " SearchObjResult: " + m_SearchObjResultList[index]);
                                if (value.Contains(m_SearchObjResultList[index]))
                                {
                                    SearchResultOne searchResultOne = new SearchResultOne()
                                    {
                                        Path = fileInfo.FullName,
                                        Value = m_SearchObjResultList[index],
                                        Sheet = i,
                                        Row = row,
                                        Col = col,
                                        CellValue = value,
                                    };
                                    m_SearchResult.Add(searchResultOne);
                                }
                            }
                        }
                        row++;
                    }

                }
            }
        }
        private static void ExchangeMode()
        {
            string btnName = (SearchResultMode)PlayerPrefsDefine.ExcelSearchResultShowMode switch
            {
                SearchResultMode.Table => "TableMode",
                SearchResultMode.Value => "ValueMode",
                _ => throw new System.NotImplementedException(),
            };
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(btnName, EditorStyles.miniButtonMid, GUILayout.Width(85)))
            {
                switch ((SearchResultMode)PlayerPrefsDefine.ExcelSearchResultShowMode)
                {
                    case SearchResultMode.Table:
                        {
                            PlayerPrefsDefine.ExcelSearchResultShowMode = (int)SearchResultMode.Value;
                            m_SearchResultInit = false;
                            break;
                        }
                    case SearchResultMode.Value:
                        {
                            PlayerPrefsDefine.ExcelSearchResultShowMode = (int)SearchResultMode.Table;
                            m_SearchResultInit = false;
                            break;
                        }
                }
            }
            GUILayout.EndHorizontal();
        }

        private static void SetSearchResult()
        {
            if (!m_SearchResultInit)
            {
                m_SearchResultInit = !m_SearchResultInit;
                m_SearchResultDic.Clear();
                switch ((SearchResultMode)PlayerPrefsDefine.ExcelSearchResultShowMode)
                {
                    case SearchResultMode.Table:
                        {
                            for(int index = 0; index < m_SearchResult.Count; index++)
                            {
                                string key = m_SearchResult[index].Path;
                                string key2 = m_SearchResult[index].Value;
                                if(!m_SearchResultDic.ContainsKey(key))
                                {
                                    m_SearchResultDic.Add(key, new Dictionary<string, LinkedList<SearchResultOne>>());
                                }
                                if (!m_SearchResultDic[key].ContainsKey(key2))
                                {
                                    m_SearchResultDic[key].Add(key2, new LinkedList<SearchResultOne>());
                                }
                                m_SearchResultDic[key][key2].AddLast(m_SearchResult[index]);
                            }
                            break;
                        }
                    case SearchResultMode.Value:
                        {
                            for (int index = 0; index < m_SearchResult.Count; index++)
                            {
                                string key = m_SearchResult[index].Value;
                                string key2 = m_SearchResult[index].Path;
                                if (!m_SearchResultDic.ContainsKey(key))
                                {
                                    m_SearchResultDic.Add(key, new Dictionary<string, LinkedList<SearchResultOne>>());
                                }
                                if (!m_SearchResultDic[key].ContainsKey(key2))
                                {
                                    m_SearchResultDic[key].Add(key2, new LinkedList<SearchResultOne>());
                                }
                                m_SearchResultDic[key][key2].AddLast(m_SearchResult[index]);
                            }
                            break;
                        }
                }
                m_FoldOutDic.Clear();
                m_FoldOutDic2.Clear();
                foreach (var item in m_SearchResultDic)
                {
                    m_FoldOutDic.Add(item.Key, true);
                    m_FoldOutDic2.Add(item.Key, new Dictionary<string, bool>());
                    foreach (var item2 in item.Value)
                    {
                        m_FoldOutDic2[item.Key].Add(item2.Key, true);
                    }
                }
            }

            m_SearchResultSV_V2 = EditorGUILayout.BeginScrollView(m_SearchResultSV_V2);
            switch ((SearchResultMode)PlayerPrefsDefine.ExcelSearchResultShowMode)
            {
                case SearchResultMode.Table:
                    {
                        SetSearchResultTableMode();
                        break;
                    }
                case SearchResultMode.Value:
                    {
                        SetSearchResultValueMode();
                        break;
                    }
            }
            EditorGUILayout.EndScrollView();
        }

        private static void SetSearchResultTableMode()
        {
            EditorGUILayout.BeginVertical();
            foreach (var item in m_SearchResultDic)
            {
                string key = item.Key;
                var value = item.Value;
                int count = 0;
                foreach(var item2 in value)
                {
                    count += item2.Value.Count;
                }
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"{key}({count})", EditorStyles.boldLabel);
                if (GUILayout.Button(m_FoldOutDic[key] ? $"Foldout({count})" : $"Unfold({count})"))
                {
                    m_FoldOutDic[key] = !m_FoldOutDic[key];
                }
                if (GUILayout.Button("Enter"))
                {
                    Process.Start(key);
                }
                EditorGUILayout.EndHorizontal();

                if (!m_FoldOutDic[key])
                {
                    EditorGUI.indentLevel++;
                    for(int index = 0; index < m_SearchObjResultList.Count; index++)
                    {
                        if (value.ContainsKey(m_SearchObjResultList[index]))
                        {
                            string key2 = m_SearchObjResultList[index];
                            var value2 = value[key2];

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField($"Value:{key2}", EditorStyles.boldLabel);
                            if (GUILayout.Button(m_FoldOutDic2[key][key2] ? $"Foldout({value2.Count})" : $"Unfold({value2.Count})"))
                            {
                                m_FoldOutDic2[key][key2] = !m_FoldOutDic2[key][key2];
                            }
                            EditorGUILayout.EndHorizontal();

                            if (!m_FoldOutDic2[key][key2])
                            {
                                EditorGUI.indentLevel++;
                                foreach (var one in value2)
                                {
                                    EditorGUILayout.LabelField($"第{one.Sheet}页，第{one.Row}行，第{one.Col}列，CellValue: {one.CellValue}");
                                }
                                EditorGUI.indentLevel--;
                            }
                        }
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndVertical();
        }

        private static void SetSearchResultValueMode()
        {
            EditorGUILayout.BeginVertical();
            for (int index = 0; index < m_SearchObjResultList.Count; index++)
            {
                string key = m_SearchObjResultList[index];
                if(m_SearchResultDic.ContainsKey(key))
                {
                    var value = m_SearchResultDic[key];
                    int count = 0;
                    foreach(var item2 in value)
                    {
                        count += item2.Value.Count;
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Value:{key}", EditorStyles.boldLabel);
                    if (GUILayout.Button(m_FoldOutDic[key]? $"Foldout({count})" : $"Unfold({count})"))
                    {
                        m_FoldOutDic[key] = !m_FoldOutDic[key];
                    }
                    EditorGUILayout.EndHorizontal();

                    if (!m_FoldOutDic[key])
                    {
                        EditorGUI.indentLevel++;
                        foreach (var item2 in value)
                        {
                            string key2 = item2.Key;
                            var value2 = item2.Value;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField($"{key2}({item2.Value.Count})", EditorStyles.boldLabel);
                            if (GUILayout.Button(m_FoldOutDic2[key][key2] ? $"Foldout({item2.Value.Count})" : $"Unfold({item2.Value.Count})"))
                            {
                                m_FoldOutDic2[key][key2] = !m_FoldOutDic2[key][key2];
                            }
                            if (GUILayout.Button("Enter"))
                            {
                                Process.Start(key2);
                            }
                            EditorGUILayout.EndHorizontal();

                            if (!m_FoldOutDic2[key][key2])
                            {
                                EditorGUI.indentLevel++;
                                foreach (var one in value2)
                                {
                                    EditorGUILayout.LabelField($"第{one.Sheet}页，第{one.Row}行，第{one.Col}列，CellValue: {one.CellValue}");
                                }
                                EditorGUI.indentLevel--;
                            }
                        }
                        EditorGUI.indentLevel--;
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}
