using System.Collections;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ninth.Editor.Window
{
    [CreateAssetMenu(fileName = "SO", menuName = "EditorWindowSO/SO")]
    public class SO : ScriptableObject, ISO
    {
        [SerializeField] private AutoDirtyProperty<Tab> tab = new AutoDirtyProperty<Tab>();

        void OnEnable()
        {
            tab = new AutoDirtyProperty<Tab>(this);
            UnityEngine.Debug.Log(tab.target);
        }
 
        void ISO.Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();
        }
    }
}