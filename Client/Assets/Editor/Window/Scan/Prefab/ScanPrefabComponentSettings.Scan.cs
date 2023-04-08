using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class ScanPrefabComponentSettings<TEnum, T>
    {
        private TEnum mode;

        // key(string) => PrefabPath
        private Dictionary<TEnum, Dictionary<string, ScanInfo<T>>> cache = new Dictionary<TEnum, Dictionary<string, ScanInfo<T>>>();

        // 检测方法分类
        private List<(TEnum, Func<T, bool>, string, Action<T>)> classifies;
        private List<(TEnum, Func<T, bool>, string, Action<T>)> Classifies
        {
            get
            {
                if (classifies == null)
                {
                    ClassifiesAssembler();
                }
                return classifies;
            }
        }

        private void SetScan()
        {
            if (GUILayout.Button("Scan"))
            {
                cache.Clear();
                string[] guids = AssetDatabase.FindAssets("t:Prefab", GetAssetsAllChildDirectory(PathDirectoryRoot).ToArray());
                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (obj == null)
                    {
                        break;
                    }
                    T[] Ts = obj.GetComponentsInChildren<T>(true);
                    for (int index = 0; index < Ts.Length; index++)
                    {
                        T T = Ts[index];
                        if (T == null)
                        {
                            break;
                        }
                        for (int i = 0; i < Classifies.Count; i++)
                        {
                            var classify = Classifies[i];
                            Func<T, bool> func = classify.Item2;
                            if (func == null)
                            {
                                break;
                            }
                            if (func.Invoke(T))
                            {
                                SetCache(classify.Item1, path, obj.name, classify.Item3, classify.Item4, GetRelativePathOfComponentOfPrefab(T, obj.transform), T.gameObject.name);
                            }
                        }
                    }
                }
                mode = cache.Keys.First();
            }
        }

        // 先序遍历
        private List<string> GetAssetsAllChildDirectory(string dirPath)
        {
            List<string> dirs = new List<string>();
            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
            if (Directory.Exists(directoryInfo.FullName))
            {
                dirs.Add(directoryInfo.FullName.Substring(Application.dataPath.Length - "Assets".Length).Replace('\\', '/'));
                DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();
                for (int index = 0; index < directoryInfoArray.Length; index++)
                {
                    dirs.AddRange(GetAssetsAllChildDirectory(directoryInfoArray[index].FullName));
                }
            }
            return dirs;
        }

        // 设置缓存
        private void SetCache(TEnum tEnum, string prefabPath, string prefabName, string scanLog, Action<T> handle, string componentObjPath, string componentObjName)
        {
            if (!cache.ContainsKey(tEnum))
            {
                cache.Add(tEnum, new Dictionary<string, ScanInfo<T>>());
            }
            if (!cache[tEnum].ContainsKey(prefabPath))
            {
                ScanInfo<T> scanInfo = new ScanInfo<T>(prefabPath, prefabName);
                cache[tEnum].Add(prefabPath, scanInfo);
            }
            if (!cache[tEnum][prefabPath].ContainsKey(scanLog))
            {
                ScanInfoItem<T> scanInfoItem = new ScanInfoItem<T>(prefabPath, scanLog, handle);
                cache[tEnum][prefabPath].Add(scanLog, scanInfoItem);
            }
            cache[tEnum][prefabPath][scanLog].ComponentObjsPath.Add(componentObjPath);
            cache[tEnum][prefabPath][scanLog].ComponentObjsName.Add(componentObjName);
        }

        // 获取组件对象在预制体中的相对路径
        private string GetRelativePathOfComponentOfPrefab(T t, Transform prefabTran)
        {
            List<string> objsName = new List<string>();
            Transform tran = t.transform;
            objsName.Add(tran.gameObject.name);
            while (tran != prefabTran)
            {
                tran = tran.parent;
                objsName.Add(tran.gameObject.name);
            }
            StringBuilder sb = new StringBuilder();
            for (int index = objsName.Count - 1; index >= 0; index--)
            {
                sb.Append(objsName[index]);
                if (index != 0)
                {
                    sb.Append("/");
                }
            }
            return sb.ToString();
        }
    }
}