using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public sealed partial class BuildSettings
    {
        private readonly BuildAssetsCommand buildAssetsCmd;

        public BuildSettings(BuildAssetsCommand buildAssetsCmd)
        {
            this.buildAssetsCmd = buildAssetsCmd;
        }

        private BuildSettingsType BuildSettingsType
        {
            get => WindowSOCore.Get<WindowBuildConfig>().BuildSettingsType;
            set => WindowSOCore.Get<WindowBuildConfig>().BuildSettingsType = value;
        }

        private BuildBundleMode BuildBundleMode
        {
            get => WindowSOCore.Get<WindowBuildConfig>().BuildBundleMode;
            set => WindowSOCore.Get<WindowBuildConfig>().BuildBundleMode = value;
        }

        private BuildPlayerMode BuildPlayerMode
        {
            get => WindowSOCore.Get<WindowBuildConfig>().BuildPlayerMode;
            set => WindowSOCore.Get<WindowBuildConfig>().BuildPlayerMode = value;
        }

        private bool bVersionInit;

        public void OnGUI()
        {
            if(bVersionInit)
            {
                VersionInit();
                bVersionInit = !bVersionInit;
            }
            EditorGUILayout.BeginVertical();
            string[] barMenu = new string[]
            {
                BuildSettingsType.Bundle.ToString(),
                BuildSettingsType.Player.ToString()
            };
            BuildSettingsType buildSettingsType = (BuildSettingsType)GUILayout.Toolbar((int)BuildSettingsType, barMenu);

            if(buildSettingsType != BuildSettingsType)
            {
                VersionInit();
                BuildSettingsType = buildSettingsType;
            }
            switch (BuildSettingsType)
            {
                case BuildSettingsType.Bundle:
                    {
                        SetBrowseBundlesTargetDirectory();
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        string[] barMenuBundle = new string[]
                        {
                            BuildBundleMode.HotUpdateBundles.ToString(),
                            BuildBundleMode.AllBundles.ToString()
                        };
                        BuildBundleMode buildBundleMode = (BuildBundleMode)GUILayout.Toolbar((int)BuildBundleMode, barMenuBundle);
                        if(buildBundleMode != BuildBundleMode)
                        {
                            VersionInit();
                            BuildBundleMode = buildBundleMode;
                        }
                        EditorGUILayout.EndHorizontal();
                        
                        SetBtnExchangeExportDirectory();
                        SetToggleActiveTarget();
                        SetBuildTarget();
                        SetVersion();
                        SetExport();
                        break;
                    }
                case BuildSettingsType.Player:
                    {
                        SetBrowseBundlesTargetDirectory();
                        SetBrowsePlayersTargetDirectory();
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        string[] barMenuPlayer = new string[]
                        {
                            BuildPlayerMode.InoperationBundle.ToString(),
                            BuildPlayerMode.RepackageBundle.ToString()
                        };
                        BuildPlayerMode buildPlayerMode = (BuildPlayerMode)GUILayout.Toolbar((int)BuildPlayerMode, barMenuPlayer);
                        if(buildPlayerMode != BuildPlayerMode)
                        {
                            VersionInit();
                            BuildPlayerMode = buildPlayerMode;
                        }
                        EditorGUILayout.EndHorizontal();

                        SetBtnExchangeExportDirectory();
                        SetToggleActiveTarget();
                        SetBuildTarget();
                        SetBuildTargetGroup();
                        if(BuildPlayerMode == BuildPlayerMode.RepackageBundle)
                        {
                            SetVersion();
                        }
                        SetExport();
                        break;
                    }
            }
            EditorGUILayout.EndVertical();
        }
    }
}
