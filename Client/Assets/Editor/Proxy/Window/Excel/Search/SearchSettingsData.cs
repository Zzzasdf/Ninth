// using System.Collections.Generic;
// using System;
// using System.Text;
// using System.Linq;
//
// namespace Ninth.Editor.Window
// {
//     using Cell = TableCollect.Table.Sheet.Cell;
//
//     public class SearchSettingsData
//     {
//         // Compile
//         public string CompileDirectoryRoot
//         {
//             get => WindowSOCore.Get<ExcelSO>().CompileDirectoryRoot;
//             set => WindowSOCore.Get<ExcelSO>().CompileDirectoryRoot = value;
//         }
//         public TableCollect CompileData { get; set; }
//         public string CompileInfo { get; set; }
//
//         // Search
//         public List<string> SearchItems { get; set; }
//         public SearchMode SearchMode
//         {
//             get => WindowSOCore.Get<ExcelSO>().SearchMode;
//             set => WindowSOCore.Get<ExcelSO>().SearchMode = value;
//         }
//         public List<Cell> SearchCells { get; set; }
//
//         // SearchData
//         public string SearchInfo { get; set; }
//         public bool IsFoldoutSearchData { get; set; } = true;
//         public StringComparison ComparisonType
//         { 
//             get => WindowSOCore.Get<ExcelSO>().SearchCompairsonType;
//             set => WindowSOCore.Get<ExcelSO>().SearchCompairsonType = value;
//         }
//         public SearchDataMode SearchDataMode
//         {
//             get => WindowSOCore.Get<ExcelSO>().SearchDataMode;
//             set => WindowSOCore.Get<ExcelSO>().SearchDataMode = value;
//         }
//         public Dictionary<string, TableCollect> SearchData { get; set; }
//
//         public int SelectionGridIndex { get; set; }
//         public int SearchDataPageIndex { get; set; }
//         public int SearchDataTempGroupItems { get; set; }
//         public int SearchDataGroupItems
//         {
//             get => WindowSOCore.Get<ExcelSO>().SearchDataGroupItems;
//             set => WindowSOCore.Get<ExcelSO>().SearchDataGroupItems = value;
//         }
//
//         public void Compile()
//         {
//             CompileData = CompileCalculator.Get(CompileDirectoryRoot);
//             SetCompileInfo();
//             ClearSearchItems();
//             ClearSearchCells();
//             ClearSearchInfo();
//             ClearSearchData();
//             
//             void SetCompileInfo()
//             {
//                 CompileInfo = string.Format("编译路径：{0}", CompileDirectoryRoot);
//             }
//
//             void ClearSearchItems()
//             {
//                 if(SearchItems == null)
//                 {
//                     SearchItems = new List<string>();
//                 }
//                 else
//                 {
//                     SearchItems.Clear();
//                 }
//             }
//
//             void ClearSearchCells()
//             {
//                 if(SearchCells == null)
//                 {
//                     SearchCells = new List<Cell>();
//                 }
//                 else
//                 {
//                     SearchCells.Clear();
//                 }
//             }
//
//             void ClearSearchInfo()
//             {
//                 SearchInfo = string.Empty;
//             }
//
//             void ClearSearchData()
//             {
//                 if(SearchData == null)
//                 {
//                     SearchData = new Dictionary<string, TableCollect>();
//                 }
//                 else
//                 {
//                     SearchData.Clear();
//                 }
//             }
//         }
//
//         public void Search()
//         {
//             List<string> filterSearchItems = FilterItems(SearchItems, ComparisonType);
//             SearchCells = SearchCalculator.Get(CompileData, filterSearchItems, SearchMode, ComparisonType);
//             SearchInfo = GetSearchInfo(SearchMode, filterSearchItems, ComparisonType);
//
//             List<string> FilterItems(List<string> searchItems, StringComparison comparison)
//             {
//                 List<string> result = new();
//                 for(int index = 0; index < searchItems.Count; index++)
//                 {
//                     if (result.Select(x => x.Equals(searchItems[index].Trim(), comparison)).ToList().Count == 0)
//                     {
//                         result.Add(searchItems[index]);
//                     }
//                 }
//                 return result;
//             }
//
//             string GetSearchInfo(SearchMode searchMode, List<string> searchItems, StringComparison comparisonType)
//             {
//                 StringBuilder result = new StringBuilder()
//                     .AppendFormat("搜索模式：[{0}]", searchMode).AppendLine()
//                     .AppendFormat("比较条件：[{0}]", ComparisonType.ToString()).AppendLine();
//                 for (int index = 0; index < searchItems.Count; index++)
//                 {
//                     result.AppendFormat("Item[{0}] => {1}", index, searchItems[index]);
//                     if (index != searchItems.Count - 1)
//                     {
//                         result.AppendLine();
//                     }
//                 }
//                 return result.ToString();
//             }
//         }
//         
//     }
// }
