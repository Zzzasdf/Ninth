using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    [CreateAssetMenu(fileName = "WindowCollectConfigSO", menuName = "EditorSOCollect/WindowCollectConfigSO")]
    public class WindowCollectConfig : ScriptableObject
    {
        [SerializeField] private NinthWindowTab ninthWindowTab;
        
        public NinthWindowTab NinthWindowTab
        {
            get => ninthWindowTab;
            set => SetProperty(ref ninthWindowTab, value);
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
