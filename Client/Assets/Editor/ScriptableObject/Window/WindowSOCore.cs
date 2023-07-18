using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class WindowSOCore
    {
        public static Dictionary<string, UnityEngine.Object> m_Cache = new Dictionary<string, UnityEngine.Object>();

        public static T Get<T>() where T : UnityEngine.Object
        {
            string soName = string.Format("{0}SO", typeof(T).Name);
            if (!m_Cache.ContainsKey(soName))
            {
                T t = AssetDatabase.LoadAssetAtPath<T>(string.Format("Assets/Editor/ScriptableObject/Window/SOData/{0}.asset", soName));
                m_Cache.Add(soName, t);
            }
            return m_Cache[soName] as T;
        }
    }
}