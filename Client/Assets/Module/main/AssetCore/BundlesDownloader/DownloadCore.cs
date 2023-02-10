using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Ninth
{
    public class DownloadCore
    {
        private Dictionary<string, VersionConfig> m_Path2VersionConfig;

        private Dictionary<string, DownloadConfig> m_Path2DownloadConfig;

        private Dictionary<string, BundleInfo> m_IncreaseBundleDic; // 新增的Bundle

        private Dictionary<string, BundleInfo> m_DecreaseBundleDic; // 废弃的Bundle

        public List<int> IncreaseTypeNodes { get; set; } // 新增的类型节点

        public List<int> DecreaseTypeNodes { get; set; } // 废弃的类型节点

        public int DownloadBundleStartPos
        {
            get => PlayerPrefsDefine.DownloadBundleStartPos;
            set => PlayerPrefsDefine.DownloadBundleStartPos = value;
        }

        public int GetDownloadBundleStartPos()
        {
            VersionConfig tempVersionConfig = GetVersionConfig(PathConfig.TempVersionInPersistentDataPath());
            if(tempVersionConfig == null)
            {
                throw new System.Exception("tempVersionConfig is missing");
            }
            if(!long.TryParse(PlayerPrefsDefine.DownloadBundleStartPosFromAssetVersion, out long lastBreakVersion)
                || tempVersionConfig.Version != lastBreakVersion)
            {
                // 重置版本 与 断点位置
                PlayerPrefsDefine.DownloadBundleStartPosFromAssetVersion = tempVersionConfig.Version.ToString();
                PlayerPrefsDefine.DownloadBundleStartPos = 0;
            }
            return PlayerPrefsDefine.DownloadBundleStartPos;
        }

        public MessageBox MessageBox { get; set; }

        // 获取Remote新增的Bundle数量
        public int GetRemoteIncreaseBundleCount { get; set; }

        // 获取Remote废弃的bundle数量
        public int GetRemoteDecreaseBundleCount { get; set; }

        // 按包的数量显示
        public int GetCompleteDownloadIncreaseBundleAmount { get; set; }

        public int GetAllIncreaseBundleCount => m_IncreaseBundleDic.Values.Count;

        // 按百分比显示
        public long GetCompleteDownloadIncreaseBundleProgress { get; set; }

        public long GetTotalIncreaseBundleSize { get; set; }


        UnityWebRequest request;

        public float Progress
        {
            get => request.downloadProgress;
        }

        public DownloadCore()
        {
            m_Path2VersionConfig = new Dictionary<string, VersionConfig>();

            m_Path2DownloadConfig = new Dictionary<string, DownloadConfig>();

            m_IncreaseBundleDic = new Dictionary<string, BundleInfo>();

            m_DecreaseBundleDic = new Dictionary<string, BundleInfo>();
        }

        public void Clear()
        {
            m_Path2VersionConfig.Clear();

            m_Path2DownloadConfig.Clear();

            m_IncreaseBundleDic.Clear();

            m_DecreaseBundleDic.Clear();

            DownloadBundleStartPos = 0;

            GetRemoteIncreaseBundleCount = 0;

            GetRemoteDecreaseBundleCount = 0;

            GetCompleteDownloadIncreaseBundleAmount = 0;

            GetCompleteDownloadIncreaseBundleProgress = 0;

            GetTotalIncreaseBundleSize = 0;

            request = null;
        }

        public async UniTask<bool> Download(string srcPath, string dstPath)
        {
            request = UnityWebRequest.Get(srcPath);

            request.downloadHandler = new DownloadHandlerFile(dstPath);

            UnityEngine.Debug.Log("请求下载到本地路径: " + dstPath);

            await request.SendWebRequest();

            if (string.IsNullOrEmpty(request.error) == false)
            {
                UnityEngine.Debug.LogError($"下载文件：{request.error}");

                return false;
            }
            return true;
        }

        #region Config

        public VersionConfig GetVersionConfig(string path)
        {
            if (m_Path2VersionConfig.TryGetValue(path, out VersionConfig versionConfig))
            {
                return versionConfig;
            }
            else
            {
                VersionConfig newVersionConfig = Utility.ToObject<VersionConfig>(path);

                if (newVersionConfig != null)
                {
                    m_Path2VersionConfig.Add(path, newVersionConfig);
                }
                return newVersionConfig;
            }
        }

        public DownloadConfig GetDownloadConfig(string path)
        {
            if (m_Path2DownloadConfig.TryGetValue(path, out DownloadConfig downloadConfig))
            {
                return downloadConfig;
            }
            else
            {
                DownloadConfig newDownloadConfig = Utility.ToObject<DownloadConfig>(path);

                m_Path2DownloadConfig.Add(path, newDownloadConfig);

                return newDownloadConfig;
            }
        }
        #endregion

        #region Bundle
        /// <summary>
        /// 获取新增Bundle列表
        /// </summary>
        public Dictionary<string, BundleInfo> GetIncreaseBundleList => m_IncreaseBundleDic;

        /// <summary>
        /// 获取废弃Bundle列表
        /// </summary>
        public Dictionary<string, BundleInfo> GetDecreaseBundleList => m_DecreaseBundleDic;

        /// <summary>
        /// 新增的Bundle
        /// </summary>
        /// <param name="bundleInfo"></param>
        public void IncreaseBundle(string bundleName, BundleInfo bundleInfo)
        {
            m_IncreaseBundleDic.Add(bundleName, bundleInfo);
        }

        /// <summary>
        /// 废弃的Bundle
        /// </summary>
        /// <param name="bundleInfo"></param>
        public void DecreaseBundle(string bundleName, BundleInfo bundleInfo)
        {
            m_DecreaseBundleDic.Add(bundleName, bundleInfo);
        }
        #endregion

        /// <summary>
        /// 工具函数 把字节数转换成字符串形式
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public string SizeToString(long size)
        {
            string sizeStr = "";

            if (size >= 1024 * 1024)
            {
                long m = size / (1024 * 1024);

                size = size % (1024 * 1024);

                sizeStr += $"{m}[M]";
            }

            if (size >= 1024)
            {
                long k = size / 1024;

                size = size % 1024;

                sizeStr += $"{k}[K]";
            }

            long b = size;

            sizeStr += $"{b}[B]";

            return sizeStr;
        }
    }
}