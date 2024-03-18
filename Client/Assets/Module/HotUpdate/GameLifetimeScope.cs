using System;
using System.Threading;
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
            
            // hotUpdate
            var iAssetConfig = assetConfig as IAssetConfig;
            switch (iAssetConfig.RuntimeEnv())
            {
                case Ninth.Utility.Environment.NonAb:
                    builder.Register<AssetProxyLoadWithNonAB>(Lifetime.Scoped).As<IAssetProxyLoad>();
                    break;
                case Ninth.Utility.Environment.LocalAb:
                case Ninth.Utility.Environment.RemoteAb:
                    builder.Register<AssetProxyLoadWithAB>(Lifetime.Scoped).As<IAssetProxyLoad>();
                    break;
                default:
                    $"未注册该类型 {iAssetConfig.RuntimeEnv()}, 请检查或实现".FrameError();
                    return;
            }
            builder.Register<AssetProxy>(Lifetime.Singleton).As<IAssetProxy>();
            
            builder.Register<ViewConfig>(Lifetime.Singleton).As<IViewConfig>();
            builder.Register<ViewProxy>(Lifetime.Singleton).As<IViewProxy>();
            
            // builder.Register<HelloWorldService>(Lifetime.Singleton);
            // builder.Register<GamePresenter>(Lifetime.Singleton);
            
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                // entryPoints.Add<HotUpdateMain>();
                // entryPoints.Add<GamePresenter>();
                entryPoints.OnException(ex => ex.FrameError());
            }); 

            666.Log();
        }
    }
}