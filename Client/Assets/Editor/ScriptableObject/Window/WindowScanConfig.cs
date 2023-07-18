using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "WindowScanConfigSO", menuName = "EditorSOCollect/WindowScanConfigSO")]
    public class WindowScanConfig : ScriptableObject
    {
        [SerializeField] private ScanMode scanMode;

        // Prefab
        [SerializeField] private ScanPrefabMode scanPrefabMode;
        // Prefab => Image
        [SerializeField] private string prefabImageScanPathDirectoryRoot;

        public ScanMode ScanMode
        {
            get => scanMode;
            set => SetProperty(ref scanMode, value);
        }

        // Prefab
        public ScanPrefabMode ScanPrefabMode
        {
            get => scanPrefabMode;
            set => SetProperty(ref scanPrefabMode, value);
        }
        // Prefab => Image
        public string PrefabImageScanPathDirectoryRoot
        {
            get => prefabImageScanPathDirectoryRoot;
            set => SetProperty(ref prefabImageScanPathDirectoryRoot, value);
        }

        private void OnEnable()
        {
            SetDefaultPrefabScanPathDirectoryRoot();
        }

        private void SetDefaultPrefabScanPathDirectoryRoot()
        {
            if (string.IsNullOrEmpty(prefabImageScanPathDirectoryRoot))
            {
                prefabImageScanPathDirectoryRoot = Application.dataPath;
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

    public enum ScanMode
    {
        Prefab,
        Others
    }

    public enum ScanPrefabMode
    {
        Image,
        Text
    }
}

