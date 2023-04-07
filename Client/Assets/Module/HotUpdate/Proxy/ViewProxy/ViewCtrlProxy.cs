using System.Collections.Generic;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public class ViewCtrlProxy
    {
        private readonly Dictionary<System.Type, IController> m_Cache = new Dictionary<System.Type, IController>();

        public T Get<T>() where T : class, IController, new()
        {
            if (!m_Cache.TryGetValue(typeof(T), out IController controller))
            {
                controller = new T();
                m_Cache[typeof(T)] = controller;
            }
            return controller as T;
        }
    }
}