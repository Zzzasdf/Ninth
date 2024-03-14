using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninth.HotUpdate;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class BuildBundleInfo : BaseBuildInfo
    {
        public BuildBundleInfo(AssetGroup assetGroup)
        {
            this.assetGroup = assetGroup;
        }

        public override void Build(string buildFolder, BuildAssetBundleOptions buildAssetBundleOptions, BuildTarget buildTarget)
        {
            BuildPipeline.BuildAssetBundles(buildFolder.Log(), assetBundleBuilds.ToArray(), buildAssetBundleOptions, buildTarget);
        }

        public override void CalculateDependencies()
        {
            foreach (var (assetPath, assetRef) in assetPath2AssetRef)
            {
                var assetBundle = assetRef.BundleRef.BundleName;
                var dependencies = AssetDatabase.GetDependencies(assetPath);
                var assetPaths = GetAssetList(dependencies, assetPath);
                var bundleNames = GetBundleNames(assetPaths, assetBundle);
                assetRef.Dependencies = new List<BundleRef>();
                foreach (var bundleName in bundleNames)
                {
                    assetRef.Dependencies.Add(bundleName2BundleRef[bundleName]);
                }
            }
            return;

            IEnumerable<string> GetAssetList(string[] dependencies, string assetPath)
            {
                if (dependencies is { Length: > 0 })
                {
                    foreach (var oneAsset in dependencies)
                    {
                        if (oneAsset == assetPath || oneAsset.EndsWith(".cs"))
                        {
                            continue;
                        }

                        yield return oneAsset;
                    }
                }
            }

            HashSet<string> GetBundleNames(IEnumerable<string> assetPaths, string assetBundle)
            {
                var bundleNames = new HashSet<string>();
                foreach (var oneAsset in assetPaths)
                {
                    if (!assetPath2AssetRef.TryGetValue(oneAsset, out var value))
                    {
                        continue;
                    }

                    var bundleName = value.BundleRef.BundleName;
                    if (bundleName == assetBundle || bundleNames.Contains(bundleName))
                    {
                        continue;
                    }

                    bundleNames.Add(bundleName);
                }

                return bundleNames;
            }
        }
    }
}
