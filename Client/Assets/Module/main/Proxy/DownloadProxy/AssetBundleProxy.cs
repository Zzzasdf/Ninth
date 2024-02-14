using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class AssetBundleProxy : IAssetBundleProxy
    {
        private readonly IPathProxy pathProxy;
        private readonly IJsonProxy jsonProxy;
        private readonly IDownloadProxy downloadProxy;
        
        private readonly ReadOnlyDictionary<Enum, VersionConfig?> versionConfigContainer;
        private readonly ReadOnlyDictionary<Enum, DownloadConfig?> downloadConfigContainer;
        private readonly ReadOnlyDictionary<string, BundleInfo?> increaseBundleContainer;

        [Inject]
        public AssetBundleProxy(IPathProxy pathProxy, IJsonProxy jsonProxy, IDownloadProxy downloadProxy)
        {
            this.pathProxy = pathProxy;
            this.jsonProxy = jsonProxy;
            this.downloadProxy = downloadProxy;

            versionConfigContainer = new ReadOnlyDictionary<Enum, VersionConfig?>(new Dictionary<Enum, VersionConfig?>());
            downloadConfigContainer = new ReadOnlyDictionary<Enum, DownloadConfig?>(new Dictionary<Enum, DownloadConfig?>());
            increaseBundleContainer = new ReadOnlyDictionary<string, BundleInfo?>(new Dictionary<string, BundleInfo?>());
        }

        async UniTask<VersionConfig?> IAssetBundleProxy.GetVersionConfig(VERSION_PATH versionPath)
        {
            if (!versionConfigContainer.TryGetValue(versionPath, out var value))
            {
                value = await jsonProxy.ToObjectAsync<VersionConfig>(versionPath);
                if (!versionConfigContainer.TryAdd(versionPath, value))
                {
                    $"重复添加 {nameof(VERSION_PATH)}: {versionPath}".FrameError();
                }
            }
            return value;
        }
        
        async UniTask<VersionConfig?> IAssetBundleProxy.GetVersionConfig(ASSET_SERVER_VERSION_PATH versionPath)
        {
            if (!versionConfigContainer.TryGetValue(versionPath, out var value))
            {
                // 先下载 ..
                var (srcPath, tempPathOrNull)= pathProxy.Get(versionPath);
                if (tempPathOrNull == null)
                {
                    $"无下载的本地目标路径 源路径：{srcPath}".Error();
                    return null;
                }
                var tempPath = tempPathOrNull.Value;
                var dstPath = pathProxy.Get(tempPath);
                var downloadResult = await downloadProxy.DownloadAsync(srcPath, dstPath);
                if (!downloadResult)
                {
                    return null;
                }
                value = await jsonProxy.ToObjectAsync<VersionConfig>(tempPath);
                if (!versionConfigContainer.TryAdd(tempPath, value))
                {
                    $"重复添加 {tempPath.GetType().Name}: {tempPath}".FrameError();
                }
            }
            return value;
        }
        
        // 获取下载配置
        
    }
}
