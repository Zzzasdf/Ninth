using System.Collections.Generic;
using System;

namespace Ninth.Editor.Excel.Search
{
    using Cell = TableData.Table.Sheet.Cell;

    public class SearchSettingsData
    {
        // Compile
        public string CompileDirectoryRoot
        {
            get => WindowSOCore.Get<WindowExcelConfig>().ExcelSearchPathDirectoryRoot;
            set => WindowSOCore.Get<WindowExcelConfig>().ExcelSearchPathDirectoryRoot = value;
        }
        public TableData CompileData { get; set; }

        // Search
        public List<string> SearchItems { get; set; }
        public SearchMode SearchMode
        {
            get => WindowSOCore.Get<WindowExcelConfig>().ExcelSearchMode;
            set => WindowSOCore.Get<WindowExcelConfig>().ExcelSearchMode = value;
        }
        public List<Cell> SearchCells { get; set; }

        public Dictionary<SearchMode, Func<string, string, bool>> ExcelSearchModeDic { get; } = new Dictionary<SearchMode, Func<string, string, bool>>()
        {
            [SearchMode.Equals] = (cellValue, searchItem) => cellValue?.Equals(searchItem) ?? false,
            [SearchMode.Contains] = (cellValue, searchItem) => cellValue?.Contains(searchItem) ?? false,
        };

        // SearchResult
        public SearchDataMode SearchDataMode { get; set; }
        public Dictionary<string, TableData> ExcelSearchResult { get; set; }
        public int SelectionGridIndex { get; set; }
    }
}
