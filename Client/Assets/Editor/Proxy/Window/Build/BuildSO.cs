// using System.Collections;
// using System.Collections.Generic;
// using System.ComponentModel;
// using Ninth.Utility;
// using UnityEditor;
// using UnityEngine;
//
// namespace Ninth.Editor.Window.Build
// {
//     [CreateAssetMenu(fileName = "BuildSO", menuName = "EditorWindowSO/BuildSO")]
//     public class BuildSO : ScriptableObject, IBuildSO
//     {
//         public AutoDirtyProperty<string> buildBundlesDirectoryRoot;
//         public AutoDirtyProperty<string> buildPlayersDirectoryRoot;
//         public AutoDirtyProperty<BuildSettingsMode> buildSettingsType;
//         public AutoDirtyProperty<BuildBundleMode> buildBundleMode;
//         public AutoDirtyProperty<int> majorVersion;
//         public AutoDirtyProperty<int> minorVersion;
//         public AutoDirtyProperty<int> versionRevisionNumber;
//         public AutoDirtyProperty<int> revisionNumber;
//         public AutoDirtyProperty<BuildExportCopyFolderMode> buildExportDirectoryType;
//         public AutoDirtyProperty<ActiveTargetMode> activeTargetMode;
//         public AutoDirtyProperty<BuildTarget> buildTarget;
//         public AutoDirtyProperty<BuildTargetGroup> buildTargetGroup;
//
//         public string BuildBundlesTargetFolderRoot
//         {
//             get => buildBundlesDirectoryRoot;
//             set => SetProperty(ref buildBundlesDirectoryRoot, value);
//         }
//         public string BuildPlayersDirectoryRoot
//         {
//             get => buildPlayersDirectoryRoot;
//             set => SetProperty(ref buildPlayersDirectoryRoot, value);
//         }
//         public BuildSettingsMode BuildSettingsMode
//         {
//             get => buildSettingsType;
//             set => SetProperty(ref buildSettingsType, value);
//         }
//         public BuildBundleMode BuildBundleMode
//         {
//             get => buildBundleMode;
//             set => SetProperty(ref buildBundleMode, value);
//         }
//         public int MajorVersion
//         {
//             get => majorVersion;
//             set => SetProperty(ref majorVersion, value);
//         }
//         public int MinorVersion
//         {
//             get => minorVersion;
//             set => SetProperty(ref minorVersion, value);
//         }
//         public int VersionRevisionNumber
//         {
//             get => versionRevisionNumber;
//             set => SetProperty(ref versionRevisionNumber, value);
//         }
//         public int RevisionNumber
//         {
//             get => revisionNumber;
//             set => SetProperty(ref revisionNumber, value);
//         }
//         public BuildExportCopyFolderMode BuildExportCopyFolderMode
//         {
//             get => buildExportDirectoryType;
//             set => SetProperty(ref buildExportDirectoryType, value);
//         }
//         public ActiveTargetMode ActiveTargetMode
//         {
//             get => activeTargetMode;
//             set => SetProperty(ref activeTargetMode, value);
//         }
//         public BuildTarget BuildTarget
//         {
//             get => buildTarget;
//             set => SetProperty(ref buildTarget, value);
//         }
//         public BuildTargetGroup BuildTargetGroup
//         {
//             get => buildTargetGroup;
//             set => SetProperty(ref buildTargetGroup, value);
//         }
//
//         private void OnEnable()
//         {
//             SetDefaultBuildBundlesDirectoryRoot();
//             SetDefaultBuildPlayersDirectoryRoot();
//         }
//
//         private void SetDefaultBuildBundlesDirectoryRoot()
//         {
//             if(string.IsNullOrEmpty(BuildBundlesTargetFolderRoot))
//             {
//                 BuildBundlesTargetFolderRoot = $"{Application.dataPath}/../../Bundles";
//             }
//         }
//
//         private void SetDefaultBuildPlayersDirectoryRoot()
//         {
//             if(string.IsNullOrEmpty(BuildPlayersDirectoryRoot))
//             {
//                 buildPlayersDirectoryRoot = $"{Application.dataPath}/../../Players";
//             }
//         }
//
//         private void SetProperty<T>(ref T field, T value)
//         {
//             if (!Equals(field, value))
//             {
//                 field = value;
//                 SaveAssets();
//             }
//         }
//
//         private void SaveAssets()
//         {
//             EditorUtility.SetDirty(this);
//             AssetDatabase.SaveAssetIfDirty(this);
//             AssetDatabase.Refresh(); 
//         }
//     }
//
//     public enum BuildSettingsMode
//     {
//         Bundle,
//         Player
//     }
//
//     public enum BuildBundleMode
//     {
//         HotUpdateBundles,
//         AllBundles
//     }
//
//     public enum BuildExportCopyFolderMode
//     {
//         StreamingAssets,
//         Remote
//     }
//     
//     public enum ActiveTargetMode
//     {
//         ActiveTarget,
//         InactiveTarget
//     }
// }