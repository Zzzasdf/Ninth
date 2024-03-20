using Ninth.Utility;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            "Main 初始化".FrameLog();
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
            
            // upgrade process
            builder.Register<PlayerPrefsConfig>(Lifetime.Singleton).As<IPlayerPrefsIntConfig, IPlayerPrefsFloatConfig, IPlayerPrefsStringConfig>();
            builder.Register<PlayerPrefsProxy>(Lifetime.Singleton).As<IPlayerPrefsIntProxy, IPlayerPrefsFloatProxy, IPlayerPrefsStringProxy>();

            builder.Register<DownloadProxy>(Lifetime.Singleton).As<IDownloadProxy>();
            builder.Register<AssetDownloadBox>(Lifetime.Scoped).As<IAssetDownloadBox>();
            builder.Register<AssetBundleProxy>(Lifetime.Scoped).As<IAssetBundleProxy>();

            builder.Register<LoadDll>(Lifetime.Scoped);
            
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<AssetBundleProxy>();
                entryPoints.OnException(ex => ex.FrameError());
            });
            "Main IOC 容器注册完成！！".FrameLog(); 
        }
    }
}