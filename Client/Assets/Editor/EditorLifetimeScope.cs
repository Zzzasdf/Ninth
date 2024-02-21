using System;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace Ninth.Editor
{
    public class EditorLifetimeScope : LifetimeScope
    {
        [InitializeOnLoadMethod]
        private static void Initialization()
        {
            var resolver = new EditorLifetimeScope().Build();
            Action<IObjectResolver>? subscribeResolverFunc = null;
            subscribeResolverFunc += Window.WindowProxy.SubscribeResolver;
            subscribeResolverFunc.Invoke(resolver);
        }
        
        protected override void Configure(IContainerBuilder builder)
        {
            "编辑器初始化！！".FrameLog();
            // core
            var assetConfig = Resources.Load<AssetConfig>("SOData/AssetConfigSO");
            var nameConfig = Resources.Load<NameConfig>("SOData/NameConfigSO");
            builder.RegisterInstance(assetConfig).As<IAssetConfig>();
            builder.RegisterInstance(nameConfig).As<INameConfig>();
            
            builder.Register<PlayerSettingsConfig>(Lifetime.Singleton).As<IPlayerSettingsConfig>();
            builder.Register<VersionPathConfig>(Lifetime.Singleton).As<IVersionPathConfig>();
            builder.Register<ConfigPathConfig>(Lifetime.Singleton).As<IConfigPathConfig>();
            builder.Register<BundlePathConfig>(Lifetime.Singleton).As<IBundlePathConfig>();
            builder.Register<PathProxy>(Lifetime.Singleton).As<IPathProxy>();
            
            builder.Register<JsonConfig>(Lifetime.Singleton).As<IJsonConfig>();
            builder.Register<JsonProxy>(Lifetime.Singleton).As<IJsonProxy>();
             
            // editor
            builder.Register<Window.WindowConfig>(resolver =>
            {
                var windowConfig = resolver.Resolve<IJsonProxy>().ToObject<Window.WindowConfig, Window.Tab>(false);
                return windowConfig ?? new Window.WindowConfig();
            },Lifetime.Singleton).As<Window.IWindowConfig>();
            builder.Register<Window.WindowProxy>(Lifetime.Singleton).As<Window.IWindowProxy>();
            
            builder.Register<Window.BuildConfig>(Lifetime.Singleton).As<Window.IBuildConfig>();
            builder.Register<Window.BuildProxy>(Lifetime.Singleton).As<Window.IBuildProxy>();
            //
            // builder.Register<Window.ExcelConfig>(Lifetime.Singleton).As<Window.IExcelConfig>();
            // builder.Register<Window.ExcelProxy>(Lifetime.Singleton).As<Window.IExcelProxy>();
            //
            // builder.Register<Window.ScanConfig>(Lifetime.Singleton).As<Window.IScanConfig>();
            // builder.Register<Window.ScanProxy>(Lifetime.Singleton).As<Window.IScanProxy>();
            //
            // builder.Register<Window.OtherConfig>(Lifetime.Singleton).As<Window.IOtherConfig>();
            // builder.Register<Window.OtherProxy>(Lifetime.Singleton).As<Window.IOtherProxy>();
            //
            // builder.Register<Window.IExcelConfig>(Lifetime.Singleton).As<Window.IExcelConfig>();
            "编辑器 IOC 容器注册完成！！".FrameLog();
        }
    }

}