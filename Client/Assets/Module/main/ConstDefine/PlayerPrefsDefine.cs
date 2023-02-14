using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public class PlayerPrefsDefine
    {
        #region Pack
        private readonly static string m_BundleSourceDataDirectoryRoot = "BundleSourceDataDirectoryRoot";
        private readonly static string m_PlayerSourceDataDirectoryRoot = "PlayerSourceDataDirectoryRoot";
        #endregion

        #region Verison
        private readonly static string m_BigBaseVersion = "BigBaseVersion";
        private readonly static string m_SmallBaseVersion = "SmallBaseVersion";
        private readonly static string m_HotUpdateVersion = "HotUpdateVersion";

        private readonly static string m_HotUpdateIteration = "HotUpdateIteration";
        private readonly static string m_BaseIteration = "BaseIteration";
        #endregion

        #region Breakpoint Continuation
        private readonly static string m_DownloadBundleStartPosFromAssetVersion = "DownloadBundleStartPosFromAssetVersion";
        private readonly static string m_DownloadBundleStartPos = "DownloadBundleStartPos";
        #endregion

        public static string BundleSourceDataDirectoryRoot
        {
            get => PlayerPrefs.GetString(m_BundleSourceDataDirectoryRoot, $"{Application.dataPath}/../../Bundles");
            set => PlayerPrefs.SetString(m_BundleSourceDataDirectoryRoot, value);
        }

        public static string PlayerSourceDataDirectoryRoot
        {
            get => PlayerPrefs.GetString(m_PlayerSourceDataDirectoryRoot, $"{Application.dataPath}/../../Players");
            set => PlayerPrefs.SetString(m_PlayerSourceDataDirectoryRoot, value);
        }

        public static int BigVersion
        {
            get => PlayerPrefs.GetInt(m_BigBaseVersion, 0);
            set => PlayerPrefs.SetInt(m_BigBaseVersion, value);
        }

        public static int SmallVersion
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

        public static string DownloadBundleStartPosFromAssetVersion
        {
            get => PlayerPrefs.GetString(m_DownloadBundleStartPosFromAssetVersion, "0.0.0.0");
            set => PlayerPrefs.SetString(m_DownloadBundleStartPosFromAssetVersion, value);
        }

        public static int DownloadBundleStartPos
        {
            get => PlayerPrefs.GetInt(m_DownloadBundleStartPos, 0);
            set => PlayerPrefs.SetInt(m_DownloadBundleStartPos, value);
        }
    }
}
