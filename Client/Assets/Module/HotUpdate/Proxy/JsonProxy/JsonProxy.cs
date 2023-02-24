using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ninth.HotUpdate
{
    public sealed partial class JsonProxy
    {
        private Dictionary<Type, IJson> m_Cache;

        public JsonProxy()
        {
            m_Cache = new Dictionary<Type, IJson>();
        }

        public async UniTask<T> Get<T>() where T : IJson
        {
            Type type = typeof(T);
            if (!m_Cache.ContainsKey(type))
            {
                string path = JsonPathConfig.Get<T>();
                IJson json = await ToObject(path);
                m_Cache.Add(type, json);
            }
            return (T)m_Cache[type];

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

        public async UniTask Set<T>() where T : IJson
        {
            try
            {
                Type type = typeof(T);
                string path = JsonPathConfig.Get<T>();
                string jsonData = LitJson.JsonMapper.ToJson(m_Cache[type]);
                await File.WriteAllTextAsync(path, jsonData, GlobalConfig.Utf8);
            }
            catch (KeyNotFoundException e)
            {
                e.Error();
            }
            catch (DirectoryNotFoundException e)
            {
                e.Error();
            }
        }
    }
}