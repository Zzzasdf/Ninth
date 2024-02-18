using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using VContainer;

namespace Ninth
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
        
        async UniTask<T?> IJsonProxy.ToObjectAsync<T>(Enum e, CancellationToken cancellationToken) where T: class
        {
            var path = jsonConfig.Get(e);
            if(path == null)
            {
                $"路径为空 {e.GetType().Name}: {e}".FrameError();
                return null;
            }
            if (!File.Exists(path))
            {
                $"路径不存在文件: {path}".Log();
                return null;
            }
            var data = await File.ReadAllTextAsync(path, encoding, cancellationToken);
            return LitJson.JsonMapper.ToObject<T>(data);
        }

        async UniTaskVoid IJsonProxy.ToJsonAsync<T>(T obj, Enum e, CancellationToken cancellationToken) where T: class
        {
            var path = jsonConfig.Get(e);
            if(path == null)
            {
                $"路径为空 {e.GetType().Name}: {e}".FrameError();
                return;
            }
            if (!File.Exists(path))
            {
                await File.Create(path).DisposeAsync();
            }
            var jsonData = LitJson.JsonMapper.ToJson(obj);
            await File.WriteAllTextAsync(path, ConvertJsonString(jsonData), encoding, cancellationToken);
            // await File.WriteAllTextAsync(path, jsonData, encoding);
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