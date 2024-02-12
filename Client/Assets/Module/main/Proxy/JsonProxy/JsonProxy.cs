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
        private readonly UTF8Encoding encoding = new UTF8Encoding(false);
        
        private readonly ReadOnlyDictionary<JsonFile, string> mapContainer;

        [Inject]
        public JsonProxy(IJsonConfig jsonConfig)
        {
            this.mapContainer = jsonConfig.MapContainer();
        }
        
        public async UniTask<T?> ToObject<T>(JsonFile jsonFile, CancellationToken cancellationToken = default)
        {
            if(!mapContainer.TryGetValue(jsonFile, out var path) || string.IsNullOrEmpty(path))
            {
                $"{jsonFile} 对应的路径为空, 请检查注册的配置文件：{nameof(IJsonConfig)}".FrameError();
                return default;
            }
            if (!File.Exists(path))
            {
                path.FrameError("无法加载到 Json 文件 在路径: {0}");
                return default;
            }
            var data = await File.ReadAllTextAsync(path, encoding, cancellationToken);
            return LitJson.JsonMapper.ToObject<T>(data);
        }

        async UniTaskVoid IJsonProxy.ToJson<T>(T obj, JsonFile jsonFile, CancellationToken cancellationToken = default)
        {
            if(!mapContainer.TryGetValue(jsonFile, out var path) || string.IsNullOrEmpty(path))
            {
                $"{jsonFile} 对应的路径为空, 请检查注册的配置文件：{nameof(IJsonConfig)}".FrameError();
                return;
            }
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            var jsonData = LitJson.JsonMapper.ToJson(obj);
            await File.WriteAllTextAsync(path, ConvertJsonString(jsonData), encoding, cancellationToken);
            // File.WriteAllText(path, jsonData, encoding);
        }

        private static string ConvertJsonString(string str)
        {
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj == null)
            {
                return str;
            }
            StringWriter textWriter = new StringWriter();
            JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
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