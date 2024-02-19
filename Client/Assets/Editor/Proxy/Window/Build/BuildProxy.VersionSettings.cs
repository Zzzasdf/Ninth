// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
//
// namespace Ninth.Editor.Window
// {
//     public partial class BuildProxy
//     {
//         private static bool bUpgradeVersion;
//         private static int majorVersionTemp;
//         private static int minorVersionTemp;
//         private static int versionRevisionNumberTemp;
//         private static int revisionNumberTemp;
//
//         private static int majorVersion
//         {
//             get => WindowSOCore.Get<BuildSO>().MajorVersion;
//             set => WindowSOCore.Get<BuildSO>().MajorVersion = value;
//         }
//         private static int minorVersion
//         {
//             get => WindowSOCore.Get<BuildSO>().MinorVersion;
//             set => WindowSOCore.Get<BuildSO>().MinorVersion = value;
//         }
//         private int versionRevisionNumber
//         {
//             get => WindowSOCore.Get<BuildSO>().VersionRevisionNumber;
//             set => WindowSOCore.Get<BuildSO>().VersionRevisionNumber = value;
//         }
//         private static int revisionNumber
//         {
//             get => WindowSOCore.Get<BuildSO>().RevisionNumber;
//             set => WindowSOCore.Get<BuildSO>().RevisionNumber = value;
//         }
//
//         private void VersionRefresh()
//         {
//             majorVersionTemp = majorVersion;
//             minorVersionTemp = minorVersion;
//             versionRevisionNumberTemp = versionRevisionNumber;
//             revisionNumberTemp = revisionNumber;
//             bUpgradeVersion = false;
//         }
//
//         private void VersionSave()
//         {
//             majorVersion = majorVersionTemp;
//             minorVersion = minorVersionTemp;
//             versionRevisionNumber = versionRevisionNumberTemp;
//             revisionNumber = revisionNumberTemp;
//         }
//
//         private void RenderVersion()
//         {
//             RenderMajorVersion();
//             RenderMinorVersion();
//             RenderVersionRevisionNumber();
//             RenderRevisionNumber();
//         }
//
//         private void RenderMajorVersion()
//         {
//             using (new GUILayout.HorizontalScope())
//             {
//                 GUI.enabled = false;
//                 majorVersionTemp = EditorGUILayout.IntField(CommonLanguage.MajorVersion.ToCurrLanguage(), Mathf.Max(0, majorVersionTemp));
//                 GUI.enabled = true;
//                 if (bUpgradeVersion)
//                 {
//                     return;
//                 }
//                 bool isActiveBtnAdd = buildSettingsMode switch
//                 {
//                     BuildSettingsMode.Bundle => buildBundleMode == BuildBundleMode.AllBundles,
//                     BuildSettingsMode.Player => true,
//                     _ => false
//                 };
//                 if(!isActiveBtnAdd)
//                 {
//                     return;
//                 }
//                 if (GUILayout.Button("+1"))
//                 {
//                     majorVersionTemp++;
//                     minorVersionTemp = 0;
//                     versionRevisionNumberTemp = 0;
//                     revisionNumberTemp++;
//                     bUpgradeVersion = !bUpgradeVersion;
//                 }
//             }
//         }
//
//         private void RenderMinorVersion()
//         {
//             using (new GUILayout.HorizontalScope())
//             {
//                 GUI.enabled = false;
//                 minorVersionTemp = EditorGUILayout.IntField(CommonLanguage.MinorVersion.ToCurrLanguage(), Mathf.Max(0, minorVersionTemp));
//                 GUI.enabled = true;
//
//                 if (bUpgradeVersion)
//                 {
//                     return;
//                 }
//                 bool isActiveBtnAdd = buildSettingsMode switch
//                 {
//                     BuildSettingsMode.Bundle => buildBundleMode == BuildBundleMode.AllBundles,
//                     BuildSettingsMode.Player => true,
//                     _ => false
//                 };
//                 if(!isActiveBtnAdd)
//                 {
//                     return;
//                 }
//                 if (GUILayout.Button("+1"))
//                 {
//                     minorVersionTemp++;
//                     versionRevisionNumberTemp = 0;
//                     revisionNumberTemp++;
//                     bUpgradeVersion = !bUpgradeVersion;
//                 }
//             }
//         }
//
//         private void RenderVersionRevisionNumber()
//         {
//             using (new EditorGUILayout.HorizontalScope())
//             {
//                 GUI.enabled = false;
//                 versionRevisionNumberTemp = EditorGUILayout.IntField(CommonLanguage.VersionRevisionNumber.ToCurrLanguage(), Mathf.Max(0, versionRevisionNumberTemp));
//                 GUI.enabled = true;
//
//                 if (bUpgradeVersion)
//                 {
//                     return;
//                 }
//                 bool isActiveBtnAdd = buildSettingsMode switch
//                 {
//                     BuildSettingsMode.Bundle => buildBundleMode == BuildBundleMode.AllBundles,
//                     BuildSettingsMode.Player => true,
//                     _ => false
//                 };
//                 if (!isActiveBtnAdd)
//                 {
//                     return;
//                 }
//                 if (GUILayout.Button("+1"))
//                 {
//                     versionRevisionNumberTemp++;
//                     revisionNumberTemp++;
//                     bUpgradeVersion = !bUpgradeVersion;
//                 }
//             }
//         }
//
//         private void RenderRevisionNumber()
//         {
//             using (new EditorGUILayout.HorizontalScope())
//             {
//                 GUI.enabled = false;
//                 revisionNumberTemp = EditorGUILayout.IntField(CommonLanguage.RevisionNumber.ToCurrLanguage(), Mathf.Max(0, revisionNumberTemp));
//                 GUI.enabled = true;
//
//                 if (bUpgradeVersion)
//                 {
//                     return;
//                 }
//                 bool isActiveBtnAdd = buildSettingsMode switch
//                 {
//                     BuildSettingsMode.Bundle => true,
//                     _ => false
//                 };
//                 if (!isActiveBtnAdd)
//                 {
//                     return;
//                 }
//                 if (GUILayout.Button("+1"))
//                 {
//                     revisionNumberTemp++;
//                     bUpgradeVersion = !bUpgradeVersion;
//                 }
//             }
//         }
//
//         private void RenderBtnResetByVersion()
//         {
//             if(majorVersionTemp == majorVersion
//                 && minorVersionTemp == minorVersion
//                 && versionRevisionNumberTemp == versionRevisionNumber
//                 && revisionNumberTemp == revisionNumber)
//             {
//                 return;
//             }
//             if (GUILayout.Button(CommonLanguage.ResetVersion.ToCurrLanguage()))
//             {
//                 VersionRefresh();
//             }
//         }
//     }
// }