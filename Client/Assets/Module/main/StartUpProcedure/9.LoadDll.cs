using Cysharp.Threading.Tasks;
using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Ninth
{
    public class LoadDll : IProcedure
    {
        public static List<string> AOTMetaAssemblyNames { get; } = new List<string>()
        {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll",
            "UniTask.dll",
        };

        private static Dictionary<string, byte[]> s_assetDatas = new Dictionary<string, byte[]>();

        public static byte[] GetAssetData(string dllName)
        {
            return s_assetDatas[dllName];
        }

        private System.Reflection.Assembly m_GameAss = null;

        public async void EnterProcedure()
        {
#if !UNITY_EDITOR
            await LoadDllFromBytes();
#else
            AssetMode assetMode = GlobalConfig.AssetMode;
            if (AssetBundleModuleConfig.ScriptMode.HasFlag(assetMode))
            {
                m_GameAss = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "Assembly-CSharp");
                ExitProcedure();
            }
            else if (AssetBundleModuleConfig.DllMode.HasFlag(assetMode))
            {
                await LoadDllFromBytes();
            }
            else
            {
                throw new Exception($"未找到资源模式{assetMode}所对应的脚本加载模式, 请检查配置");
            }
#endif
        }

        private async UniTask LoadDllFromBytes()
        {
            var assets = new List<string>
                    {
                        "Assembly-CSharp.dll",
                    }.Concat(AOTMetaAssemblyNames);
            foreach (var asset in assets)
            {
                string dir = GlobalConfig.AssetMode switch
                {
                    AssetMode.LocalAB => PathConfig.BunldeInDllInStreamingAssetPath(asset),
                    AssetMode.RemoteAB => PathConfig.BundleInDllInPersistentDataPath(asset),
                    _ => throw new NotImplementedException(),
                };

                string dllPath = GetWebRequestPath(dir);
                Debug.Log($"start download asset:{dllPath}");
                UnityWebRequest www = UnityWebRequest.Get(dllPath);
                await www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
#else
                            if (www.isHttpError || www.isNetworkError)
                            {
                                Debug.Log(www.error);
                            }
#endif
                else
                {
                    // Or retrieve results as binary data
                    byte[] assetData = www.downloadHandler.data;
                    Debug.Log($"dll:{asset}  size:{assetData.Length}");
                    s_assetDatas[asset] = assetData;
                }
            }
            LoadMetadataForAOTAssemblies();
            m_GameAss = System.Reflection.Assembly.Load(GetAssetData("Assembly-CSharp.dll"));
            ExitProcedure();
        }

        private string GetWebRequestPath(string path)
        {
            if (!path.Contains("://"))
            {
                path = "file://" + path;
            }
            if (path.EndsWith(".dll"))
            {
                path += ".bytes";
            }
            return path;
        }

        /// <summary>
        /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
        /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
        /// </summary>
        private static void LoadMetadataForAOTAssemblies()
        {
            /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
            /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
            /// 
            HomologousImageMode mode = HomologousImageMode.SuperSet;
            foreach (var aotDllName in AOTMetaAssemblyNames)
            {
                byte[] dllBytes = GetAssetData(aotDllName);
                // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
                LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
                Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
            }
        }

        public void ExitProcedure()
        {
            if (m_GameAss == null)
            {
                throw new Exception("未找到对应热更的程序集");
            }
            var appType = m_GameAss.GetType("Ninth.HotUpdate.GameDriver");
            var mainMethod = appType.GetMethod("Init");
            mainMethod.Invoke(null, null);
        }
    }
}