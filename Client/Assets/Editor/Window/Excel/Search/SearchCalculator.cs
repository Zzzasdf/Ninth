using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Ninth.Editor.Excel.Search
{
    using Table = TableCollect.Table;
    using Sheet = TableCollect.Table.Sheet;
    using Cell = TableCollect.Table.Sheet.Cell;
    using ISearchModeCalculator = SearchModeCalculatorFactory.ISearchModeCalculator;

    public class SearchCalculator
    {
        public static List<Cell> Get(TableCollect compileData, List<string> searchItems, SearchMode searchMode, StringComparison stringComparison)
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
                        if (calculator.IsMatch(cell.Value, searchItems, stringComparison))
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
