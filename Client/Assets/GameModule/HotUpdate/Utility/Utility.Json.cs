// //using Newtonsoft.Json;
// using Cysharp.Threading.Tasks;
// using System.Collections.Generic;
// using System.IO;

// namespace Ninth.HotUpdate
// {
//     public sealed partial class Utility
//     {
//         private static Dictionary<string, object> cacheDic = new Dictionary<string, object>();

//         public static async UniTask<T> ToObjectWithLock<T>(string path)
//         {
//             if (!cacheDic.ContainsKey(path))
//             {
//                 cacheDic.Add(path, new object());
//             }
//             await UniTask.SwitchToThreadPool();
//             T t = ToObject(path);
//             await UniTask.Yield(PlayerLoopTiming.Update);
//             return t;

//             static T ToObject(string path)
//             {
//                 try
//                 {
//                     lock (cacheDic[path])
//                     {
//                         string fileContent = File.ReadAllText(path, SOCore.GetGlobalConfig().Encoding);
//                         T t = LitJson.JsonMapper.ToObject<T>(fileContent);
//                         return t;
//                     }
//                 }
//                 catch (FileNotFoundException e)
//                 {
//                     e.Error();
//                 }
//                 return default(T);
//             }
//         }

//         public static async UniTask ToJsonWithLock<T>(T obj, string path)
//         {
//             if (!cacheDic.ContainsKey(path))
//             {
//                 cacheDic.Add(path, new object());
//             }
//             await UniTask.SwitchToThreadPool();
//             ToJson(obj, path);
//             await UniTask.Yield(PlayerLoopTiming.Update);

//             static void ToJson(T obj, string path)
//             {
//                 try
//                 {
//                     lock (cacheDic[path])
//                     {
//                         string jsonData = LitJson.JsonMapper.ToJson(obj);
//                         File.WriteAllText(path, jsonData, SOCore.GetGlobalConfig().Encoding);
//                     }
//                 }
//                 catch (DirectoryNotFoundException e)
//                 {
//                     e.Error();
//                 }
//             }
//         }

//         ///// <summary>
//         ///// 格式化json
//         ///// </summary>
//         ///// <param name="str">输入json字符串</param>
//         ///// <returns>返回格式化后的字符串</returns>
//         //private static string ConvertJsonString(string str)
//         //{
//         //    JsonSerializer serializer = new JsonSerializer();

//         //    TextReader tr = new StringReader(str);

//         //    JsonTextReader jtr = new JsonTextReader(tr);

//         //    object m_Obj = serializer.Deserialize(jtr);
//         //    if (m_Obj != null)
//         //    {
//         //        StringWriter textWriter = new StringWriter();

//         //        JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
//         //        {
//         //            Formatting = Formatting.Indented,

//         //            Indentation = 4,

//         //            IndentChar = ' '
//         //        };

//         //        serializer.Serialize(jsonWriter, m_Obj);

//         //        return textWriter.ToString();
//         //    }
//         //    else
//         //    {
//         //        return str;
//         //    }
//         //}
//     }
// }