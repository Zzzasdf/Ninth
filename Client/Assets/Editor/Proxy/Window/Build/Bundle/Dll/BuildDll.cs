using System.IO;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public class BuildDll: IBuildAssets
    {
        public AssetGroupsPaths? AssetGroupsPaths { get; }
        public string FolderByGroup { get; }
        public BaseBuildInfo BuildInfo { get; }

        public BuildDll(AssetGroup assetGroup, string folderByGroup)
        {
            this.FolderByGroup = folderByGroup;
            this.BuildInfo = new BuildDllInfo(assetGroup);
        }
        
        public void ScanAssets(string buildFolder, BuildTarget buildTarget)
        {
            var groupFolder = $"{buildFolder}/{FolderByGroup}";
            if (!Directory.Exists(groupFolder))
                Directory.CreateDirectory(groupFolder);
            
            CompileDllCommand.CompileDll(buildTarget);
            
            var buildBundleInfo = (BuildInfo as BuildDllInfo)!;
            // AOT
            var aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(buildTarget);
            foreach (var dll in LoadDll.AOTMetaAssemblyNames)
            {
                var srcDllPath = $"{aotAssembliesSrcDir}/{dll}";
                if (!File.Exists(srcDllPath))
                {
                    Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                    continue;
                }
                var dllBytesPath = $"{dll}.bytes";
                buildBundleInfo.AddDepend(dllBytesPath, new []{dllBytesPath});
            }

            // HotFix
            foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesExcludePreserved)
            {
                var dllBytesPath = $"{dll}.bytes";
                buildBundleInfo.AddDepend(dllBytesPath, new []{dllBytesPath});
            }
        }

        public void Build(string buildFolder, BuildAssetBundleOptions buildAssetBundleOptions, BuildTarget buildTarget)
        {
            // AOT
            var aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(buildTarget);
            foreach (var dll in LoadDll.AOTMetaAssemblyNames)
            {
                var srcDllPath = $"{aotAssembliesSrcDir}/{dll}";
                if (!File.Exists(srcDllPath))
                {
                    Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                    continue;
                }
                var dllBytesPath = $"{buildFolder}/{dll}.bytes";
                File.Copy(srcDllPath, dllBytesPath, true);
                Debug.Log($"[CopyAOTAssembliesToStreamingAssets] copy AOT dll {srcDllPath} -> {dllBytesPath}");
            }

            // HotFix
            var hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(buildTarget);
            foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesExcludePreserved)
            {
                var dllPath = $"{hotfixDllSrcDir}/{dll}";
                var dllBytesPath = $"{buildFolder}/{dll}.bytes";
                File.Copy(dllPath, dllBytesPath, true);
                Debug.Log($"[CopyHotUpdateAssembliesToStreamingAssets] copy hotfix dll {dllPath} -> {dllBytesPath}");
            }
        }
        
        public void CalculateDependencies()
        {
        }

        public void SaveConfig(string fullFolder, IJsonProxy jsonProxy, string loadConfigName, string downloadConfigName)
        {
            BuildInfo.SaveConfig(fullFolder, jsonProxy, loadConfigName, downloadConfigName);
        }
    }
}
