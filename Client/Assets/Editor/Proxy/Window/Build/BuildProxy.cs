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
using System.IO;

namespace Ninth.Editor
{
    public class BuildProxy: IBuildProxy
    {
        private readonly IBuildConfig buildConfig;
        private readonly BuildWindow buildWindow;
        private readonly IAssetConfig assetConfig;
        private readonly INameConfig nameConfig;
        
        [Inject]
        public BuildProxy(IBuildConfig buildConfig, BuildWindow buildWindow, IAssetConfig assetConfig, INameConfig nameConfig)
        {
            this.buildConfig = buildConfig;
            this.buildWindow = buildWindow.Subscribe(Tab, Content, CheckForCompleteness, Export);
            this.assetConfig = assetConfig;
            this.nameConfig = nameConfig;
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

        private BuildConfig.BuildSettings Content()
        {
            var current = (BuildSettingsMode)Get<BuildSettingsMode>();
            return Get(current);
        }

        // 检查配置完整性
        private bool CheckForCompleteness(BuildConfig.BuildSettings build)
        {
            // 打包路径检查
            var paths = build.Paths;
            foreach (var path in paths)
            {
                if (!Directory.Exists(path.Folder))
                {
                    $"无法找到打包路径：{path.Folder}".FrameError();
                    return false;
                }
            }
            // 拷贝路径检查
            var copyMode = build.CopyMode;
            if (!Directory.Exists(copyMode.Folder))
            {
                $"无法找到拷贝路径：{copyMode.Folder}".FrameError();
                return false;
            }
            // 版本检查
            var version = build.Version;
            if (string.IsNullOrEmpty(version.Display))
            {
                "客户端显示的版本为空".FrameWarning();
            }
            if (!version.IsModify)
            {
                "版本未更新".FrameError();
                return false;
            }
            return true;
        }

        private void Export(BuildConfig.BuildSettings build)
        {
            var buildTargetMode = build.BuildTargetMode;
            // 构建 bundle
            BuildBundles();
            // 拷贝 bundle

            // 构建 player
        }

        private void BuildBundles()
        {
            // var gAssets = $"{Application.dataPath}/GAssets";
            // var remoteGroups = new List<string>
            // {
            //     "RemoteGroup",
            // };
            // var localGroup = new List<string>();
            // var gAssetsInfo = new DirectoryInfo(gAssets);
            // DirectoryInfo[] gAssetsFolders = gAssetsInfo.GetDirectories();
            // foreach (var item in gAssetsFolders)
            // {
            //     if (!remoteGroups.Contains(item.Name))
            //     {
            //         localGroup.Add(item.Name.Log());
            //     }
            // }
            //
            // Dictionary<AssetLocate, List<string>> bundleSorts = new()
            // {
            //     []
            // };

            //
            // for (int index = 0; index < groupListArgs.Length; index++)
            // {
            //     var groupLst = groupListArgs[index].groupList;
            //     AssetLocate assetLocate = groupListArgs[index].assetLocate;
            //
            //     foreach (var groupName in groupLst)
            //     {
            //         string groupPath = gAssets + "/" + groupName;
            //         DirectoryInfo groupDir = new DirectoryInfo(groupPath);
            //         ScanChildDireations(groupDir, assetLocate);
            //     }
            // }
        }

        private void CopyBundles()
        {
            
        }

        private void BuildPlayer()
        {
            
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
