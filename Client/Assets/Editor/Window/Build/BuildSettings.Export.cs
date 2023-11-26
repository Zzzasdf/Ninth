using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildSettings
    {
        private ReadOnlyDictionary<BuildSettingsMode, ReadOnlyDictionary<Enum, Action>> _exportDic;
        
        private ReadOnlyDictionary<BuildSettingsMode, ReadOnlyDictionary<Enum, Action>> exportDic
        {
            get
            {
                if(_exportDic == null)
                {
                    _exportDic = new(new Dictionary<BuildSettingsMode, ReadOnlyDictionary<Enum, Action>>()
                    {
                        { BuildSettingsMode.Bundle, new(new Dictionary<Enum, Action>()
                        {
                            { BuildBundleMode.HotUpdateBundles, Bundle_HotUpdateBundles },
                            { BuildBundleMode.AllBundles, Bundle_AllBundles },
                        }) },
                        { BuildSettingsMode.Player, new(new Dictionary<Enum, Action>()
                        {
                            { BuildPlayerMode.InoperationBundle, Player_InoperationBundle },
                            { BuildPlayerMode.RepackageAllBundle, Player_RepackageAllBundle },
                        }) }
                    });
                }
                return _exportDic;
            }
        }
        
        
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
                if(exportDic.TryGetValue(buildSettingsMode, out ReadOnlyDictionary<Enum, Action> value))
                {
                    return;
                }
                Enum enumType = buildSettingsMode switch
                {
                    BuildSettingsMode.Bundle => buildBundleMode,
                    BuildSettingsMode.Player => buildPlayerMode,
                    _ => throw new NotImplementedException(),
                };
                if(!value.TryGetValue(enumType, out Action action))
                {
                    return;
                }
                Debug.Log(Log.Exporting.ToCurrLanguage());
                action?.Invoke();
                VersionSave();
                VersionRefresh();
                Debug.Log(Log.ExportCompleted);
            }
        }

        private void Bundle_HotUpdateBundles()
        {
            buildAssetsCmd.BuildHotUpdateBundles(buildTarget,
                                            buildExportCopyFolderMode == BuildExportCopyFolderMode.StreamingAssets ? AssetMode.LocalAB : AssetMode.RemoteAB,
                                            string.Join(".", majorVersionTemp, minorVersionTemp, revisionNumber));

            if (buildExportCopyFolderMode == BuildExportCopyFolderMode.Remote)
            {
                buildAssetsCmd.RemoteApply();
            }
        }

        private void Bundle_AllBundles()
        {
            buildAssetsCmd.BuildAllBundles(buildTarget,
                                           buildExportCopyFolderMode == BuildExportCopyFolderMode.StreamingAssets ? AssetMode.LocalAB : AssetMode.RemoteAB,
                                           string.Join(".", majorVersionTemp, minorVersionTemp, revisionNumber));

            if (buildExportCopyFolderMode == BuildExportCopyFolderMode.Remote)
            {
                buildAssetsCmd.RemoteApply();
            }
        }

        private void Player_InoperationBundle()
        {
            buildAssetsCmd.BuildPlayer(buildTargetGroup, buildTarget);
        }

        private void Player_RepackageAllBundle()
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