using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ninth.HotUpdate
{
    public static partial class JsonProxy
    {
        public static async UniTask<T> Get<T>() where T : IModel, new()
        {
            Type type = typeof(T);
            string path = JsonPathConfig.Get<T>();
            T json = await ToObject(path);
            return json;

            async UniTask<T> ToObject(string path)
            {
                try
                {
                    string fileContent = await File.ReadAllTextAsync(path, GlobalConfig.Utf8);
                    T t = LitJson.JsonMapper.ToObject<T>(fileContent);
                    return t;
                }
                catch(DirectoryNotFoundException e)
                {
                    e.Error();
                }
                catch (FileNotFoundException e)
                {
                    e.Error();
                }
                return new T();
            }
        }

        public static async UniTask Store<T>(T obj) where T : IModel
        {
            try
            {
                Type type = typeof(T);
                string path = JsonPathConfig.Get<T>();
                string jsonData = LitJson.JsonMapper.ToJson(obj);
                await File.WriteAllTextAsync(path, jsonData, GlobalConfig.Utf8);
            }
            catch (DirectoryNotFoundException e)
            {
                e.Error();
            }
            catch (KeyNotFoundException e)
            {
                e.Error();
            }
        }
    }
}