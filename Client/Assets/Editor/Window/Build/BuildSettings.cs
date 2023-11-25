using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Ninth.Editor
{
    public sealed partial class BuildSettings
    {
        private readonly BuildAssetsCommand buildAssetsCmd;

        public BuildSettings(BuildAssetsCommand buildAssetsCmd)
        {
            this.buildAssetsCmd = buildAssetsCmd;
        }

        private ReadOnlyDictionary<BuildSettingsType, UnityAction> TabActionDic = new(new Dictionary<BuildSettingsType, UnityAction>()
        {
            { BuildSettingsType.Bundle, new BuildBundleSettings().OnGUI },
            { BuildSettingsType.Player, new BuildPlayerSettings().OnGUI },
        });

        private BuildSettingsType BuildSettingsType
        {
            get => WindowSOCore.Get<WindowBuildConfig>().BuildSettingsType;
            set => WindowSOCore.Get<WindowBuildConfig>().BuildSettingsType = value;
        }

        private BuildBundleMode BuildBundleMode
        {
            get => WindowSOCore.Get<WindowBuildConfig>().BuildBundleMode;
            set => WindowSOCore.Get<WindowBuildConfig>().BuildBundleMode = value;
        }

        private BuildPlayerMode BuildPlayerMode
        {
            get => WindowSOCore.Get<WindowBuildConfig>().BuildPlayerMode;
            set => WindowSOCore.Get<WindowBuildConfig>().BuildPlayerMode = value;
        }

        private bool bVersionInit;

        public void OnGUI()
        {
            if (bVersionInit)
            {
                VersionInit();
                bVersionInit = !bVersionInit;
            }
            using (new EditorGUILayout.VerticalScope())
            {
                RenderTag();
                switch (BuildSettingsType)
                {
                    case BuildSettingsType.Bundle:
                        {
                            SetBrowseBundlesTargetDirectory();
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                GUILayout.FlexibleSpace();
                                string[] barMenuBundle = new string[]
                                {
                                BuildBundleMode.HotUpdateBundles.ToString(),
                                BuildBundleMode.AllBundles.ToString()
                                };
                                BuildBundleMode buildBundleMode = (BuildBundleMode)GUILayout.Toolbar((int)BuildBundleMode, barMenuBundle);
                                if (buildBundleMode != BuildBundleMode)
                                {
                                    VersionInit();
                                    BuildBundleMode = buildBundleMode;
                                }
                            }
                            SetBtnExchangeExportDirectory();
                            SetToggleActiveTarget();
                            SetBuildTarget();
                            SetVersion();
                            SetExport();
                            break;
                        }
                    case BuildSettingsType.Player:
                        {
                            SetBrowseBundlesTargetDirectory();
                            SetBrowsePlayersTargetDirectory();
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                GUILayout.FlexibleSpace();
                                string[] barMenuPlayer = new string[]
                                {
                                BuildPlayerMode.InoperationBundle.ToString(),
                                BuildPlayerMode.RepackageBundle.ToString()
                                };
                                BuildPlayerMode buildPlayerMode = (BuildPlayerMode)GUILayout.Toolbar((int)BuildPlayerMode, barMenuPlayer);
                                if (buildPlayerMode != BuildPlayerMode)
                                {
                                    VersionInit();
                                    BuildPlayerMode = buildPlayerMode;
                                }
                            }
                            SetBtnExchangeExportDirectory();
                            SetToggleActiveTarget();
                            SetBuildTarget();
                            SetBuildTargetGroup();
                            if(BuildPlayerMode == BuildPlayerMode.RepackageBundle)
                            {
                                SetVersion();
                            }
                            SetExport();
                            break;
                        }
                }
            }
        }

        private void RenderTag()
        {
            string[] barMenu = TabActionDic.Keys.Select(x => x.ToString()).ToArray();
            BuildSettingsType buildSettingsType = (BuildSettingsType)GUILayout.Toolbar((int)BuildSettingsType, barMenu);
            if (buildSettingsType != BuildSettingsType)
            {
                VersionInit();
                BuildSettingsType = buildSettingsType;
            }
        }

        public class BuildBundleSettings
        {
            public void OnGUI()
            {

            }
        }
        
        public class BuildPlayerSettings
        {
            public void OnGUI()
            {

            }
        }
    }
}
