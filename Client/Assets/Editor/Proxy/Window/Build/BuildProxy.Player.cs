// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
//
// namespace Ninth.Editor
// {
//     public partial class BuildProxy
//     {
//         public bool BuildPlayer(BuildTargetGroup buildTargetGroup, BuildTarget target)
//         {
//             packConfig.BuildPlatform = target.ToString();
//             VersionConfig versionConfig = jsonProxy.ToObjectAsync<VersionConfig>(packConfig.BaseVersion());
//             if(versionConfig == null)
//             {
//                 UnityEngine.Debug.LogError($"在路径{packConfig.BaseVersion()}下不存在版本配置文件, 请先打一个版本包！！");
//                 return false;
//             }
//             string outputPath = packConfig.PlayerSourceDirectory(versionConfig.BaseVersion);
//             var buildOptions = BuildOptions.None;
//             string location = $"{outputPath}/HybridCLRTrial.exe";
//             Debug.Log("====> Build App");
//             BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
//             {
//                 scenes = new string[] { "Assets/Scenes/main.unity" },
//                 locationPathName = location,
//                 options = buildOptions,
//                 target = target,
//                 targetGroup = buildTargetGroup,
//             };
//
//             var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
//             if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
//             {
//                 Debug.LogError("客户端生成失败，请检查！！");
//                 return false;
//             }
//
// #if UNITY_EDITOR
//             Application.OpenURL($"file:///{location}");
// #endif
//             return true;
//         }
//     }
// }
