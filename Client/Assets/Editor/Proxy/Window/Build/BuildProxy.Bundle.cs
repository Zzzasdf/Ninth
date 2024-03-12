using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using Ninth.HotUpdate;
using Ninth.Utility;
using NPOI.SS.Formula.Functions;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace Ninth.Editor
{
    public partial class BuildProxy
    {
        private void BuildBundles(BuildBundlesConfig buildBundlesConfig)
        {
            // 创建文件夹
            var groupFolders = new Dictionary<AssetGroup, string>
            {
                [AssetGroup.Local] = $"{buildBundlesConfig.PrefixFolder}/{nameConfig.DirectoryNameByLocalGroup()}",
                [AssetGroup.Remote] = $"{buildBundlesConfig.PrefixFolder}/{nameConfig.DirectoryNameByRemoteGroup()}",
                [AssetGroup.Dll] = $"{buildBundlesConfig.PrefixFolder}/{nameConfig.DirectoryNameByDllGroup()}",
            };
            foreach (var item in groupFolders)
            {
                if (!Directory.Exists($"{buildBundlesConfig.BuildFolder}/{item.Value}"))
                {
                    Directory.CreateDirectory($"{buildBundlesConfig.BuildFolder}/{item.Value}");
                }
            }
            
            // 扫描资源
            var buildBundleInfo = resolver.Resolve<BuildBundleInfo>();
            foreach (var (assetGroup, value) in buildBundlesConfig.AssetGroups)
            {
                var groupPaths = value.Value;
                var allSubFolders = Ninth.Utility.Utility.GetAllSubFolders(groupPaths);
                var prefixFolder = groupFolders[assetGroup];
                foreach (var folder in allSubFolders)
                {
                    var bundleName = folder[(Application.dataPath.Length + 1)..].Replace('/', '_').Replace('\\', '_').ToLower();
                    var folderInfo = new DirectoryInfo(folder);
                    var assetPaths = ScanCurrDirectory(folderInfo).ToArray();
                    buildBundleInfo.Add(assetGroup, $"{prefixFolder}/{bundleName}", assetPaths);
                }
            }
            var outputPath = buildBundlesConfig.BuildFolder;
            var buildTarget = buildBundlesConfig.BuildTarget;
            var buildAssetBundleOptions = buildBundlesConfig.BuildTargetPlatformInfo.BuildAssetBundleOptions;
            BuildPipeline.BuildAssetBundles(outputPath, buildBundleInfo.AssetBundleBuilds, buildAssetBundleOptions, buildTarget);
            
            // 删除 manifest 文件
            foreach (var (_ , folder) in groupFolders)
            {
                var files = Directory.GetFiles($"{buildBundlesConfig.BuildFolder}/{folder}", "*.manifest");
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
            {
                var files = Directory.GetFiles(buildBundlesConfig.BuildFolder);
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
            
            // 添加依赖
            buildBundleInfo.CalculateDependencies();
            
            // 添加 Dll
            buildBundleInfo.BuildDll(buildBundlesConfig.BuildTarget, $"{buildBundlesConfig.BuildFolder}/{buildBundlesConfig.PrefixFolder}/{nameConfig.DirectoryNameByDllGroup()}");
            
            // 下载配置
            buildBundleInfo.DownloadConfig(outputPath);
            
            // 保存配置
            buildBundleInfo.SaveConfigs(buildBundlesConfig.BuildTargetPlatformInfo, $"{buildBundlesConfig.BuildFolder}/{buildBundlesConfig.PrefixFolder}");
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

        public class BuildBundleInfo
        {
            private readonly IJsonProxy jsonProxy;
            private readonly INameConfig nameConfig;

            private readonly List<AssetBundleBuild> assetBundleBuilds = new();
            private readonly Dictionary<AssetGroup, LoadConfig> loadConfigDic = new();
            private readonly Dictionary<AssetGroup, DownloadConfig> downloadConfigDic = new();

            private readonly Dictionary<AssetGroup, List<string>> assetGroup2BundleNameList = new();
            private readonly Dictionary<string, BundleRef> bundleName2BundleRef = new();

            private readonly Dictionary<AssetGroup, List<string>> assetGroup2AssetPathList = new();
            private readonly Dictionary<string, AssetRef> assetPath2AssetRef = new();

            public AssetBundleBuild[] AssetBundleBuilds => assetBundleBuilds.ToArray();

            [Inject]
            public BuildBundleInfo(IJsonProxy jsonProxy, INameConfig nameConfig)
            {
                this.jsonProxy = jsonProxy;
                this.nameConfig = nameConfig;
            }

            public void Add(AssetGroup assetGroup, string bundleName, string[] assetPaths)
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
                if (!assetGroup2BundleNameList.TryGetValue(assetGroup, out var bundleNameList))
                {
                    assetGroup2BundleNameList.Add(assetGroup, bundleNameList = new List<string>());
                }

                bundleNameList.Add(bundleName);

                // asset
                if (!assetGroup2AssetPathList.TryGetValue(assetGroup, out var assetPathList))
                {
                    assetGroup2AssetPathList.Add(assetGroup, assetPathList = new List<string>());
                }

                assetPathList.AddRange(assetPaths);
                var assetRefs = assetPaths
                    .Select(assetPath => new AssetRef
                    {
                        AssetPath = assetPath,
                        BundleRef = bundleRef
                    });

                // loadConfig
                if (!loadConfigDic.TryGetValue(assetGroup, out var loadConfig))
                {
                    loadConfigDic.Add(assetGroup, loadConfig = new LoadConfig());
                }

                foreach (var assetRef in assetRefs)
                {
                    assetPath2AssetRef.Add(assetRef.AssetPath, assetRef);
                    loadConfig.AssetRefList.Add(assetRef);
                }
            }

            public void CalculateDependencies()
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

            public void BuildDll(BuildTarget buildTarget, string dstDir)
            {
                CompileDllCommand.CompileDll(buildTarget);
                CopyAOTAssemblies2SourceDataTempPath(buildTarget, dstDir);
                CopyHotUpdateAssemblies2SourceDataTempPath(buildTarget, dstDir);
                return;

                void CopyAOTAssemblies2SourceDataTempPath(BuildTarget target, string dstDir)
                {
                    var aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
                    var aotAssembliesDstDir = dstDir;

                    foreach (var dll in LoadDll.AOTMetaAssemblyNames)
                    {
                        var srcDllPath = $"{aotAssembliesSrcDir}/{dll}";
                        if (!File.Exists(srcDllPath))
                        {
                            Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                            continue;
                        }
                        var dllBytesPath = $"{aotAssembliesDstDir}/{dll}.bytes";
                        File.Copy(srcDllPath, dllBytesPath, true);
                        Debug.Log($"[CopyAOTAssembliesToStreamingAssets] copy AOT dll {srcDllPath} -> {dllBytesPath}");
                        DllSortOut($"{dll}.bytes");
                    }
                }

                void CopyHotUpdateAssemblies2SourceDataTempPath(BuildTarget target, string dstDir)
                {
                    var hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
                    var hotfixAssembliesDstDir = dstDir;
                    foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesExcludePreserved)
                    {
                        var dllPath = $"{hotfixDllSrcDir}/{dll}";
                        var dllBytesPath = $"{hotfixAssembliesDstDir}/{dll}.bytes";
                        File.Copy(dllPath, dllBytesPath, true);
                        Debug.Log($"[CopyHotUpdateAssembliesToStreamingAssets] copy hotfix dll {dllPath} -> {dllBytesPath}");
                        DllSortOut($"{dll}.bytes");
                    }
                }
                
                void DllSortOut(string dllName)
                {
                    var assetGroup = AssetGroup.Dll;
                    if (!assetGroup2BundleNameList.ContainsKey(assetGroup))
                    {
                        assetGroup2BundleNameList.Add(assetGroup, new List<string>());
                    }
                    assetGroup2BundleNameList[assetGroup].Add(dllName);

                    // 配置Bundle引用
                    var bundleRef = new BundleRef
                    {
                        BundleName = dllName,
                        AssetGroup = assetGroup
                    };
                    bundleName2BundleRef.Add(dllName, bundleRef);

                    // 资源
                    var assetRef = new AssetRef
                    {
                        AssetPath = dllName,
                        BundleRef = bundleRef
                    };

                    // 配置加载配置
                    if (!loadConfigDic.ContainsKey(assetGroup))
                    {
                        loadConfigDic.Add(assetGroup, new LoadConfig()
                        {
                            AssetRefList = new List<AssetRef>()
                        });
                    }
                    loadConfigDic[assetGroup].AssetRefList.Add(assetRef);
                }
            }
            

            public void DownloadConfig(string bundleFolder)
            {
                foreach (var item in assetGroup2BundleNameList)
                {
                    downloadConfigDic.Add(item.Key, new DownloadConfig());
                }
                foreach (var (bundleName, value) in bundleName2BundleRef)
                {
                    var assetGroup = value.AssetGroup;
                    var bundleInfo = new BundleInfo
                    {
                        BundleName = bundleName,
                    };
                    var bundleFilePath = $"{bundleFolder}/{bundleName}";
                    using (var stream = File.OpenRead(bundleFilePath))
                    {
                        bundleInfo.Crc = Utility.GetCRC32Hash(stream);
                        bundleInfo.Size = (int)stream.Length;
                    }
                    downloadConfigDic[assetGroup].BundleInfos.Add(bundleName, bundleInfo);
                }
            }

            public void SaveConfigs(VersionJson versionJson, string prefixFolder)
            {
                // 保存版本号
                var versionSourceFileName = $"{prefixFolder}/{nameConfig.FileNameByVersionConfig()}";
                var versionDestFileName = $"{prefixFolder}/../{nameConfig.FileNameByVersionConfig()}";
                jsonProxy.ToJson(new VersionConfig 
                    {
                        DisplayVersion = versionJson.DisplayVersion,
                        FrameVersion = versionJson.FrameVersion,
                        HotUpdateVersion = versionJson.HotUpdateVersion,
                        IterateVersion = versionJson.IterateVersion,
                    }, versionSourceFileName);
                // 拷贝版本号到外部
                File.Copy(versionSourceFileName, versionDestFileName, true);
                
                // 空包也添加配置 =》解决拉取配置404问题
                var packAssetGroup = new List<(AssetGroup assetGroup, string folder, string loadConfigPath, string? downLoadConfigPath)>
                {
                    (AssetGroup.Local, 
                        $"{prefixFolder}/{nameConfig.DirectoryNameByLocalGroup()}",
                        nameConfig.LoadConfigNameByLocalGroup(),
                        null),
                    (AssetGroup.Remote, 
                        $"{prefixFolder}/{nameConfig.DirectoryNameByRemoteGroup()}",
                        nameConfig.LoadConfigNameByRemoteGroup(), 
                        nameConfig.DownloadConfigNameByRemoteGroup()),
                    (AssetGroup.Dll, 
                        $"{prefixFolder}/{nameConfig.DirectoryNameByDllGroup()}",
                        nameConfig.LoadConfigNameByDllGroup(), 
                        nameConfig.DownloadConfigNameByDllGroup()),
                };
                foreach (var (assetGroup, folder, loadConfigPath, downLoadConfigPath) in packAssetGroup)
                {
                    if (!loadConfigDic.TryGetValue(assetGroup, out var loadConfig))
                    {
                        loadConfigDic.Add(assetGroup, loadConfig = new LoadConfig());
                    }
                    jsonProxy.ToJson(loadConfig, $"{folder}/{loadConfigPath}");

                    if (string.IsNullOrEmpty(downLoadConfigPath))
                    {
                        continue;
                    }
                    if (!downloadConfigDic.TryGetValue(assetGroup, out var downLoadConfig))
                    {
                        downloadConfigDic.Add(assetGroup, downLoadConfig = new DownloadConfig());
                    }
                    jsonProxy.ToJson(downLoadConfig, $"{folder}/{downLoadConfigPath}");
                }
            }
        }
    }
}