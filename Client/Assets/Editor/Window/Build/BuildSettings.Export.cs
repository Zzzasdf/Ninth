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
                Action<string> action = buildSettingsMode switch
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
                string version = string.Join(".", majorVersionTemp, minorVersionTemp, revisionNumber);
                action.Invoke(version);
                VersionSave();
                VersionRefresh();
                Debug.Log(Log.ExportCompleted);
            }
        }

        // �����ȸ�ab
        private void CreateHotUpdateBundles(string version)
        {
            AssetMode assetMode = buildExportCopyFolderMode switch
            {
                BuildExportCopyFolderMode.StreamingAssets => AssetMode.LocalAB,
                BuildExportCopyFolderMode.Remote => AssetMode.RemoteAB,
                _ => default,
            };
            if(assetMode == default)
            {
                Debug.LogError($"δ�ҵ���ģʽ��{assetMode}");
                return;
            }
            BuildAssetsCommand buildAssetsCmd = EditorEntry.BuildAssetsCmd;
            buildAssetsCmd.BuildHotUpdateBundles(buildTarget, assetMode, version);
            if (buildExportCopyFolderMode == BuildExportCopyFolderMode.Remote)
            {
                buildAssetsCmd.RemoteApply();
            }
        }

        // ��������ab
        private void CreateAllBundles(string version)
        {
            AssetMode assetMode = buildExportCopyFolderMode switch
            {
                BuildExportCopyFolderMode.StreamingAssets => AssetMode.LocalAB,
                BuildExportCopyFolderMode.Remote => AssetMode.RemoteAB,
                _ => default,
            };
            if (assetMode == default)
            {
                Debug.LogError($"δ�ҵ���ģʽ��{assetMode}");
                return;
            }
            BuildAssetsCommand buildAssetsCmd = EditorEntry.BuildAssetsCmd;
            buildAssetsCmd.BuildAllBundles(buildTarget, assetMode, version);
            if (buildExportCopyFolderMode == BuildExportCopyFolderMode.Remote)
            {
                buildAssetsCmd.RemoteApply();
            }
        }

        // �����ͻ��˺�����ab
        private void CreatePlayerAndAllBundles(string version)
        {
            AssetMode assetMode = buildExportCopyFolderMode switch
            {
                BuildExportCopyFolderMode.StreamingAssets => AssetMode.LocalAB,
                BuildExportCopyFolderMode.Remote => AssetMode.RemoteAB,
                _ => default,
            };
            if (assetMode == default)
            {
                Debug.LogError($"δ�ҵ���ģʽ��{assetMode}");
                return;
            }
            BuildAssetsCommand buildAssetsCmd = EditorEntry.BuildAssetsCmd;
            buildAssetsCmd.BuildPlayerAndAllBundles(buildTargetGroup, buildTarget, assetMode, version);
            if (buildExportCopyFolderMode == BuildExportCopyFolderMode.Remote)
            {
                buildAssetsCmd.RemoteApply();
            }
        }
    }
}