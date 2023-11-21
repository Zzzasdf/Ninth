using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildSettings
    {
        private void SetExport()
        {
            if (BuildSettingsType == BuildSettingsType.Player && BuildPlayerMode == BuildPlayerMode.InoperationBundle)
            {
                bSetVersion = true;
            }
            if (!bSetVersion)
            {
                GUI.enabled = false;
                if (GUILayout.Button("Please Adjust The Version"))
                {
                    
                }
                GUI.enabled = true;
            }
            else
            {
                if (GUILayout.Button("Export"))
                {
                    UnityEngine.Debug.Log("Exporting！！");
                    switch (BuildSettingsType)
                    {
                        case BuildSettingsType.Bundle:
                            {
                                switch(BuildBundleMode)
                                {
                                    case BuildBundleMode.HotUpdateBundles:
                                        {
                                            buildAssetsCmd.BuildHotUpdateBundles(BuildTarget,
                                                BuildExportDirectoryType == BuildExportDirectoryType.Local ? AssetMode.LocalAB : AssetMode.RemoteAB,
                                                string.Join(".", majorVersionTemp, minorVersionTemp, RevisionNumber));

                                            if(BuildExportDirectoryType == BuildExportDirectoryType.Remote)
                                            {
                                                buildAssetsCmd.RemoteApply();
                                            }
                                            break;
                                        }
                                    case BuildBundleMode.AllBundles:
                                        {
                                            buildAssetsCmd.BuildAllBundles(BuildTarget,
                                                BuildExportDirectoryType == BuildExportDirectoryType.Local ? AssetMode.LocalAB : AssetMode.RemoteAB,
                                                string.Join(".", majorVersionTemp, minorVersionTemp, RevisionNumber));
                                            
                                            if (BuildExportDirectoryType == BuildExportDirectoryType.Remote)
                                            {
                                                buildAssetsCmd.RemoteApply();
                                            }
                                            break;
                                        }
                                }
                                break;
                            }
                        case BuildSettingsType.Player:
                            {
                                switch(BuildPlayerMode)
                                {
                                    case BuildPlayerMode.InoperationBundle:
                                        {
                                            buildAssetsCmd.BuildPlayer(BuildTargetGroup, BuildTarget);
                                            break;
                                        }
                                    case BuildPlayerMode.RepackageBundle:
                                        {
                                            buildAssetsCmd.BuildPlayerRepackage(BuildTargetGroup, BuildTarget,
                                                BuildExportDirectoryType == BuildExportDirectoryType.Local ? AssetMode.LocalAB : AssetMode.RemoteAB,
                                                string.Join(".", majorVersionTemp, minorVersionTemp, RevisionNumber));
                                            
                                            if (BuildExportDirectoryType == BuildExportDirectoryType.Remote)
                                            {
                                                buildAssetsCmd.RemoteApply();
                                            }
                                            break;
                                        }
                                }
                                break;
                            }
                    }
                    VersionSave();
                    VersionInit();
                    UnityEngine.Debug.Log("Export completed！！");
                }
            }
        }
    }
}