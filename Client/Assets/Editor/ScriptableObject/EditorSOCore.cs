using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class EditorSOCore
    {
        public static Dictionary<string, UnityEngine.Object> m_Cache = new Dictionary<string, UnityEngine.Object>();

        public static EditorBuildConfig GetBuildConfig()
        {
            return Get<EditorBuildConfig>("BuildConfigSO");
        }

        public static EditorExcelConfig GetExcelConfig()
        {
            return Get<EditorExcelConfig>("ExcelConfigSO");
        }

        public static EditorScanConfig GetScanConfig()
        {
            return Get<EditorScanConfig>("ScanConfigSO");
        }

        private static T Get<T>(string soName) where T : UnityEngine.Object
        {
            if (!m_Cache.ContainsKey(soName))
            {
                T t = AssetDatabase.LoadAssetAtPath<T>(string.Format("Assets/Editor/ScriptableObject/SOData/{0}.asset", soName));
                m_Cache.Add(soName, t);
            }
            return m_Cache[soName] as T;
        }
    }
}