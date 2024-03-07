using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninth.HotUpdate;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildProxy
    {
        private void BuildBundles(BuildConfig.BuildSettingssss build)
        {
            var versionInfo = build.VersionInfo;
            versionConfig.DisplayVersion = versionInfo.Display;
            versionConfig.FrameVersion = versionInfo.Frame;
            versionConfig.HotUpdateVersion = versionInfo.HotUpdate;
            versionConfig.IterateVersion = versionInfo.Iterate;

            var bundleInfo = build.BundleInfo;
            foreach (var index in bundleInfo.CurrentAssetGroupsByIndex)
            {
                var assetGroup = bundleInfo.GetAssetGroup(index);
                var groupPaths = bundleInfo.Get(index);
                var allSubFolders = Ninth.Utility.Utility.GetAllSubFolders(groupPaths);
                foreach (var folder in allSubFolders)
                {
                    var bundleName = folder[(Application.dataPath.Length + 1)..].Replace('/', '_').Replace('\\', '_').ToLower();
                    var folderInfo = new DirectoryInfo(folder);
                    var assetPaths = ScanCurrDirectory(folderInfo).ToArray();
                    buildBundleInfo.Add(assetGroup, bundleName, assetPaths);
                }
            }

            // 压缩选项详解
            // BuildAssetBundleOptions.None：使用LZMA算法压缩，压缩的包更小，但是加载时间更长。使用之前需要整体解压。一旦被解压，这个包会使用LZ4重新压缩。使用资源的时候不需要整体解压。在下载的时候可以使用LZMA算法，一旦它被下载了之后，它会使用LZ4算法保存到本地上。
            // BuildAssetBundleOptions.UncompressedAssetBundle：不压缩，包大，加载快
            // BuildAssetBundleOptions.ChunkBasedCompression：使用LZ4压缩，压缩率没有LZMA高，但是我们可以加载指定资源而不用解压全部
            // 参数一: bundle文件列表的输出路径
            // 参数二：生成bundle文件列表所需要的AssetBundleBuild对象数组（用来指导Unity生成哪些bundle文件，每个文件的名字以及文件里包含哪些资源）
            // 参数三：压缩选项BuildAssetBundleOptions.None默认是LZMA算法压缩
            // 参数四：生成哪个平台的bundle文件，即目标平台
            var path = GetStringByEnumType<BuildFolder>();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var buildTarget = build.BuildTargetInfo.CurrentBuildTarget;
            BuildPipeline.BuildAssetBundles(path, buildBundleInfo.AssetBundleBuilds, BuildAssetBundleOptions.None, buildTarget);

            // 添加依赖
            buildBundleInfo.CalculateDependencies();

            // 下载配置
            buildBundleInfo.DownloadConfig(path);
            
            // 保存配置
            buildBundleInfo.SaveConfigs();
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
            
            private readonly List<AssetBundleBuild> assetBundleBuilds = new();
            private readonly Dictionary<AssetGroup, LoadConfig> loadConfigDic = new();
            private readonly Dictionary<AssetGroup, DownloadConfig> downloadConfigDic = new();

            private readonly Dictionary<AssetGroup, List<string>> assetGroup2BundleNameList = new();
            private readonly Dictionary<string, BundleRef> bundleName2BundleRef = new();

            private readonly Dictionary<AssetGroup, List<string>> assetGroup2AssetPathList = new();
            private readonly Dictionary<string, AssetRef> assetPath2AssetRef = new();

            public AssetBundleBuild[] AssetBundleBuilds => assetBundleBuilds.ToArray();

            public BuildBundleInfo(IJsonProxy jsonProxy)
            {
                this.jsonProxy = jsonProxy;
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
                    loadConfigDic.Add(assetGroup, loadConfig = jsonProxy.ToObject<LoadConfig>((int)assetGroup, true));
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

            public void DownloadConfig(string bundleFolder)
            {
                foreach (var item in assetGroup2BundleNameList)
                {
                    downloadConfigDic.Add(item.Key, jsonProxy.ToObject<DownloadConfig>((int)item.Key, true));
                }
                foreach (var (bundleName, value) in bundleName2BundleRef)
                {
                    var assetGroup = value.AssetGroup;

                    var bundleInfo = new BundleInfo()
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

            public void SaveConfigs()
            {
                // 保存版本号
                jsonProxy.ToJson<VersionConfig>();

                // 空包也添加配置 =》解决拉取配置404问题
                var packAssetGroup = new List<AssetGroup>()
                {
                    AssetGroup.Local,
                    AssetGroup.Remote,
                    AssetGroup.Dll
                };

                foreach (var assetGroup in packAssetGroup)
                {
                    if (!assetGroup2BundleNameList.ContainsKey(assetGroup))
                    {
                        loadConfigDic.Add(assetGroup, jsonProxy.ToObject<LoadConfig>((int)assetGroup, true));
                        downloadConfigDic.Add(assetGroup, jsonProxy.ToObject<DownloadConfig>((int)assetGroup, true));
                        assetGroup2BundleNameList.Add(assetGroup, new List<string>());
                    }

                    jsonProxy.ToJson<LoadConfig>((int)assetGroup);
                    if (assetGroup == AssetGroup.Local) // 本地资源不需要下载配置
                    {
                        continue;
                    }
                    jsonProxy.ToJson<DownloadConfig>((int)assetGroup);
                }
            }

            // // 从临时目录移动至源目录
            // private void MoveFiles()
            // {
            //     // 生成源目录
            //     List<string> increaseSouceDataDirectoty = new List<string>()
            //     {
            //         packConfig.SourceDataLocalPathDirectory(m_VersionConfig.Version),
            //         packConfig.SourceDataRemotePathDirectory(m_VersionConfig.Version),
            //         packConfig.SourceDataDllPathDirectory(m_VersionConfig.Version),
            //     };
            //     for (int index = 0; index < increaseSouceDataDirectoty.Count; index++)
            //     {
            //         Editor.Utility.DirectoryCreateNew(increaseSouceDataDirectoty[index]);
            //     }
            //
            //     // 移动AB包至源目录
            //     Dictionary<Ninth.AssetGroup, Func<string, string>> locate2SourceDataPath = new Dictionary<Ninth.AssetGroup, Func<string, string>>()
            //     {
            //         { Ninth.AssetGroup.Local, (bundleName) => packConfig.BundleInLocalInSourceDataPath(m_VersionConfig.Version, bundleName) },
            //         { Ninth.AssetGroup.Remote, (bundleName) => packConfig.BundleInRemoteInSourceDataPath(m_VersionConfig.Version, bundleName) },
            //         { Ninth.AssetGroup.Dll, (bundleName) => packConfig.BundleInDllInSourceDataPath(m_VersionConfig.Version, bundleName) },
            //     };
            //     foreach (var item in m_AssetLocate2BundleNameList)
            //     {
            //         Ninth.AssetGroup assetGroup = item.Key;
            //         List<string> bundleList = item.Value;
            //         string move2SourceDataFolder = string.Empty;
            //         string copy2OutputFolder = string.Empty;
            //
            //         for (int i = 0; i < bundleList.Count; i++)
            //         {
            //             string bundleName = bundleList[i];
            //             File.Move(packConfig.BundleInTempInSourceDataPath(m_VersionConfig.Version, bundleName), locate2SourceDataPath[assetGroup].Invoke(bundleName).Log());
            //         }
            //     }
            //
            //     // 移动配置至源目录
            //     List<(string scrPath, string dstPath)> temp2SourceDataPath = new List<(string, string)>()
            //     {
            //         // 版号
            //         (packConfig.VersionInSourceDataTempPath(m_VersionConfig.Version), packConfig.VersionInSourceDataPath(m_VersionConfig.Version)),
            //
            //         // 加载配置
            //         (packConfig.LoadConfigInLocalInSourceDataTempPath(m_VersionConfig.Version), packConfig.LoadConfigInLocalInSourceDataPath(m_VersionConfig.Version)),
            //         (packConfig.LoadConfigInRemoteInSourceDataTempPath(m_VersionConfig.Version), packConfig.LoadConfigInRemoteInSourceDataPath(m_VersionConfig.Version)),
            //         (packConfig.LoadConfigInDllInSourceDataTempPath(m_VersionConfig.Version), packConfig.LoadConfigInDllInSourceDataPath(m_VersionConfig.Version)),
            //
            //         // 下载配置
            //         (packConfig.DownloadConfigInRemoteInSourceDataTempPath(m_VersionConfig.Version), packConfig.DownloadConfigInRemoteInSourceDataPath(m_VersionConfig.Version)),
            //         (packConfig.DownloadConfigInDllInSourceDataTempPath(m_VersionConfig.Version), packConfig.DownloadConfigInDllInSourceDataPath(m_VersionConfig.Version)),
            //     };
            //     for (int index = 0; index < temp2SourceDataPath.Count; index++)
            //     {
            //         string srcPath = temp2SourceDataPath[index].scrPath;
            //         string dstPath = temp2SourceDataPath[index].dstPath;
            //         File.Move(srcPath, dstPath);
            //     }
            // }
            //
            // // 删除无用文件
            // private void DeleteFiles()
            // {
            //     // 删除所有manifest文件
            //     string tempSourceOutPath = packConfig.SourceDataTempPathDirectory(m_VersionConfig.Version);
            //     FileInfo[] files = new DirectoryInfo(tempSourceOutPath).GetFiles();
            //     foreach (FileInfo file in files)
            //     {
            //         if (file.Name.EndsWith(".manifest"))
            //         {
            //             file.Delete();
            //         }
            //     }
            //
            //     // 删除文件夹文件
            //     File.Delete(tempSourceOutPath + "/" + new DirectoryInfo(tempSourceOutPath).Name);
            //     // 删除文件夹
            //     Directory.Delete(tempSourceOutPath);
            //     Debug.Log("删除临时文件夹成功！！");
            // }
            //
            // // 复制文件
            // private void CopyFiles(bool backPack)
            // {
            //     switch (PackEnvironment)
            //     {
            //         case Environment.LocalAb:
            //         {
            //             // 全部拷贝到streamingAssets
            //             CopyFilesSortOut(Ninth.AssetGroup.All);
            //             break;
            //         }
            //         case Environment.RemoteAb:
            //         {
            //             if (backPack)
            //             {
            //                 // 拷贝local资源到streamingAssets
            //                 CopyFilesSortOut(Ninth.AssetGroup.Local);
            //             }
            //
            //             break;
            //         }
            //     }
            //
            //     void CopyFilesSortOut(Ninth.AssetGroup assetLocate)
            //     {
            //         List<(string srcPath, string dstPath)> assetDirList = new List<(string, string)>();
            //
            //         if (assetLocate.HasFlag(Ninth.AssetGroup.Local))
            //         {
            //             assetDirList.Add((packConfig.SourceDataLocalPathDirectory(m_VersionConfig.Version), packConfig.CopyDataLocalPathDirectory()));
            //         }
            //
            //         if (assetLocate.HasFlag(Ninth.AssetGroup.Remote))
            //         {
            //             assetDirList.Add((packConfig.SourceDataRemotePathDirectory(m_VersionConfig.Version), packConfig.CopyDataRemotePathDirectory()));
            //         }
            //
            //         if (assetLocate.HasFlag(Ninth.AssetGroup.Dll))
            //         {
            //             assetDirList.Add((packConfig.SourceDataDllPathDirectory(m_VersionConfig.Version), packConfig.CopyDataDllPathDirectory()));
            //         }
            //
            //         for (int index = 0; index < assetDirList.Count; index++)
            //         {
            //             Editor.Utility.DirectoryCopy(assetDirList[index].srcPath, assetDirList[index].dstPath);
            //         }
            //
            //         File.Copy(packConfig.VersionInSourceDataPath(m_VersionConfig.Version), packConfig.VersionInCopyDataPath(), true);
            //     }
            // }
        }
    }
}