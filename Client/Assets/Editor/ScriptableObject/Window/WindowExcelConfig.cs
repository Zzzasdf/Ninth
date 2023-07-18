using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "WindowExcelConfigSO", menuName = "EditorSOCollect/WindowExcelConfigSO")]
    public class WindowExcelConfig : ScriptableObject
    {
        [SerializeField] private ExcelMode excelMode;

        // Encode
        [SerializeField] private string excelEncodePathDirectoryRoot;

        // RenderBtnSearch
        [SerializeField] private string excelSearchPathDirectoryRoot;
        [SerializeField] private SearchMode excelSearchMode;
        [SerializeField] private SearchDataMode excelSearchResultMode;

        public ExcelMode ExcelMode
        {
            get => excelMode;
            set => SetProperty(ref excelMode, value);
        }
        
        // Encode
        public string ExcelEncodePathDirectoryRoot
        {
            get => excelEncodePathDirectoryRoot;
            set => SetProperty(ref excelEncodePathDirectoryRoot, value);
        }

        // RenderBtnSearch
        public string ExcelSearchPathDirectoryRoot
        {
            get => excelSearchPathDirectoryRoot;
            set => SetProperty(ref excelSearchPathDirectoryRoot, value);
        }
        public SearchMode ExcelSearchMode
        {
            get => excelSearchMode;
            set => SetProperty(ref excelSearchMode, value);
        }
        public SearchDataMode ExcelSearchResultMode
        {
            get => excelSearchResultMode;
            set => SetProperty(ref excelSearchResultMode, value);
        }

        private void OnEnable()
        {
            SetDefaultExcelEncodePathDirectoryRoot();
            SetDefaultExcelSearchPathDirectoryRoot();
        }

        private void SetDefaultExcelSearchPathDirectoryRoot()
        {
            if (string.IsNullOrEmpty(ExcelSearchPathDirectoryRoot))
            {
                ExcelSearchPathDirectoryRoot = $"{Application.dataPath}/../../Excels";
            }
        }

        private void SetDefaultExcelEncodePathDirectoryRoot()
        {
            if (string.IsNullOrEmpty(ExcelEncodePathDirectoryRoot))
            {
                ExcelEncodePathDirectoryRoot = $"{Application.dataPath}/../../Excels";
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

    public enum ExcelMode
    {
        Encode,
        Search,
    }

    public enum SearchMode
    {
        Equals,
        Contains,
    }

    public enum SearchDataMode
    {
        Value,
        Table,
    }
}