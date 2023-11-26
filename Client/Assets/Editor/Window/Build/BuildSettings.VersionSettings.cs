using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildSettings
    {
        private static bool bUpgradeVersion;
        private static int majorVersionTemp;
        private static int minorVersionTemp;
        private static int revisionNumberTemp;

        private static int majorVersion
        {
            get => WindowSOCore.Get<WindowBuildConfig>().MajorVersion;
            set => WindowSOCore.Get<WindowBuildConfig>().MajorVersion = value;
        }
        private static int minorVersion
        {
            get => WindowSOCore.Get<WindowBuildConfig>().MinorVersion;
            set => WindowSOCore.Get<WindowBuildConfig>().MinorVersion = value;
        }
        private static int revisionNumber
        {
            get => WindowSOCore.Get<WindowBuildConfig>().RevisionNumber;
            set => WindowSOCore.Get<WindowBuildConfig>().RevisionNumber = value;
        }

        private void VersionRefresh()
        {
            majorVersionTemp = majorVersion;
            minorVersionTemp = minorVersion;
            revisionNumberTemp = revisionNumber;
            bUpgradeVersion = false;
        }

        private void VersionSave()
        {
            majorVersion = majorVersionTemp;
            minorVersion = minorVersionTemp;
            revisionNumber = revisionNumberTemp;
        }

        private void RenderVersion()
        {
            RenderMajorVersion();
            RenderMinorVersion();
            RenderRevisionNumber();
        }

        private void RenderMajorVersion()
        {
            using (new GUILayout.HorizontalScope())
            {
                GUI.enabled = false;
                majorVersionTemp = EditorGUILayout.IntField(CommonLanguage.MajorVersion.ToCurrLanguage(), Mathf.Max(0, majorVersionTemp));
                GUI.enabled = true;
                if (bUpgradeVersion)
                {
                    return;
                }
                bool isActiveBtnAdd = buildSettingsMode switch
                {
                    BuildSettingsMode.Bundle => buildBundleMode == BuildBundleMode.AllBundles,
                    BuildSettingsMode.Player => true,
                    _ => false
                };
                if(!isActiveBtnAdd)
                {
                    return;
                }
                if (GUILayout.Button("+1"))
                {
                    majorVersionTemp++;
                    minorVersionTemp = 0;
                    revisionNumberTemp++;
                    bUpgradeVersion = !bUpgradeVersion;
                }
            }
        }

        private void RenderMinorVersion()
        {
            using (new GUILayout.HorizontalScope())
            {
                GUI.enabled = false;
                minorVersionTemp = EditorGUILayout.IntField(CommonLanguage.MinorVersion.ToCurrLanguage(), Mathf.Max(0, minorVersionTemp));
                GUI.enabled = true;

                if (bUpgradeVersion)
                {
                    return;
                }
                bool isActiveBtnAdd = buildSettingsMode switch
                {
                    BuildSettingsMode.Bundle => buildBundleMode == BuildBundleMode.AllBundles,
                    BuildSettingsMode.Player => true,
                    _ => false
                };
                if(!isActiveBtnAdd)
                {
                    return;
                }
                if (GUILayout.Button("+1"))
                {
                    minorVersionTemp++;
                    revisionNumberTemp++;
                    bUpgradeVersion = !bUpgradeVersion;
                }
            }
        }

        private void RenderRevisionNumber()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUI.enabled = false;
                revisionNumberTemp = EditorGUILayout.IntField(CommonLanguage.RevisionNumber.ToCurrLanguage(), Mathf.Max(0, revisionNumberTemp));
                GUI.enabled = true;

                if (bUpgradeVersion)
                {
                    return;
                }
                bool isActiveBtnAdd = buildSettingsMode switch
                {
                    BuildSettingsMode.Bundle => buildBundleMode == BuildBundleMode.HotUpdateBundles,
                    _ => false
                };
                if (!isActiveBtnAdd)
                {
                    return;
                }
                if (GUILayout.Button("+1"))
                {
                    revisionNumberTemp++;
                    bUpgradeVersion = !bUpgradeVersion;
                }
            }
        }

        private void RenderBtnResetByVersion()
        {
            if(majorVersionTemp == majorVersion
                && minorVersionTemp == minorVersion
                && revisionNumberTemp == revisionNumber)
            {
                return;
            }
            if (GUILayout.Button(CommonLanguage.ResetVersion.ToCurrLanguage()))
            {
                VersionRefresh();
            }
        }
    }
}