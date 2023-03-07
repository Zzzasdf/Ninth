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
            get => (BuildMode)PlayerPrefsDefine.BuildMode;
            set => PlayerPrefsDefine.BuildMode = (int)value;
        }

        private static int m_BigBaseVersionTemp;
        private static int m_BigBaseVersion
        {
            get => PlayerPrefsDefine.BigBaseVersion;
            set => PlayerPrefsDefine.BigBaseVersion = value;
        }

        private static int m_SmallBaseVersionTemp;
        private static int m_SmallBaseVersion
        {
            get => PlayerPrefsDefine.SmallBaseVersion;
            set => PlayerPrefsDefine.SmallBaseVersion = value;
        }

        private static int m_HotUpdateVersionTemp;
        private static int m_HotUpdateVersion
        {
            get => PlayerPrefsDefine.HotUpdateVersion;
            set => PlayerPrefsDefine.HotUpdateVersion = value;
        }

        private static int m_BaseIterationTemp;
        private static int m_BaseIteration
        {
            get => PlayerPrefsDefine.BaseIteration;
            set => PlayerPrefsDefine.BaseIteration = value;
        }

        private static int m_HotUpdateIterationTemp;
        private static int m_HotUpdateIteration
        {
            get => PlayerPrefsDefine.HotUpdateIteration;
            set => PlayerPrefsDefine.HotUpdateIteration = value;
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

        public enum BuildMode
        {
            Test,
            Formal
        }
    }
}