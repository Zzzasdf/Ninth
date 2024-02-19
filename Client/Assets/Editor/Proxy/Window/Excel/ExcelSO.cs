// using Ninth.Editor.Excel.Search;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
//
// namespace Ninth.Editor.Window
// {
//     public enum ExcelMode
//     {
//         Encode,
//         Search,
//     }
//     
//     [CreateAssetMenu(fileName = "WindowExcelSO", menuName = "EditorSOCollect/WindowExcelSO")]
//     public class ExcelSO: ScriptableObject, IExcelSO
//     {
//         [SerializeField] private string encodePathDirectoryRoot;
//         
//         [SerializeField] private string compileDirectoryRoot;
//         [SerializeField] private StringComparison searchComparisonType;
//         [SerializeField] private SearchMode searchMode;
//         [SerializeField] private SearchDataMode searchDataMode;
//         [SerializeField] private int searchDataGroupItems;
//
//         
//         private void OnEnable()
//         {
//             SetDefaultExcelEncodePathDirectoryRoot();
//             
//             SetDefaultCompileDirectoryRoot();
//             SetDefaultSearchStringComparison();
//             SetDefaultSearchDataGroupItems();
//             
//             void SetDefaultExcelEncodePathDirectoryRoot()
//             {
//                 if (string.IsNullOrEmpty(EncodePathDirectoryRoot))
//                 {
//                     EncodePathDirectoryRoot = $"{Application.dataPath}/../../Excels";
//                 }
//             }
//
//             void SetDefaultCompileDirectoryRoot()
//             {
//                 if (string.IsNullOrEmpty(CompileDirectoryRoot))
//                 {
//                     CompileDirectoryRoot = $"{Application.dataPath}/../../Excels";
//                 }
//             }
//             void SetDefaultSearchStringComparison()
//             {
//                 if(searchComparisonType.Equals(default(StringComparison)))
//                 {
//                     SearchCompairsonType = StringComparison.CurrentCultureIgnoreCase;
//                 }
//             }
//             void SetDefaultSearchDataGroupItems()
//             {
//                 if(searchDataGroupItems == 0)
//                 {
//                     searchDataGroupItems = 100;
//                 }
//             }
//         }
//         
//         public string EncodePathDirectoryRoot
//         {
//             get => encodePathDirectoryRoot;
//             set => SetProperty(ref encodePathDirectoryRoot, value);
//         }
//
//         public string CompileDirectoryRoot
//         {
//             get => compileDirectoryRoot;
//             set => SetProperty(ref compileDirectoryRoot, value);
//         }
//         public StringComparison SearchCompairsonType
//         {
//             get => searchComparisonType;
//             set => SetProperty(ref searchComparisonType, value);
//         }
//         public SearchMode SearchMode
//         {
//             get => searchMode;
//             set => SetProperty(ref searchMode, value);
//         }
//         public SearchDataMode SearchDataMode
//         {
//             get => searchDataMode;
//             set => SetProperty(ref searchDataMode, value);
//         }
//         public int SearchDataGroupItems
//         {
//             get => searchDataGroupItems;
//             set
//             {
//                 if(value > 0)
//                 {
//                     SetProperty(ref searchDataGroupItems, value);
//                 }
//             }
//         }
//
//         private void SetProperty<T>(ref T field, T value)
//         {
//             if (!Equals(field, value))
//             {
//                 field = value;
//                 SaveAssets();
//             }
//         }
//         private void SaveAssets()
//         {
//             EditorUtility.SetDirty(this);
//             AssetDatabase.SaveAssetIfDirty(this);
//         }
//     }
// }
