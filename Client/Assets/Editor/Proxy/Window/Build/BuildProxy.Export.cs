// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using UnityEngine;
//
// namespace Ninth.Editor.Window
// {
//     public partial class BuildProxy
//     {
//         private void RenderExport()
//         {
//             if (!bUpgradeVersion)
//             {
//                 GUI.enabled = false;
//                 if (GUILayout.Button(CommonLanguage.PleaseAdjustTheVersion.ToCurrLanguage()))
//                 {
//                 }
//                 GUI.enabled = true;
//                 return;
//             }
//             if (GUILayout.Button(CommonLanguage.Export.ToCurrLanguage()))
//             {
//                 Action<string> action = buildSettingsMode switch
//                 {
//                     BuildSettingsMode.Bundle => buildBundleMode switch
//                     {
//                         BuildBundleMode.HotUpdateBundles => CreateHotUpdateBundles,
//                         BuildBundleMode.AllBundles => CreateAllBundles,
//                         _ => null,
//                     },
//                     BuildSettingsMode.Player => CreatePlayerAndAllBundles,
//                     _ => null,
//                 };
//                 if(action == null)
//                 {
//                     Debug.LogError(Log.FuncIsNull.ToCurrLanguage());
//                     return;
//                 }
//                 Debug.Log(Log.Exporting.ToCurrLanguage());
//                 string version = string.Join(".", majorVersionTemp, minorVersionTemp, revisionNumber);
//                 action.Invoke(version);
//                 VersionSave();
//                 VersionRefresh();
//                 Debug.Log(Log.ExportCompleted);
//             }
//         }
//
//         // 构建热更ab
//         private void CreateHotUpdateBundles(string version)
//         {
//             Environment environment = buildExportCopyFolderMode switch
//             {
//                 BuildExportCopyFolderMode.StreamingAssets => Environment.LocalAb,
//                 BuildExportCopyFolderMode.Remote => Environment.RemoteAb,
//                 _ => default,
//             };
//             if(environment == default)
//             {
//                 Debug.LogError($"未找到该模式：{environment}");
//                 return;
//             }
//             BuildAssetsCommand buildAssetsCmd = EditorEntry.BuildAssetsCmd;
//             buildAssetsCmd.BuildHotUpdateBundles(buildTarget, environment, version);
//             if (buildExportCopyFolderMode == BuildExportCopyFolderMode.Remote)
//             {
//                 buildAssetsCmd.RemoteApply();
//             }
//         }
//
//         // 构建所有ab
//         private void CreateAllBundles(string version)
//         {
//             Environment environment = buildExportCopyFolderMode switch
//             {
//                 BuildExportCopyFolderMode.StreamingAssets => Environment.LocalAb,
//                 BuildExportCopyFolderMode.Remote => Environment.RemoteAb,
//                 _ => default,
//             };
//             if (environment == default)
//             {
//                 Debug.LogError($"未找到该模式：{environment}");
//                 return;
//             }
//             BuildAssetsCommand buildAssetsCmd = EditorEntry.BuildAssetsCmd;
//             buildAssetsCmd.BuildAllBundles(buildTarget, environment, version);
//             if (buildExportCopyFolderMode == BuildExportCopyFolderMode.Remote)
//             {
//                 buildAssetsCmd.RemoteApply();
//             }
//         }
//
//         // 构建客户端和所有ab
//         private void CreatePlayerAndAllBundles(string version)
//         {
//             Environment environment = buildExportCopyFolderMode switch
//             {
//                 BuildExportCopyFolderMode.StreamingAssets => Environment.LocalAb,
//                 BuildExportCopyFolderMode.Remote => Environment.RemoteAb,
//                 _ => default,
//             };
//             if (environment == default)
//             {
//                 Debug.LogError($"未找到该模式：{environment}");
//                 return;
//             }
//             BuildAssetsCommand buildAssetsCmd = EditorEntry.BuildAssetsCmd;
//             buildAssetsCmd.BuildPlayerAndAllBundles(buildTargetGroup, buildTarget, environment, version);
//             if (buildExportCopyFolderMode == BuildExportCopyFolderMode.Remote)
//             {
//                 buildAssetsCmd.RemoteApply();
//             }
//         }
//     }
// }