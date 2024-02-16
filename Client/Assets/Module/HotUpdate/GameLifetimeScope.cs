using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.HotUpdate
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private AssetConfig? assetConfig;
        [SerializeField] private NameConfig? nameConfig;
        protected override void Configure(IContainerBuilder builder)
        {
            if (assetConfig == null)
            {
                $"该类的组件 {nameof(AssetConfig) } 必须挂载".FrameError();
                return;
            }
            if (nameConfig == null)
            {
                $"该类的组件 {nameof(NameConfig) } 必须挂载".FrameError();
                return;
            }
            builder.RegisterInstance(assetConfig).As<IAssetConfig>();
            builder.RegisterInstance(nameConfig).As<INameConfig>();

            builder.Register<PlayerSettingsConfig>(Lifetime.Singleton).As<IPlayerSettingsConfig>();
            builder.Register<VersionPathConfig>(Lifetime.Singleton).As<IVersionPathConfig>();
            builder.Register<ConfigPathConfig>(Lifetime.Singleton).As<IConfigPathConfig>(); 
            builder.Register<BundlePathConfig>(Lifetime.Singleton).As<IBundlePathConfig>();
            builder.Register<PathProxy>(Lifetime.Singleton).As<IPathProxy>();

            IAssetConfig iAssetConfig = Container.Resolve<IAssetConfig>();
            switch (iAssetConfig.RuntimeEnv())
            {
                case Environment.NonAb:
                    builder.Register<AssetProxyLoadWithNonAB>(Lifetime.Scoped).As<IAssetProxyLoad>();
                    break;
                case Environment.LocalAb:
                case Environment.RemoteAb:
                    builder.Register<AssetProxyLoadWithAB>(Lifetime.Scoped).As<IAssetProxyLoad>();
                    break;
                default:
                    $"未注册该类型 {iAssetConfig.RuntimeEnv()}, 请检查或实现".FrameError();
                    return;
            }
            builder.Register<AssetProxy>(Lifetime.Singleton).As<IAssetProxy>();
            
            builder.Register<ViewConfig>(Lifetime.Singleton).As<IViewConfig>();
            builder.Register<ViewProxy>(Lifetime.Singleton).As<IViewProxy>();
            
            builder.Register<HelloWorldService>(Lifetime.Singleton);
            builder.Register<GamePresenter>(Lifetime.Singleton);
            
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                // entryPoints.Add<HotUpdateMain>();
                entryPoints.Add<GamePresenter>();
                entryPoints.OnException(ex => ex.FrameError());
            });
        }
    }
}