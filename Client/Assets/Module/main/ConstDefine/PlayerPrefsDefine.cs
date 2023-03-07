using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public sealed partial class PlayerPrefsDefine
    {
        #region Breakpoint Continuation
        private readonly static string m_DownloadBundleStartPosFromAssetVersion = "DownloadBundleStartPosFromAssetVersion";
        private readonly static string m_DownloadBundleStartPos = "DownloadBundleStartPos";
        #endregion

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
