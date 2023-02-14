using UnityEditor;

namespace Ninth.Editor
{
    public partial class BuildAssetsCommand
    {
        #region ActiveBuildTarget
        [MenuItem("Tools/BuildHotUpdateBundles/ActiveBuildTarget/Local")]
        public static void BuildHotUpdateBundlesInActiveBuildTargetInLocal()
        {
            new BuildWindowParam()
            {
                IsBase = false,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildHotUpdateBundles(EditorUserBuildSettings.activeBuildTarget, AssetMode.LocalAB, newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildHotUpdateBundles/ActiveBuildTarget/Remote")]
        public static void BuildHotUpdateBundlesInActiveBuildTargetInRemote()
        {
            new BuildWindowParam()
            {
                IsBase = false,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildHotUpdateBundles(EditorUserBuildSettings.activeBuildTarget, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion

        #region Win64
        [MenuItem("Tools/BuildHotUpdateBundles/Win64/Local")]
        public static void BuildHotUpdateBundlesInWin64InLocal()
        {
            new BuildWindowParam()
            {
                IsBase = false,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildHotUpdateBundles(BuildTarget.StandaloneWindows64, AssetMode.LocalAB, newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildHotUpdateBundles/Win64/Remote")]
        public static void BuildHotUpdateBundlesInWin64InRemote()
        {
            new BuildWindowParam()
            {
                IsBase = false,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildHotUpdateBundles(BuildTarget.StandaloneWindows64, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion

        #region Win32
        [MenuItem("Tools/BuildHotUpdateBundles/Win32/Local")]
        public static void BuildHotUpdateBundlesInWin32InLocal()
        {
            new BuildWindowParam()
            {
                IsBase = false,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildHotUpdateBundles(BuildTarget.StandaloneWindows, AssetMode.LocalAB, newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildHotUpdateBundles/Win32/Remote")]
        public static void BuildHotUpdateBundlesInWin32InRemote()
        {
            new BuildWindowParam()
            {
                IsBase = false,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildHotUpdateBundles(BuildTarget.StandaloneWindows, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion

        #region Android
        [MenuItem("Tools/BuildHotUpdateBundles/Android/Local")]
        public static void BuildHotUpdateBundlesInAndroidInLocal()
        {
            new BuildWindowParam()
            {
                IsBase = false,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildHotUpdateBundles(BuildTarget.Android, AssetMode.LocalAB, newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildHotUpdateBundles/Android/Remote")]
        public static void BuildHotUpdateBundlesInAndroidInRemote()
        {
            new BuildWindowParam()
            {
                IsBase = false,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildHotUpdateBundles(BuildTarget.Android, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion

        #region IOS
        [MenuItem("Tools/BuildHotUpdateBundles/IOS/Local")]
        public static void BuildHotUpdateBundlesInIOSInLocal()
        {
            new BuildWindowParam()
            {
                IsBase = false,
                IsRemote = false,
                DelegateConfirm = (newVersion) => BuildHotUpdateBundles(BuildTarget.iOS, AssetMode.LocalAB, newVersion)
            }.Show();
        }

        [MenuItem("Tools/BuildHotUpdateBundles/IOS/Remote")]
        public static void BuildHotUpdateBundlesInIOSInRemote()
        {
            new BuildWindowParam()
            {
                IsBase = false,
                IsRemote = true,
                DelegateConfirm = (newVersion) => BuildHotUpdateBundles(BuildTarget.iOS, AssetMode.RemoteAB, newVersion)
            }.Show();
        }
        #endregion
    }
}