using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildSettings
    {
        private static bool bSetVersion;
        private static int majorVersionTemp;
        private static int minorVersionTemp;
        private static int revisionNumberTemp;

        private static int MajorVersion
        {
            get => WindowSOCore.Get<WindowBuildConfig>().MajorVersion;
            set => WindowSOCore.Get<WindowBuildConfig>().MajorVersion = value;
        }
        private static int MinorVersion
        {
            get => WindowSOCore.Get<WindowBuildConfig>().MinorVersion;
            set => WindowSOCore.Get<WindowBuildConfig>().MinorVersion = value;
        }
        private static int RevisionNumber
        {
            get => WindowSOCore.Get<WindowBuildConfig>().RevisionNumber;
            set => WindowSOCore.Get<WindowBuildConfig>().RevisionNumber = value;
        }

        private void VersionInit()
        {
            majorVersionTemp = MajorVersion;
            minorVersionTemp = MinorVersion;
            revisionNumberTemp = RevisionNumber;
            bSetVersion = false;
        }

        private void VersionSave()
        {
            MajorVersion = majorVersionTemp;
            MinorVersion = minorVersionTemp;
            RevisionNumber = revisionNumberTemp;
        }

        private void SetVersion()
        {
            SetBigBaseVersion();
            SetSmallBaseVersion();
            SetHotUpdateVersion();
            ResetVersion();
        }

        private void SetBigBaseVersion()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            majorVersionTemp = EditorGUILayout.IntField("MajorVersion", Mathf.Max(0, majorVersionTemp));
            GUI.enabled = true;

            if (!bSetVersion)
            {
                if ((BuildSettingsType == BuildSettingsType.Bundle && BuildBundleMode == BuildBundleMode.AllBundles)
                    || BuildSettingsType == BuildSettingsType.Player && BuildPlayerMode == BuildPlayerMode.RepackageBundle)
                {
                    if(GUILayout.Button("+1"))
                    {
                        majorVersionTemp++;
                        minorVersionTemp = 0;
                        revisionNumberTemp++;
                        bSetVersion = !bSetVersion;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SetSmallBaseVersion()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            minorVersionTemp = EditorGUILayout.IntField("MinorVersion", Mathf.Max(0, minorVersionTemp));
            GUI.enabled = true;

            if (!bSetVersion)
            {
                if ((BuildSettingsType == BuildSettingsType.Bundle && BuildBundleMode == BuildBundleMode.AllBundles)
                    || BuildSettingsType == BuildSettingsType.Player && BuildPlayerMode == BuildPlayerMode.RepackageBundle)
                {
                    if (GUILayout.Button("+1"))
                    {
                        minorVersionTemp++;
                        revisionNumberTemp++;
                        bSetVersion = !bSetVersion;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SetHotUpdateVersion()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            revisionNumberTemp = EditorGUILayout.IntField("RevisionNumber", Mathf.Max(0, revisionNumberTemp));
            GUI.enabled = true;

            if (!bSetVersion)
            {
                if (BuildSettingsType == BuildSettingsType.Bundle && BuildBundleMode == BuildBundleMode.HotUpdateBundles)
                {
                    if (GUILayout.Button("+1"))
                    {
                        revisionNumberTemp++;
                        bSetVersion = !bSetVersion;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ResetVersion()
        {
            if(bSetVersion
                && !(BuildSettingsType == BuildSettingsType.Player && BuildPlayerMode == BuildPlayerMode.InoperationBundle))
            {
                if(GUILayout.Button("ResetVersion"))
                {
                    VersionInit();
                }
            }
        }
    }
}