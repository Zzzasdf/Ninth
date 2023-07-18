using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "WindowOtherConfigSO", menuName = "EditorSOCollect/WindowOtherConfig")]
    public class WindowOtherConfig : ScriptableObject
    {
        [SerializeField] private bool appFoldout;
        [SerializeField] private bool browserFoldout;
        [SerializeField] private bool directoryFoldout;

        public bool AppFoldout
        {
            get => appFoldout;
            set => SetProperty(ref appFoldout, value);
        }

        public bool BrowserFoldout
        {
            get => browserFoldout;
            set => SetProperty(ref browserFoldout, value);
        }

        public bool DirectoryFoldout
        { 
            get => directoryFoldout;
            set => SetProperty(ref directoryFoldout, value);
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

