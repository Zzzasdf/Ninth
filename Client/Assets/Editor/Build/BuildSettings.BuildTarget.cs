using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildSettings
    {
        private ActiveTargetMode m_ActiveTargetMode
        {
            get => (ActiveTargetMode)PlayerPrefsDefine.ActiveTargetMode;
            set => PlayerPrefsDefine.ActiveTargetMode = (int)value;
        }

        private BuildTarget m_BuildTarget
        {
            get => (BuildTarget)PlayerPrefsDefine.BuildTarget;
            set => PlayerPrefsDefine.BuildTarget = (int)value;
        }

        private BuildTargetGroup m_BuildTargetGroup
        {
            get => (BuildTargetGroup)PlayerPrefsDefine.BuildTargetGroup;
            set => PlayerPrefsDefine.BuildTargetGroup = (int)value;
        }

        private void SetToggleActiveTarget()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            string[] barMenu = new string[]
            {
                "ActiveTarget",
                "InactiveTarget"
            };
            m_ActiveTargetMode = (ActiveTargetMode)GUILayout.Toolbar((int)m_ActiveTargetMode, barMenu);
            if (m_ActiveTargetMode == ActiveTargetMode.ActiveTarget)
            {
                m_BuildTarget = EditorUserBuildSettings.activeBuildTarget;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SetBuildTarget()
        {
            if(m_ActiveTargetMode == ActiveTargetMode.ActiveTarget)
            {
                GUI.enabled = false;
            }
            string[] sArray = new string[]
            {
                 BuildTarget.StandaloneWindows64.ToString(),
                 BuildTarget.StandaloneWindows.ToString(),
                 BuildTarget.Android.ToString(),
                 BuildTarget.iOS.ToString(),
            };
            int[] iArray = new int[]
            {
                (int)BuildTarget.StandaloneWindows64,
                (int)BuildTarget.StandaloneWindows,
                (int)BuildTarget.Android,
                (int)BuildTarget.iOS,
            };
            m_BuildTarget = (BuildTarget)EditorGUILayout.IntPopup("BuildTarget", (int)m_BuildTarget, sArray, iArray);
            if (m_ActiveTargetMode == ActiveTargetMode.ActiveTarget)
            {
                GUI.enabled = true;
            }
        }

        private void SetBuildTargetGroup()
        {
            GUI.enabled = false;
            switch(m_BuildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneWindows:
                    {
                        m_BuildTargetGroup = BuildTargetGroup.Standalone;
                        break;
                    }
                case BuildTarget.Android:
                    {
                        m_BuildTargetGroup = BuildTargetGroup.Android;
                        break;
                    }
                case BuildTarget.iOS:
                    {
                        m_BuildTargetGroup = BuildTargetGroup.iOS;
                        break;
                    }
            }
            m_BuildTargetGroup = (BuildTargetGroup)EditorGUILayout.EnumPopup("BuildTargetGroup", m_BuildTargetGroup);
            GUI.enabled = true;
        }

        public enum ActiveTargetMode
        {
            ActiveTarget,
            InactiveTarget
        }
    }
}