using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildSettings
    {
        private void RenderExport()
        {
            if (!bUpgradeVersion)
            {
                GUI.enabled = false;
                if (GUILayout.Button(CommonLanguage.PleaseAdjustTheVersion.ToCurrLanguage()))
                {
                }
                GUI.enabled = true;
                return;
            }
            if (GUILayout.Button(CommonLanguage.Export.ToCurrLanguage()))
            {
                Action action = buildSettingsMode switch
                {
                    BuildSettingsMode.Bundle => buildBundleMode switch
                    {
                        BuildBundleMode.HotUpdateBundles => CreateHotUpdateBundles,
                        BuildBundleMode.AllBundles => CreateAllBundles,
                        _ => null,
                    },
                    BuildSettingsMode.Player => CreatePlayerAndAllBundles,
                    _ => null,
                };
                if(action == null)
                {
                    Debug.LogError(Log.FuncIsNull.ToCurrLanguage());
                    return;
                }
                Debug.Log(Log.Exporting.ToCurrLanguage());
                action.Invoke();
                VersionSave();
                VersionRefresh();
                Debug.Log(Log.ExportCompleted);
            }
        }

        // 构建热更ab
        private void CreateHotUpdateBundles()
        {
            buildAssetsCmd.BuildHotUpdateBundles(buildTarget,
                                            buildExportCopyFolderMode == BuildExportCopyFolderMode.StreamingAssets ? AssetMode.LocalAB : AssetMode.RemoteAB,
                                            string.Join(".", majorVersionTemp, minorVersionTemp, revisionNumber));

            if (buildExportCopyFolderMode == BuildExportCopyFolderMode.Remote)
            {
                buildAssetsCmd.RemoteApply();
            }
        }

        // 构建所有ab
        private void CreateAllBundles()
        {
            buildAssetsCmd.BuildAllBundles(buildTarget,
                                           buildExportCopyFolderMode == BuildExportCopyFolderMode.StreamingAssets ? AssetMode.LocalAB : AssetMode.RemoteAB,
                                           string.Join(".", majorVersionTemp, minorVersionTemp, revisionNumber));

            if (buildExportCopyFolderMode == BuildExportCopyFolderMode.Remote)
            {
                buildAssetsCmd.RemoteApply();
            }
        }

        // 构建客户端和所有ab
        private void CreatePlayerAndAllBundles()
        {
            buildAssetsCmd.BuildPlayerAndAllBundles(buildTargetGroup, buildTarget,
                                            buildExportCopyFolderMode == BuildExportCopyFolderMode.StreamingAssets ? AssetMode.LocalAB : AssetMode.RemoteAB,
                                            string.Join(".", majorVersionTemp, minorVersionTemp, revisionNumber));

            if (buildExportCopyFolderMode == BuildExportCopyFolderMode.Remote)
            {
                buildAssetsCmd.RemoteApply();
            }
        }
    }
}