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
            if (GlobalConfig.AssetMode == AssetMode.LocalAB
                || GlobalConfig.AssetMode == AssetMode.RemoteAB)
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

        public void ExitProcedure()
        {
            System.Reflection.Assembly gameAss = null;
#if !UNITY_EDITOR
            gameAss = System.Reflection.Assembly.Load(GetAssetData("Assembly-CSharp.dll"));
#else
            switch (GlobalConfig.AssetMode)
            {
                case AssetMode.NonAB:
                    {
                        gameAss = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "Assembly-CSharp");
                        break;
                    }
                case AssetMode.LocalAB:
                case AssetMode.RemoteAB:
                    {
                        LoadMetadataForAOTAssemblies();
                        gameAss = System.Reflection.Assembly.Load(GetAssetData("Assembly-CSharp.dll"));
                        break;
                    }
            }
#endif
            if (gameAss == null)
            {
                UnityEngine.Debug.LogError("dll未加载");
                return;
            }
            var appType = gameAss.GetType("Ninth.HotUpdate.GameDriver");
            var mainMethod = appType.GetMethod("Init");
            mainMethod.Invoke(null, null);
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
    }
}