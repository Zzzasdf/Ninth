using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    [DisallowMultipleComponent]
    public abstract class BaseHelper<TEnum, TMap> : MonoBehaviour
        where TEnum: Enum
        where TMap : BaseHelperMap<TEnum, TMap>, new()
    {
        public int EditLock;
        public TEnum Enum;

        public List<TEnum> Enums;
        public int EnumIndex;

        public List<TEnum> DataBarModes;
        public List<KeyList> Keys;
        public List<ValueList> Values;

        private Dictionary<Type, Dictionary<string, UnityEngine.Object>> cache;

        private void Init()
        {
            cache = new Dictionary<Type, Dictionary<string, UnityEngine.Object>>();
            for (int index = 0; index < DataBarModes.Count; index++)
            {
                Type type = Map().GetType(DataBarModes[index]);
                cache.Add(type, new Dictionary<string, UnityEngine.Object>());
                for (int i = 0; i < Keys[index].Values.Count; i++)
                {
                    cache[type].Add(Keys[index].Values[i], Values[index].Values[i]);
                }
            }
        }

        public abstract TMap Map();

        public T Get<T>(string key) where T : class
        {
            try
            {
                if (cache == null)
                {
                    Init();
                }
                return cache[typeof(T)][key] as T;
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