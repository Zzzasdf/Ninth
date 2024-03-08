using System;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;
using UnityFS;
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
            subscribeResolverFunc += Window.WindowCollect.SubscribeResolver;
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
            builder.Register<VersionConfig>(resolver => resolver.Resolve<IJsonProxy>().ToObject<VersionConfig>(newIfNotExist: true), Lifetime.Singleton).AsSelf();
            
            builder.Register<WindowJson>(resolver => resolver.Resolve<IJsonProxy>().ToObject<WindowJson, Tab>(true), Lifetime.Singleton).AsSelf();
            builder.Register<WindowConfig>(Lifetime.Singleton).As<IWindowConfig>();
            builder.Register<WindowProxy>(Lifetime.Singleton).As<IWindowProxy>();
            
            builder.Register<BuildJson>(resolver => resolver.Resolve<IJsonProxy>().ToObject<BuildJson>(Tab.Build, true), Lifetime.Singleton).AsSelf();
            builder.Register<BuildConfig>(Lifetime.Singleton).As<IBuildConfig>();
            builder.Register<BuildProxy>(Lifetime.Singleton).As<IBuildProxy>();
            
            
            builder.Register<ExcelConfig>(Lifetime.Singleton).As<IExcelConfig>();
            builder.Register<ExcelProxy>(Lifetime.Singleton).As<IExcelProxy>();
            
            builder.Register<ScanConfig>(Lifetime.Singleton).As<IScanConfig>();
            builder.Register<ScanProxy>(Lifetime.Singleton).As<IScanProxy>();
            //
            // builder.Register<Window.OtherConfig>(Lifetime.Singleton).As<Window.IOtherConfig>();
            // builder.Register<Window.OtherProxy>(Lifetime.Singleton).As<Window.IOtherProxy>();
            //
            // builder.Register<Window.IExcelConfig>(Lifetime.Singleton).As<Window.IExcelConfig>();
            "编辑器 IOC 容器注册完成！！".FrameLog();
        }
    }
}