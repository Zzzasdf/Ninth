using Cysharp.Threading.Tasks;
using System.IO;

namespace Ninth.HotUpdate
{
    public partial class JsonProxy
    {
        private interface IJsonProxyNode { }

        private sealed partial class JsonProxyNode<T> : IJsonProxyNode
                where T : IJson
        {
            private string m_Path;
            private T m_Cache;

            public async UniTask<T> Get()
            {
                if (m_Cache != null)
                {
                    return m_Cache;
                }
                if (string.IsNullOrEmpty(m_Path))
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

            public async UniTask Set()
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
}