using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor.Excel.Search
{
    using Table = TableData.Table;
    using Sheet = TableData.Table.Sheet;
    using Cell = TableData.Table.Sheet.Cell;

    public class SearchSettings
    {
        private SearchSettingsData data;

        public SearchSettings()
        {
            data = new SearchSettingsData();
        }

        public void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope("frameBox"))
            {
                RenderCompileDirectoryRoot();
                RenderBtnCompile();
            }

            if (data?.CompileData?.Tables == null
                || data.CompileData.Tables.Count == 0)
            {
                return;
            }

            using (new EditorGUILayout.VerticalScope("frameBox"))
            {
                RenderSearchItems();
                RenderSearchMode();
                RenderBtnSearch();
            }

            if(data?.SearchCells == null
                || data.SearchCells.Count == 0)
            {
                return;
            }

            using (new EditorGUILayout.VerticalScope("frameBox"))
            {
                RenderSearchDataMode();
                RenderSearchData();
            }
        }

        #region Compile
        private void RenderCompileDirectoryRoot()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                data.CompileDirectoryRoot = WindowUtility.RenderDirectory(data.CompileDirectoryRoot);
            }
        }

        private void RenderBtnCompile()
        {
            if (GUILayout.Button("Compile"))
            {
                data.CompileData = CompileCalculator.Get(data.CompileDirectoryRoot);
            }
        }
        #endregion

        #region Search
        private void RenderSearchItems()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                if (data.SearchItems == null)
                {
                    data.SearchItems = new List<string>();
                }
                int count = EditorGUILayout.IntField(string.Format("{0}.Count", nameof(data.SearchItems)), data.SearchItems.Count);
                for (int index = 0; index < data.SearchItems.Count; index++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        data.SearchItems[index] = EditorGUILayout.TextField(string.Format("Item{0}", index), data.SearchItems[index]);
                        if (GUILayout.Button("Remove"))
                        {
                            data.SearchItems.RemoveAt(index);
                            count--;
                        }
                    }
                }
                if (GUILayout.Button("+"))
                {
                    count++;
                }
                if (count > data.SearchItems.Count)
                {
                    int addCount = count - data.SearchItems.Count;
                    for (int index = 0; index < addCount; index++)
                    {
                        data.SearchItems.Add(string.Empty);
                    }
                }
                else if (count < data.SearchItems.Count)
                {
                    data.SearchItems = data.SearchItems.Take(count).ToList();
                }
            }
        }

        private void RenderSearchMode()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                string[] barMenu = SearchModeCalculatorFactory.GetHeads().Select(x => x.ToString()).ToArray();
                SearchMode tempMode = (SearchMode)GUILayout.Toolbar((int)data.SearchMode, barMenu);
                if (tempMode != data.SearchMode)
                {
                    data.SearchMode = tempMode;
                }
            }
        }

        private void RenderBtnSearch()
        {
            if (GUILayout.Button("Search"))
            {
                data.SearchCells = SearchCalculator.Get(data.CompileData, data.SearchItems, data.SearchMode);
                ResetRenderSearchResult();
            }
        }
        #endregion

        #region SearchResult
        private void RenderSearchDataMode()
        {
            string[] barMenu = SearchDataModeCalculatorFactory.GetHeads().Select(x => x.ToString()).ToArray();
            SearchDataMode tempMode = (SearchDataMode)GUILayout.Toolbar((int)data.SearchDataMode, barMenu);
            if (tempMode != data.SearchDataMode)
            {
                data.SearchDataMode = tempMode;
                ResetRenderSearchResult();
            }
        }

        private void RenderSearchData()
        {
            string[] texts = data.ExcelSearchResult.Select(x => SearchDataModeCalculatorFactory.Get(data.SearchDataMode).GridName(x)).ToArray();
            if (data.SelectionGridIndex > data.ExcelSearchResult.Count - 1)
            {
                data.SelectionGridIndex = data.ExcelSearchResult.Count - 1;
            }
            data.SelectionGridIndex = GUILayout.SelectionGrid(data.SelectionGridIndex, texts, 5);
            TableData result = data.ExcelSearchResult.Values.ToList()[data.SelectionGridIndex];
            foreach (var table in result.Tables)
            {
                string tableKey = table.Key;
                Table tableValue = table.Value;
                result.SetFolder(tableKey, EditorGUILayout.Foldout(result.GetFolder(tableKey), tableValue.Name));
                if(result.GetFolder(tableKey))
                {
                    foreach (var sheet in tableValue.Sheets)
                    {
                        int sheetKey = sheet.Key;
                        Sheet sheetValue = sheet.Value;
                        foreach (var cell in sheetValue.Cells)
                        {
                            (int, int) cellKey = cell.Key;
                            Cell cellValue = cell.Value;
                            using (new EditorGUILayout.VerticalScope("frameBox"))
                            {
                                using (new EditorGUILayout.HorizontalScope())
                                {
                                    EditorGUILayout.LabelField(string.Format("Table:{0}, Sheet:{1}, Row:{2}, Col{3}", cellValue.Sheet.Table.Name, cellValue.Sheet.SheetIndex, cellValue.Row, cellValue.Col));
                                    if (GUILayout.Button("OpenExcel"))
                                    {
                                        Process.Start(cellValue.Sheet.Table.FullName);
                                    }
                                }
                                EditorGUILayout.TextArea(cellValue.Value);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        private void ResetRenderSearchResult()
        {
            if (data.ExcelSearchResult == null)
            {
                data.ExcelSearchResult = new Dictionary<string, TableData>();
            }
            else
            {
                data.ExcelSearchResult.Clear();
            }
            if (data.SearchCells == null)
            {
                data.SearchCells = new List<Cell>();
            }
            for (int index = 0; index < data.SearchCells.Count; index++)
            {
                Cell cell = data.SearchCells[index];
                string key = SearchDataModeCalculatorFactory.Get(data.SearchDataMode).GroupName(cell, data.SearchItems);
                if (!data.ExcelSearchResult.TryGetValue(key, out TableData value))
                {
                    value = new TableData();
                    data.ExcelSearchResult.Add(key, value);
                }
                value.AddCell(cell);
            }
            data.SelectionGridIndex = 0;
        }
    }
}
