using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildSettings
    {
        private ActiveTargetMode ActiveTargetMode
        {
            get => WindowSOCore.Get<WindowBuildConfig>().ActiveTargetMode;
            set => WindowSOCore.Get<WindowBuildConfig>().ActiveTargetMode = value;
        }

        private BuildTarget BuildTarget
        {
            get => WindowSOCore.Get<WindowBuildConfig>().BuildTarget;
            set => WindowSOCore.Get<WindowBuildConfig>().BuildTarget = value;
        }

        private BuildTargetGroup BuildTargetGroup
        {
            get => WindowSOCore.Get<WindowBuildConfig>().BuildTargetGroup;
            set => WindowSOCore.Get<WindowBuildConfig>().BuildTargetGroup = value;
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
            ActiveTargetMode = (ActiveTargetMode)GUILayout.Toolbar((int)ActiveTargetMode, barMenu);
            if (ActiveTargetMode == ActiveTargetMode.ActiveTarget)
            {
                BuildTarget = EditorUserBuildSettings.activeBuildTarget;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SetBuildTarget()
        {
            if (ActiveTargetMode == ActiveTargetMode.ActiveTarget)
            {
                GUI.enabled = false;
            }
            string[] sArray = new string[]
            {
                 BuildTarget.StandaloneWindows64.ToString(),
                 BuildTarget.Android.ToString(),
                 BuildTarget.iOS.ToString(),
            };
            int[] iArray = new int[]
            {
                (int)BuildTarget.StandaloneWindows64,
                (int)BuildTarget.Android,
                (int)BuildTarget.iOS,
            };
            BuildTarget = (BuildTarget)EditorGUILayout.IntPopup("BuildTarget", (int)BuildTarget, sArray, iArray);
            if (ActiveTargetMode == ActiveTargetMode.ActiveTarget)
            {
                GUI.enabled = true;
            }
        }

        private void SetBuildTargetGroup()
        {
            GUI.enabled = false;
            switch (BuildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneWindows:
                    {
                        BuildTargetGroup = BuildTargetGroup.Standalone;
                        break;
                    }
                case BuildTarget.Android:
                    {
                        BuildTargetGroup = BuildTargetGroup.Android;
                        break;
                    }
                case BuildTarget.iOS:
                    {
                        BuildTargetGroup = BuildTargetGroup.iOS;
                        break;
                    }
            }
            BuildTargetGroup = (BuildTargetGroup)EditorGUILayout.EnumPopup("BuildTargetGroup", BuildTargetGroup);
            GUI.enabled = true;
        }
    }
}