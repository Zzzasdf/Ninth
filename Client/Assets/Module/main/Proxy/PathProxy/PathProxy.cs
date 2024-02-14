using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;
using Ninth;
using UnityEngine;
using VContainer;

namespace Ninth
{
    public class PathProxy : IPathProxy
    {
        private readonly IVersionPath versionPath;
        private readonly IConfigPath configPath;
        private readonly IBundlePath bundlePath;

        private readonly ReadOnlyDictionary<ASSET_SERVER_VERSION_PATH, VERSION_PATH> versionTempPathMapContainer;
        private readonly ReadOnlyDictionary<ASSET_SERVER_CONFIG_PATH, CONFIG_PATH> configTempPathMapContainer;
        private readonly ReadOnlyDictionary<ASSET_SERVER_BUNDLE_PATH, BUNDLE_PATH> bundleTempPathMapContainer;
        
        [Inject]
        public PathProxy(IVersionPath versionPath, IConfigPath configPath, IBundlePath bundlePath)
        {
            this.versionPath = versionPath;
            this.configPath = configPath;
            this.bundlePath = bundlePath;

            versionTempPathMapContainer = new ReadOnlyDictionary<ASSET_SERVER_VERSION_PATH, VERSION_PATH>(new Dictionary<ASSET_SERVER_VERSION_PATH, VERSION_PATH>());
            configTempPathMapContainer = new ReadOnlyDictionary<ASSET_SERVER_CONFIG_PATH, CONFIG_PATH>(new Dictionary<ASSET_SERVER_CONFIG_PATH, CONFIG_PATH>());
            bundleTempPathMapContainer = new ReadOnlyDictionary<ASSET_SERVER_BUNDLE_PATH, BUNDLE_PATH>(new Dictionary<ASSET_SERVER_BUNDLE_PATH, BUNDLE_PATH>());

            Subscribe(ASSET_SERVER_VERSION_PATH.AssetServer, VERSION_PATH.PersistentDataTemp);
            
            Subscribe(ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByRemoteGroup, CONFIG_PATH.DownloadConfigTempPathByRemoteGroupByPersistentData);
            Subscribe(ASSET_SERVER_CONFIG_PATH.DownloadConfigPathByDllGroup, CONFIG_PATH.DownloadConfigTempPathByDllGroupByPersistentData);
        }
        
        void Subscribe(ASSET_SERVER_VERSION_PATH assetServerVersionPath, VERSION_PATH versionPath)
        {
            if (!versionTempPathMapContainer.TryAdd(assetServerVersionPath, versionPath))
            {
                $"重复订阅 {nameof(VERSION_PATH)}: {assetServerVersionPath}".FrameError();
            }
        }
        
        void Subscribe(ASSET_SERVER_CONFIG_PATH assetServerConfigPath, CONFIG_PATH configPath)
        {
            if (!configTempPathMapContainer.TryAdd(assetServerConfigPath, configPath))
            {
                $"重复订阅 {nameof(ASSET_SERVER_CONFIG_PATH)}: {assetServerConfigPath}".FrameError();
            }
        }
        
        void Subscribe(ASSET_SERVER_BUNDLE_PATH assetServerBundlePath, BUNDLE_PATH bundlePath)
        {
            if (!bundleTempPathMapContainer.TryAdd(assetServerBundlePath, bundlePath))
            {
                $"重复订阅 {nameof(ASSET_SERVER_BUNDLE_PATH)}: {assetServerBundlePath}".FrameError();
            }
        }

        string? IPathProxy.Get(VERSION_PATH versionPath)
        {
            return this.versionPath.Get(versionPath);
        }

        (string? assetServerVersionPath, VERSION_PATH? versionPersistentDataTempPath) IPathProxy.Get(ASSET_SERVER_VERSION_PATH versionPath)
        {
            string? assetServerVersionPath = this.versionPath.Get(versionPath);
            if (!versionTempPathMapContainer.TryGetValue(versionPath, out var value))
            {
                return (assetServerVersionPath, null);
            }
            return (assetServerVersionPath, value);
        }

        string? IPathProxy.Get(CONFIG_PATH configPath)
        {
            return this.configPath.Get(configPath);
        }

        (string? assetServerConfigPath, CONFIG_PATH? configPersistentDataTempPath) IPathProxy.Get(ASSET_SERVER_CONFIG_PATH configPath, string version)
        {
            string? assetServerConfigPath = this.configPath.Get(configPath, version);
            if (!configTempPathMapContainer.TryGetValue(configPath, out var value))
            {
                return (assetServerConfigPath, null);
            }
            return (assetServerConfigPath, value);
        }

        string? IPathProxy.Get(BUNDLE_PATH bundlePath, string bundleName)
        {
            return this.bundlePath.Get(bundlePath, bundleName);
        }

        (string? assetServerBundlePath, BUNDLE_PATH? bundlePersistentDataTempPath) IPathProxy.Get(ASSET_SERVER_BUNDLE_PATH bundlePath, string version, string bundleName)
        {
            string? assetServerBundlePath = this.bundlePath.Get(bundlePath, version, bundleName);
            if (!bundleTempPathMapContainer.TryGetValue(bundlePath, out var value))
            {
                return (assetServerBundlePath, null);
            }
            return (assetServerBundlePath, value);
        }

    }
}