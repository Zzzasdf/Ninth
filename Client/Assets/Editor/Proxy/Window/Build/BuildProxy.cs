using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Ninth.HotUpdate;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.Editor
{
    public class BuildProxy: IBuildProxy
    {
        private readonly IBuildConfig buildConfig;
        private readonly BuildWindow buildWindow;
        
        [Inject]
        public BuildProxy(IBuildConfig buildConfig, BuildWindow buildWindow)
        {
            this.buildConfig = buildConfig;
            this.buildWindow = buildWindow.Subscribe(Tab, Content);
        }
        
        void IOnGUI.OnGUI()
        {
            (buildWindow as IStartable).Start();
        }

        private void Tab()
        {
            var barMenu = TabKeys().ToArrayString();
            var current = Get<BuildSettingsMode>();
            var temp = GUILayout.Toolbar(current, barMenu);
            if (temp == current)
            {
                return;
            }
            Set<BuildSettingsMode>(temp);
        }

        BuildConfig.BuildSettings Content()
        {
            var current = (BuildSettingsMode)Get<BuildSettingsMode>();
            return Get(current);
        }

        private int Get<TKeyEnum>() where TKeyEnum: Enum
        {
            return buildConfig.IntEnumTypeSubscribe.Get<TKeyEnum>();
        }

        private void Set<TEnumKey>(int value) where TEnumKey: Enum
        {
            buildConfig.IntEnumTypeSubscribe.Set<TEnumKey>(value);
        }

        private BuildConfig.BuildSettings Get(BuildSettingsMode mode)
        {
            return buildConfig.TabCommonSubscribe.Get(mode);
        }

        private Dictionary<BuildSettingsMode, LinkedListReactiveProperty<BuildConfig.BuildSettings>>.KeyCollection TabKeys()
        {
            return buildConfig.TabCommonSubscribe.Keys();
        }
    }
}
