using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildSettings
    {
        private BuildExportDirectoryType BuildExportDirectoryType
        {
            get => WindowSOCore.Get<WindowBuildConfig>().BuildExportDirectoryType;
            set => WindowSOCore.Get<WindowBuildConfig>().BuildExportDirectoryType = value;
        }

        private void SetBtnExchangeExportDirectory()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            string[] barMenu = new string[]
            {
                "Copy2StreamingAsset",
                "ApplyRemoteAsset"
            };
            BuildExportDirectoryType = (BuildExportDirectoryType)GUILayout.Toolbar((int)BuildExportDirectoryType, barMenu);
            GUILayout.EndHorizontal();
        }
    }
}