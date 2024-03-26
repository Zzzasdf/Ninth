using Cysharp.Threading.Tasks;
using Ninth.Utility;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;
using VContainer.Unity;
using Environment = Ninth.Utility.Environment;

namespace Ninth.HotUpdate
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            "HotUpdate 初始化".FrameLog();
#if UNITY_EDITOR
            builder.Register<AssetProxyLoadWithNonAB>(Lifetime.Scoped).As<IAssetProxyLoad>();
#else
            var nameConfig = Resources.Load<NameConfig>("SOData/NameConfigSO");
            var playerVersionConfig = GameDriver.PlayerVersionConfig;
            builder.RegisterInstance(playerVersionConfig).AsSelf();
            builder.RegisterInstance(nameConfig).As<INameConfig>();
            
            builder.Register<PlayerSettingsConfig>(Lifetime.Singleton).As<IPlayerSettingsConfig>();
            builder.Register<VersionPathConfig>(Lifetime.Singleton).As<IVersionPathConfig>();
            builder.Register<ConfigPathConfig>(Lifetime.Singleton).As<IConfigPathConfig>(); 
            builder.Register<BundlePathConfig>(Lifetime.Singleton).As<IBundlePathConfig>();
            builder.Register<PathProxy>(Lifetime.Singleton).As<IPathProxy>();
            
            builder.Register<AssetProxyLoadWithAB>(Lifetime.Scoped).As<IAssetProxyLoad>();
#endif
            builder.Register<JsonConfig>(Lifetime.Singleton).As<IJsonConfig>();
            builder.Register<JsonProxy>(Lifetime.Singleton).As<IJsonProxy>();
            
            builder.Register<AssetProxy>(Lifetime.Singleton).As<IAssetProxy>();
            
            builder.Register<ViewConfig>(Lifetime.Singleton).As<IViewConfig>();
            builder.Register<ViewProxy>(Lifetime.Singleton).As<IViewProxy>();
            
            builder.Register<LoginCtrl>(Lifetime.Transient).AsSelf();
            builder.Register<LoginInputSystem>(Lifetime.Transient).AsSelf();

            builder.Register<SettingsCtrl>(Lifetime.Transient).AsSelf();
            builder.Register<SettingsModel>(Lifetime.Transient).AsSelf();
            builder.Register<SettingsInputSystem>(Lifetime.Transient).AsSelf();

            builder.Register<StartUp>(Lifetime.Scoped);
            builder.UseEntryPoints(Lifetime.Singleton, configuration =>
            {
                configuration.Add<StartUp>();
                configuration.OnException(ex => ex.FrameError());
            });
            "HotUpdate IOC 容器注册完成！！".FrameLog(); 
        }
    }
}