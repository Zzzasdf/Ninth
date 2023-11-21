using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "WindowReviewConfigSO", menuName = "EditorSOCollect/WindowReviewConfigSO")]
    public class WindowReviewConfig : ScriptableObject
    {
        [SerializeField] private ReviewMode reviewMode;

        [SerializeField] private string txtFiledPath;

        public ReviewMode ReviewMode
        {
            get => reviewMode;
            set => SetProperty(ref reviewMode, value);
        }

        public string TxtFiledPath
        {
            get => txtFiledPath;
            set => SetProperty(ref txtFiledPath, value);
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

    public enum ReviewMode
    {
        PrefabAndScrpit,
        Others
    }
}

