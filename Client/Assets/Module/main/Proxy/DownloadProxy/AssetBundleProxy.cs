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
    public class AssetBundleProxy : IAssetBundleProxy, IDisposable
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

        public AssetBundleProxy(ReadOnlyDictionary<string, BundleInfo?> increaseBundleContainer)
        {
            this.increaseBundleContainer = increaseBundleContainer;
        }

        async UniTask<VersionConfig?> IAssetBundleProxy.GetVersionConfig(VERSION_PATH versionPath, CancellationToken cancellationToken)
        {
            if (!versionConfigContainer.TryGetValue(versionPath, out var value))
            {
                value = await jsonProxy.ToObjectAsync<VersionConfig>(versionPath, cancellationToken);
                versionConfigContainer.TryAdd(versionPath, value);
            }
            return value;
        }

        async UniTask<VersionConfig?> IAssetBundleProxy.GetVersionConfig(ASSET_SERVER_VERSION_PATH versionPath, CancellationToken cancellationToken)
        {
            if (!versionConfigContainer.TryGetValue(versionPath, out var value))
            {
                // 先下载 ..
                var (srcPath, tempPathOrNull) = pathProxy.Get(versionPath);
                if (tempPathOrNull == null)
                {
                    $"无下载的本地目标路径 源路径：{srcPath}".Error();
                    return null;
                }
                var tempPath = tempPathOrNull.Value;
                var dstPath = pathProxy.Get(tempPath);
                var downloadResult = await downloadProxy.DownloadAsync(srcPath, dstPath, cancellationToken);
                if (!downloadResult)
                {
                    return null;
                }
                value = await jsonProxy.ToObjectAsync<VersionConfig>(tempPath, cancellationToken);
                if (!versionConfigContainer.TryAdd(tempPath, value))
                {
                    $"重复添加 {tempPath.GetType().Name}: {tempPath}".FrameError();
                }
            }
            return value;
        }
        
        async UniTask<DownloadConfig?> IAssetBundleProxy.GetDownloadConfig(CONFIG_PATH configPath, CancellationToken cancellationToken)
        {
            if (!downloadConfigContainer.TryGetValue(configPath, out var value))
            {
                value = await jsonProxy.ToObjectAsync<DownloadConfig>(configPath, cancellationToken);
                downloadConfigContainer.TryAdd(configPath, value);
            }
            return value;
        }
        
        async UniTask<DownloadConfig?> IAssetBundleProxy.GetDownloadConfig(ASSET_SERVER_CONFIG_PATH configPath, string version, CancellationToken cancellationToken)
        {
            if (!downloadConfigContainer.TryGetValue(configPath, out var value))
            {
                // 先下载 ..
                var (srcPath, tempPathOrNull) = pathProxy.Get(configPath, version);
                if (tempPathOrNull == null)
                {
                    $"无下载的本地目标路径 源路径：{srcPath}".Error();
                    return null;
                }
                var tempPath = tempPathOrNull.Value;
                var dstPath = pathProxy.Get(tempPath);
                var downloadResult = await downloadProxy.DownloadAsync(srcPath, dstPath, cancellationToken);
                if (!downloadResult)
                {
                    return null;
                }
                value = await jsonProxy.ToObjectAsync<DownloadConfig>(tempPath, cancellationToken);
                if (!downloadConfigContainer.TryAdd(tempPath, value))
                {
                    $"重复添加 {tempPath.GetType().Name}: {tempPath}".FrameError();
                }
            }
            return value;
        }
        
        // 需要用新的下载配置和旧下载配置比较完后，把需要下载的 bundle 配置加入 increaseBundleContainer 后才能调用这个方法，TODO：构思代码，改改改！！
        // 一个方法完成，不要将 单个下载配置 和 单个下载 bundle 方法公开！！ 版本配置？？ 待定
        // await 下载完所有下载配置文件，比较后注入 increaseBundleContainer
        // await 弹窗提示下载
        // await 下载所有新增 bundle
        // 启动
        async UniTask<bool> IAssetBundleProxy.DownloadBundle(ASSET_SERVER_BUNDLE_PATH bundlePath, string version, string bundleName, CancellationToken cancellationToken)
        {
            if (!increaseBundleContainer.TryGetValue(bundleName, out var value))
            {
                $"调用错误，此 bundle 无需下载 bundleName：{bundlePath}".Error();
                return false;
            }
            var (srcPath, tempPathOrNull) = pathProxy.Get(bundlePath, version, bundleName);
            if (tempPathOrNull == null)
            {
                $"无下载的本地目标路径 源路径：{srcPath}".Error();
                return false;
            }
            var tempPath = tempPathOrNull.Value;
            var dstPath = pathProxy.Get(tempPath, bundleName);
            return await downloadProxy.DownloadAsync(srcPath, dstPath, cancellationToken);
        }

        public void Dispose()
        {
            
        }
    }
}
