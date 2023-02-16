//using Newtonsoft.Json;
using System.IO;

namespace Ninth.HotUpdate
{
    public sealed partial class Utility
    {
        public static T ToObject<T>(string path)
        {
            if (!File.Exists(path))
            {
                return default(T);
            }
            return LitJson.JsonMapper.ToObject<T>(File.ReadAllText(path, GlobalConfig.Utf8));
        }

        public static void ToJson<T>(T obj, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                UnityEngine.Debug.LogError("保存Json的路径为空！！保存失败！！");
            }

            // 开始写入Json文件
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.Create(path).Dispose();

            string jsonData = LitJson.JsonMapper.ToJson(obj);

            //File.WriteAllText(path, ConvertJsonString(jsonData), GlobalConfig.Utf8);
            File.WriteAllText(path, jsonData, GlobalConfig.Utf8);
        }

        ///// <summary>
        ///// 格式化json
        ///// </summary>
        ///// <param name="str">输入json字符串</param>
        ///// <returns>返回格式化后的字符串</returns>
        //private static string ConvertJsonString(string str)
        //{
        //    JsonSerializer serializer = new JsonSerializer();

        //    TextReader tr = new StringReader(str);

        //    JsonTextReader jtr = new JsonTextReader(tr);

        //    object obj = serializer.Deserialize(jtr);
        //    if (obj != null)
        //    {
        //        StringWriter textWriter = new StringWriter();

        //        JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
        //        {
        //            Formatting = Formatting.Indented,

        //            Indentation = 4,

        //            IndentChar = ' '
        //        };

        //        serializer.Serialize(jsonWriter, obj);

        //        return textWriter.ToString();
        //    }
        //    else
        //    {
        //        return str;
        //    }
        //}
    }
}
