using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Editor
{
    public partial class BuildSettings
    {
        private void SetExport()
        {
            if (m_BuildSettingsType == BuildSettingsType.Player && m_BuildPlayerMode == BuildPlayerMode.InoperationBundle)
            {
                m_SetVersion = true;
            }
            if (!m_SetVersion)
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
                    switch (m_BuildSettingsType)
                    {
                        case BuildSettingsType.Bundle:
                            {
                                switch(m_BuildBundleMode)
                                {
                                    case BuildBundleMode.HotUpdateBundles:
                                        {
                                            BuildAssetsCommand.BuildHotUpdateBundles(m_BuildTarget,
                                                m_BuildExportDirectoryType == BuildExportDirectoryType.Local ? AssetMode.LocalAB : AssetMode.RemoteAB,
                                                string.Join(".", m_BigBaseVersionTemp, m_SmallBaseVersionTemp, m_HotUpdateVersion, m_BaseIterationTemp, m_HotUpdateIterationTemp));

                                            if(m_BuildExportDirectoryType == BuildExportDirectoryType.Remote)
                                            {
                                                BuildAssetsCommand.RemoteApply();
                                            }
                                            break;
                                        }
                                    case BuildBundleMode.AllBundles:
                                        {
                                            BuildAssetsCommand.BuildAllBundles(m_BuildTarget,
                                                m_BuildExportDirectoryType == BuildExportDirectoryType.Local ? AssetMode.LocalAB : AssetMode.RemoteAB,
                                                string.Join(".", m_BigBaseVersionTemp, m_SmallBaseVersionTemp, m_HotUpdateVersion, m_BaseIterationTemp, m_HotUpdateIterationTemp));
                                            
                                            if (m_BuildExportDirectoryType == BuildExportDirectoryType.Remote)
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
                                switch(m_BuildPlayerMode)
                                {
                                    case BuildPlayerMode.InoperationBundle:
                                        {
                                            BuildAssetsCommand.BuildPlayer(m_BuildTargetGroup, m_BuildTarget);
                                            break;
                                        }
                                    case BuildPlayerMode.RepackageBundle:
                                        {
                                            BuildAssetsCommand.BuildPlayerRepackage(m_BuildTargetGroup, m_BuildTarget,
                                                m_BuildExportDirectoryType == BuildExportDirectoryType.Local ? AssetMode.LocalAB : AssetMode.RemoteAB,
                                                string.Join(".", m_BigBaseVersionTemp, m_SmallBaseVersionTemp, m_HotUpdateVersion, m_BaseIterationTemp, m_HotUpdateIterationTemp));
                                            
                                            if (m_BuildExportDirectoryType == BuildExportDirectoryType.Remote)
                                            {
                                                BuildAssetsCommand.RemoteApply();
                                            }
                                            break;
                                        }
                                }
                                break;
                            }
                    }
                    VersionStore();
                    SetVersionInit();
                    UnityEngine.Debug.Log("Export completed！！");
                }
            }
        }
    }
}