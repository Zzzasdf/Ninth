using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildSettings
    {
        private BuildExportDirectoryType m_BuildExportDirectoryType
        {
            get => (BuildExportDirectoryType)PlayerPrefsDefine.BuildExportDirectoryType;
            set => PlayerPrefsDefine.BuildExportDirectoryType = (int)value;
        }

        private void SetBtnExchangeExportDirectory()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            string[] barMenu = new string[]
            {
                "Copy2StreamingAsset",
                "ApplyRemoteCAsset"
            };
            m_BuildExportDirectoryType = (BuildExportDirectoryType)GUILayout.Toolbar((int)m_BuildExportDirectoryType, barMenu);
            GUILayout.EndHorizontal();
        }

        public enum BuildExportDirectoryType
        {
            Local,
            Remote
        }
    }
}