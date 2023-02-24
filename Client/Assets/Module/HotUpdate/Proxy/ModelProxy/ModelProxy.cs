using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public sealed partial class ModelProxy
    {
        private Dictionary<Type, IModel> m_Cache;

        public ModelProxy()
        {
            m_Cache = new Dictionary<Type, IModel>();
        }

        public T Get<T>() where T : IModel, new()
        {
            Type type = typeof(T);
            if(!m_Cache.ContainsKey(type))
            {
                m_Cache.Add(type, new T());
            }
            return (T)m_Cache[type];
        }

        public ModelProxy Remove<T>() where T: IModel, new()
        {
            Type type = typeof(T);
            m_Cache.Remove(type);
            return this;
        }

        public ModelProxy Clear()
        {
            m_Cache.Clear();
            return this;
        }
    }
}