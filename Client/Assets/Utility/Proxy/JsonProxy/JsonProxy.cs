using System;
using System.Collections.Generic;
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

        private readonly Dictionary<string, object> objectCache;
        
        [Inject]
        public JsonProxy(IJsonConfig jsonConfig)
        {
            encoding = new UTF8Encoding(false);
            this.jsonConfig = jsonConfig;
            objectCache = new Dictionary<string, object>();
        }
        
        // Generics
        async UniTask<TKey> IJsonProxy.ToObjectAsync<TKey>(CancellationToken cancellationToken, int markBit, Func<TKey>? notExistHandle)
        {
            var path = jsonConfig.TypeSubscriber.GetValue<TKey>(markBit);
            var obj = await ToObjectAsync(path, cancellationToken, notExistHandle);
            return obj;
        }

        TKey IJsonProxy.ToObject<TKey>(int markBit, Func<TKey>? notExistHandle)
        {
            var path = jsonConfig.TypeSubscriber.GetValue<TKey>(markBit);
            var obj = ToObject(path, notExistHandle);
            return obj;
        }

        async UniTask IJsonProxy.ToJsonAsync<TKey>(CancellationToken cancellationToken, int markBit)
        {
            var path = jsonConfig.TypeSubscriber.GetValue<TKey>(markBit);
            if (!objectCache.TryGetValue(path, out var obj)) return;
            await ToJsonAsync((TKey)obj, path, cancellationToken);
        }

        void IJsonProxy.ToJson<TKey>(int markBit)
        {
            var path = jsonConfig.TypeSubscriber.GetValue<TKey>(markBit);
            if (!objectCache.TryGetValue(path, out var obj)) return;
            ToJson((TKey)obj, path);
        }

        bool IJsonProxy.CacheExists<TKey>(int markBit)
        {
            var path = jsonConfig.TypeSubscriber.GetValue<TKey>(markBit);
            return CacheExists<TKey>(path);
        }
        
        // EnumType
        async UniTask<TResult> IJsonProxy.ToObjectAsync<TResult, TKeyEnum>(CancellationToken cancellationToken, int markBit, Func<TResult>? notExistHandle)
        {
            var path = jsonConfig.TypeSubscriber.GetValue<TKeyEnum>(markBit);
            var obj = await ToObjectAsync(path, cancellationToken, notExistHandle);
            return obj;
        }

        TResult IJsonProxy.ToObject<TResult, TKeyEnum>(int markBit, Func<TResult>? notExistHandle)
        {
            var path = jsonConfig.TypeSubscriber.GetValue<TKeyEnum>(markBit);
            var obj = ToObject(path, notExistHandle);
            return obj;
        }

        async UniTask IJsonProxy.ToJsonAsync<T, TKeyEnum>(CancellationToken cancellationToken, int markBit)
        {
            var path = jsonConfig.TypeSubscriber.GetValue<TKeyEnum>(markBit);
            if (!objectCache.TryGetValue(path, out var obj)) return;
            await ToJsonAsync((T)obj, path, cancellationToken);
        }

        void IJsonProxy.ToJson<T, TKeyEnum>(int markBit)
        {
            var path = jsonConfig.TypeSubscriber.GetValue<TKeyEnum>(markBit);
            if (!objectCache.TryGetValue(path, out var obj)) return;
            ToJson((T)obj, path);
        }
        
        bool IJsonProxy.CacheExists<T, TKeyEnum>(int markBit)
        {
            var path = jsonConfig.TypeSubscriber.GetValue<TKeyEnum>(markBit);
            return CacheExists<T>(path);
        }

        // Enum
        async UniTask<TResult> IJsonProxy.ToObjectAsync<TResult>(Enum key, CancellationToken cancellationToken, int markBit, Func<TResult>? notExistHandle)
        {
            var path = jsonConfig.Subscriber.GetValue(key);
            var obj = await ToObjectAsync(path, cancellationToken, notExistHandle);
            return obj;
        }

        TResult IJsonProxy.ToObject<TResult>(Enum key, int markBit, Func<TResult>? notExistHandle)
        {
            var path = jsonConfig.Subscriber.GetValue(key, markBit);
            var obj = ToObject(path, notExistHandle);
            return obj;
        }

        async UniTask IJsonProxy.ToJsonAsync<T>(Enum key, CancellationToken cancellationToken, int markBit)
        {
            var path = jsonConfig.Subscriber.GetValue(key, markBit);
            if (!objectCache.TryGetValue(path, out var obj)) return;
            await ToJsonAsync((T)obj, path, cancellationToken);
        }

        void IJsonProxy.ToJson<T>(Enum key, int markBit)
        {
            var path = jsonConfig.Subscriber.GetValue(key, markBit);
            if (!objectCache.TryGetValue(path, out var obj)) return;
            ToJson((T)obj, path);
        }

        bool IJsonProxy.CacheExists<T>(Enum key, int markBit)
        {
            var path = jsonConfig.Subscriber.GetValue(key, markBit);
            return CacheExists<T>(path);
        }

        // Base
        public async UniTask<T> ToObjectAsync<T>(string path, CancellationToken cancellationToken, Func<T>? notExistHandle) where T: class, IJson
        {
            if (objectCache.TryGetValue(path, out var result))
            {
                return (T)result;
            }
            if (!File.Exists(path))
            {
                if (notExistHandle == null)
                {
                    $"路径不存在文件: {path}".Error();
                    result = default!;
                }
                else
                {
                    result = notExistHandle.Invoke();
                }
            }
            else
            {
                var data = await File.ReadAllTextAsync(path, encoding, cancellationToken);
                result = LitJson.JsonMapper.ToObject<T>(data);
            }
            objectCache.Add(path, result);
            return (T)result;
        }
        
        public T ToObject<T>(string path, Func<T>? notExistHandle) where T: class, IJson
        {
            if (objectCache.TryGetValue(path, out var result))
            {
                return (T)result;
            }
            if (!File.Exists(path))
            {
                if (notExistHandle == null)
                {
                    $"路径不存在文件: {path}".Log();
                    result = default!;
                }
                else
                {
                    result = notExistHandle.Invoke();
                }
            }
            else
            {
                var data = File.ReadAllText(path, encoding);
                result = LitJson.JsonMapper.ToObject<T>(data);
            }
            objectCache.Add(path, result);
            return (T)result;
        }
        
        public async UniTask ToJsonAsync<T>(T obj, string path, CancellationToken cancellationToken) where T: class, IJson
        {
            if (!File.Exists(path))
            {
                await File.Create(path).DisposeAsync();
            }
            if(objectCache.TryGetValue(path, out var item))
            {
                CopyHelper.ShallowCopy(item, obj);
            }
            var jsonData = LitJson.JsonMapper.ToJson(obj);
            await File.WriteAllTextAsync(path, ConvertJsonString(jsonData), encoding, cancellationToken);
        }
        
        public void ToJson<T>(T obj, string path) where T: class, IJson
        {
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            if(objectCache.TryGetValue(path, out var item))
            {
                CopyHelper.ShallowCopy(item, obj);
            }
            var jsonData = LitJson.JsonMapper.ToJson(obj);
            File.WriteAllText(path, ConvertJsonString(jsonData), encoding);
        }
        
        public bool CacheExists<T>(string path) where T: class, IJson
        {
            return objectCache.ContainsKey(path);
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