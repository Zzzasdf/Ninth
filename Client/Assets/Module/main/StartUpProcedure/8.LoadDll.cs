using Cysharp.Threading.Tasks;
using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        };

        private static Dictionary<string, byte[]> s_assetDatas = new Dictionary<string, byte[]>();

        public static byte[] GetAssetData(string dllName)
        {
            return s_assetDatas[dllName];
        }

        public async void EnterProcedure()
        {
            var assets = new List<string>
            {
                "Assembly-CSharp.dll",
            }.Concat(AOTMetaAssemblyNames);

            foreach (var asset in assets)
            {
                string dir = GlobalConfig.AssetMode switch
                {
                    AssetMode.RemoteAB => PathConfig.BundleInDllInPersistentDataPath(asset),
                    AssetMode.LocalAB => PathConfig.BunldeInDllInStreamingAssetPath(asset),
                    _ => throw new Exception("unknown assetMode")
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

        public async void ExitProcedure()
        {
            // TODO .. 这里加载了资源bundle
            // 只能把资源加载模块放入非热更去
            // HOPE .. 想要将资源加载模块放入热更区, 所以就不能用加载资源的方式去启动热更模块,
            // 可能可以用程序集代码反射调用启动热更区，但热更模块好像不打包成dll文件 

            // TODO .. 打包Local时, remote和dll文件夹上会有一个版号文件夹，需要在打包时去掉
            var asset = "gassets_remotegroup";
            string dir = GlobalConfig.AssetMode switch
            {
                AssetMode.RemoteAB => PathConfig.BundleInRemoteInPersistentDataPath(asset),
                AssetMode.LocalAB => PathConfig.BunldeInRemoteInStreamingAssetPath(asset),
                _ => throw new Exception("unknown assetMode")
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

#if !UNITY_EDITOR
        System.Reflection.Assembly.Load(GetAssetData("Assembly-CSharp.dll"));
#endif
            AssetBundle prefabAb = AssetBundle.LoadFromMemory(GetAssetData("gassets_remotegroup"));

            GameObject testPrefab = GameObject.Instantiate(prefabAb.LoadAsset<GameObject>("HotUpdatePrefab.prefab"));
        }
    }
}