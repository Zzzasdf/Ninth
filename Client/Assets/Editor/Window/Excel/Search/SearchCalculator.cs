using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Editor.Excel.Search
{
    using Table = TableData.Table;
    using Sheet = TableData.Table.Sheet;
    using Cell = TableData.Table.Sheet.Cell;
    using ISearchModeCalculator = SearchModeCalculatorFactory.ISearchModeCalculator;

    public class SearchCalculator
    {
        public static List<Cell> Get(TableData compileData, List<string> searchItems, SearchMode searchMode)
        {
            ISearchModeCalculator calculator = SearchModeCalculatorFactory.Get(searchMode);
            List<Cell> result = new List<Cell>();
            foreach (var tablePair in compileData.Tables)
            {
                Table table = tablePair.Value;
                foreach (var sheetPair in table.Sheets)
                {
                    Sheet sheet = sheetPair.Value;
                    foreach (var cellPair in sheet.Cells)
                    {
                        Cell cell = cellPair.Value;
                        if (calculator.IsMatch(cell.Value, searchItems))
                        {
                            result.Add(cell);
                        }
                    }
                }
            }
            return result;
        }
    }
}
