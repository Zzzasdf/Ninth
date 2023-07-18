using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ninth.Editor.Excel.Search
{
    using Cell = TableData.Table.Sheet.Cell;

    public class SearchDataModeCalculatorFactory
    {
        private readonly static Dictionary<SearchDataMode, ISearchDataModeCalculator> factory = new Dictionary<SearchDataMode, ISearchDataModeCalculator>()
        {
            [SearchDataMode.Value] = new SearchDataModeValueCalculator(),
            [SearchDataMode.Table] = new SearchDataModeTableCalculator(),
        };

        public static List<SearchDataMode> GetHeads()
        {
            return factory.Keys.ToList();
        }

        public static ISearchDataModeCalculator Get(SearchDataMode searchDataMode)
        {
            if(!factory.TryGetValue(searchDataMode, out ISearchDataModeCalculator result))
            {
                throw new NullReferenceException();
            }
            return result;
        }

        public interface ISearchDataModeCalculator
        {
            string GroupName(Cell cell, List<string> searchItems);
            string GridName(KeyValuePair<string, TableData> keyValuePair);
        }

        public class SearchDataModeValueCalculator : ISearchDataModeCalculator
        {
            public string GroupName(Cell cell, List<string> searchItems)
            {
                if (cell == null
                    || string.IsNullOrEmpty(cell.Value)
                    || searchItems == null
                    || searchItems.Count == 0)
                {
                    throw new ArgumentNullException();
                }
                string result = string.Empty;
                for (int index = 0; index < searchItems.Count; index++)
                {
                    if (cell.Value.Contains(searchItems[index]))
                    {
                        result = searchItems[index];
                        return result;
                    }
                }
                throw new ArgumentOutOfRangeException();
            }

            public string GridName(KeyValuePair<string, TableData> keyValuePair)
            {
                if (keyValuePair.Equals(default(KeyValuePair<string, TableData>))
                    || keyValuePair.Value?.Tables?.Values == null)
                {
                    throw new ArgumentNullException();
                }
                int count = 0;
                foreach (var table in keyValuePair.Value.Tables.Values)
                {
                    if (table?.Sheets?.Values == null)
                    {
                        throw new ArgumentNullException();
                    }
                    foreach (var sheet in table.Sheets.Values)
                    {
                        count += sheet.Cells.Count;
                    }
                }
                return string.Format("{0}[{1}]", keyValuePair.Key, count);
            }
        }

        public class SearchDataModeTableCalculator : ISearchDataModeCalculator
        {
            public string GroupName(Cell cell, List<string> searchItems)
            {
                if (cell?.Sheet?.Table?.FullName == null)
                {
                    throw new ArgumentNullException();
                }
                string result = cell.Sheet.Table.FullName;
                return result;
            }
            public string GridName(KeyValuePair<string, TableData> keyValuePair)
            {
                if (keyValuePair.Equals(default(KeyValuePair<string, TableData>))
                    || keyValuePair.Value?.Tables?.Values == null)
                {
                    throw new ArgumentNullException();
                }
                int count = 0;
                foreach (var table in keyValuePair.Value.Tables.Values)
                {
                    if (table?.Sheets?.Values == null)
                    {
                        throw new ArgumentNullException();
                    }
                    foreach (var sheet in table.Sheets.Values)
                    {
                        if (sheet?.Cells == null)
                        {
                            throw new ArgumentNullException();
                        }
                        count += sheet.Cells.Count;
                    }
                }
                if (keyValuePair.Value.Tables.Count < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return string.Format("{0}[{1}]", keyValuePair.Value.Tables.First().Value.Name, count);
            }
        }
    }
}
