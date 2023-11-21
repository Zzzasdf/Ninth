using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using Ninth.HotUpdate;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildAssetsCommand
    {
        public string ToRelativeAssetPath(string s)
        {
            return s.Substring(s.IndexOf("Assets/"));
        }

        public void BuildDll(BuildTarget target, string dstDir)
        {
            ScanAssembly(target, dstDir);
        }

        /// <summary>
        /// 打包程序集
        /// </summary>
        /// <param name="directoryInfo"></param>
        private void ScanAssembly(BuildTarget target, string dstDir)
        {
            CompileDllCommand.CompileDll(target);
            CopyAOTAssemblies2SourceDataTempPath(dstDir);
            CopyHotUpdateAssemblies2SourceDataTempPath(dstDir);
        }

        public void CopyAOTAssemblies2SourceDataTempPath(string dstDir)
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            string aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
            string aotAssembliesDstDir = dstDir;

            foreach (var dll in LoadDll.AOTMetaAssemblyNames)
            {
                string srcDllPath = $"{aotAssembliesSrcDir}/{dll}";
                if (!File.Exists(srcDllPath))
                {
                    Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                    continue;
                }
                string dllBytesPath = $"{aotAssembliesDstDir}/{dll}.bytes";
                File.Copy(srcDllPath, dllBytesPath, true);
                Debug.Log($"[CopyAOTAssembliesToStreamingAssets] copy AOT dll {srcDllPath} -> {dllBytesPath}");
                DllSortOut($"{dll}.bytes");
            }
        }

        public void CopyHotUpdateAssemblies2SourceDataTempPath(string dstDir)
        {
            var target = EditorUserBuildSettings.activeBuildTarget;

            string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
            string hotfixAssembliesDstDir = dstDir;

            foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesExcludePreserved)
            {
                string dllPath = $"{hotfixDllSrcDir}/{dll}";
                string dllBytesPath = $"{hotfixAssembliesDstDir}/{dll}.bytes";
                File.Copy(dllPath, dllBytesPath, true);
                Debug.Log($"[CopyHotUpdateAssembliesToStreamingAssets] copy hotfix dll {dllPath} -> {dllBytesPath}");
                DllSortOut($"{ dll}.bytes");
            }
        }

        private void DllSortOut(string dllName)
        {
            AssetLocate assetLocate = AssetLocate.Dll;
            if (!m_AssetLocate2BundleNameList.ContainsKey(assetLocate))
            {
                m_AssetLocate2BundleNameList.Add(assetLocate, new List<string>());
            }
            m_AssetLocate2BundleNameList[assetLocate].Add(dllName);

            // 配置Bundle引用
            BundleRef bundleRef = new BundleRef()
            {
                BundleName = dllName,
                AssetLocate = assetLocate
            };
            m_BundleName2BundleRef.Add(dllName, bundleRef);

            // 资源
            AssetRef assetRef = new AssetRef()
            {
                AssetPath = dllName,
                BundleRef = bundleRef
            };

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
}
