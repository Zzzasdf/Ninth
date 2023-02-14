using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildAssetsCommand
    {
        #region ActiveBuildTarget
        [MenuItem("Tools/BuildPlayer/ActiveBuildTarget/Execute")]
        public static void BuildPlayerActiveBuildTarget()
        {
            BuildPlayer(EditorUserBuildSettings.selectedBuildTargetGroup, EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("Tools/BuildPlayer/ActiveBuildTarget/RepackageLocal")]
        public static void BuildPlayerActiveBuildTargetInLocal()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildPlayerRepackage(EditorUserBuildSettings.selectedBuildTargetGroup, EditorUserBuildSettings.activeBuildTarget, AssetMode.LocalAB, newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildPlayer/ActiveBuildTarget/RepackageRemote")]
        public static void BuildPlayerActiveBuildTargetInRemote()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildPlayerRepackage(EditorUserBuildSettings.selectedBuildTargetGroup, EditorUserBuildSettings.activeBuildTarget, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion

        #region Win64
        [MenuItem("Tools/BuildPlayer/Win64/Execute")]
        public static void BuildPlayerInWin64()
        {
            BuildPlayer(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
        }

        [MenuItem("Tools/BuildPlayer/Win64/RepackageLocal")]
        public static void BuildPlayerInWin64InLocal()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildPlayerRepackage(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64, AssetMode.LocalAB, newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildPlayer/Win64/RepackageRemote")]
        public static void BuildPlayerInWin64InRemote()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildPlayerRepackage(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion

        #region Win32
        [MenuItem("Tools/BuildPlayer/Win32/Execute")]
        public static void BuildPlayerInWin32()
        {
            BuildPlayer(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
        }

        [MenuItem("Tools/BuildPlayer/Win32/RepackageLocal")]
        public static void BuildPlayerInWin32InLocal()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildPlayerRepackage(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows, AssetMode.LocalAB, newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildPlayer/Win32/RepackageRemote")]
        public static void BuildPlayerInWin32InRemote()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildPlayerRepackage(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion

        #region Android
        [MenuItem("Tools/BuildPlayer/Android/Execute")]
        public static void BuildPlayerInAndroid()
        {
            BuildPlayer(BuildTargetGroup.Android, BuildTarget.Android);
        }

        [MenuItem("Tools/BuildPlayer/Android/RepackageLocal")]
        public static void BuildPlayerInAndroidInLocal()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildPlayerRepackage(BuildTargetGroup.Android, BuildTarget.Android, AssetMode.LocalAB, newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildPlayer/Android/RepackageRemote")]
        public static void BuildPlayerInAndroidInRemote()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildPlayerRepackage(BuildTargetGroup.Android, BuildTarget.Android, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion

        #region IOS
        [MenuItem("Tools/BuildPlayer/IOS/Execute")]
        public static void BuildPlayerInIOS()
        {
            BuildPlayer(BuildTargetGroup.iOS, BuildTarget.iOS);
        }

        [MenuItem("Tools/BuildPlayer/IOS/RepackageLocal")]
        public static void BuildPlayerInIOSInLocal()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildPlayerRepackage(BuildTargetGroup.iOS, BuildTarget.iOS, AssetMode.LocalAB, newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildPlayer/IOS/RepackageRemote")]
        public static void BuildPlayerInIOSInRemote()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildPlayerRepackage(BuildTargetGroup.iOS, BuildTarget.iOS, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion
    }
}
