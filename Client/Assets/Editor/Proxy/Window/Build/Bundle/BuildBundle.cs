using System.Collections.Generic;
using System.IO;
using System.Linq;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class BuildBundle: IBuildAssets
    {
        public AssetGroupsPaths AssetGroupsPaths { get; }
        public string FolderByGroup { get; }
        public BaseBuildInfo BuildInfo { get; }

        public BuildBundle(AssetGroup assetGroup, AssetGroupsPaths assetGroupsPaths, string folderByGroup)
        {
            this.AssetGroupsPaths = assetGroupsPaths;
            this.FolderByGroup = folderByGroup;
            this.BuildInfo = new BuildBundleInfo(assetGroup);
        }


        public void ScanAssets(string buildFolder, BuildTarget buildTarget)
        {
            var groupFolder = $"{buildFolder}/{FolderByGroup}";
            if (!Directory.Exists(groupFolder))
                Directory.CreateDirectory(groupFolder);
            var buildBundleInfo = (BuildInfo as BuildBundleInfo)!;
            
            var groupPaths = AssetGroupsPaths.AssetGroupPaths.Value;
            var allSubFolders = Ninth.Utility.Utility.GetAllSubFolders(groupPaths);
            foreach (var folder in allSubFolders)
            {
                var bundleName = folder[(Application.dataPath.Length + 1)..].Replace('/', '_').Replace('\\', '_').ToLower();
                var folderInfo = new DirectoryInfo(folder);
                var assetPaths = ScanCurrDirectory(folderInfo).ToArray();
                buildBundleInfo.AddDepend(bundleName, assetPaths);
            }
            return;
            
            IEnumerable<string> ScanCurrDirectory(DirectoryInfo folderInfo)
            {
                var fileInfoList = folderInfo.GetFiles();
                foreach (var fileInfo in fileInfoList)
                {
                    if (fileInfo.FullName.EndsWith(".meta") || fileInfo.FullName.EndsWith(".DS_Store"))
                    {
                        continue;
                    }
                    yield return fileInfo.FullName[(Application.dataPath.Length - "Assets".Length)..].Replace('\\', '/');
                }
            }
        }

        public void Build(string buildFolder, BuildAssetBundleOptions buildAssetBundleOptions, BuildTarget buildTarget)
        {
            BuildInfo.Build(buildFolder, buildAssetBundleOptions, buildTarget);
            var files = Directory.GetFiles(buildFolder, "*.manifest");
            foreach (var file in files)
            {
                File.Delete(file);
            }
            File.Delete($"{buildFolder}/{Path.GetFileName(buildFolder)}");
        }

        public void CalculateDependencies()
        {
            BuildInfo.CalculateDependencies();
        }

        public void SaveConfig(string fullFolder, IJsonProxy jsonProxy, string loadConfigName, string? downloadConfigName)
        {
            BuildInfo.SaveConfig(fullFolder, jsonProxy, loadConfigName, downloadConfigName);
        }
    }

    public class AssetGroupsPaths
    {
        public ReactiveProperty<List<string>> AssetGroupPaths { get; }
        public string AssetGroupLabel { get; }
        public string AssetGroupDefaultName { get; }
        public AssetGroupsPaths(ReactiveProperty<List<string>> assetGroupPaths, string assetGroupLabel, string assetGroupDefaultName)
        {
            this.AssetGroupPaths = assetGroupPaths;
            this.AssetGroupLabel = assetGroupLabel;
            this.AssetGroupDefaultName = assetGroupDefaultName;
        }
    }
}
