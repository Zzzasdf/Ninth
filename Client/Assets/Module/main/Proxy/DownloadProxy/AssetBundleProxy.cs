using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Ninth.Utility;

namespace Ninth
{
    public class AssetBundleProxy : IAssetBundleProxy, IAsyncStartable
    {
        private readonly IPathProxy pathProxy;
        private readonly IJsonProxy jsonProxy;
        private readonly IDownloadProxy downloadProxy;
        private readonly IObjectResolver resolver;

        [Inject]
        public AssetBundleProxy(IPathProxy pathProxy, IJsonProxy jsonProxy, IDownloadProxy downloadProxy, IObjectResolver resolver)
        {
            this.pathProxy = pathProxy;
            this.jsonProxy = jsonProxy;
            this.downloadProxy = downloadProxy;
            this.resolver = resolver;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            // versionConfig
            var versionConfigByServer = await GetVersionConfigAsync(ASSET_SERVER_VERSION_PATH.AssetServer, cancellation);
            var versionConfigByPersistentData = await jsonProxy.ToObjectAsync<VersionConfig>(VERSION_PATH.PersistentData, cancellation, notExistHandle: () => null);
            var versionConfigByStreamingAssets = await jsonProxy.ToObjectAsync<PlayerVersionConfig>(VERSION_PATH.StreamingAssets, cancellation);
            var update = await VersionConfig.UpdateCompare(versionConfigByServer, versionConfigByPersistentData, versionConfigByStreamingAssets, cancellation);
            if (versionConfigByServer == null)
            {
                "无法加载到资源服务器的版本配置".FrameError();
                return;
            }
            if (!update)
            {
                File.Delete(pathProxy.Get(VERSION_PATH.PersistentData));
                File.Move(pathProxy.Get(VERSION_PATH.PersistentDataTemp), pathProxy.Get(VERSION_PATH.PersistentData));
                await resolver.Resolve<LoadDll>().StartAsync(cancellation);
                return;
            }

            // downloadConfig
            var downloadConfigByServerRemote = await GetDownloadConfigAsync(ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByRemoteGroup, versionConfigByServer.BundleByBuiltIn(), cancellation);
            var downloadConfigByPersistentDataRemote = await jsonProxy.ToObjectAsync<DownloadConfig>(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData, cancellation, notExistHandle: () => null);
            var bundleInfosByRemote = DownloadConfig.UpdateCompare(downloadConfigByServerRemote, downloadConfigByPersistentDataRemote, cancellation);
            var downloadConfigByServerDll = await GetDownloadConfigAsync(ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByDllGroup, versionConfigByServer.BundleByBuiltIn(), cancellation);
            var downloadConfigByPersistentDataDll = await jsonProxy.ToObjectAsync<DownloadConfig>(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData, cancellation, notExistHandle: () => null);
            var bundleInfosByDll = DownloadConfig.UpdateCompare(downloadConfigByServerDll, downloadConfigByPersistentDataDll, cancellation);
            //  download bundle
            if (bundleInfosByRemote is { Count: > 0 }
                || bundleInfosByDll is { Count: > 0 })
            {
                // box => 下载 bundle
                var complete = await resolver.Resolve<IAssetDownloadBox>().PopUpAsync(versionConfigByServer.BundleByBuiltIn(), cancellation, (ASSET_SERVER_BUNDLE_PATH.BundlePathByRemoteGroup, bundleInfosByRemote), (ASSET_SERVER_BUNDLE_PATH.BundlePathByDllGroup, bundleInfosByDll));
                if (!complete)
                {
                    Application.Quit();
                    return;
                }
            }
            
            // delete bundle
            if (downloadConfigByPersistentDataRemote?.BundleInfos != null)
            {
                foreach (var bundle in downloadConfigByPersistentDataRemote.BundleInfos)
                {
                    var bundleName = bundle.Key;
                    if (bundleName == null)
                    {
                        continue;
                    }
                    if (downloadConfigByServerRemote?.BundleInfos.ContainsKey(bundleName) == true)
                    {
                        continue;
                    }
                    File.Delete(bundleName);
                }
            }
            if (downloadConfigByPersistentDataDll?.BundleInfos != null)
            {
                foreach (var bundle in downloadConfigByPersistentDataDll.BundleInfos)
                {
                    var bundleName = bundle.Key;
                    if (bundleName == null)
                    {
                        continue;
                    }
                    if (downloadConfigByServerDll?.BundleInfos.ContainsKey(bundleName) == true)
                    {
                        continue;
                    }
                    File.Delete(bundleName);
                }
            }
            
            // 下载 loadConfig
            await DownloadLoadConfigAsync(ASSET_SERVER_CONFIG_PATH.LoadConfigPathByRemoteGroup, versionConfigByServer.BundleByBuiltIn(), cancellation);
            await DownloadLoadConfigAsync(ASSET_SERVER_CONFIG_PATH.LoadConfigPathByDllGroup, versionConfigByServer.BundleByBuiltIn(), cancellation);
            
            // 替换 versionConfig, DownloadConfig
            File.Delete(pathProxy.Get(VERSION_PATH.PersistentData));
            File.Move(pathProxy.Get(VERSION_PATH.PersistentDataTemp), pathProxy.Get(VERSION_PATH.PersistentData));
            File.Delete(pathProxy.Get(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData));
            File.Move(pathProxy.Get(CONFIG_PATH.DownloadConfigTempPathByRemoteGroupByPersistentData), pathProxy.Get(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData)); 
            File.Delete(pathProxy.Get(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData));
            File.Move(pathProxy.Get(CONFIG_PATH.DownloadConfigTempPathByDllGroupByPersistentData), pathProxy.Get(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData));
            
            // 启动
            await resolver.Resolve<LoadDll>().StartAsync(cancellation);
        }

        private async UniTask<VersionConfig?> GetVersionConfigAsync(ASSET_SERVER_VERSION_PATH versionPath, CancellationToken cancellationToken)
        {
            var (serverPath, cachePath) = pathProxy.Get(versionPath);
            var dstPath = pathProxy.Get(cachePath);
            var downloadResult = await downloadProxy.DownloadAsync(serverPath, dstPath, cancellationToken);
            if (!downloadResult)
            {
                return null;
            }
            return await jsonProxy.ToObjectAsync<VersionConfig>(cachePath, cancellationToken);
        }

        private async UniTask<DownloadConfig?> GetDownloadConfigAsync(ASSET_SERVER_CONFIG_PATH configPath, string version, CancellationToken cancellationToken)
        {
            var (serverPath, cachePath) = pathProxy.Get(configPath, version);
            var dstPath = pathProxy.Get(cachePath);
            var downloadResult = await downloadProxy.DownloadAsync(serverPath, dstPath, cancellationToken);
            if (!downloadResult)
            {
                return null;
            }
            return await jsonProxy.ToObjectAsync<DownloadConfig>(cachePath, cancellationToken);
        }

        private async UniTask DownloadLoadConfigAsync(ASSET_SERVER_CONFIG_PATH configPath, string version, CancellationToken cancellationToken)
        {
            var (serverPath, cachePath) = pathProxy.Get(configPath, version);
            var dstPath = pathProxy.Get(cachePath);
            await downloadProxy.DownloadAsync(serverPath, dstPath, cancellationToken);
        }
    }
}
