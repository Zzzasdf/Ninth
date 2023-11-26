using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor.Excel.Search
{
    using Table = TableCollect.Table;
    using Sheet = TableCollect.Table.Sheet;
    using Cell = TableCollect.Table.Sheet.Cell;

    public class SearchSettings
    {
        private SearchSettingsData data;
        private Vector2 vec2SearchData;

        public SearchSettings()
        {
            data = new SearchSettingsData();
        }

        public void OnGUI()
        {
            if (data.IsFoldoutSearchData)
            {
                using (new EditorGUILayout.VerticalScope(CommonLanguage.FrameBox.ToCurrLanguage()))
                {
                    RenderCompileDirectoryRoot();
                    RenderBtnCompile();
                }

                if (data?.CompileData?.Tables == null
                    || data.CompileData.Tables.Count == 0)
                {
                    return;
                }

                using (new EditorGUILayout.VerticalScope(CommonLanguage.FrameBox.ToCurrLanguage()))
                {
                    RenderCompileInfo();
                }

                using (new EditorGUILayout.VerticalScope(CommonLanguage.FrameBox.ToCurrLanguage()))
                {
                    RenderSearchItems();
                    RenderSearchIgnoreCase();
                    RenderSearchMode();
                    RenderBtnSearch();
                }

                if (data?.SearchCells == null
                    || data.SearchCells.Count == 0)
                {
                    return;
                }

                using (new EditorGUILayout.VerticalScope(CommonLanguage.FrameBox.ToCurrLanguage()))
                {
                    RenderSearchInfo();
                }
            }
            RenderBtnFolderSearchData();

            using (new EditorGUILayout.VerticalScope(CommonLanguage.FrameBox.ToCurrLanguage()))
            {
                RenderSearchDataMode();
                RenderSearchDataSelectionGrids();
                using (new EditorGUILayout.HorizontalScope(CommonLanguage.FrameBox.ToCurrLanguage()))
                {
                    RenderSearchDataGroups();
                    using (var scrollView = new EditorGUILayout.ScrollViewScope(vec2SearchData))
                    {
                        vec2SearchData = scrollView.scrollPosition;
                        RenderSearchData();
                    }
                }
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
            if (GUILayout.Button(CommonLanguage.Compile.ToCurrLanguage()))
            {
                data.Compile();
            }
        }
        #endregion

        private void RenderCompileInfo()
        {
            EditorGUILayout.TextArea(data.CompileInfo);
        }

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

        private void RenderSearchIgnoreCase()
        {
            using(new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                data.ComparisonType = (StringComparison)EditorGUILayout.EnumPopup(data.ComparisonType);
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
                data.Search();
                ResetRenderSearchResult();
            }
        }
        #endregion

        private void RenderSearchInfo()
        {
            EditorGUILayout.TextArea(data.SearchInfo);
        }

        #region SearchData
        private void RenderBtnFolderSearchData()
        {
            char charTxt = data.IsFoldoutSearchData ? '∨' : '∧';
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < 65; index++)
            {
                sb.Append(charTxt);
            }
            if (GUILayout.Button(sb.ToString()))
            {
                data.IsFoldoutSearchData = !data.IsFoldoutSearchData;
            }
        }

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

        private void RenderSearchDataSelectionGrids()
        {
            string[] texts = data.SearchData.Select(x => SearchDataModeCalculatorFactory.Get(data.SearchDataMode).GridName(x)).ToArray();
            if (data.SelectionGridIndex > data.SearchData.Count - 1)
            {
                data.SelectionGridIndex = data.SearchData.Count - 1;
            }
            data.SelectionGridIndex = GUILayout.SelectionGrid(data.SelectionGridIndex, texts, 5);
        }

        private void RenderSearchDataGroups()
        {
            using (new GUILayout.VerticalScope())
            {
                using (new GUILayout.HorizontalScope())
                {
                    data.SearchDataTempGroupItems = EditorGUILayout.IntField(data.SearchDataTempGroupItems);
                    if (GUILayout.Button("Confirm"))
                    {
                        if (data.SearchDataGroupItems != data.SearchDataTempGroupItems)
                        {
                            data.SearchDataGroupItems = data.SearchDataTempGroupItems;
                            data.SearchDataTempGroupItems = data.SearchDataGroupItems;
                        }
                    }
                    
                }
                TableCollect tableCollect = data.SearchData.Values.ToList()[data.SelectionGridIndex];
                int count = 0;
                foreach (var table in tableCollect.Tables)
                {
                    Table tableValue = table.Value;
                    foreach (var sheet in tableValue.Sheets)
                    {
                        Sheet sheetValue = sheet.Value;
                        foreach (var cell in sheetValue.Cells)
                        {
                            count++;
                        }
                    }
                }
                List<string> itemsName = new List<string>();
                int wholeCount = count / data.SearchDataGroupItems;
                bool isHavePart = count % data.SearchDataGroupItems != 0;
                for (int index = 0; index < wholeCount; index++)
                {
                    itemsName.Add(string.Format("[{0}-{1}]", data.SearchDataGroupItems * index, data.SearchDataGroupItems * (index + 1) - 1));
                }
                if (isHavePart)
                {
                    itemsName.Add(string.Format("[{0}-{1}]", wholeCount * data.SearchDataGroupItems, count - 1));
                }
                int page = wholeCount + (isHavePart ? 1 : 0);
                data.SearchDataPageIndex = Mathf.Min(data.SearchDataPageIndex, page - 1);
                int tempPage = GUILayout.SelectionGrid(data.SearchDataPageIndex, itemsName.ToArray(), 1);
                if (tempPage != data.SearchDataPageIndex)
                {
                    data.SearchDataPageIndex = tempPage;
                    RenderSearchData();
                }
            }
        }

        private void RenderSearchData()
        {
            TableCollect result = data.SearchData.Values.ToList()[data.SelectionGridIndex];
            int count = 0;
            int startPos = data.SearchDataGroupItems * data.SearchDataPageIndex;
            int endPos = data.SearchDataGroupItems * (data.SearchDataPageIndex + 1) - 1;
            foreach (var table in result.Tables)
            {
                string tableKey = table.Key;
                Table tableValue = table.Value;
                foreach (var sheet in tableValue.Sheets)
                {
                    int sheetKey = sheet.Key;
                    Sheet sheetValue = sheet.Value;
                    foreach (var cell in sheetValue.Cells)
                    {
                        count++;
                        if (count < startPos + 1)
                        {
                            continue;
                        }
                        if (count > endPos + 1)
                        {
                            return;
                        }
                        (int, int) cellKey = cell.Key;
                        Cell cellValue = cell.Value;
                        using (new EditorGUILayout.VerticalScope(CommonLanguage.FrameBox.ToCurrLanguage()))
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
        #endregion

        private void ResetRenderSearchResult()
        {
            if (data.SearchData == null)
            {
                data.SearchData = new Dictionary<string, TableCollect>();
            }
            else
            {
                data.SearchData.Clear();
            }
            if (data.SearchCells == null)
            {
                data.SearchCells = new List<Cell>();
            }
            for (int index = 0; index < data.SearchCells.Count; index++)
            {
                Cell cell = data.SearchCells[index];
                List<string> keys = SearchDataModeCalculatorFactory.Get(data.SearchDataMode).GroupName(cell, data.SearchItems, data.ComparisonType);
                foreach (var key in keys)
                {
                    if (!data.SearchData.TryGetValue(key, out TableCollect value))
                    {
                        value = new TableCollect();
                        data.SearchData.Add(key, value);
                    }
                    value.AddCell(cell);
                }
            }
            data.SelectionGridIndex = 0;
            data.SearchDataTempGroupItems = data.SearchDataGroupItems;
        }
    }
}
