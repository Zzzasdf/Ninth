using System;
using System.Collections.Generic;
using Ninth.HotUpdate;
using Ninth.Utility;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.IO;
using System.Linq;
using NPOI.SS.UserModel;

namespace Ninth.Editor
{
    public partial class BuildProxy: IBuildProxy
    {
        private readonly IBuildConfig buildConfig;
        private readonly BuildWindow buildWindow;
        private readonly VersionConfig versionConfig;
        private readonly BuildBundleInfo buildBundleInfo;
        
        [Inject]
        public BuildProxy(IBuildConfig buildConfig, BuildWindow buildWindow, VersionConfig versionConfig, BuildBundleInfo buildBundleInfo)
        {
            this.buildConfig = buildConfig;
            this.buildWindow = buildWindow.Subscribe(Tab, Content, CheckForCompleteness, Export);
            this.versionConfig = versionConfig;
            this.buildBundleInfo = buildBundleInfo;
        }
        
        void IOnGUI.OnGUI()
        {
            (buildWindow as IStartable).Start();
        }

        private void Tab()
        {
            var barMenu = KeysByCommon().Select(x => x.key.ToString()).ToArray();
            var current = GetIntByEnumType<BuildSettingsMode>();
            var temp = GUILayout.Toolbar(current, barMenu);
            if (temp == current)
            {
                return;
            }
            SetIntByEnumType<BuildSettingsMode>(temp);
        }

        private BuildConfig.BuildSettings Content()
        {
            var current = (BuildSettingsMode)GetIntByEnumType<BuildSettingsMode>();
            return Get(current);
        }

        // 检查配置完整性
        private bool CheckForCompleteness(BuildConfig.BuildSettings build)
        {
            // bundle 打包临时缓存路径
            var bundlesTemp = GetStringByEnumType<BuildFolder>();
            if (string.IsNullOrEmpty(bundlesTemp))
            {
                "bundle 打包临时缓存路径不能为空".FrameError();
                return false;
            }
            // 打包路径检查
            var pathInfo = build.PathInfo;
            foreach (var path in pathInfo.Items)
            {
                if (!Directory.Exists(path.Folder))
                {
                    $"无法找到打包路径：{path.Folder}".FrameError();
                    return false;
                }
                if (path.Folder.Contains(Application.dataPath))
                {
                    $"打包路径: {path} 不能包含 {Application.dataPath}".FrameError();
                    return false;
                }
            }
            // 打包资源组检查
            var bundleInfo = build.BundleInfo;
            var localGroupPaths = bundleInfo.LocalGroupPaths;
            foreach (var path in localGroupPaths)
            {
                if (!Directory.Exists(path))
                {
                    $"无法找到 Local 打包资源组路径: {path}".FrameError();
                    return false;
                }
                if (!path.Contains(Application.dataPath))
                {
                    $"Local 打包资源组路径: {path} 必须包含 {Application.dataPath}".FrameError();
                    return false;
                }
            }
            var remoteGroupPaths = bundleInfo.RemoteGroupPaths;
            if (remoteGroupPaths.Count == 0)
            {
                "Remote 打包资源组至少为一个".FrameError();
                return false;
            }
            foreach (var path in remoteGroupPaths)
            {
                if (!Directory.Exists(path))
                {
                    $"无法找到 Remote 打包资源组路径: {path}".FrameError();
                    return false;
                }
                if (!path.Contains(Application.dataPath))
                {
                    $"Remote 打包资源组路径: {path} 必须包含 {Application.dataPath}".FrameError();
                    return false;
                }
            }
            // 拷贝路径检查
            var copyInfo = build.CopyInfo;
            if (!Directory.Exists(copyInfo.Folder))
            {
                $"无法找到拷贝路径：{copyInfo.Folder}".FrameError();
                return false;
            }
            // 版本检查
            var versionInfo = build.VersionInfo;
            if (string.IsNullOrEmpty(versionInfo.Display))
            {
                "客户端显示的版本为空".FrameWarning();
            }
            if (!versionInfo.IsModify)
            {
                "版本未更新".FrameError();
                return false;
            }
            return true;
        }

        private void Export(BuildConfig.BuildSettings build)
        {
            // 构建 bundle
            BuildBundles(build);
            // 拷贝 bundle

            // 构建 player
        }

        
        

        private void CopyBundles()
        {
            
        }

        private void BuildPlayer()
        {
            
        }

        private string GetStringByEnumType<TKeyEnum>() where TKeyEnum: Enum
        {
            return buildConfig.StringSubscribe.Get<TKeyEnum>();
        }

        private int GetIntByEnumType<TKeyEnum>() where TKeyEnum: Enum
        {
            return buildConfig.IntSubscribe.Get<TKeyEnum>();
        }

        private void SetIntByEnumType<TKeyEnum>(int value) where TKeyEnum: Enum
        {
            buildConfig.IntSubscribe.Set<TKeyEnum>(value);
        }

        private string GetString(Enum key)
        {
            return buildConfig.StringSubscribe.Get(key);
        }

        private BuildConfig.BuildSettings Get(BuildSettingsMode mode)
        {
            return buildConfig.BuildSettingsSubscribe.Get(mode);
        }

        private Dictionary<(Enum key, int markBit), ReactiveProperty<BuildConfig.BuildSettings>>.KeyCollection KeysByCommon()
        {
            return buildConfig.BuildSettingsSubscribe.KeysByCommon();
        }
    }
}
