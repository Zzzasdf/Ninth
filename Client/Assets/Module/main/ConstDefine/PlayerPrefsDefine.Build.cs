using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public partial class PlayerPrefsDefine
    {
        private readonly static string m_BuildBundlesDirectoryRoot = "BuildBundlesDirectoryRoot";
        private readonly static string m_BuildPlayersDirectoryRoot = "BuildPlayersDirectoryRoot";

        private readonly static string m_BuildSettingsType = "BuildSettingsType";
        private readonly static string m_BuildBundleMode = "BuildBundleMode";
        private readonly static string m_BuildPlayerMode = "BuildPlayerMode";
        private readonly static string m_BuildExportDirectoryType = "BuildExportDirectoryType";

        private readonly static string m_ActiveTargetMode = "ActiveTargetMode";
        private readonly static string m_BuildTarget = "BuildTarget";
        private readonly static string m_BuildTargetGroup = "BuildTargetGroup";

        private readonly static string m_BuildMode = "BuildMode";
        private readonly static string m_BigBaseVersion = "BigBaseVersion";
        private readonly static string m_SmallBaseVersion = "SmallBaseVersion";
        private readonly static string m_HotUpdateVersion = "HotUpdateVersion";
        private readonly static string m_BaseIteration = "BaseIteration";
        private readonly static string m_HotUpdateIteration = "HotUpdateIteration";

        #region Directory
        public static string BuildBundlesDirectoryRoot
        {
            get => PlayerPrefs.GetString(m_BuildBundlesDirectoryRoot, $"{Application.dataPath}/../../Bundles");
            set => PlayerPrefs.SetString(m_BuildBundlesDirectoryRoot, value);
        }

        public static string BuildPlayersDirectoryRoot
        {
            get => PlayerPrefs.GetString(m_BuildPlayersDirectoryRoot, $"{Application.dataPath}/../../Players");
            set => PlayerPrefs.SetString(m_BuildPlayersDirectoryRoot, value);
        }
        #endregion

        #region Settings
        public static int BuildSettingsType
        {
            get => PlayerPrefs.GetInt(m_BuildSettingsType, 0);
            set => PlayerPrefs.SetInt(m_BuildSettingsType, value);
        }

        public static int BuildBundleMode
        {
            get => PlayerPrefs.GetInt(m_BuildBundleMode, 0);
            set => PlayerPrefs.SetInt(m_BuildBundleMode, value);
        }

        public static int BuildPlayerMode
        {
            get => PlayerPrefs.GetInt(m_BuildPlayerMode, 0);
            set => PlayerPrefs.SetInt(m_BuildPlayerMode, value);
        }

        public static int BuildExportDirectoryType
        {
            get => PlayerPrefs.GetInt(m_BuildExportDirectoryType, 0);
            set => PlayerPrefs.SetInt(m_BuildExportDirectoryType, value);
        }
        #endregion

        #region Target
        public static int ActiveTargetMode
        {
            get => PlayerPrefs.GetInt(m_ActiveTargetMode, 0);
            set => PlayerPrefs.SetInt(m_ActiveTargetMode, value);
        }

        public static int BuildTarget
        {
            get => PlayerPrefs.GetInt(m_BuildTarget, 0);
            set => PlayerPrefs.SetInt(m_BuildTarget, value);
        }

        public static int BuildTargetGroup
        {
            get => PlayerPrefs.GetInt(m_BuildTargetGroup, 0);
            set => PlayerPrefs.SetInt(m_BuildTargetGroup, value);
        }
        #endregion

        #region Version
        public static int BuildMode
        {
            get => PlayerPrefs.GetInt(m_BuildMode, 0);
            set => PlayerPrefs.SetInt(m_BuildMode, value);
        }

        public static int BigBaseVersion
        {
            get => PlayerPrefs.GetInt(m_BigBaseVersion, 0);
            set => PlayerPrefs.SetInt(m_BigBaseVersion, value);
        }

        public static int SmallBaseVersion
        {
            get => PlayerPrefs.GetInt(m_SmallBaseVersion, 0);
            set => PlayerPrefs.SetInt(m_SmallBaseVersion, value);
        }

        public static int HotUpdateVersion
        {
            get => PlayerPrefs.GetInt(m_HotUpdateVersion, 0);
            set => PlayerPrefs.SetInt(m_HotUpdateVersion, value);
        }

        public static int BaseIteration
        {
            get => PlayerPrefs.GetInt(m_BaseIteration, 0);
            set => PlayerPrefs.SetInt(m_BaseIteration, value);
        }

        public static int HotUpdateIteration
        {
            get => PlayerPrefs.GetInt(m_HotUpdateIteration, 0);
            set => PlayerPrefs.SetInt(m_HotUpdateIteration, value);
        }
        #endregion
    }
}
