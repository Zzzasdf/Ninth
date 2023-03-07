using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public sealed partial class BuildSettings: EditorWindow
    {
        [MenuItem("Tools/Build/Settings")]
        private static void PanelOpen()
        {
            GetWindow<BuildSettings>();
        }

        private BuildSettingsType m_BuildSettingsType
        {
            get => (BuildSettingsType)PlayerPrefsDefine.BuildSettingsType;
            set => PlayerPrefsDefine.BuildSettingsType = (int)value;
        }

        private BuildBundleMode m_BuildBundleMode
        {
            get => (BuildBundleMode)PlayerPrefsDefine.BuildBundleMode;
            set => PlayerPrefsDefine.BuildBundleMode = (int)value;
        }

        private BuildPlayerMode m_BuildPlayerMode
        {
            get => (BuildPlayerMode)PlayerPrefsDefine.BuildPlayerMode;
            set => PlayerPrefsDefine.BuildPlayerMode = (int)value;
        }

        private void Awake()
        {
            SetVersionInit();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            string[] barMenu = new string[]
            {
                BuildSettingsType.Bundle.ToString(),
                BuildSettingsType.Player.ToString()
            };
            BuildSettingsType buildSettingsType = (BuildSettingsType)GUILayout.Toolbar((int)m_BuildSettingsType, barMenu);

            if(buildSettingsType != m_BuildSettingsType)
            {
                SetVersionInit();
                m_BuildSettingsType = buildSettingsType;
            }
            switch (m_BuildSettingsType)
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
                        BuildBundleMode buildBundleMode = (BuildBundleMode)GUILayout.Toolbar((int)m_BuildBundleMode, barMenuBundle);
                        if(buildBundleMode != m_BuildBundleMode)
                        {
                            SetVersionInit();
                            m_BuildBundleMode = buildBundleMode;
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
                        BuildPlayerMode buildPlayerMode = (BuildPlayerMode)GUILayout.Toolbar((int)m_BuildPlayerMode, barMenuPlayer);
                        if(buildPlayerMode != m_BuildPlayerMode)
                        {
                            SetVersionInit();
                            m_BuildPlayerMode = buildPlayerMode;
                        }
                        EditorGUILayout.EndHorizontal();

                        SetBtnExchangeExportDirectory();
                        SetToggleActiveTarget();
                        SetBuildTarget();
                        SetBuildTargetGroup();
                        if(m_BuildPlayerMode == BuildPlayerMode.RepackageBundle)
                        {
                            SetVersion();
                        }
                        SetExport();
                        break;
                    }
            }
            EditorGUILayout.EndVertical();
        }

        public enum BuildSettingsType
        {
            Bundle,
            Player
        }

        public enum BuildBundleMode
        {
            HotUpdateBundles,
            AllBundles
        }

        public enum BuildPlayerMode
        {
            InoperationBundle,
            RepackageBundle
        }
    }
}
