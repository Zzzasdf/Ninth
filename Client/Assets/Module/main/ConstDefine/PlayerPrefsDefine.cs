using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public class PlayerPrefsDefine
    {
        private readonly static string m_DownloadBundleStartPosFromAssetVersion = "DownloadBundleStartPosFromAssetVersion";
        public static string DownloadBundleStartPosFromAssetVersion
        {
            get => PlayerPrefs.GetString(m_DownloadBundleStartPosFromAssetVersion, "0");
            set => PlayerPrefs.SetString(m_DownloadBundleStartPosFromAssetVersion, value);
        }

        private readonly static string m_DownloadBundleStartPos = "DownloadBundleStartPos";
        public static int DownloadBundleStartPos
        {
            get => PlayerPrefs.GetInt(m_DownloadBundleStartPos, 0);
            set => PlayerPrefs.SetInt(m_DownloadBundleStartPos, value);
        }
    }
}
