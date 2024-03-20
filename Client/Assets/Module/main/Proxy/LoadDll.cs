using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using HybridCLR;
using Ninth.Utility;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;
using VContainer.Unity;
using Environment = Ninth.Utility.Environment;

namespace Ninth
{
    public class LoadDll : IAsyncStartable
    {
        public static List<string> AOTMetaAssemblyNames { get; } = new()
        {
            // "mscorlib.dll.bytes", 
            "System.dll",
            "System.Core.dll",
            "UniTask.dll",
            "Utility.dll",
        };

        private static Dictionary<string, byte[]> assetDatas = new();

        public static byte[] GetAssetData(string dllName)
        {
            return assetDatas[dllName];
        }

        private readonly IAssetConfig assetConfig;
        private readonly INameConfig nameConfig;

        [Inject]
        public LoadDll(IAssetConfig assetConfig, INameConfig nameConfig)
        {
            this.assetConfig = assetConfig;
            this.nameConfig = nameConfig;
        }

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            var environment = assetConfig.RuntimeEnv();
            if (assetConfig.DllRuntimeEnv().Contains(environment))
            {
                await LoadDllFromBytes();
            }

            LoadHotUpdatePart();
        }

        private async UniTask LoadDllFromBytes()
        {
            var assets = new List<string>
            {
                "HotUpdateMain.dll",
            }.Concat(AOTMetaAssemblyNames);
            var folderPrefix = assetConfig.RuntimeEnv() switch
            {
                Environment.LocalAb => Application.streamingAssetsPath,
                Environment.RemoteAb => Application.persistentDataPath,
                _ => null,
            };
            foreach (var asset in assets)
            {
                var dllPath = GetWebRequestPath($"{folderPrefix}/{nameConfig.FolderByDllGroup()}/{asset}");
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
                    assetDatas[asset] = assetData;
                }
            }

            LoadMetadataForAOTAssemblies();
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
            var environment = assetConfig.RuntimeEnv();
            Assembly assembly = null;
            if(assetConfig.DllRuntimeEnv().Contains(environment))
            {
                assembly = Assembly.Load(GetAssetData("HotUpdateMain.dll"));
            }
            else
            {
#if UNITY_EDITOR
                assembly = Assembly.Load("HotUpdateMain");
#endif
            }
            if (assembly == null)
            {
                throw new Exception("未找到对应热更的程序集");
            }
            "加载程序集成功".Log();
            var type = assembly.GetType("Ninth.HotUpdate.GameDriver");
            var mainMethod = type.GetMethod("Init");
            mainMethod.Invoke(null, null);
        }
    }
}