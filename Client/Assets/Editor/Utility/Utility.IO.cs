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
        
        public static void ClearFolderContents(string folderPath)  
        {  
            // 确保提供的路径存在  
            if (!Directory.Exists(folderPath))
                return;
            // 获取文件夹中的所有文件和子文件夹  
            var files = Directory.GetFiles(folderPath);
            var dirs = Directory.GetDirectories(folderPath);

            // 删除所有文件  
            foreach (string file in files)
            {
                File.Delete(file);
            }

            // 递归删除所有子文件夹及其内容  
            foreach (var dir in dirs)
            {
                ClearFolderContents(dir); // 递归调用以清空子文件夹  
                Directory.Delete(dir); // 删除空的子文件夹  
            }  
        }
        
        public static void CopyDirectory(string sourceDir, string destinationDir)  
        {  
            // 确保源目录存在  
            if (!Directory.Exists(sourceDir))  
            {  
                throw new DirectoryNotFoundException("源目录不存在: " + sourceDir);  
            }  
  
            // 如果目标目录不存在，则创建它  
            if (!Directory.Exists(destinationDir))  
            {  
                Directory.CreateDirectory(destinationDir);  
            }  
  
            // 获取源目录中的文件  
            FileInfo[] files = new DirectoryInfo(sourceDir).GetFiles();  
  
            // 拷贝文件  
            foreach (FileInfo file in files)  
            {  
                string tempPath = Path.Combine(destinationDir, file.Name);  
                file.CopyTo(tempPath, true); // 第二个参数为true表示可以覆盖同名文件  
            }  
  
            // 获取源目录中的子目录  
            DirectoryInfo[] dirs = new DirectoryInfo(sourceDir).GetDirectories();  
  
            // 递归拷贝子目录  
            foreach (DirectoryInfo dir in dirs)  
            {  
                string newDestinationSubDir = Path.Combine(destinationDir, dir.Name);  
                CopyDirectory(dir.FullName, newDestinationSubDir);  
            }  
        }
    }
}
