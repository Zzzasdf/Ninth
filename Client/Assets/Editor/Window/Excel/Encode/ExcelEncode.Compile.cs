using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using System.Diagnostics;

namespace Ninth.Editor
{
    public partial class ExcelEncode
    {
        private ExcelsEncodeInfo info;

        //EncodeCompile
        private EncodeCompile compile;

        private void SetCompile()
        {
            if (compile == null)
            {
                compile = new EncodeCompile();
                info = Utility.Get<ExcelsEncodeInfo>(Application.dataPath + "/Editor/Window/Excel/Encode/ExcelEncodeInfo.Json");
                if (info == null)
                {
                    info = new ExcelsEncodeInfo();
                }
            }
            else
            {
                compile.Tables.Clear();
            }
            DirectoryInfo directory = new DirectoryInfo(ExcelEncodePathDirectoryRoot);
            FileInfo[] fileInfos = directory.GetFiles();
            for (int index = 0; index < fileInfos.Length; index++)
            {
                FileInfo fileInfo = fileInfos[index];
                if (fileInfo.Name.StartsWith("~$"))
                {
                    continue;
                }
                compile.Tables.Add(new EncodeCompileTable(fileInfo));
            }
            for (int index = 0; index < compile.Tables.Count; index++)
            {
                EditorGUILayout.BeginHorizontal();
                EncodeCompileTable table = compile.Tables[index];
                EditorGUILayout.TextField(table.FileName);
                EditorGUILayout.TextField("上次修改的时间=>");
                EditorGUILayout.TextField(table.LastWriteTimeUtc.ToString());
                EditorGUILayout.TextField(info.GetVersion(table.FullName).ToString());
                if (GUILayout.Button("Enter"))
                {
                    Process.Start(table.FullName);
                }

                if (!info.TryGetValue(table.FullName, out string LastWriteTimeUtc)
                    || LastWriteTimeUtc != table.LastWriteTimeUtc.ToString())
                {
                    if (GUILayout.Button("Encode"))
                    {
                        info.Save(table.FullName, table.LastWriteTimeUtc.ToString());
                        AssetDatabase.Refresh();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        public class EncodeCompile
        {
            public List<EncodeCompileTable> Tables;

            public EncodeCompile()
            {
                Tables = new List<EncodeCompileTable>();
            }
        }

        public class EncodeCompileTable
        {
            public string FullName { get; private set; }
            public string FileName { get; private set; }

            public DateTime LastWriteTimeUtc { get; private set; }

            public EncodeCompileTable(FileInfo fileInfo)
            {
                FullName = fileInfo.FullName;
                FileName = fileInfo.Name;
                LastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
            }
        }

        public class ExcelsEncodeInfo
        {
            public Dictionary<string, EncodeInfo> Cache { get; private set; }

            public ExcelsEncodeInfo()
            {
                Cache = new Dictionary<string, EncodeInfo>();
            }

            public int GetVersion(string key)
            {
                if (Cache.TryGetValue(key, out EncodeInfo info))
                {
                    return info.Version;
                }
                return 0;
            }

            public bool TryGetValue(string key, out string LastWriteTimeUtc)
            {
                if (Cache.TryGetValue(key, out EncodeInfo info))
                {
                    LastWriteTimeUtc = info.LastWriteTimeUtc;
                    return true;
                }
                LastWriteTimeUtc = null;
                return false;
            }

            public void Save(string key, string LastWriteTimeUtc)
            {
                if (!Cache.TryGetValue(key, out EncodeInfo info))
                {
                    info = new EncodeInfo();
                    Cache.Add(key, info);
                }
                info.LastWriteTimeUtc = LastWriteTimeUtc;
                info.Version++;
                // 保存文件
                Utility.Store(this, Application.dataPath + "/Editor/Window/Excel/Encode/ExcelEncodeInfo.Json");
            }

            public class EncodeInfo
            {
                public string LastWriteTimeUtc;

                public int Version;
            }
        }
    }
}


