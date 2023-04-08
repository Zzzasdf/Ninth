using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public sealed partial class BuildSettings: EditorWindow
    {
        [MenuItem("Tools/BuildSettings")]
        private static void PanelOpen()
        {
            GetWindow<BuildSettings>();
        }

        private BuildSettingsType BuildSettingsType
        {
            get => EditorSOCore.GetBuildConfig().BuildSettingsType;
            set => EditorSOCore.GetBuildConfig().BuildSettingsType = value;
        }

        private BuildBundleMode BuildBundleMode
        {
            get => EditorSOCore.GetBuildConfig().BuildBundleMode;
            set => EditorSOCore.GetBuildConfig().BuildBundleMode = value;
        }

        private BuildPlayerMode BuildPlayerMode
        {
            get => EditorSOCore.GetBuildConfig().BuildPlayerMode;
            set => EditorSOCore.GetBuildConfig().BuildPlayerMode = value;
        }

        private void Awake()
        {
            VersionInit();
        }

        private void OnGUI()
        {
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
