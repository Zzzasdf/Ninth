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
        /// <param name="directoryPath"></param>
        public static void CreateNewDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath) == true)
            {
                Directory.Delete(directoryPath, true);
            }
            Directory.CreateDirectory(directoryPath);
        }
    }
}
