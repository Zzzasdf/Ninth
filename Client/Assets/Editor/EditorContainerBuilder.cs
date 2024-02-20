using System.Collections;
using System.Collections.Generic;
using Ninth.Utility;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.Editor
{
    public class EditorContainerBuilder : ContainerBuilder // 基类包装写成 LifetimeScope
    {
        public static IObjectResolver Resolver;
        
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            var builder = new EditorContainerBuilder();
        }
        
        public EditorContainerBuilder()
        {
            "编辑器初始化！！".FrameLog(); 
            var builder = this;
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
            // builder.Register<Window.WindowConfig>(resolver =>
            // {
            //     var jsonProxy = resolver.Resolve<IJsonProxy>();
            //     return jsonProxy.ToObject<Window.WindowConfig>() ?? new Window.WindowConfig();
            // }, Lifetime.Singleton).As<Window.IWindowConfig>();
            builder.Register<Window.WindowConfig>(Lifetime.Singleton).As<Window.IWindowConfig>();
            builder.Register<Window.WindowProxy>(Lifetime.Singleton).As<Window.IWindowProxy>();
            
            // builder.Register<Window.BuildConfig>(Lifetime.Singleton).As<Window.IBuildConfig>();
            // builder.Register<Window.BuildProxy>(Lifetime.Singleton).As<Window.IBuildProxy>();
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
             Resolver = builder.Build();
            "编辑器 IOC 容器注册完成！！".FrameLog(); 
        }
    }

}