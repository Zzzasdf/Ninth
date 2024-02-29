using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly IAssetConfig assetConfig;
        private readonly IAssetDownloadBox assetDownloadBox;
        private readonly IObjectResolver objResolver;

        [Inject]
        public AssetBundleProxy(IPathProxy pathProxy, IJsonProxy jsonProxy, IDownloadProxy downloadProxy, IAssetConfig assetConfig, IAssetDownloadBox assetDownloadBox, IObjectResolver objResolver)
        {
            this.pathProxy = pathProxy;
            this.jsonProxy = jsonProxy;
            this.downloadProxy = downloadProxy;
            this.assetConfig = assetConfig;
            this.assetDownloadBox = assetDownloadBox;
            this.objResolver = objResolver;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            if (assetConfig.RuntimeEnv() != Environment.RemoteAb)
            {
                // 启动
                objResolver.Resolve<LoadDll>();
                return;
            }
            // versionConfig
            var versionConfigByServer = await GetVersionConfigAsync(ASSET_SERVER_VERSION_PATH.AssetServer, cancellation);
            var versionConfigByPersistentData = await jsonProxy.ToObjectAsync<VersionConfig>(VERSION_PATH.PersistentData, cancellation);
            var versionConfigByStreamingAssets = await jsonProxy.ToObjectAsync<VersionConfig>(VERSION_PATH.StreamingAssets, cancellation);
            var update = await VersionConfig.UpdateCompare(versionConfigByServer, versionConfigByPersistentData, versionConfigByStreamingAssets, cancellation);
            if (!update || versionConfigByServer == null)
            {
                return;
            }
            
            // downloadConfig
            var downloadConfigByServerRemote = await GetDownloadConfigAsync(ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByRemoteGroup, versionConfigByServer.BuiltIn, cancellation);
            var downloadConfigByPersistentDataRemote = await jsonProxy.ToObjectAsync<DownloadConfig>(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData, cancellation);
            var bundleInfosByRemote = DownloadConfig.UpdateCompare(downloadConfigByServerRemote, downloadConfigByPersistentDataRemote, cancellation);
            
            var downloadConfigByServerDll = await GetDownloadConfigAsync(ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByDllGroup, versionConfigByServer.BuiltIn, cancellation);
            var downloadConfigByPersistentDataDll = await jsonProxy.ToObjectAsync<DownloadConfig>(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData, cancellation);
            var bundleInfosByDll = DownloadConfig.UpdateCompare(downloadConfigByServerDll, downloadConfigByPersistentDataDll, cancellation);

            //  download bundle
            if (bundleInfosByRemote is { Count: > 0 }
                || bundleInfosByDll is { Count: > 0 })
            {
                // box => 下载 bundle
                var complete = await assetDownloadBox.PopUpAsync(versionConfigByServer.BuiltIn, cancellation, (ASSET_SERVER_BUNDLE_PATH.BundlePathByRemoteGroup, bundleInfosByRemote), (ASSET_SERVER_BUNDLE_PATH.BundlePathByDllGroup, bundleInfosByDll));
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
            await DownloadLoadConfigAsync(ASSET_SERVER_CONFIG_PATH.LoadConfigPathByRemoteGroup, versionConfigByServer.BuiltIn, cancellation);
            await DownloadLoadConfigAsync(ASSET_SERVER_CONFIG_PATH.LoadConfigPathByDllGroup, versionConfigByServer.BuiltIn, cancellation);
            
            // 保存临时 versionConfig, DownloadConfig
            jsonProxy.ToJsonAsync<VersionConfig>(VERSION_PATH.PersistentData, cancellation);
            if (downloadConfigByServerRemote != null)
            {
                jsonProxy.ToJsonAsync<DownloadConfig>(CONFIG_PATH.DownloadConfigPathByRemoteGroupByPersistentData, cancellation);
            }
            if (downloadConfigByServerDll != null)
            {
                jsonProxy.ToJsonAsync<DownloadConfig>(CONFIG_PATH.DownloadConfigPathByDllGroupByPersistentData, cancellation);
            }
            // 启动
            objResolver.Resolve<LoadDll>();
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
