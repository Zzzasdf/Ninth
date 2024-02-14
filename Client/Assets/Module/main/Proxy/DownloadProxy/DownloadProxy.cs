using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;

namespace Ninth
{
    public class DownloadProxy: IDownloadProxy
    {
        private readonly IPlayerPrefsIntProxy playerPrefsIntProxy;
        private readonly IPlayerPrefsStringProxy playerPrefsStringProxy;
        private readonly IServerPathProxy serverPathProxy;
        private readonly IJsonProxy jsonProxy;
        
        [Inject]
        public DownloadProxy(IPlayerPrefsIntProxy playerPrefsIntProxy, IPlayerPrefsStringProxy playerPrefsStringProxy, IServerPathProxy serverPathProxy, IJsonProxy jsonProxy)
        {
            this.playerPrefsIntProxy = playerPrefsIntProxy;
            this.playerPrefsStringProxy = playerPrefsStringProxy;
            this.serverPathProxy = serverPathProxy;
            this.jsonProxy = jsonProxy;
        }

        public List<int> IncreaseTypeNodes { get; set; } // 新增的类型节点

        public int DownloadBundleStartPos
        {
            get => PlayerPrefsConfig.DownloadBundleStartPos;
            set => PlayerPrefsConfig.DownloadBundleStartPos = value;
        }

        public int GetDownloadBundleStartPos()
        {
            VersionConfig tempVersionConfig = GetVersionConfig(pathConfig.TempVersionInPersistentDataPath());
            VersionConfig versionConfig = GetVersionConfig(pathConfig.VersionInPersistentDataPath());

            if (tempVersionConfig == null)
            {
                throw new System.Exception("tempVersionConfig is missing");
            }

            if (tempVersionConfig.Version != PlayerPrefsConfig.DownloadBundleStartPosFromAssetVersion
                || versionConfig == null) // 防止出现持久化目录数据被删除但有缓存的情况，这种情况下会少下资源包
            {
                // 重置版本 与 断点位置
                PlayerPrefsConfig.DownloadBundleStartPosFromAssetVersion = tempVersionConfig.Version;
                DownloadBundleStartPos = 0;
            }

            return DownloadBundleStartPos;
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

        public DownloadProxy()
        {
        }

        public void Clear()
        {
            m_Path2VersionConfig.Clear();
            m_Path2DownloadConfig.Clear();
            m_IncreaseBundleDic.Clear();
            GetRemoteIncreaseBundleCount = 0;
            GetRemoteDecreaseBundleCount = 0;
            GetCompleteDownloadIncreaseBundleAmount = 0;
            GetCompleteDownloadIncreaseBundleProgress = 0;
            GetTotalIncreaseBundleSize = 0;
            IDownloadProxy.DownloadAsync("", "");
            request = null;
        }

        async UniTask<bool> IDownloadProxy.DownloadAsync(string? srcPath, string? dstPath, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(srcPath) || string.IsNullOrEmpty(dstPath))
            {
                $"下载的源路径或目标路径为空，源路径：{srcPath}, 目标路径: {dstPath}".Error();
                return false;
            }
            request = UnityWebRequest.Get(srcPath);
            request.downloadHandler = new DownloadHandlerFile(dstPath);
            await request.SendWebRequest();
            if (string.IsNullOrEmpty(request.error) == false)
            {
                $"下载错误, 远端路径: {srcPath}, 本地路径: {dstPath}, 错误日志: {request.error}".Error();
                return false;
            }
            return true;
        }

        #region Config

       


        public void ClearConfigCache()
        {
            m_Path2VersionConfig.Clear();
            m_Path2DownloadConfig.Clear();
        }

        #endregion

        #region Bundle

        /// <summary>
        /// 获取新增Bundle列表
        /// </summary>
        public Dictionary<string, BundleInfo> GetIncreaseBundleList => m_IncreaseBundleDic;

        /// <summary>
        /// 新增的Bundle
        /// </summary>
        /// <param name="bundleInfo"></param>
        public void IncreaseBundle(string bundleName, BundleInfo bundleInfo)
        {
            m_IncreaseBundleDic.Add(bundleName, bundleInfo);
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