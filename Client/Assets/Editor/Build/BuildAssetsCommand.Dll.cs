//using HybridCLR.Editor;
//using HybridCLR.Editor.Commands;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;

//namespace Ninth.Editor
//{
//    public sealed partial class BuildAssetsCommand
//    {
//        public static string ToRelativeAssetPath(string s)
//        {
//            return s.Substring(s.IndexOf("Assets/"));
//        }

//        public static void BuildDllByTarget(BuildTarget target)
//        {
//            ScanAssembly(HybridCLR.Editor.BuildAssetsCommand.GetAssetBundleTempDirByTarget(target), target);
//        }

//        /// <summary>
//        /// 打包程序集
//        /// </summary>
//        /// <param name="directoryInfo"></param>
//        private static void ScanAssembly(string assetDirSourceDataDir, BuildTarget target)
//        {
//            CompileDllCommand.CompileDll(target);

//            List<string> notSceneAssets = new List<string>();

//            string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);

//            foreach (var dll in AssemblyConfig.HotUpdateAssemblies)
//            {
//                string dllPath = $"{hotfixDllSrcDir}/{dll}";

//                string dllBytesPath = $"{assetDirSourceDataDir}/{dll}.bytes";

//                File.Copy(dllPath, dllBytesPath, true);

//                notSceneAssets.Add(dllBytesPath);
//            }
//            string aotDllDir = BuildConfig.GetAssembliesPostIl2CppStripDir(target);

//            foreach (var dll in AssemblyConfig.AOTMetaAssemblies)
//            {
//                string dllPath = $"{aotDllDir}/{dll}";

//                if (!File.Exists(dllPath))
//                {
//                    Debug.LogError($"ab中添加AOT补充元数据dll:{dllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");

//                    continue;
//                }
//                string dllBytesPath = $"{assetDirSourceDataDir}/{dll}.bytes";

//                File.Copy(dllPath, dllBytesPath, true);

//                notSceneAssets.Add(dllBytesPath);
//            }

//            // 配置bundle数据
//            string bundleName = NameConfig.DllsBundleName;

//            AssetLocate assetLocate = AssetLocate.Dll;

//            // 分类
//            m_AssetLocate2BundleNameList.AddAndDefine(assetLocate, bundleName);

//            // Bundle
//            AssetBundleBuild build = new AssetBundleBuild()
//            {
//                assetBundleName = bundleName,

//                assetNames = notSceneAssets.Select(s => ToRelativeAssetPath(s)).ToArray(),
//            };
//            m_AssetBundleBuildList.Add(build);

//            // 加载配置 -- 资源所在的AB包
//            BundleRef bundleRef = new BundleRef()
//            {
//                BundleName = bundleName,

//                AssetLocate = assetLocate
//            };
//            m_BundleName2BundleRef.Add(bundleName, bundleRef);

//            string assetPath = bundleName;

//            m_AssetLocate2AssetPathList.AddAndDefine(assetLocate, assetPath);

//            // 资源
//            AssetRef assetRef = new AssetRef()
//            {
//                AssetPath = assetPath,

//                BundleRef = bundleRef
//            };
//            m_AssetPath2AssetRef.Add(assetPath, assetRef);

//            // 配置
//            if (!m_LoadConfig.ContainsKey(assetLocate))
//            {
//                m_LoadConfig.Add(assetLocate, new LoadConfig()
//                {
//                    AssetRefList = new List<AssetRef>()
//                });
//            }
//            m_LoadConfig[assetLocate].AssetRefList.Add(assetRef);
//        }
//    }
//}
