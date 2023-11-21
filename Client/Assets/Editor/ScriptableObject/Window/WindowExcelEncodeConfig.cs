using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "WindowExcelEncodeConfigSO", menuName = "EditorSOCollect/WindowExcelEncodeConfigSO")]
    public class WindowExcelEncodeConfig : ScriptableObject
    {
        [SerializeField] private string encodePathDirectoryRoot;

        public string EncodePathDirectoryRoot
        {
            get => encodePathDirectoryRoot;
            set => SetProperty(ref encodePathDirectoryRoot, value);
        }

        private void OnEnable()
        {
            SetDefaultExcelEncodePathDirectoryRoot();
        }

        private void SetDefaultExcelEncodePathDirectoryRoot()
        {
            if (string.IsNullOrEmpty(EncodePathDirectoryRoot))
            {
                EncodePathDirectoryRoot = $"{Application.dataPath}/../../Excels";
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
}