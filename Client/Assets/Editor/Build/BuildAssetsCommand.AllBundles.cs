using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildAssetsCommand
    {
        #region ActiveBuildTarget
        [MenuItem("Tools/BuildAllBundles/ActiveBuildTarget/Local")]
        public static void BuildAllBundlesInActiveBuildTargetInLocal()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildAllBundles(EditorUserBuildSettings.activeBuildTarget, AssetMode.LocalAB, newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildAllBundles/ActiveBuildTarget/Remote")]
        public static void BuildAllBundlesInActiveBuildTargetInRemote()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildAllBundles(EditorUserBuildSettings.activeBuildTarget, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion

        #region Win64
        [MenuItem("Tools/BuildAllBundles/Win64/Local")]
        public static void BuildAllBundlesInWin64InLocal()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildAllBundles(BuildTarget.StandaloneWindows64, AssetMode.LocalAB, newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildAllBundles/Win64/Remote")]
        public static void BuildAllBundlesInWin64InRemote()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildAllBundles(BuildTarget.StandaloneWindows64, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion

        #region Win32
        [MenuItem("Tools/BuildAllBundles/Win32/Local")]
        public static void BuildAllBundlesInWin32InLocal()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildAllBundles(BuildTarget.StandaloneWindows, AssetMode.LocalAB,newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildAllBundles/Win32/Remote")]
        public static void BuildAllBundlesInWin32InRemote()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildAllBundles(BuildTarget.StandaloneWindows, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion

        #region Android
        [MenuItem("Tools/BuildAllBundles/Android/Local")]
        public static void BuildAllBundlesInAndroidInLocal()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildAllBundles(BuildTarget.Android, AssetMode.LocalAB, newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildAllBundles/Android/Remote")]
        public static void BuildAllBundlesInAndroidInRemote()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildAllBundles(BuildTarget.Android, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion

        #region IOS
        [MenuItem("Tools/BuildAllBundles/IOS/Local")]
        public static void BuildAllBundlesInIOSInLocal()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildAllBundles(BuildTarget.iOS, AssetMode.LocalAB, newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildAllBundles/IOS/Remote")]
        public static void BuildAllBundlesInIOSInRemote()
        {
            new BuildWindowParam()
            {
                IsBase = true,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildAllBundles(BuildTarget.iOS, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion
    }
}