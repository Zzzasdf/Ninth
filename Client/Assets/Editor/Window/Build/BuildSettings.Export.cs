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
                                            BuildAssetsCommand.BuildHotUpdateBundles(BuildTarget,
                                                BuildExportDirectoryType == BuildExportDirectoryType.Local ? AssetMode.LocalAB : AssetMode.RemoteAB,
                                                string.Join(".", majorVersionTemp, minorVersionTemp, RevisionNumber));

                                            if(BuildExportDirectoryType == BuildExportDirectoryType.Remote)
                                            {
                                                BuildAssetsCommand.RemoteApply();
                                            }
                                            break;
                                        }
                                    case BuildBundleMode.AllBundles:
                                        {
                                            BuildAssetsCommand.BuildAllBundles(BuildTarget,
                                                BuildExportDirectoryType == BuildExportDirectoryType.Local ? AssetMode.LocalAB : AssetMode.RemoteAB,
                                                string.Join(".", majorVersionTemp, minorVersionTemp, RevisionNumber));
                                            
                                            if (BuildExportDirectoryType == BuildExportDirectoryType.Remote)
                                            {
                                                BuildAssetsCommand.RemoteApply();
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
                                            BuildAssetsCommand.BuildPlayer(BuildTargetGroup, BuildTarget);
                                            break;
                                        }
                                    case BuildPlayerMode.RepackageBundle:
                                        {
                                            BuildAssetsCommand.BuildPlayerRepackage(BuildTargetGroup, BuildTarget,
                                                BuildExportDirectoryType == BuildExportDirectoryType.Local ? AssetMode.LocalAB : AssetMode.RemoteAB,
                                                string.Join(".", majorVersionTemp, minorVersionTemp, RevisionNumber));
                                            
                                            if (BuildExportDirectoryType == BuildExportDirectoryType.Remote)
                                            {
                                                BuildAssetsCommand.RemoteApply();
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