using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public sealed class SOCore 
    {
        public static Dictionary<string, UnityEngine.Object> m_Cache = new Dictionary<string, UnityEngine.Object>();

        private static T Get<T>(string soName) where T : UnityEngine.Object
        {
            if (!m_Cache.ContainsKey(soName))
            {
                T t = Resources.Load<T>(string.Format("SOData/{0}", soName));
                m_Cache.Add(soName, t);
            }
            return m_Cache[soName] as T;
        }

        public static GlobalConfig GetGlobalConfig()
        {
            return Get<GlobalConfig>("GlobalConfigSO");
        }

        public static NameConfig GetNameConfig()
        {
            return Get<NameConfig>("NameConfigSO");
        }
    }
}
