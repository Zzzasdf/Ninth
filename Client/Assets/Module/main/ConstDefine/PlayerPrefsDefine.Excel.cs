using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth
{
    public partial class PlayerPrefsDefine
    {
        private readonly static string m_ExcelSettingsType = "ExcelSettingsType";
        private readonly static string m_ExcelSearchPathDirectoryRoot = "ExcelSearchPathDirectoryRoot";
        private readonly static string m_ExcelSearchMode = "ExcelSearchMode";
        private readonly static string m_ExcelSearchResultShowMode = "ExcelSearchResultShowMode";


        public static int ExcelSettingsType
        {
            get => PlayerPrefs.GetInt(m_ExcelSettingsType, 0);
            set => PlayerPrefs.SetInt(m_ExcelSettingsType, value);
        }

        public static string ExcelSearchPathDirectoryRoot
        {
            get => PlayerPrefs.GetString(m_ExcelSearchPathDirectoryRoot, $"{Application.dataPath}/../../Excels");
            set => PlayerPrefs.SetString(m_ExcelSearchPathDirectoryRoot, value);
        }

        public static int ExcelSearchMode
        {
            get => PlayerPrefs.GetInt(m_ExcelSearchMode, 0);
            set => PlayerPrefs.SetInt(m_ExcelSearchMode, value);
        }

        public static int ExcelSearchResultShowMode
        {
            get => PlayerPrefs.GetInt(m_ExcelSearchResultShowMode, 0);
            set => PlayerPrefs.SetInt(m_ExcelSearchResultShowMode, value);
        }
    }
}

