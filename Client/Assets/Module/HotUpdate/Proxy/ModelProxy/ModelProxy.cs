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

        public async UniTask<T> Get<T>() where T : IModel, new()
        {
            Type type = typeof(T);
            if(!m_Cache.ContainsKey(type))
            {
                if(JsonPathConfig.IsExist<T>())
                {
                    m_Cache.Add(type, await JsonProxy.Get<T>());
                }
                else
                {
                    m_Cache.Add(type, new T());
                }
            }
            return (T)m_Cache[type];
        }

        public async UniTask<T> GetAlone<T>() where T : IModel, new()
        {
            Type type = typeof(T);
            if (JsonPathConfig.IsExist<T>())
            {
                return await JsonProxy.Get<T>();
            }
            else
            {
                return new T();
            }
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