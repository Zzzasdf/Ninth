using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using VContainer;

namespace Ninth.Utility
{
    public class JsonProxy : IJsonProxy
    {
        private readonly UTF8Encoding encoding;

        private readonly IJsonConfig jsonConfig;

        [Inject]
        public JsonProxy(IJsonConfig jsonConfig)
        {
            encoding = new UTF8Encoding(false);
            this.jsonConfig = jsonConfig;
        }
        
        // Generics
        async UniTask<T?> IJsonProxy.ToObjectAsync<T>(CancellationToken cancellationToken) where T: class
        {
            var path = jsonConfig.Get<T>();
            if(path == null)
            {
                $"路径为空 {nameof(T)}".FrameError();
                return null;
            }
            return await ToObjectAsync<T>(path, cancellationToken);
        }

        T? IJsonProxy.ToObject<T>() where T : class
        {
            var path = jsonConfig.Get<T>();
            if(path == null)
            {
                $"路径为空 {nameof(T)}".FrameError();
                return null;
            }
            return ToObject<T>(path);
        }

        async UniTask IJsonProxy.ToJsonAsync<T>(T obj, CancellationToken cancellationToken) where T: class
        {
            var path = jsonConfig.Get<T>();
            if(path == null)
            {
                $"路径为空 {nameof(T)}".FrameError();
                return;
            }
            await ToJsonAsync(obj, path, cancellationToken);
        }

        void IJsonProxy.ToJson<T>(T obj) where T : class
        {
            var path = jsonConfig.Get<T>();
            if(path == null)
            {
                $"路径为空 {nameof(T)}".FrameError();
                return;
            }
            ToJson(obj, path);
        }
        
        // EnumType
        async UniTask<T?> IJsonProxy.ToObjectAsync<T, TEnum>(CancellationToken cancellationToken) where T : class
        {
            var path = jsonConfig.GetEnum<TEnum>();
            if(path == null)
            {
                $"路径为空 {nameof(T)}".FrameError();
                return null;
            }
            return await ToObjectAsync<T>(path, cancellationToken);
        }

        T? IJsonProxy.ToObject<T, TEnum>() where T : class
        {
            var path = jsonConfig.GetEnum<TEnum>();
            if(path == null)
            {
                $"路径为空 {nameof(T)}".FrameError();
                return null;
            }
            return ToObject<T>(path);
        }

        async UniTask IJsonProxy.ToJsonAsync<T, TEnum>(T obj, CancellationToken cancellationToken) where T : class
        {
            var path = jsonConfig.GetEnum<TEnum>();
            if(path == null)
            {
                $"路径为空 {nameof(T)}".FrameError();
                return;
            }
            await ToJsonAsync(obj, path, cancellationToken);
        }

        void IJsonProxy.ToJson<T, TEnum>(T obj) where T : class
        {
            var path = jsonConfig.GetEnum<TEnum>();
            if(path == null)
            {
                $"路径为空 {nameof(T)}".FrameError();
                return;
            }
            ToJson(obj, path);
        }

        // Enum
        async UniTask<T?> IJsonProxy.ToObjectAsync<T>(Enum e, CancellationToken cancellationToken) where T: class
        {
            var path = jsonConfig.Get(e);
            if(path == null)
            {
                $"路径为空 {e.GetType().Name}: {e}".FrameError();
                return null;
            }
            return await ToObjectAsync<T>(path, cancellationToken);
        }

        T? IJsonProxy.ToObject<T>(Enum e) where T : class
        {
            var path = jsonConfig.Get(e);
            if(path == null)
            {
                $"路径为空 {e.GetType().Name}: {e}".FrameError();
                return null;
            }
            return ToObject<T>(path);
        }

        async UniTask IJsonProxy.ToJsonAsync<T>(T obj, Enum e, CancellationToken cancellationToken) where T: class
        {
            var path = jsonConfig.Get(e);
            if(path == null)
            {
                $"路径为空 {e.GetType().Name}: {e}".FrameError();
                return;
            }
            await ToJsonAsync(obj, path, cancellationToken);
        }

        void IJsonProxy.ToJson<T>(T obj, Enum e) where T : class
        {
            var path = jsonConfig.Get(e);
            if(path == null)
            {
                $"路径为空 {e.GetType().Name}: {e}".FrameError();
                return;
            }
            ToJson(obj, path);
        }
        
        private async UniTask<T?> ToObjectAsync<T>(string path, CancellationToken cancellationToken) where T: class
        {
            if (!File.Exists(path))
            {
                $"路径不存在文件: {path}".Log();
                return null;
            }
            var data = await File.ReadAllTextAsync(path, encoding, cancellationToken);
            return LitJson.JsonMapper.ToObject<T>(data);
        }
        
        private T? ToObject<T>(string path) where T: class
        {
            if (!File.Exists(path))
            {
                $"路径不存在文件: {path}".Log();
                return null;
            }
            var data = File.ReadAllText(path, encoding);
            return LitJson.JsonMapper.ToObject<T>(data);
        }
        
        private async UniTask ToJsonAsync<T>(T obj, string path, CancellationToken cancellationToken) where T: class
        {
            if (!File.Exists(path))
            {
                await File.Create(path).DisposeAsync();
            }
            var jsonData = LitJson.JsonMapper.ToJson(obj);
            await File.WriteAllTextAsync(path, ConvertJsonString(jsonData), encoding, cancellationToken);
        }
        
        private void ToJson<T>(T obj, string path) where T : class
        {
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            var jsonData = LitJson.JsonMapper.ToJson(obj);
            File.WriteAllText(path, ConvertJsonString(jsonData), encoding);
        }


        private static string ConvertJsonString(string str)
        {
            var serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            var jtr = new JsonTextReader(tr);
            var obj = serializer.Deserialize(jtr);
            if (obj == null)
            {
                return str;
            }
            var textWriter = new StringWriter();
            var jsonWriter = new JsonTextWriter(textWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' '
            };
            serializer.Serialize(jsonWriter, obj);
            return textWriter.ToString();
        }
    }
}