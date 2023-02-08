using UnityEditor;

namespace Ninth.Editor
{
    public sealed partial class BuildAssetsCommand
    {
        #region BuildPlayerInRemote
        [MenuItem("Tools/BuildPlayer/ActiveBuildTarget/Remote")]
        public static void BuildPlayerActiveBuildTargetInRemote()
        {
            BuildPlayer(EditorUserBuildSettings.activeBuildTarget, AssetMode.RemoteAB);
        }

        [MenuItem("Tools/BuildPlayer/Win64/Remote")]
        public static void BuildPlayerInWin64InRemote()
        {
            BuildPlayer(BuildTarget.StandaloneWindows64, AssetMode.RemoteAB);
        }

        [MenuItem("Tools/BuildPlayer/Win32/Remote")]
        public static void BuildPlayerInWin32InRemote()
        {
            BuildPlayer(BuildTarget.StandaloneWindows, AssetMode.RemoteAB);
        }

        [MenuItem("Tools/BuildPlayer/Android/Remote")]
        public static void BuildPlayerInAndroidInRemote()
        {
            BuildPlayer(BuildTarget.Android, AssetMode.RemoteAB);
        }

        [MenuItem("Tools/BuildPlayer/IOS/Remote")]
        public static void BuildPlayerInIOSInRemote()
        {
            BuildPlayer(BuildTarget.iOS, AssetMode.RemoteAB);
        }
        #endregion

        #region BuildPlayerInLocal
        [MenuItem("Tools/BuildPlayer/ActiveBuildTarget/Local")]
        public static void BuildPlayerActiveBuildTargetInLocal()
        {
            BuildPlayer(EditorUserBuildSettings.activeBuildTarget, AssetMode.LocalAB);
        }

        [MenuItem("Tools/BuildPlayer/Win64/Local")]
        public static void BuildPlayerInWin64InLocal()
        {
            BuildPlayer(BuildTarget.StandaloneWindows64, AssetMode.LocalAB);
        }

        [MenuItem("Tools/BuildPlayer/Win32/Local")]
        public static void BuildPlayerInWin32InLocal()
        {
            BuildPlayer(BuildTarget.StandaloneWindows, AssetMode.LocalAB);
        }

        [MenuItem("Tools/BuildPlayer/Android/Local")]
        public static void BuildPlayerInAndroidInLocal()
        {
            BuildPlayer(BuildTarget.Android, AssetMode.LocalAB);
        }

        [MenuItem("Tools/BuildPlayer/IOS/Local")]
        public static void BuildPlayerInIOSInLocal()
        {
            BuildPlayer(BuildTarget.iOS, AssetMode.LocalAB);
        }
        #endregion

        #region BuildAllBundlesInRemote
        [MenuItem("Tools/BuildAllBundles/ActiveBuildTarget/Remote")]
        public static void BuildAllBundlesInActiveBuildTargetInRemote()
        {
            BuildAllBundles(EditorUserBuildSettings.activeBuildTarget, AssetMode.RemoteAB);
        }

        [MenuItem("Tools/BuildAllBundles/Win64/Remote")]
        public static void BuildAllBundlesInWin64InRemote()
        {
            BuildAllBundles(BuildTarget.StandaloneWindows64, AssetMode.RemoteAB);
        }

        [MenuItem("Tools/BuildAllBundles/Win32/Remote")]
        public static void BuildAllBundlesInWin32InRemote()
        {
            BuildAllBundles(BuildTarget.StandaloneWindows, AssetMode.RemoteAB);
        }

        [MenuItem("Tools/BuildAllBundles/Android/Remote")]
        public static void BuildAllBundlesInAndroidInRemote()
        {
            BuildAllBundles(BuildTarget.Android, AssetMode.RemoteAB);
        }

        [MenuItem("Tools/BuildAllBundles/IOS/Remote")]
        public static void BuildAllBundlesInIOSInRemote()
        {
            BuildAllBundles(BuildTarget.iOS, AssetMode.RemoteAB);
        }
        #endregion

        #region BuildAllBundlesInLocal
        [MenuItem("Tools/BuildAllBundles/ActiveBuildTarget/Local")]
        public static void BuildAllBundlesInActiveBuildTargetInLocal()
        {
            BuildAllBundles(EditorUserBuildSettings.activeBuildTarget, AssetMode.LocalAB);
        }

        [MenuItem("Tools/BuildAllBundles/Win64/Local")]
        public static void BuildAllBundlesInWin64InLocal()
        {
            BuildAllBundles(BuildTarget.StandaloneWindows64, AssetMode.LocalAB);
        }

        [MenuItem("Tools/BuildAllBundles/Win32/Local")]
        public static void BuildAllBundlesInWin32InLocal()
        {
            BuildAllBundles(BuildTarget.StandaloneWindows, AssetMode.LocalAB);
        }

        [MenuItem("Tools/BuildAllBundles/Android/Local")]
        public static void BuildAllBundlesInAndroidInLocal()
        {
            BuildAllBundles(BuildTarget.Android, AssetMode.LocalAB);
        }

        [MenuItem("Tools/BuildAllBundles/IOS/Local")]
        public static void BuildAllBundlesInIOSInLocal()
        {
            BuildAllBundles(BuildTarget.iOS, AssetMode.LocalAB);
        }
        #endregion

        #region BuildHotFixBundlesInRemote
        [MenuItem("Tools/BuildHotUpdateBundles/ActiveBuildTarget/Remote")]
        public static void BuildHotFixBundlesInActiveBuildTargetInRemote()
        {
            BuildHotUpdateBundles(EditorUserBuildSettings.activeBuildTarget, AssetMode.RemoteAB);
        }

        [MenuItem("Tools/BuildHotUpdateBundles/Win64/Remote")]
        public static void BuildHotFixBundlesInWin64InRemote()
        {
            BuildHotUpdateBundles(BuildTarget.StandaloneWindows64, AssetMode.RemoteAB);
        }

        [MenuItem("Tools/BuildHotUpdateBundles/Win32/Remote")]
        public static void BuildHotFixBundlesInWin32InRemote()
        {
            BuildHotUpdateBundles(BuildTarget.StandaloneWindows, AssetMode.RemoteAB);
        }

        [MenuItem("Tools/BuildHotUpdateBundles/Android/Remote")]
        public static void BuildHotFixBundlesInAndroidInRemote()
        {
            BuildHotUpdateBundles(BuildTarget.Android, AssetMode.RemoteAB);
        }

        [MenuItem("Tools/BuildHotUpdateBundles/IOS/Remote")]
        public static void BuildHotFixBundlesInIOSInRemote()
        {
            BuildHotUpdateBundles(BuildTarget.iOS, AssetMode.RemoteAB);
        }
        #endregion

        #region BuildHotFixBundlesInLocal
        [MenuItem("Tools/BuildHotUpdateBundles/ActiveBuildTarget/Local")]
        public static void BuildHotFixBundlesInActiveBuildTargetInLocal()
        {
            BuildHotUpdateBundles(EditorUserBuildSettings.activeBuildTarget, AssetMode.LocalAB);
        }

        [MenuItem("Tools/BuildHotUpdateBundles/Win64/Local")]
        public static void BuildHotFixBundlesInWin64InLocal()
        {
            BuildHotUpdateBundles(BuildTarget.StandaloneWindows64, AssetMode.LocalAB);
        }

        [MenuItem("Tools/BuildHotUpdateBundles/Win32/Local")]
        public static void BuildHotFixBundlesInWin32InLocal()
        {
            BuildHotUpdateBundles(BuildTarget.StandaloneWindows, AssetMode.LocalAB);
        }

        [MenuItem("Tools/BuildHotUpdateBundles/Android/Local")]
        public static void BuildHotFixBundlesInAndroidInLocal()
        {
            BuildHotUpdateBundles(BuildTarget.Android, AssetMode.LocalAB);
        }

        [MenuItem("Tools/BuildHotUpdateBundles/IOS/Local")]
        public static void BuildHotFixBundlesInIOSInLocal()
        {
            BuildHotUpdateBundles(BuildTarget.iOS, AssetMode.LocalAB);
        }
        #endregion
    }
}