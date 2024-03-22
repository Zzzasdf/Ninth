using Cysharp.Threading.Tasks;
using Ninth.Utility;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.HotUpdate
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            "HotUpdate 初始化".FrameLog();
            // core
            var assetConfig = Resources.Load<AssetConfig>("SOData/AssetConfigSO") as IAssetConfig;
            var nameConfig = Resources.Load<NameConfig>("SOData/NameConfigSO") as INameConfig;
            builder.RegisterInstance(assetConfig).As<IAssetConfig>();
            builder.RegisterInstance(nameConfig).As<INameConfig>();
            
            builder.Register<PlayerSettingsConfig>(Lifetime.Singleton).As<IPlayerSettingsConfig>();
            builder.Register<VersionPathConfig>(Lifetime.Singleton).As<IVersionPathConfig>();
            builder.Register<ConfigPathConfig>(Lifetime.Singleton).As<IConfigPathConfig>(); 
            builder.Register<BundlePathConfig>(Lifetime.Singleton).As<IBundlePathConfig>();
            builder.Register<PathProxy>(Lifetime.Singleton).As<IPathProxy>();
            
            builder.Register<JsonConfig>(Lifetime.Singleton).As<IJsonConfig>();
            builder.Register<JsonProxy>(Lifetime.Singleton).As<IJsonProxy>();
            
            // hotUpdate
            switch (assetConfig.RuntimeEnv())
            {
                case Environment.NonAb:
                    builder.Register<AssetProxyLoadWithNonAB>(Lifetime.Scoped).As<IAssetProxyLoad>();
                    break;
                case Environment.LocalAb:
                case Environment.RemoteAb:
                    builder.Register<AssetProxyLoadWithAB>(Lifetime.Scoped).As<IAssetProxyLoad>();
                    break;
                default:
                    $"未注册该类型 {assetConfig.RuntimeEnv()}, 请检查或实现".FrameError();
                    return;
            }
            builder.Register<AssetProxy>(Lifetime.Singleton).As<IAssetProxy>();
            
            builder.Register<ViewConfig>(Lifetime.Singleton).As<IViewConfig>();
            builder.Register<ViewProxy>(Lifetime.Singleton).As<IViewProxy>();
            
            builder.Register<LoginCtrl>(Lifetime.Transient).AsSelf();
            builder.Register<LoginModel>(Lifetime.Transient).AsSelf();
            builder.Register<LoginInputSystem>(Lifetime.Transient).AsSelf();

            builder.Register<StartUp>(Lifetime.Scoped).AsSelf();
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<StartUp>();
                entryPoints.OnException(ex => ex.FrameError());
            });
            "HotUpdate IOC 容器注册完成！！".FrameLog(); 
        }
    }
}