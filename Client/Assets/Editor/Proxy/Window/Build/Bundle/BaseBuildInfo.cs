using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninth.HotUpdate;
using Ninth.Utility;
using UnityEditor;

namespace Ninth.Editor
{
    public abstract class BaseBuildInfo
    {
        protected AssetGroup assetGroup;

        protected readonly List<AssetBundleBuild> assetBundleBuilds = new();

        protected readonly List<string> bundleNameList = new();
        protected readonly Dictionary<string, BundleRef> bundleName2BundleRef = new();

        protected readonly List<string> assetPathList = new();
        protected readonly Dictionary<string, AssetRef> assetPath2AssetRef = new();

        protected readonly LoadConfig loadConfig = new();
        protected readonly DownloadConfig downloadConfig = new();

        public void AddDepend(string bundleName, string[] assetPaths)
        {
            if (assetPaths.Length == 0)
            {
                return;
            }

            // assetBundleBuild
            var assetBundleBuild = new AssetBundleBuild
            {
                assetBundleName = bundleName,
                assetNames = assetPaths
            };
            assetBundleBuilds.Add(assetBundleBuild);
            // bundle
            var bundleRef = new BundleRef
            {
                BundleName = bundleName,
                AssetGroup = assetGroup
            };
            bundleName2BundleRef.Add(bundleName, bundleRef);
            bundleNameList.Add(bundleName);

            // asset
            assetPathList.AddRange(assetPaths);
            var assetRefs = assetPaths
                .Select(assetPath => new AssetRef
                {
                    AssetPath = assetPath,
                    BundleRef = bundleRef
                });
            foreach (var assetRef in assetRefs)
            {
                assetPath2AssetRef.Add(assetRef.AssetPath, assetRef);
            }
        }

        public abstract void Build(string buildFolder, BuildAssetBundleOptions buildAssetBundleOptions, BuildTarget buildTarget);
        public abstract void CalculateDependencies();

        public void SaveConfig(string fullFolder, IJsonProxy jsonProxy, string loadConfigName, string? downloadConfigName)
        {
            var assetRefs = assetPath2AssetRef.Values;

            // loadConfig
            foreach (var assetRef in assetRefs)
            {
                loadConfig.AssetRefList.Add(assetRef);
            }
            jsonProxy.ToJson(loadConfig, $"{fullFolder}/{loadConfigName}");

            // downloadConfig
            if (!string.IsNullOrEmpty(downloadConfigName))
            {
                foreach (var (bundleName, value) in bundleName2BundleRef)
                {
                    var bundleInfo = new BundleInfo
                    {
                        BundleName = bundleName,
                    };
                    var bundleFilePath = $"{fullFolder}/{bundleName}";
                    using (var stream = File.OpenRead(bundleFilePath))
                    {
                        bundleInfo.Crc = Utility.GetCRC32Hash(stream);
                        bundleInfo.Size = (int)stream.Length;
                    }
                    downloadConfig.BundleInfos.Add(bundleName, bundleInfo);
                }
                jsonProxy.ToJson(downloadConfig, $"{fullFolder}/{downloadConfigName}");
            }
        }
    }
}