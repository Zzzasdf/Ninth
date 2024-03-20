using System;
using System.Collections.Generic;
using System.IO;

namespace Ninth.Utility
{
    public class Utility
    {
        public static IEnumerable<string> GetAllSubFolders(IEnumerable<string> rootFolderPaths)
        {
            foreach (var rootFolderPath in rootFolderPaths)
            {
                var allFolders = GetAllSubFolders(rootFolderPath);
                foreach (var folder in allFolders)
                {
                    yield return folder;
                }
            }
        }
        
        public static IEnumerable<string> GetAllSubFolders(string rootFolderPath)
        {
            var folderQueue = new Queue<string>();
            folderQueue.Enqueue(rootFolderPath);

            while (folderQueue.Count > 0)
            {
                var currentFolder = folderQueue.Dequeue();
                yield return currentFolder; // 返回当前文件夹路径  

                try
                {
                    // 获取当前文件夹下的所有子文件夹并加入队列  
                    var subFolders = Directory.GetDirectories(currentFolder);
                    foreach (var subFolder in subFolders)
                    {
                        folderQueue.Enqueue(subFolder);
                    }
                }
                catch (Exception ex)
                {
                    // 处理任何访问文件夹时发生的异常（例如权限问题）  
                    Console.WriteLine($"Error accessing folder '{currentFolder}': {ex.Message}");
                }
            }
        }
    }
}