using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Ninth.HotUpdate
{
    public sealed partial class JsonProxy
    {
        private Dictionary<Type, IJsonProxyNode> m_Cache;

        public JsonProxy()
        {
            m_Cache = new Dictionary<Type, IJsonProxyNode>();
        }

        public async UniTask<T> Get<T>() where T : IJson
        {
            Type type = typeof(T);
            if (!m_Cache.ContainsKey(type))
            {
                JsonProxyNode<T> node = new JsonProxyNode<T>();
                m_Cache.Add(type, node);
            }
            return await ((JsonProxyNode<T>)m_Cache[type]).Get();
        }

        public async UniTask Set<T>() where T : IJson
        {
            try
            {
                Type type = typeof(T);
                await ((JsonProxyNode<T>)m_Cache[type]).Set();
            }
            catch (KeyNotFoundException e)
            {
                e.Error();
            }
        }
    }
}