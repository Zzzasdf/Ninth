using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ninth.Editor
{
    [Serializable]
    public class AssetPath
    {
        public static string Convert2Relative(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                return string.Empty;
            }
            return fullPath[(Application.dataPath.Length - "Assets".Length)..].Replace('\\', '/');
        }
        
        public string RelativePath;
        public string FullPath => Path.Combine(Application.dataPath.Substring(0, Application.dataPath.IndexOf("Assets", StringComparison.OrdinalIgnoreCase)), RelativePath);

        public bool TrySetFullPath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath)) return false;
            if (!IsVerify(fullPath))
            {
                "请选择项目里的路径".FrameError();
                return false;
            }
            RelativePath = Convert2Relative(fullPath);
            return true;
        }

        public bool IsVerify(string fullPath)
        {
            return fullPath.Replace('\\', '/').Contains(Application.dataPath);
        }
    }
    
    [Serializable]
    public class AssetPathList: IEnumerable<AssetPath>
    {
        public List<AssetPath> AssetPaths = new();

        public AssetPath this[int index]
        {
            get => AssetPaths[index];
            set => AssetPaths[index] = value;
        }
        
        public int Count => AssetPaths.Count;

        public void Add(AssetPath assetPath)
        {
            AssetPaths.Add(assetPath);
        }

        public bool TryModify(int index, string modifyFullPath)
        {
            if (AssetPaths.Select(x => x.RelativePath).Contains(AssetPath.Convert2Relative(modifyFullPath)))
            {
                "该路径已存在".FrameError();
                return false;
            }
            var assetPath = AssetPaths[index];
            if (!assetPath.TrySetFullPath(modifyFullPath))
            {
                return false;
            }
            return true;
        }

        public bool TryAdd(string fullPath)
        {
            if (AssetPaths.Select(x => x.RelativePath).Contains(AssetPath.Convert2Relative(fullPath)))
            {
                "该路径已存在".FrameError();
                return false;
            }
            var assetPath = new AssetPath();
            if (!assetPath.TrySetFullPath(fullPath))
            {
                return false;
            }
            AssetPaths.Add(assetPath);
            return true;
        }
        
        public void RemoveAt(int index)
        {
            AssetPaths.RemoveAt(index);
        }

        public IEnumerator<AssetPath> GetEnumerator()
        {
            return AssetPaths.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
