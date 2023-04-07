using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "ExcelConfigSO", menuName = "EditorConfig/ExcelConfigSO")]
    public class EditorExcelConfig : ScriptableObject
    {
        [SerializeField] private ExcelSettingsType excelSettingsType;
        [SerializeField] private string excelSearchPathDirectoryRoot;
        [SerializeField] private ExcelSearchMode excelSearchMode;
        [SerializeField] private ExcelSearchResultMode excelSearchResultMode;

        public ExcelSettingsType ExcelSettingsType
        {
            get => excelSettingsType;
            set => SetProperty(ref excelSettingsType, value);
        }
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
            SetDefaultExcelSearchPathDirectoryRoot();
        }

        private void SetDefaultExcelSearchPathDirectoryRoot()
        {
            if (string.IsNullOrEmpty(ExcelSearchPathDirectoryRoot))
            {
                ExcelSearchPathDirectoryRoot = $"{Application.dataPath}/../../Excels";
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

    public enum ExcelSettingsType
    {
        Encoder,
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