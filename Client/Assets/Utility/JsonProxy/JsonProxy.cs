using System;
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

        private readonly GenericsSubscribe<IJson, object> genericsSubscribe;
        private readonly EnumTypeSubscribe<object> enumTypeSubscribe;
        private readonly CommonSubscribe<Enum, object> commonSubscribe;
        
        [Inject]
        public JsonProxy(IJsonConfig jsonConfig)
        {
            encoding = new UTF8Encoding(false);
            this.jsonConfig = jsonConfig;

            genericsSubscribe = new GenericsSubscribe<IJson, object>();
            enumTypeSubscribe = new EnumTypeSubscribe<object>();
            commonSubscribe = new CommonSubscribe<Enum, object>();
        }
        
        // Generics
        async UniTask<TKey> IJsonProxy.ToObjectAsync<TKey>(CancellationToken cancellationToken, bool newIfNotExist)
        {
            if (genericsSubscribe.TryGetValue<TKey>(out var value))
            {
                return (TKey)value.Value;
            }
            var path = jsonConfig.GenericsSubscribe.Get<TKey>();
            var obj = await ToObjectAsync<TKey>(path, cancellationToken, newIfNotExist);
            genericsSubscribe.Subscribe<TKey>(obj);
            return obj;
        }

        TKey IJsonProxy.ToObject<TKey>(int markBit, bool newIfNotExist)
        {
            if (genericsSubscribe.TryGetValue<TKey>(out var value))
            {
                return (TKey)value.Value;
            }
            var path = jsonConfig.GenericsSubscribe.Get<TKey>(markBit);
            var obj = ToObject<TKey>(path, newIfNotExist);
            genericsSubscribe.Subscribe<TKey>(obj);
            return obj;
        }

        async UniTask IJsonProxy.ToJsonAsync<TKey>(CancellationToken cancellationToken, bool throwEmptyError)
        {
            if (!throwEmptyError && !genericsSubscribe.ContainsKey<TKey>())
            {
                return;
            }
            var obj = genericsSubscribe.Get<TKey>();
            var path = jsonConfig.GenericsSubscribe.Get<TKey>();
            await ToJsonAsync((TKey)obj, path, cancellationToken);
        }

        void IJsonProxy.ToJson<TKey>(int markBit, bool throwEmptyError) where TKey : class
        {
            if (!throwEmptyError && !genericsSubscribe.ContainsKey<TKey>())
            {
                return;
            }
            var obj = genericsSubscribe.Get<TKey>();
            var path = jsonConfig.GenericsSubscribe.Get<TKey>(markBit);
            ToJson((TKey)obj, path);
        }

        string IJsonProxy.GetPath<TKey>(int markBit) where TKey : class
        {
            return jsonConfig.GenericsSubscribe.Get<TKey>(markBit);
        }

        // EnumType
        async UniTask<TResult> IJsonProxy.ToObjectAsync<TResult, TKeyEnum>(CancellationToken cancellationToken, bool newIfNotExist)
        {
            if (enumTypeSubscribe.TryGetValue<TKeyEnum>(out var value))
            {
                return (TResult)value.Value;
            }
            var path = jsonConfig.EnumTypeSubscribe.Get<TKeyEnum>();
            var obj = await ToObjectAsync<TResult>(path, cancellationToken, newIfNotExist);
            enumTypeSubscribe.Subscribe<TKeyEnum>(obj);
            return obj;
        }

        TResult IJsonProxy.ToObject<TResult, TKeyEnum>(bool newIfNotExist)
        {
            if (enumTypeSubscribe.TryGetValue<TKeyEnum>(out var value))
            {
                return (TResult)value.Value;
            }
            var path = jsonConfig.EnumTypeSubscribe.Get<TKeyEnum>();
            var obj = ToObject<TResult>(path, newIfNotExist);
            enumTypeSubscribe.Subscribe<TKeyEnum>(obj);
            return obj;
        }

        async UniTask IJsonProxy.ToJsonAsync<T, TKeyEnum>(CancellationToken cancellationToken, bool throwEmptyError)
        {
            if (!throwEmptyError && !enumTypeSubscribe.ContainsKey<TKeyEnum>())
            {
                return;
            }
            var obj = enumTypeSubscribe.Get<TKeyEnum>();
            var path = jsonConfig.EnumTypeSubscribe.Get<TKeyEnum>();
            await ToJsonAsync((T)obj, path, cancellationToken);
        }

        void IJsonProxy.ToJson<T, TKeyEnum>(bool throwEmptyError) where T : class
        {
            if (!throwEmptyError && !enumTypeSubscribe.ContainsKey<TKeyEnum>())
            {
                return;
            }
            var obj = enumTypeSubscribe.Get<TKeyEnum>();
            var path = jsonConfig.EnumTypeSubscribe.Get<TKeyEnum>();
            ToJson((T)obj, path);
        }

        string IJsonProxy.GetPathByEnumType<TKeyEnum>()
        {
            return jsonConfig.EnumTypeSubscribe.Get<TKeyEnum>();
        }

        // Enum
        async UniTask<TResult> IJsonProxy.ToObjectAsync<TResult>(Enum key, CancellationToken cancellationToken, bool newIfNotExist)
        {
            if (commonSubscribe.TryGetValue(key, out var value))
            {
                return (TResult)value.Value;
            }

            var path = jsonConfig.CommonSubscribe.Get(key);
            var obj = await ToObjectAsync<TResult>(path, cancellationToken, newIfNotExist);
            commonSubscribe.Subscribe(key, obj);
            return obj;
        }

        TResult IJsonProxy.ToObject<TResult>(Enum key, bool newIfNotExist)
        {
            if (commonSubscribe.TryGetValue(key, out var value))
            {
                return (TResult)value.Value;
            }
            var path = jsonConfig.CommonSubscribe.Get(key);
            var obj = ToObject<TResult>(path, newIfNotExist);
            commonSubscribe.Subscribe(key, obj);
            return obj;
        }

        async UniTask IJsonProxy.ToJsonAsync<T>(Enum key, CancellationToken cancellationToken, bool throwEmptyError)
        {
            if (!throwEmptyError && !commonSubscribe.ContainsKey(key))
            {
                return;
            }
            var obj = commonSubscribe.Get(key); 
            var path = jsonConfig.CommonSubscribe.Get(key);
            await ToJsonAsync((T)obj, path, cancellationToken);
        }

        void IJsonProxy.ToJson<T>(Enum key, bool throwEmptyError)
        {
            if (!throwEmptyError && !commonSubscribe.ContainsKey(key))
            {
                return;
            }
            var obj = commonSubscribe.Get(key);
            var path = jsonConfig.CommonSubscribe.Get(key);
            ToJson((T)obj, path);
        }

        // Base
        private async UniTask<T> ToObjectAsync<T>(string? path, CancellationToken cancellationToken, bool newIfNotExist) where T: class, new()
        {
            if (!File.Exists(path))
            {
                if (newIfNotExist)
                {
                    $"路径不存在文件: {path}".Log();
                    return default!;
                }
                return new T();
            }
            var data = await File.ReadAllTextAsync(path, encoding, cancellationToken);
            return LitJson.JsonMapper.ToObject<T>(data);
        }
        
        private T ToObject<T>(string path, bool newIfNotExist) where T: class, new()
        {
            if (!File.Exists(path))
            {
                if (!newIfNotExist)
                {
                    $"路径不存在文件: {path}".Log();
                    return default!;
                }
                return new T();
            }
            var data = File.ReadAllText(path, encoding);
            return LitJson.JsonMapper.ToObject<T>(data);
        }
        
        private async UniTask ToJsonAsync<T>(T obj, string path, CancellationToken cancellationToken)
        {
            if (!File.Exists(path))
            {
                await File.Create(path).DisposeAsync();
            }
            var jsonData = LitJson.JsonMapper.ToJson(obj);
            await File.WriteAllTextAsync(path, ConvertJsonString(jsonData), encoding, cancellationToken);
        }
        
        private void ToJson<T>(T obj, string path)
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