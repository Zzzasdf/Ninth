using Cysharp.Threading.Tasks;
using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;
using VContainer.Unity;

namespace Ninth
{
    public class LoadDll: IAsyncStartable
    {
        public static List<string> AOTMetaAssemblyNames { get; } = new()
        {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll",
            "UniTask.dll",
        };

        private static Dictionary<string, byte[]> s_assetDatas = new();

        public static byte[] GetAssetData(string dllName)
        {
            return s_assetDatas[dllName];
        }

        private readonly IAssetConfig assetConfig;
        private readonly IPathProxy pathProxy;
        
        [Inject]
        public LoadDll(IAssetConfig assetConfig, IPathProxy pathProxy)
        {
            this.assetConfig = assetConfig;
            this.pathProxy = pathProxy;
        }

        private System.Reflection.Assembly m_GameAss = null;

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
#if !UNITY_EDITOR
            await LoadDllFromBytes();
#else
            var environment = assetConfig.RuntimeEnv();
            if (!assetConfig.DllRuntimeEnv().Contains(environment))
            {
                m_GameAss = AppDomain.CurrentDomain.GetAssemblies()
                    .First(assembly => assembly.GetName().Name == "Assembly-CSharp");
                LoadHotUpdatePart();
            }
            else
            {
                await LoadDllFromBytes();
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
                var dir = assetConfig.RuntimeEnv() switch
                {
                    Environment.LocalAb => pathProxy.Get(VERSION_PATH.StreamingAssets),
                    Environment.RemoteAb => pathProxy.Get(VERSION_PATH.PersistentData),
                    _ => null,
                };
                if (dir == null)
                {
                    continue;
                }
                var dllPath = GetWebRequestPath(dir);
                Debug.Log($"start download asset:{dllPath}");
                var www = UnityWebRequest.Get(dllPath);
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
                    var assetData = www.downloadHandler.data;
                    Debug.Log($"dll:{asset}  size:{assetData.Length}");
                    s_assetDatas[asset] = assetData;
                }
            }

            LoadMetadataForAOTAssemblies();
            LoadHotUpdatePart();
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
            // 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
            // 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
            var mode = HomologousImageMode.SuperSet;
            foreach (var aotDllName in AOTMetaAssemblyNames)
            {
                var dllBytes = GetAssetData(aotDllName);
                // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
                var err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
                $"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}".Log();
            }
        }

        private void LoadHotUpdatePart()
        {
            m_GameAss = System.Reflection.Assembly.Load(GetAssetData("Assembly-CSharp.dll"));
            if (m_GameAss == null)
            {
                throw new Exception("未找到对应热更的程序集");
            }
            // TODO
            // var appType = m_GameAss.GetType("Ninth.HotUpdate.GameDriver");
            // var mainMethod = appType.GetMethod("Init");
            // mainMethod.Invoke(null);
        }
    }
}