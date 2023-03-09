using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ninth.HotUpdate
{
    [DisallowMultipleComponent]
    public abstract class IHelper<TEnum, TMap> : MonoBehaviour
        where TEnum: Enum
        where TMap : IHelperMap<TEnum, TMap>, new()
    {
        public int m_Lock;
        public TEnum m_TEnum;

        public List<TEnum> m_TEnums;
        public int m_TEnumIndex;

        public List<TEnum> m_DataBarModes;
        public List<KeyList> m_Keys;
        public List<ValueList> m_Values;

        private Dictionary<Type, Dictionary<string, UnityEngine.Object>> m_Cache;

        private void Init()
        {
            m_Cache = new Dictionary<Type, Dictionary<string, UnityEngine.Object>>();
            for (int index = 0; index < m_DataBarModes.Count; index++)
            {
                Type type = Map().GetType(m_DataBarModes[index]);
                m_Cache.Add(type, new Dictionary<string, UnityEngine.Object>());
                for (int i = 0; i < m_Keys[index].Values.Count; i++)
                {
                    m_Cache[type].Add(m_Keys[index].Values[i], m_Values[index].Values[i]);
                }
            }
        }

        public abstract TMap Map();

        public T Get<T>(string key) where T : class
        {
            try
            {
                if (m_Cache == null)
                {
                    Init();
                }
                return m_Cache[typeof(T)][key] as T;
            }
            catch(KeyNotFoundException e)
            {
                e.Error();
                return default(T);
            }
        }
    }

    [System.Serializable]
    public class KeyList
    {
        public List<string> Values;
        public KeyList()
        {
            Values = new List<string>();
        }
    }

    [System.Serializable]
    public class ValueList
    {
        public List<UnityEngine.Object> Values;
        public ValueList()
        {
            Values = new List<UnityEngine.Object>();
        }
    }
}