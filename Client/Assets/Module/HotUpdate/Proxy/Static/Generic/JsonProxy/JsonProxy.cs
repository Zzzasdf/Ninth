using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ninth.HotUpdate
{
    public static class JsonProxy<T>
            where T : IJsonProxy
    {
        private static string m_Path;
        private static T m_Cache;

        static JsonProxy()
        {
            GameDriver.ProxyCtrl.StaticGenericProxyRegisterClear(Clear);
        }

        private static void Clear()
        {
            m_Path = string.Empty;
            m_Cache = default(T);
        }

        public static async UniTask<T> Get()
        {
            if(m_Cache != null)
            {
                return m_Cache;
            }
            if(string.IsNullOrEmpty(m_Path))
            {
                m_Path = JsonPathConfig.Get<T>();
            }
            T t = await ToObject(m_Path);
            m_Cache = t;
            return t;

            async UniTask<T> ToObject(string path)
            {
                try
                {
                    string fileContent = await File.ReadAllTextAsync(path, GlobalConfig.Utf8);
                    T t = LitJson.JsonMapper.ToObject<T>(fileContent);
                    return t;
                }
                catch (FileNotFoundException e)
                {
                    e.Error();
                }
                return default(T);
            }
        }

        public static async UniTask Set()
        {
            try
            {
                string jsonData = LitJson.JsonMapper.ToJson(m_Cache);
                await File.WriteAllTextAsync(m_Path, jsonData, GlobalConfig.Utf8);
            }
            catch (DirectoryNotFoundException e)
            {
                e.Error();
            }
        }
    }
}