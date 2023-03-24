using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildSettings
    {
        private static bool m_SetVersion;

        private static BuildMode m_BuildMode
        {
            get => SOCore.GetGlobalConfig().BuildMode;
            set => SOCore.GetGlobalConfig().BuildMode = value;
        }

        private static int m_BigBaseVersionTemp;
        private static int m_BigBaseVersion
        {
            get => EditorSOCore.GetBuildConfig().BigBaseVersion;
            set => EditorSOCore.GetBuildConfig().BigBaseVersion = value;
        }

        private static int m_SmallBaseVersionTemp;
        private static int m_SmallBaseVersion
        {
            get => EditorSOCore.GetBuildConfig().SmallBaseVersion;
            set => EditorSOCore.GetBuildConfig().SmallBaseVersion = value;
        }

        private static int m_HotUpdateVersionTemp;
        private static int m_HotUpdateVersion
        {
            get => EditorSOCore.GetBuildConfig().HotUpdateVersion;
            set => EditorSOCore.GetBuildConfig().HotUpdateVersion = value;
        }

        private static int m_BaseIterationTemp;
        private static int m_BaseIteration
        {
            get => EditorSOCore.GetBuildConfig().BaseIteration;
            set => EditorSOCore.GetBuildConfig().BaseIteration = value;
        }

        private static int m_HotUpdateIterationTemp;
        private static int m_HotUpdateIteration
        {
            get => EditorSOCore.GetBuildConfig().HotUpdateIteration;
            set => EditorSOCore.GetBuildConfig().HotUpdateIteration = value;
        }

        private void SetVersionInit()
        {
            m_BigBaseVersionTemp = m_BigBaseVersion;
            m_SmallBaseVersionTemp = m_SmallBaseVersion;
            m_HotUpdateVersionTemp = m_HotUpdateVersion;
            m_BaseIterationTemp = m_BaseIteration;
            m_HotUpdateIterationTemp = m_HotUpdateIteration;
            m_SetVersion = false;
        }

        private void VersionStore()
        {
            m_BigBaseVersion = m_BigBaseVersionTemp;
            m_SmallBaseVersion = m_SmallBaseVersionTemp;
            m_HotUpdateVersion = m_HotUpdateVersionTemp;
            m_BaseIteration = m_BaseIterationTemp;
            m_HotUpdateIteration = m_HotUpdateIterationTemp;
        }

        private void SetVersion()
        {
            SetBuildMode();
            SetBigBaseVersion();
            SetSmallBaseVersion();
            SetHotUpdateVersion();
            SetBaseIteration();
            SetHotUpdateIteration();
            ResetVersion();
        }

        private void SetBuildMode()
        {
            if(!(m_BuildSettingsType == BuildSettingsType.Player && m_BuildPlayerMode == BuildPlayerMode.InoperationBundle))
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                string[] barMenu = new string[]
                {
                    "TestVersion",
                    "FormalVersion"
                };
                BuildMode buildMode = (BuildMode)GUILayout.Toolbar((int)m_BuildMode, barMenu);
                if (buildMode != m_BuildMode)
                {
                    SetVersionInit();
                    m_BuildMode = buildMode;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void SetBigBaseVersion()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            m_BigBaseVersionTemp = EditorGUILayout.IntField("BigBaseVersion", Mathf.Max(0, m_BigBaseVersionTemp));
            GUI.enabled = true;

            if (!m_SetVersion
                && m_BuildMode == BuildMode.Formal)
            {
                if ((m_BuildSettingsType == BuildSettingsType.Bundle && m_BuildBundleMode == BuildBundleMode.AllBundles)
                    || m_BuildSettingsType == BuildSettingsType.Player && m_BuildPlayerMode == BuildPlayerMode.RepackageBundle)
                {
                    if(GUILayout.Button("+1"))
                    {
                        m_BigBaseVersionTemp++;
                        m_SmallBaseVersionTemp = 0;
                        m_BaseIterationTemp++;
                        m_HotUpdateVersionTemp = 0;
                        m_HotUpdateIterationTemp++;
                        m_SetVersion = !m_SetVersion;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SetSmallBaseVersion()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            m_SmallBaseVersionTemp = EditorGUILayout.IntField("SmallBaseVersion", Mathf.Max(0, m_SmallBaseVersionTemp));
            GUI.enabled = true;

            if (!m_SetVersion 
                && m_BuildMode == BuildMode.Formal)
            {
                if ((m_BuildSettingsType == BuildSettingsType.Bundle && m_BuildBundleMode == BuildBundleMode.AllBundles)
                    || m_BuildSettingsType == BuildSettingsType.Player && m_BuildPlayerMode == BuildPlayerMode.RepackageBundle)
                {
                    if (GUILayout.Button("+1"))
                    {
                        m_SmallBaseVersionTemp++;
                        m_BaseIterationTemp++;
                        m_HotUpdateVersionTemp = 0;
                        m_HotUpdateIterationTemp++;
                        m_SetVersion = !m_SetVersion;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SetHotUpdateVersion()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            m_HotUpdateVersionTemp = EditorGUILayout.IntField("HotUpdateVersion", Mathf.Max(0, m_HotUpdateVersionTemp));
            GUI.enabled = true;

            if (!m_SetVersion
                && m_BuildMode == BuildMode.Formal)
            {
                if (m_BuildSettingsType == BuildSettingsType.Bundle && m_BuildBundleMode == BuildBundleMode.HotUpdateBundles)
                {
                    if (GUILayout.Button("+1"))
                    {
                        m_HotUpdateVersionTemp++;
                        m_HotUpdateIterationTemp++;
                        m_SetVersion = !m_SetVersion;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SetBaseIteration()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            m_BaseIterationTemp = EditorGUILayout.IntField("BaseIteration", Mathf.Max(0, m_BaseIterationTemp));
            GUI.enabled = true;

            if(!m_SetVersion
                && m_BuildMode == BuildMode.Test)
            {
                if ((m_BuildSettingsType == BuildSettingsType.Bundle && m_BuildBundleMode == BuildBundleMode.AllBundles)
                    || m_BuildSettingsType == BuildSettingsType.Player && m_BuildPlayerMode == BuildPlayerMode.RepackageBundle)
                {
                    if (GUILayout.Button("+1"))
                    {
                        m_BaseIterationTemp++;
                        m_SetVersion = !m_SetVersion;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SetHotUpdateIteration()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            m_HotUpdateIterationTemp = EditorGUILayout.IntField("HotUpdateIteration", Mathf.Max(0, m_HotUpdateIterationTemp));
            GUI.enabled = true;

            if (!m_SetVersion
                && m_BuildMode == BuildMode.Test)
            {
                if (m_BuildSettingsType == BuildSettingsType.Bundle && m_BuildBundleMode == BuildBundleMode.HotUpdateBundles)
                {
                    if (GUILayout.Button("+1"))
                    {
                        m_HotUpdateIterationTemp++;
                        m_SetVersion = !m_SetVersion;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ResetVersion()
        {
            if(m_SetVersion
                && !(m_BuildSettingsType == BuildSettingsType.Player && m_BuildPlayerMode == BuildPlayerMode.InoperationBundle))
            {
                if(GUILayout.Button("ResetVersion"))
                {
                    SetVersionInit();
                }
            }
        }
    }
}