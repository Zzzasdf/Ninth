using System.Collections.Generic;
using UnityEditor;
using Ninth.HotUpdate;
using UnityEngine;
using System;
using HybridCLR.Editor;
using System.IO;
using HybridCLR.Editor.Commands;

namespace Ninth.Editor
{
    public sealed partial class BuildAssetsCommand
    {
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

        private static void BuildPlayer(BuildTargetGroup buildTargetGroup, BuildTarget target, AssetMode assetMode)
        {
            BuildAllBundles(target, assetMode);

            string outputPath = PackConfig.OutputPlayerDirectory(m_VersionConfig.Version);
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
                Debug.LogError("打包失败");
                return;
            }

#if UNITY_EDITOR
            Application.OpenURL($"file:///{location}");
#endif
        }

        private static void BuildAllBundles(BuildTarget target, AssetMode assetMode)
        {
            PackConfig.BuildPlatform = target.ToString();
            PackConfig.PackAssetMode = assetMode;

            Build(target,
                (AssetBundleModuleConfig.LocalGroup, AssetLocate.Local),
                (AssetBundleModuleConfig.RemoteGroup, AssetLocate.Remote));

            AssetDatabase.Refresh();
        }

        private static void BuildHotUpdateBundles(BuildTarget target, AssetMode assetMode)
        {
            PackConfig.BuildPlatform = target.ToString();
            PackConfig.PackAssetMode = assetMode;

            Build(target,
                (AssetBundleModuleConfig.RemoteGroup, AssetLocate.Remote));

            AssetDatabase.Refresh();
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
        private static void Build(BuildTarget target, params (List<string> groupList, AssetLocate assetLocate)[] groupListArgs)
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
                    m_VersionConfig.BaseVersion = GenerateVersion();
                    m_VersionConfig.Version = m_VersionConfig.BaseVersion;
                }
                else
                {
                    VersionConfig versionConfig = Utility.ToObject<VersionConfig>(PackConfig.VersionInOutputPath());
                    if (versionConfig == null)
                    {
                        Debug.LogError("需要先打一个版本包!!");
                        return;
                    }
                    m_VersionConfig.BaseVersion = versionConfig.BaseVersion;
                    m_VersionConfig.Version = GenerateVersion();
                }

                // 临时源输出目录
                string tempSourceOutPath = PackConfig.SourceDataTempPathDirectory(m_VersionConfig.Version);
                // 生成
                Utility.CreateNewDirectory(tempSourceOutPath);

                // 打包资源
                string gAssets = PackConfig.GAssets();
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

                // 删除无用文件
                DeleteManifest(tempSourceOutPath);

                File.Delete(tempSourceOutPath + "/" + new DirectoryInfo(tempSourceOutPath).Name);

                // 保存配置
                SaveConfigAndCopyBundles();
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
        /// 创建版号
        /// </summary>
        /// <returns></returns>
        private static long GenerateVersion()
        {
            DateTime date = DateTime.Now;
            long version = 0;
            version += (long)date.Year * 10000000000;
            version += (long)date.Month * 100000000;
            version += (long)date.Day * 1000000;
            version += (long)date.Hour * 10000;
            version += (long)date.Minute * 100;
            version += (long)date.Second;
            return version;
        }

        /// <summary>
        /// 删除Unity帮我们生成的.manifest文件，我们是不需要的
        /// </summary>
        /// <param name="modulePackConfig">模块对应的ab文件输出路径</param>
        private static void DeleteManifest(string modulePackConfig)
        {
            FileInfo[] files = new DirectoryInfo(modulePackConfig).GetFiles();

            foreach (FileInfo file in files)
            {
                if (file.Name.EndsWith(".manifest"))
                {
                    file.Delete();
                }
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private static void SaveConfigAndCopyBundles()
        {
            // 保存版本号
            Utility.ToJson(m_VersionConfig, PackConfig.VersionInSourceDataPath(m_VersionConfig.Version));

            // 空包也添加配置 =》解决拉取配置404问题
            List<AssetLocate> nullPackAssetLocate = new List<AssetLocate>()
            {
                AssetLocate.Remote,
                AssetLocate.Dll
            };

            for (int index = 0; index < nullPackAssetLocate.Count; index++)
            {
                AssetLocate assetLocate = nullPackAssetLocate[index];

                if (!m_AssetLocate2BundleNameList.ContainsKey(assetLocate))
                {
                    m_LoadConfig.Add(assetLocate, new LoadConfig());
                    m_DownloadConfig.Add(assetLocate, new DownloadConfig());
                    m_AssetLocate2BundleNameList.Add(assetLocate, new List<string>());
                }
            }

            // 复制本地的AB包
            foreach (var item in m_AssetLocate2BundleNameList)
            {
                AssetLocate assetLocate = item.Key;
                List<string> bundleList = item.Value;
                string move2SourceDataFolder = string.Empty;
                string copy2OutputFolder = string.Empty;

                if (assetLocate == AssetLocate.Local)
                {
                    move2SourceDataFolder = PackConfig.SourceDataLocalPathDirectory(m_VersionConfig.Version);
                    Utility.CreateNewDirectory(move2SourceDataFolder);

                    copy2OutputFolder = PackConfig.OutputLocalPathDirectory(m_VersionConfig.Version);
                    Utility.CreateNewDirectory(copy2OutputFolder);

                    // 保存加载配置
                    Utility.ToJson(m_LoadConfig[assetLocate], PackConfig.LoadConfigInLocalInSourceDataPath(m_VersionConfig.Version));

                    // Source -> Target
                    for (int i = 0; i < bundleList.Count; i++)
                    {
                        string bundleName = bundleList[i];

                        // ab包移动到源目录
                        File.Move(PackConfig.BundleInTempInSourceDataPath(m_VersionConfig.Version, bundleName), PackConfig.BundleInLocalInSourceDataPath(m_VersionConfig.Version, bundleName));

                        // 拷贝一份到使用目录
                        File.Copy(PackConfig.BundleInLocalInSourceDataPath(m_VersionConfig.Version, bundleName), PackConfig.BundleInLocalInOutputPath(bundleName));
                    }
                    if (File.Exists(PackConfig.BaseVersion()))
                    {
                        File.Delete(PackConfig.BaseVersion());
                    }
                    File.Copy(PackConfig.VersionInSourceDataPath(m_VersionConfig.Version), PackConfig.BaseVersion());

                    if (File.Exists(PackConfig.LoadConfigInLocalInOutputPath(m_VersionConfig.Version)))
                    {
                        File.Delete(PackConfig.LoadConfigInLocalInOutputPath(m_VersionConfig.Version));
                    }
                    File.Copy(PackConfig.LoadConfigInLocalInSourceDataPath(m_VersionConfig.Version), PackConfig.LoadConfigInLocalInOutputPath(m_VersionConfig.Version));
                }
                else if (assetLocate == AssetLocate.Remote)
                {
                    move2SourceDataFolder = PackConfig.SourceDataRemotePathDirectory(m_VersionConfig.Version);
                    Utility.CreateNewDirectory(move2SourceDataFolder);

                    copy2OutputFolder = PackConfig.OutputRemotePathDirectory(m_VersionConfig.Version);
                    Utility.CreateNewDirectory(copy2OutputFolder);

                    // 保存加载下载配置
                    Utility.ToJson(m_LoadConfig[assetLocate], PackConfig.LoadConfigInRemoteInSourceDataPath(m_VersionConfig.Version));
                    Utility.ToJson(m_DownloadConfig[assetLocate], PackConfig.DownloadConfigInRemoteInSourceDataPath(m_VersionConfig.Version));

                    // Source -> Target
                    for (int i = 0; i < bundleList.Count; i++)
                    {
                        string bundleName = bundleList[i];

                        // ab包移动到源目录
                        File.Move(PackConfig.BundleInTempInSourceDataPath(m_VersionConfig.Version, bundleName), PackConfig.BundleInRemoteInSourceDataPath(m_VersionConfig.Version, bundleName));

                        // 拷贝一份到使用目录
                        File.Copy(PackConfig.BundleInRemoteInSourceDataPath(m_VersionConfig.Version, bundleName), PackConfig.BundleInRemoteInOutputPath(m_VersionConfig.Version, bundleName));
                    }
                    if (File.Exists(PackConfig.VersionInOutputPath()))
                    {
                        File.Delete(PackConfig.VersionInOutputPath());
                    }
                    File.Copy(PackConfig.VersionInSourceDataPath(m_VersionConfig.Version), PackConfig.VersionInOutputPath());
                    File.Copy(PackConfig.LoadConfigInRemoteInSourceDataPath(m_VersionConfig.Version), PackConfig.LoadConfigInRemoteInOutputPath(m_VersionConfig.Version));
                    File.Copy(PackConfig.DownloadConfigInRemoteInSourceDataPath(m_VersionConfig.Version), PackConfig.DownloadConfigInRemoteInOutputPath(m_VersionConfig.Version));
                }
                else if (assetLocate == AssetLocate.Dll)
                {
                    move2SourceDataFolder = PackConfig.SourceDataDllPathDirectory(m_VersionConfig.Version);
                    Utility.CreateNewDirectory(move2SourceDataFolder);

                    copy2OutputFolder = PackConfig.OutputDllPathDirectory(m_VersionConfig.Version);
                    Utility.CreateNewDirectory(copy2OutputFolder);

                    // 保存加载下载配置
                    Utility.ToJson(m_LoadConfig[assetLocate], PackConfig.LoadConfigInDllInSourceDataPath(m_VersionConfig.Version));
                    Utility.ToJson(m_DownloadConfig[assetLocate], PackConfig.DownloadConfigInDllInSourceDataPath(m_VersionConfig.Version));

                    // Source -> Target
                    for (int i = 0; i < bundleList.Count; i++)
                    {
                        string bundleName = bundleList[i];

                        // ab包移动到源目录
                        File.Move(PackConfig.BundleInTempInSourceDataPath(m_VersionConfig.Version, bundleName), PackConfig.BundleInDllInSourceDataPath(m_VersionConfig.Version, bundleName));

                        // 拷贝一份到使用目录
                        File.Copy(PackConfig.BundleInDllInSourceDataPath(m_VersionConfig.Version, bundleName), PackConfig.BundleInDllInOutputPath(m_VersionConfig.Version, bundleName));
                    }
                    if (File.Exists(PackConfig.VersionInOutputPath()))
                    {
                        File.Delete(PackConfig.VersionInOutputPath());
                    }
                    File.Copy(PackConfig.VersionInSourceDataPath(m_VersionConfig.Version), PackConfig.VersionInOutputPath());
                    File.Copy(PackConfig.LoadConfigInDllInSourceDataPath(m_VersionConfig.Version), PackConfig.LoadConfigInDllInOutputPath(m_VersionConfig.Version));
                    File.Copy(PackConfig.DownloadConfigInDllInSourceDataPath(m_VersionConfig.Version), PackConfig.DownloadConfigInDllInOutputPath(m_VersionConfig.Version));
                }
            }
            // 删除临时文件
            Directory.Delete(PackConfig.SourceDataTempPathDirectory(m_VersionConfig.Version));
        }
    }
}
