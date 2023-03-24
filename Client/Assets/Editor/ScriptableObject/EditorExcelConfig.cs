using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "ExcelConfigSO", menuName = "EditorConfig/ExcelConfigSO")]
    public class EditorExcelConfig: ScriptableObject
    {
        public ExcelSettingsType ExcelSettingsType;
        // Encoder

        // Search
        public string ExcelSearchPathDirectoryRoot;
        public ExcelSearchMode ExcelSearchMode;
        public ExcelSearchResultMode ExcelSearchResultMode;

        private void OnEnable()
        {
            if (ExcelSearchPathDirectoryRoot == null)
            {
                ExcelSearchPathDirectoryRoot = $"{Application.dataPath}/../../Excels";
            }
        }

        private void OnValidate()
        {
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
        Value
    }
}