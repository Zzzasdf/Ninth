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
        private ReadOnlyDictionary<BuildSettingsMode, UnityAction> _tabActionDic;
        private ReadOnlyDictionary<BuildSettingsMode, UnityAction> tabActionDic
        { 
            get
            {
                if(_tabActionDic == null)
                {
                    _tabActionDic = new(new Dictionary<BuildSettingsMode, UnityAction>()
                    {
                        { BuildSettingsMode.Bundle, OnBundleGUI },
                        { BuildSettingsMode.Player, OnPlayerGUI },
                    });
                }
                return _tabActionDic;
            }
        }

        private BuildSettingsMode buildSettingsMode
        {
            get => WindowSOCore.Get<WindowBuildConfig>().BuildSettingsMode;
            set => WindowSOCore.Get<WindowBuildConfig>().BuildSettingsMode = value;
        }

        private bool bVersionInit;

        public void OnGUI()
        {
            if (!bVersionInit)
            {
                VersionRefresh();
                bVersionInit = !bVersionInit;
            }
            using (new GUILayout.VerticalScope())
            {
                RenderTag();
                RenderContent();
            }
        }

        private void RenderTag()
        {
            string[] barMenu = tabActionDic.Keys.ToArray().ToCurrLanguage();
            BuildSettingsMode buildSettingsModeTemp = (BuildSettingsMode)GUILayout.Toolbar((int)buildSettingsMode, barMenu);
            if (buildSettingsModeTemp != buildSettingsMode)
            {
                VersionRefresh();
                buildSettingsMode = buildSettingsModeTemp;
            }
        }

        private void RenderContent()
        {
            tabActionDic[buildSettingsMode]?.Invoke();
        }

        #region Bundle
        private void OnBundleGUI()
        {
            GUILayout.Space(10);
            GUILayout.Label(CommonLanguage.ExportTargetFolderSettings.ToCurrLanguage(), EditorStyles.boldLabel);
            RenderBuildBundlesTargetFolderRootAndBrowse();
            RenderBuildBundleMode();
            RenderToolbarByBuildExportCopyFolderMode();
            RenderToolbarByActiveTarget();
            RenderPopupByBuildTarget();
            RenderVersion();
            RenderBtnResetByVersion();
            RenderExport();
        }
        #endregion

        #region Player
        private void OnPlayerGUI()
        {
            GUILayout.Space(10);
            GUILayout.Label(CommonLanguage.ExportTargetFolderSettings.ToCurrLanguage(), EditorStyles.boldLabel);
            RenderBuildBundlesTargetFolderRootAndBrowse();
            RenderBuildPlayersTargetFolderRootAndBrowse();
            RenderBuildAllBundles();
            RenderToolbarByBuildExportCopyFolderMode();
            RenderToolbarByActiveTarget();
            RenderPopupByBuildTarget();
            RenderPopupByBuildTargetGroup();
            RenderVersion();
            RenderBtnResetByVersion();
            RenderExport();
        }
        #endregion
    }
}
