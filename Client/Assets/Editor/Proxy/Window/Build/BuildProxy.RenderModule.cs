// using System.Collections;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.Linq;
// using UnityEditor;
// using UnityEngine;
//
// namespace Ninth.Editor.Window
// {
//     public partial class BuildProxy
//     {
//         #region Bundle
//         private string buildBundlesTargetFolderRoot
//         {
//             get => WindowSOCore.Get<BuildSO>().BuildBundlesTargetFolderRoot;
//             set => WindowSOCore.Get<BuildSO>().BuildBundlesTargetFolderRoot = value;
//         }
//         private void RenderBuildBundlesTargetFolderRootAndBrowse()
//         {
//             using (new GUILayout.HorizontalScope())
//             {
//                 GUI.enabled = false;
//                 buildBundlesTargetFolderRoot = EditorGUILayout.TextField(CommonLanguage.BuildBundlesTargetFolderRoot.ToCurrLanguage(), buildBundlesTargetFolderRoot);
//                 GUI.enabled = true;
//                 if (GUILayout.Button(CommonLanguage.Browse.ToCurrLanguage()))
//                 {
//                     string path = EditorUtility.OpenFolderPanel(CommonLanguage.SelectATargetFolderRootToExport.ToCurrLanguage(), buildBundlesTargetFolderRoot, "Bundles");
//                     if (!string.IsNullOrEmpty(path))
//                     {
//                         buildBundlesTargetFolderRoot = path;
//                     }
//                 }
//             }
//         }
//
//         private BuildBundleMode buildBundleMode
//         {
//             get => WindowSOCore.Get<BuildSO>().BuildBundleMode;
//             set => WindowSOCore.Get<BuildSO>().BuildBundleMode = value;
//         }
//         private void RenderBuildBundleMode()
//         {
//             using (new GUILayout.HorizontalScope())
//             {
//                 GUILayout.FlexibleSpace();
//                 string[] barMenuBundle = (typeof(BuildBundleMode).GetEnumValues() as BuildBundleMode[]).ToCurrLanguage();
//                 BuildBundleMode buildBundleModeTemp = (BuildBundleMode)GUILayout.Toolbar((int)buildBundleMode, barMenuBundle);
//                 if (buildBundleModeTemp != buildBundleMode)
//                 {
//                     VersionRefresh();
//                     buildBundleMode = buildBundleModeTemp;
//                 }
//             }
//         }
//         #endregion
//
//         #region Player
//         private string buildPlayersTargetFolderRoot
//         {
//             get => WindowSOCore.Get<BuildSO>().BuildPlayersDirectoryRoot;
//             set => WindowSOCore.Get<BuildSO>().BuildPlayersDirectoryRoot = value;
//         }
//         private void RenderBuildPlayersTargetFolderRootAndBrowse()
//         {
//             using (new GUILayout.HorizontalScope())
//             {
//                 GUI.enabled = false;
//                 buildPlayersTargetFolderRoot = EditorGUILayout.TextField(CommonLanguage.BuildPlayersTargetFolderRoot.ToCurrLanguage(), buildPlayersTargetFolderRoot);
//                 GUI.enabled = true;
//                 if (GUILayout.Button(CommonLanguage.Browse.ToCurrLanguage()))
//                 {
//                     string path = EditorUtility.OpenFolderPanel(CommonLanguage.SelectATargetFolderRootToExport.ToCurrLanguage(), buildPlayersTargetFolderRoot, "Players");
//                     if (!string.IsNullOrEmpty(path))
//                     {
//                         buildPlayersTargetFolderRoot = path;
//                     }
//                 }
//             }
//         }
//
//         private void RenderBuildAllBundles()
//         {
//             using (new GUILayout.HorizontalScope())
//             {
//                 GUILayout.FlexibleSpace();
//                 GUI.enabled = false;
//                 if(GUILayout.Button(BuildBundleMode.AllBundles.ToCurrLanguage()))
//                 {
//                 }
//                 GUI.enabled = true;
//             }
//         }
//
//         private BuildTargetGroup buildTargetGroup
//         {
//             get => WindowSOCore.Get<BuildSO>().BuildTargetGroup;
//             set => WindowSOCore.Get<BuildSO>().BuildTargetGroup = value;
//         }
//         private ReadOnlyDictionary<BuildTarget, BuildTargetGroup> buildTargetDic = new(new Dictionary<BuildTarget, BuildTargetGroup>()
//         {
//             { BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone },
//             { BuildTarget.StandaloneWindows, BuildTargetGroup.Standalone },
//             { BuildTarget.Android, BuildTargetGroup.Android },
//             { BuildTarget.iOS, BuildTargetGroup.iOS },
//         });
//         private void RenderPopupByBuildTargetGroup()
//         {
//             GUI.enabled = false;
//             buildTargetGroup = buildTargetDic[buildTarget];
//             buildTargetGroup = (BuildTargetGroup)EditorGUILayout.EnumPopup(CommonLanguage.BuildTargetGroup.ToCurrLanguage(), buildTargetGroup);
//             GUI.enabled = true;
//         }
//         #endregion
//
//         #region Common
//         private BuildExportCopyFolderMode buildExportCopyFolderMode
//         {
//             get => WindowSOCore.Get<BuildSO>().BuildExportCopyFolderMode;
//             set => WindowSOCore.Get<BuildSO>().BuildExportCopyFolderMode = value;
//         }
//         private void RenderToolbarByBuildExportCopyFolderMode()
//         {
//             using (new GUILayout.HorizontalScope())
//             {
//                 GUILayout.FlexibleSpace();
//                 GUILayout.Label(CommonLanguage.CopyOperate.ToCurrLanguage(), EditorStyles.boldLabel);
//                 string[] barMenu = (typeof(BuildExportCopyFolderMode).GetEnumValues() as BuildExportCopyFolderMode[]).ToCurrLanguage();
//                 buildExportCopyFolderMode = (BuildExportCopyFolderMode)GUILayout.Toolbar((int)buildExportCopyFolderMode, barMenu);
//             }
//         }
//
//         private ActiveTargetMode activeTargetMode
//         {
//             get => WindowSOCore.Get<BuildSO>().ActiveTargetMode;
//             set => WindowSOCore.Get<BuildSO>().ActiveTargetMode = value;
//         }
//         private void RenderToolbarByActiveTarget()
//         {
//             using (new GUILayout.HorizontalScope())
//             {
//                 GUILayout.FlexibleSpace();
//                 string[] barMenu = (typeof(ActiveTargetMode).GetEnumValues() as ActiveTargetMode[]).ToCurrLanguage();
//                 activeTargetMode = (ActiveTargetMode)GUILayout.Toolbar((int)activeTargetMode, barMenu);
//             }
//         }
//
//         private BuildTarget buildTarget
//         {
//             get => WindowSOCore.Get<BuildSO>().BuildTarget;
//             set => WindowSOCore.Get<BuildSO>().BuildTarget = value;
//         }
//         private List<BuildTarget> buildTargets = new()
//         {
//             BuildTarget.StandaloneWindows64,
//             BuildTarget.Android,
//             BuildTarget.iOS,
//         };
//         private void RenderPopupByBuildTarget()
//         {
//             switch(activeTargetMode)
//             {
//                 case ActiveTargetMode.ActiveTarget:
//                     {
//                         GUI.enabled = false;
//                         buildTarget = EditorUserBuildSettings.activeBuildTarget;
//                         RenderBuildTarget();
//                         GUI.enabled = true;
//                         break;
//                     }
//                 case ActiveTargetMode.InactiveTarget:
//                     {
//                         RenderBuildTarget();
//                         break;
//                     }
//             }
//
//             void RenderBuildTarget()
//             {
//                 buildTarget = (BuildTarget)EditorGUILayout.IntPopup(CommonLanguage.BuildTarget.ToCurrLanguage(), (int)buildTarget, buildTargets.Select(x => x.ToString()).ToArray(), buildTargets.Select(x => (int)x).ToArray());
//             }
//         }
//         #endregion
//     }
// }
