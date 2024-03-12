// using HybridCLR.Editor;
// using HybridCLR.Editor.Commands;
// using Ninth.HotUpdate;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using UnityEditor;
// using UnityEngine;
//
// namespace Ninth.Editor.Window
// {
//     public class BuildAssetsCommand
//     {
//         public void BuildDll(BuildTarget target, string dstDir)
//         {
//             ScanAssembly(target, dstDir);
//         }
//
//         /// <summary>
//         /// 打包程序集
//         /// </summary>
//         /// <param name="directoryInfo"></param>
//         private void ScanAssembly(BuildTarget target, string dstDir)
//         {
//             CompileDllCommand.CompileDll(target);
//             CopyAOTAssemblies2SourceDataTempPath(target, dstDir);
//             CopyHotUpdateAssemblies2SourceDataTempPath(target, dstDir);
//             return;
//
//             void CopyAOTAssemblies2SourceDataTempPath(BuildTarget target, string dstDir)
//             {
//                 var aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
//                 var aotAssembliesDstDir = dstDir;
//
//                 foreach (var dll in LoadDll.AOTMetaAssemblyNames)
//                 {
//                     var srcDllPath = $"{aotAssembliesSrcDir}/{dll}";
//                     if (!File.Exists(srcDllPath))
//                     {
//                         Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
//                         continue;
//                     }
//                     var dllBytesPath = $"{aotAssembliesDstDir}/{dll}.bytes";
//                     File.Copy(srcDllPath, dllBytesPath, true);
//                     Debug.Log($"[CopyAOTAssembliesToStreamingAssets] copy AOT dll {srcDllPath} -> {dllBytesPath}");
//                     DllSortOut($"{dll}.bytes");
//                 }
//             }
//
//             void CopyHotUpdateAssemblies2SourceDataTempPath(BuildTarget target, string dstDir)
//             {
//                 var hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
//                 var hotfixAssembliesDstDir = dstDir;
//                 foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesExcludePreserved)
//                 {
//                     var dllPath = $"{hotfixDllSrcDir}/{dll}";
//                     var dllBytesPath = $"{hotfixAssembliesDstDir}/{dll}.bytes";
//                     File.Copy(dllPath, dllBytesPath, true);
//                     Debug.Log($"[CopyHotUpdateAssembliesToStreamingAssets] copy hotfix dll {dllPath} -> {dllBytesPath}");
//                     DllSortOut($"{dll}.bytes");
//                 }
//             }
//         }
//
//         private void DllSortOut(string dllName)
//         {
//             var assetGroup = AssetGroup.Dll;
//             if (!m_AssetLocate2BundleNameList.ContainsKey(assetGroup))
//             {
//                 m_AssetLocate2BundleNameList.Add(assetGroup, new List<string>());
//             }
//             m_AssetLocate2BundleNameList[assetGroup].Add(dllName);
//
//             // 配置Bundle引用
//             var bundleRef = new BundleRef
//             {
//                 BundleName = dllName,
//                 AssetGroup = assetGroup
//             };
//             m_BundleName2BundleRef.Add(dllName, bundleRef);
//
//             // 资源
//             var assetRef = new AssetRef
//             {
//                 AssetPath = dllName,
//                 BundleRef = bundleRef
//             };
//
//             // 配置加载配置
//             if (!m_LoadConfig.ContainsKey(assetGroup))
//             {
//                 m_LoadConfig.Add(assetGroup, new LoadConfig()
//                 {
//                     AssetRefList = new List<AssetRef>()
//                 });
//             }
//             m_LoadConfig[assetGroup].AssetRefList.Add(assetRef);
//         }
//     }
// }
