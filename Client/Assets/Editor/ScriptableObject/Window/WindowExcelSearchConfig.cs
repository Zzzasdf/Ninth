using Ninth.Editor.Excel.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "WindowExcelSearchConfigSO", menuName = "EditorSOCollect/WindowExcelSearchConfigSO")]
    public class WindowExcelSearchConfig: ScriptableObject
    {
        [SerializeField] private string compileDirectoryRoot;
        [SerializeField] private StringComparison searchComparisonType;
        [SerializeField] private SearchMode searchMode;
        [SerializeField] private SearchDataMode searchDataMode;
        [SerializeField] private int searchDataGroupItems;

        private void OnEnable()
        {
            SetDefaultCompileDirectoryRoot();
            SetDefaultSearchStringComparison();
            SetDefaultSearchDataGroupItems();

            void SetDefaultCompileDirectoryRoot()
            {
                if (string.IsNullOrEmpty(CompileDirectoryRoot))
                {
                    CompileDirectoryRoot = $"{Application.dataPath}/../../Excels";
                }
            }
            void SetDefaultSearchStringComparison()
            {
                if(searchComparisonType.Equals(default(StringComparison)))
                {
                    SearchCompairsonType = StringComparison.CurrentCultureIgnoreCase;
                }
            }
            void SetDefaultSearchDataGroupItems()
            {
                if(searchDataGroupItems == 0)
                {
                    searchDataGroupItems = 100;
                }
            }
        }

        public string CompileDirectoryRoot
        {
            get => compileDirectoryRoot;
            set => SetProperty(ref compileDirectoryRoot, value);
        }
        public StringComparison SearchCompairsonType
        {
            get => searchComparisonType;
            set => SetProperty(ref searchComparisonType, value);
        }
        public SearchMode SearchMode
        {
            get => searchMode;
            set => SetProperty(ref searchMode, value);
        }
        public SearchDataMode SearchDataMode
        {
            get => searchDataMode;
            set => SetProperty(ref searchDataMode, value);
        }
        public int SearchDataGroupItems
        {
            get => searchDataGroupItems;
            set
            {
                if(value > 0)
                {
                    SetProperty(ref searchDataGroupItems, value);
                }
            }
        }

        private void SetProperty<T>(ref T field, T value)
        {
            if (!Equals(field, value))
            {
                field = value;
                SaveAssets();
            }
        }
        private void SaveAssets()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
    }
}
