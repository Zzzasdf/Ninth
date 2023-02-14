using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ninth.Editor
{
    public sealed partial class Utility
    {
        /// <summary>
        /// 创建新的文件夹
        /// </summary>
        /// <param name="dstPath"></param>
        public static void DirectoryCreateNew(string dstPath)
        {
            if (Directory.Exists(dstPath))
            {
                Directory.Delete(dstPath, true);
            }
            Directory.CreateDirectory(dstPath);
        }

        public static void DirectoryCopy(string srcPath, string dstPath)
        {
            if(string.IsNullOrEmpty(srcPath))
            {
                return;
            }
            DirectoryCreateNew(dstPath);
            FileInfo[] files = new DirectoryInfo(srcPath).GetFiles();
            for(int index = 0; index < files.Length; index++)
            {
                File.Copy(files[index].FullName, dstPath + "/" + files[index].Name, true);
            }
        }
    }
}
