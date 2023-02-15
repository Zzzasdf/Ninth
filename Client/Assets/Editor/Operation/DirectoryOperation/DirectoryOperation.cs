using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NinthEditor
{
    public class DirectoryOperation : Editor
    {
        [MenuItem("Tools/Directory/StreamingAssets/Open")]
        public static void StreamingAssetsPathOpen()
        {
            Application.OpenURL(Application.streamingAssetsPath);
        }

        [MenuItem("Tools/Directory/StreamingAssets/Clear")]
        public static void StreamingAssetsPathClear()
        {
            Directory.Delete(Application.streamingAssetsPath, true);
            AssetDatabase.Refresh();
        }
        [MenuItem("Tools/Directory/PersistentData/Open")]
        public static void PersistentDataPathOpen()
        {
            Application.OpenURL(Application.persistentDataPath);
        }

        [MenuItem("Tools/Directory/PersistentData/Clear")]
        public static void PersistentDataPathClear()
        {
            Directory.Delete(Application.persistentDataPath, true);
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Directory/OutputBundles/Open")]
        public static void OutputBundlesPathOpen()
        {
            Application.OpenURL(Ninth.PlayerPrefsDefine.BundleSourceDataDirectoryRoot);
        }

        [MenuItem("Tools/Directory/OutputPlayer/Open")]
        public static void AssetBundleSourceDataPathOpen()
        {
            Application.OpenURL(Ninth.PlayerPrefsDefine.PlayerSourceDataDirectoryRoot);
        }

        [MenuItem("Tools/DeleteAllCache")]
        public static void DeleteAllCache()
        {
            Directory.Delete(Application.persistentDataPath, true);
            PlayerPrefs.DeleteAll();
            AssetDatabase.Refresh();
        }

        private static void ClearDirectory(string dirPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);

            if (Directory.Exists(directoryInfo.FullName))
            {
                FileInfo[] fileArray = directoryInfo.GetFiles();
                for (int index = 0; index < fileArray.Length; index++)
                {
                    File.Delete(fileArray[index].FullName);
                }
                DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();

                for (int index = 0; index < directoryInfoArray.Length; index++)
                {
                    DeleteDirectory(directoryInfoArray[index].FullName);
                }
            }
        }

        private static void DeleteDirectory(string dirPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);

            if (Directory.Exists(directoryInfo.FullName))
            {
                FileInfo[] fileArray = directoryInfo.GetFiles();
                for (int index = 0; index < fileArray.Length; index++)
                {
                    File.Delete(fileArray[index].FullName);
                }
                DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();

                for (int index = 0; index < directoryInfoArray.Length; index++)
                {
                    DeleteDirectory(directoryInfoArray[index].FullName);
                }
                Directory.Delete(directoryInfo.FullName);
            }
        }
    }
}