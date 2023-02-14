using System.Collections.Generic;
using UnityEditor;
using Ninth.HotUpdate;
using UnityEngine;
using System;
using HybridCLR.Editor;
using System.IO;
using HybridCLR.Editor.Commands;
using System.Linq;

namespace Ninth.Editor
{
    public sealed partial class BuildAssetsCommand
    {
        // 打包模式哦
        public static AssetMode m_PackAssetMode;

        // 版本配置
        private static VersionConfig m_VersionConfig;

        // AB包
        private static List<AssetBundleBuild> m_AssetBundleBuildList;

        // 加载配置
        // 整包 Local + Remote + Dll
        // eg!! Local 放在StreamingAssets文件夹下
        // 增量包 Remote + Dll
        private static Dictionary<AssetLocate, LoadConfig> m_LoadConfig;
        private static Dictionary<AssetLocate, List<string>> m_AssetLocate2AssetPathList;
        private static Dictionary<string, AssetRef> m_AssetPath2AssetRef;
        private static Dictionary<string, BundleRef> m_BundleName2BundleRef;

        // 下载配置 Remote + Dll
        // 与版本包对比需要热更的部分
        private static Dictionary<AssetLocate, DownloadConfig> m_DownloadConfig;
        private static Dictionary<AssetLocate, List<string>> m_AssetLocate2BundleNameList;

        static BuildAssetsCommand()
        {
            m_VersionConfig = new VersionConfig();
            m_AssetBundleBuildList = new List<AssetBundleBuild>();
            m_AssetLocate2BundleNameList = new Dictionary<AssetLocate, List<string>>();
            m_LoadConfig = new Dictionary<AssetLocate, LoadConfig>();
            m_AssetLocate2AssetPathList = new Dictionary<AssetLocate, List<string>>();
            m_AssetPath2AssetRef = new Dictionary<string, AssetRef>();
            m_BundleName2BundleRef = new Dictionary<string, BundleRef>();
            m_DownloadConfig = new Dictionary<AssetLocate, DownloadConfig>();
        }

        private static bool BuildPlayerRepackage(BuildTargetGroup buildTargetGroup, BuildTarget target, AssetMode assetMode, string newVersion)
        {
            bool result = BuildAllBundles(target, assetMode, newVersion);
            if (!result)
            {
                return result;
            }
            BuildPlayer(buildTargetGroup, target);
            return true;
        }

        private static bool BuildPlayer(BuildTargetGroup buildTargetGroup, BuildTarget target)
        {
            PackConfig.BuildPlatform = target.ToString();
            VersionConfig versionConfig = Utility.ToObject<VersionConfig>(PackConfig.BaseVersion());
            if(versionConfig == null)
            {
                UnityEngine.Debug.LogError($"在路径{PackConfig.BaseVersion()}下不存在版本配置文件, 请先打一个版本包！！");
                return false;
            }
            string outputPath = PackConfig.PlayerSourceDirectory(versionConfig.BaseVersion);
            var buildOptions = BuildOptions.None;
            string location = $"{outputPath}/HybridCLRTrial.exe";
            Debug.Log("====> Build App");
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
            {
                scenes = new string[] { "Assets/Scenes/main.unity" },
                locationPathName = location,
                options = buildOptions,
                target = target,
                targetGroup = buildTargetGroup,
            };

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.LogError("客户端生成失败，请检查！！");
                return false;
            }

#if UNITY_EDITOR
            Application.OpenURL($"file:///{location}");
#endif
            return true;
        }

        private static bool BuildAllBundles(BuildTarget target, AssetMode assetMode, string newVersion)
        {
            PackConfig.BuildPlatform = target.ToString();
            m_PackAssetMode = assetMode;

            bool result = Build(newVersion, target,
                (AssetBundleModuleConfig.LocalGroup, AssetLocate.Local),
                (AssetBundleModuleConfig.RemoteGroup, AssetLocate.Remote));
            if(!result)
            {
                return false;
            }
            AssetDatabase.Refresh();
            return true;
        }

        private static bool BuildHotUpdateBundles(BuildTarget target, AssetMode assetMode, string newVersion)
        {
            PackConfig.BuildPlatform = target.ToString();
            m_PackAssetMode = assetMode;

            bool result = Build(newVersion, target,
                (AssetBundleModuleConfig.RemoteGroup, AssetLocate.Remote));
            if(!result)
            {
                return false;
            }
            AssetDatabase.Refresh();
            return true;
        }

        /// <summary>
        /// 重置
        /// </summary>
        private static void Init()
        {
            m_VersionConfig = new VersionConfig();
            m_AssetBundleBuildList.Clear();
            m_AssetLocate2BundleNameList.Clear();
            m_LoadConfig.Clear();
            m_AssetLocate2AssetPathList.Clear();
            m_AssetPath2AssetRef.Clear();
            m_BundleName2BundleRef.Clear();
            m_DownloadConfig.Clear();
        }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="gAsset"></param>
        /// <param name="groupListArgs"></param>
        private static bool Build(string newVersion, BuildTarget target, params (List<string> groupList, AssetLocate assetLocate)[] groupListArgs)
        {
            try
            {
                Init();

                bool basePack = false;
                for (int index = 0; index < groupListArgs.Length; index++)
                {
                    if (groupListArgs[index].assetLocate == AssetLocate.Local)
                    {
                        basePack = true;
                        break;
                    }
                }
                if (basePack) // 版本包
                {
                    if (Directory.Exists(Application.streamingAssetsPath))
                    {
                        Directory.Delete(Application.streamingAssetsPath, true);
                    }
                    m_VersionConfig.BaseVersion = newVersion;
                    m_VersionConfig.Version = newVersion;
                }
                else
                {
                    VersionConfig versionConfig = Utility.ToObject<VersionConfig>(PackConfig.ApplyVersionInSourceDataPath());
                    if (versionConfig == null)
                    {
                        Debug.LogError($"路径{PackConfig.ApplyVersionInSourceDataPath()}下检测不到版本文件，请将版本文件复制到此处，或先打一个版本包并应用此版本!!");
                        return false;
                    }
                    string[] versionList = versionConfig.BaseVersion.Split(".");
                    string[] newVersionList = newVersion.Split(".");
                    if (versionList[0] != newVersionList[0] 
                        || versionList[1] != newVersionList[1]
                        || versionList[3] != newVersionList[3])
                    {
                        Debug.LogError("增量版本所依赖的基础版本的基础版号部分不一致，请检查！！");
                        return false;
                    }
                    m_VersionConfig.BaseVersion = versionConfig.BaseVersion;
                    m_VersionConfig.Version = newVersion;
                }

                // 临时源输出目录
                string tempSourceOutPath = PackConfig.SourceDataTempPathDirectory(m_VersionConfig.Version);
                // 生成
                Utility.DirectoryCreateNew(tempSourceOutPath);

                // 打包资源
                string gAssets = PackConfig.GAssets;
                for (int index = 0; index < groupListArgs.Length; index++)
                {
                    List<string> groupLst = groupListArgs[index].groupList;
                    AssetLocate assetLocate = groupListArgs[index].assetLocate;

                    for (int i = 0; i < groupLst.Count; i++)
                    {
                        string groupName = groupLst[i];
                        string groupPath = gAssets + "/" + groupName;
                        DirectoryInfo groupDir = new DirectoryInfo(groupPath);

                        ScanChildDireations(groupDir, assetLocate);
                    }
                }

                // 打包程序集
                BuildDll(target, tempSourceOutPath);

                // 压缩选项详解
                // BuildAssetBundleOptions.None：使用LZMA算法压缩，压缩的包更小，但是加载时间更长。使用之前需要整体解压。一旦被解压，这个包会使用LZ4重新压缩。使用资源的时候不需要整体解压。在下载的时候可以使用LZMA算法，一旦它被下载了之后，它会使用LZ4算法保存到本地上。
                // BuildAssetBundleOptions.UncompressedAssetBundle：不压缩，包大，加载快
                // BuildAssetBundleOptions.ChunkBasedCompression：使用LZ4压缩，压缩率没有LZMA高，但是我们可以加载指定资源而不用解压全部
                // 参数一: bundle文件列表的输出路径
                // 参数二：生成bundle文件列表所需要的AssetBundleBuild对象数组（用来指导Unity生成哪些bundle文件，每个文件的名字以及文件里包含哪些资源）
                // 参数三：压缩选项BuildAssetBundleOptions.None默认是LZMA算法压缩
                // 参数四：生成哪个平台的bundle文件，即目标平台
                BuildPipeline.BuildAssetBundles(tempSourceOutPath, m_AssetBundleBuildList.ToArray(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

                // 添加依赖
                CalculateDependencies();

                // 添加下载配置
                DownloadConfig();

                // 保存配置
                SaveConfigs();

                // 从临时目录移动至源目录
                MoveFiles();

                // 删除无用文件
                DeleteFiles();

                // 拷贝文件
                CopyFiles(basePack);

                return true;
            }
            finally
            {
                // 将明文的协议文件拷贝回来
            }
        }

        /// <summary>
        /// 根据指定的文件夹
        /// 1. 将这个文件夹下的所有一级子文件打成一个AssetBundle
        /// 2. 并且递归遍历这个文件夹下的所有子文件夹
        /// </summary>
        /// <param name="directoryInfo"></param>
        public static void ScanChildDireations(DirectoryInfo directoryInfo, AssetLocate assetLocate)
        {
            // 收集当前路径下的文件 把它们打成一个AB包
            ScanCurrDirectory(directoryInfo, assetLocate);

            // 遍历当前路径下的子文件夹
            DirectoryInfo[] dirs = directoryInfo.GetDirectories();

            // 子文件夹递归打包
            foreach (DirectoryInfo info in dirs)
            {
                ScanChildDireations(info, assetLocate);
            }
        }

        /// <summary>
        /// 遍历当前路径下的文件 把它们打成一个AB包
        /// </summary>
        /// <param name="directoryInfo"></param>
        private static void ScanCurrDirectory(DirectoryInfo directoryInfo, AssetLocate assetLocate)
        {
            List<string> assetPaths = new List<string>();
            FileInfo[] fileInfoList = directoryInfo.GetFiles();

            foreach (FileInfo fileInfo in fileInfoList)
            {
                if (fileInfo.FullName.EndsWith(".meta") || fileInfo.FullName.EndsWith(".DS_Store"))
                {
                    continue;
                }
                // assetName的格式类似 "Assets/GAssets/Launch/Sphere.prefab"
                string assetPath = fileInfo.FullName.Substring(Application.dataPath.Length - "Assets".Length).Replace('\\', '/');
                assetPaths.Add(assetPath);
            }

            if (assetPaths.Count > 0)
            {
                // 格式类似 gassets_Launch
                string bundleName = directoryInfo.FullName.Substring(Application.dataPath.Length + 1).Replace('\\', '_').ToLower();
                bundleName = bundleName.Replace('/', '_').ToLower();
                SortOut(assetLocate, bundleName, assetPaths);
            }
        }

        // 分类
        private static void SortOut(AssetLocate assetLocate, string bundleName, List<string> assetPaths)
        {
            if (!m_AssetLocate2BundleNameList.ContainsKey(assetLocate))
            {
                m_AssetLocate2BundleNameList.Add(assetLocate, new List<string>());
            }
            m_AssetLocate2BundleNameList[assetLocate].Add(bundleName);

            // 配置Bundle
            AssetBundleBuild build = new AssetBundleBuild()
            {
                assetBundleName = bundleName,
                assetNames = assetPaths.ToArray()
            };
            m_AssetBundleBuildList.Add(build);

            // 配置Bundle引用
            BundleRef bundleRef = new BundleRef()
            {
                BundleName = bundleName,
                AssetLocate = assetLocate
            };
            m_BundleName2BundleRef.Add(bundleName, bundleRef);

            for (int i = 0; i < assetPaths.Count; i++)
            {
                string assetPath = assetPaths[i];

                if (!m_AssetLocate2AssetPathList.ContainsKey(assetLocate))
                {
                    m_AssetLocate2AssetPathList.Add(assetLocate, new List<string>());
                }
                m_AssetLocate2AssetPathList[assetLocate].Add(assetPath);

                // 资源
                AssetRef assetRef = new AssetRef()
                {
                    AssetPath = assetPath,
                    BundleRef = bundleRef
                };
                m_AssetPath2AssetRef.Add(assetPath, assetRef);

                // 配置加载配置
                if (!m_LoadConfig.ContainsKey(assetLocate))
                {
                    m_LoadConfig.Add(assetLocate, new LoadConfig()
                    {
                        AssetRefList = new List<AssetRef>()
                    });
                }
                m_LoadConfig[assetLocate].AssetRefList.Add(assetRef);
            }
        }

        /// <summary>
        /// 下载配置
        /// </summary>
        public static void DownloadConfig()
        {
            foreach (var item in m_AssetLocate2BundleNameList)
            {
                m_DownloadConfig.Add(item.Key, new DownloadConfig());
            }
            foreach (var item in m_BundleName2BundleRef)
            {
                string bundleName = item.Key;
                AssetLocate assetLocate = item.Value.AssetLocate;

                BundleInfo bundleInfo = new BundleInfo()
                {
                    BundleName = bundleName,
                };
                string bundleFilePath = PackConfig.BundleInTempInSourceDataPath(m_VersionConfig.Version, bundleName);
                using (FileStream stream = File.OpenRead(bundleFilePath))
                {
                    bundleInfo.Crc = Utility.GetCRC32Hash(stream);

                    bundleInfo.Size = (int)stream.Length;
                }
                m_DownloadConfig[assetLocate].BundleInfos.Add(bundleName, bundleInfo);
            }
        }

        /// <summary>
        /// 计算每个资源所依赖的ab包文件列表
        /// </summary>
        private static void CalculateDependencies()
        {
            foreach (string asset in m_AssetPath2AssetRef.Keys)
            {
                // 这个资源自己所在的bundle
                string assetBundle = m_AssetPath2AssetRef[asset].BundleRef.BundleName;
                string[] dependencies = AssetDatabase.GetDependencies(asset);

                List<string> assetList = new List<string>();
                if (dependencies != null && dependencies.Length > 0)
                {
                    foreach (string oneAsset in dependencies)
                    {
                        if (oneAsset == asset || oneAsset.EndsWith(".cs"))
                        {
                            continue;
                        }
                        assetList.Add(oneAsset);
                    }
                }
                if (assetList.Count > 0)
                {
                    List<string> abList = new List<string>();
                    foreach (string oneAsset in assetList)
                    {
                        bool result = m_AssetPath2AssetRef.TryGetValue(oneAsset, out AssetRef assetRef);
                        if (result == true)
                        {
                            string bundle = assetRef.BundleRef.BundleName;
                            if (bundle != assetBundle)
                            {
                                if (abList.Contains(bundle) == false)
                                {
                                    abList.Add(bundle);
                                }
                            }
                        }
                    }
                    m_AssetPath2AssetRef[asset].Dependencies = new List<BundleRef>();
                    for (int index = 0; index < abList.Count; index++)
                    {
                        m_AssetPath2AssetRef[asset].Dependencies.Add(m_BundleName2BundleRef[abList[index]]);
                    }
                }
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private static void SaveConfigs()
        {
            // 保存版本号
            Utility.ToJson(m_VersionConfig, PackConfig.VersionInSourceDataTempPath(m_VersionConfig.Version));

            // 空包也添加配置 =》解决拉取配置404问题
            List<AssetLocate> packAssetLocate = new List<AssetLocate>()
            {
                AssetLocate.Local,
                AssetLocate.Remote,
                AssetLocate.Dll
            };

            // 保存加载配置位置
            List<string> saveLoadConfigTempPath = new List<string>()
            {
                 PackConfig.LoadConfigInLocalInSourceDataTempPath(m_VersionConfig.Version),
                 PackConfig.LoadConfigInRemoteInSourceDataTempPath(m_VersionConfig.Version),
                 PackConfig.LoadConfigInDllInSourceDataTempPath(m_VersionConfig.Version)
            };

            // 保存下载配置位置
            List<string> saveDownloadConfigTempPath = new List<string>()
            {
                null,  // 本地资源不需要下载配置 
                PackConfig.DownloadConfigInRemoteInSourceDataTempPath(m_VersionConfig.Version),
                PackConfig.DownloadConfigInDllInSourceDataTempPath(m_VersionConfig.Version),
            };

            for (int index = 0; index < packAssetLocate.Count; index++)
            {
                AssetLocate assetLocate = packAssetLocate[index];

                if (!m_AssetLocate2BundleNameList.ContainsKey(assetLocate))
                {
                    m_LoadConfig.Add(assetLocate, new LoadConfig());
                    m_DownloadConfig.Add(assetLocate, new DownloadConfig());
                    m_AssetLocate2BundleNameList.Add(assetLocate, new List<string>());
                }
                Utility.ToJson(m_LoadConfig[assetLocate], saveLoadConfigTempPath[index]);
                Utility.ToJson(m_DownloadConfig[assetLocate], saveDownloadConfigTempPath[index]);
            }
        }

        // 从临时目录移动至源目录
        private static void MoveFiles()
        {
            // 生成源目录
            List<string> increaseSouceDataDirectoty = new List<string>()
            {
                PackConfig.SourceDataLocalPathDirectory(m_VersionConfig.Version),
                PackConfig.SourceDataRemotePathDirectory(m_VersionConfig.Version),
                PackConfig.SourceDataDllPathDirectory(m_VersionConfig.Version),
            };
            for (int index = 0; index < increaseSouceDataDirectoty.Count; index++)
            {
                Utility.DirectoryCreateNew(increaseSouceDataDirectoty[index]);
            }

            // 移动AB包至源目录
            Dictionary<AssetLocate, Func<string, string>> locate2SourceDataPath = new Dictionary<AssetLocate, Func<string, string>>()
            {
                { AssetLocate.Local, (bundleName) => PackConfig.BundleInLocalInSourceDataPath(m_VersionConfig.Version, bundleName) },
                { AssetLocate.Remote, (bundleName) => PackConfig.BundleInRemoteInSourceDataPath(m_VersionConfig.Version, bundleName) },
                { AssetLocate.Dll, (bundleName) => PackConfig.BundleInDllInSourceDataPath(m_VersionConfig.Version, bundleName) },
            };
            foreach(var item in m_AssetLocate2BundleNameList)
            {
                AssetLocate assetLocate = item.Key;
                List<string> bundleList = item.Value;
                string move2SourceDataFolder = string.Empty;
                string copy2OutputFolder = string.Empty;

                for (int i = 0; i < bundleList.Count; i++)
                {
                    string bundleName = bundleList[i];
                    File.Move(PackConfig.BundleInTempInSourceDataPath(m_VersionConfig.Version, bundleName), locate2SourceDataPath[assetLocate].Invoke(bundleName).Log());
                }
            }

            // 移动配置至源目录
            List<(string scrPath, string dstPath)> temp2SourceDataPath = new List<(string, string)>()
            {
                // 版号
                (PackConfig.VersionInSourceDataTempPath(m_VersionConfig.Version), PackConfig.VersionInSourceDataPath(m_VersionConfig.Version)),

                // 加载配置
                (PackConfig.LoadConfigInLocalInSourceDataTempPath(m_VersionConfig.Version), PackConfig.LoadConfigInLocalInSourceDataPath(m_VersionConfig.Version)),
                (PackConfig.LoadConfigInRemoteInSourceDataTempPath(m_VersionConfig.Version), PackConfig.LoadConfigInRemoteInSourceDataPath(m_VersionConfig.Version)),
                (PackConfig.LoadConfigInDllInSourceDataTempPath(m_VersionConfig.Version), PackConfig.LoadConfigInDllInSourceDataPath(m_VersionConfig.Version)),
                
                // 下载配置
                (PackConfig.DownloadConfigInRemoteInSourceDataTempPath(m_VersionConfig.Version), PackConfig.DownloadConfigInRemoteInSourceDataPath(m_VersionConfig.Version)),
                (PackConfig.DownloadConfigInDllInSourceDataTempPath(m_VersionConfig.Version), PackConfig.DownloadConfigInDllInSourceDataPath(m_VersionConfig.Version)),
            };
            for (int index = 0; index < temp2SourceDataPath.Count; index++)
            {
                string srcPath = temp2SourceDataPath[index].scrPath;
                string dstPath = temp2SourceDataPath[index].dstPath;
                File.Move(srcPath, dstPath);
            }
        }

        // 删除无用文件
        private static void DeleteFiles()
        {
            // 删除所有manifest文件
            string tempSourceOutPath = PackConfig.SourceDataTempPathDirectory(m_VersionConfig.Version);
            FileInfo[] files = new DirectoryInfo(tempSourceOutPath).GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Name.EndsWith(".manifest"))
                {
                    file.Delete();
                }
            }
            // 删除文件夹文件
            File.Delete(tempSourceOutPath + "/" + new DirectoryInfo(tempSourceOutPath).Name);
            // 删除文件夹
            Directory.Delete(tempSourceOutPath);
            Debug.Log("删除临时文件夹成功！！");
        }

        // 复制文件
        private static void CopyFiles(bool backPack)
        {
            switch(m_PackAssetMode)
            {
                case AssetMode.LocalAB:
                    {
                        // 全部拷贝到streamingAssets
                        CopyFilesSortOut(AssetLocate.Max);
                        break;
                    }
                case AssetMode.RemoteAB:
                    {
                        if(backPack)
                        {
                            // 拷贝local资源到streamingAssets
                            CopyFilesSortOut(AssetLocate.Local);
                        }
                        break;
                    }
            }

            void CopyFilesSortOut(AssetLocate assetLocate)
            {
                List<(string srcPath, string dstPath)> assetDirList = new List<(string, string)>();

                if (assetLocate.HasFlag(AssetLocate.Local))
                {
                    assetDirList.Add((PackConfig.SourceDataLocalPathDirectory(m_VersionConfig.Version), PackConfig.CopyDataLocalPathDirectory()));
                }
                if(assetLocate.HasFlag(AssetLocate.Remote))
                {
                    assetDirList.Add((PackConfig.SourceDataRemotePathDirectory(m_VersionConfig.Version), PackConfig.CopyDataRemotePathDirectory()));
                }
                if(assetLocate.HasFlag(AssetLocate.Dll))
                {
                    assetDirList.Add((PackConfig.SourceDataDllPathDirectory(m_VersionConfig.Version), PackConfig.CopyDataDllPathDirectory()));
                }
                for (int index = 0; index < assetDirList.Count; index++)
                {
                    Utility.DirectoryCopy(assetDirList[index].srcPath, assetDirList[index].dstPath);
                }
                File.Copy(PackConfig.VersionInSourceDataPath(m_VersionConfig.Version), PackConfig.VersionInCopyDataPath(), true);
            }
        }

        // 远端应用
        public static void RemoteApply()
        {
            // 将版本配置拷贝至外目录
            File.Copy(PackConfig.VersionInSourceDataPath(m_VersionConfig.Version), PackConfig.ApplyVersionInSourceDataPath(), true);
        }
    }
}
