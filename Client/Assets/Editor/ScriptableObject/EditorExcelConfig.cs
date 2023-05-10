using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "ExcelConfigSO", menuName = "EditorConfig/ExcelConfigSO")]
    public class EditorExcelConfig : ScriptableObject
    {
        [SerializeField] private ExcelMode excelMode;

        // Encode
        [SerializeField] private string excelEncodePathDirectoryRoot;

        // Search
        [SerializeField] private string excelSearchPathDirectoryRoot;
        [SerializeField] private ExcelSearchMode excelSearchMode;
        [SerializeField] private ExcelSearchResultMode excelSearchResultMode;

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

        // Search
        public string ExcelSearchPathDirectoryRoot
        {
            get => excelSearchPathDirectoryRoot;
            set => SetProperty(ref excelSearchPathDirectoryRoot, value);
        }
        public ExcelSearchMode ExcelSearchMode
        {
            get => excelSearchMode;
            set => SetProperty(ref excelSearchMode, value);
        }
        public ExcelSearchResultMode ExcelSearchResultMode
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

    public enum ExcelSearchMode
    {
        Exact,
        Exist,
    }

    public enum ExcelSearchResultMode
    {
        Table,
        Value,
    }
}