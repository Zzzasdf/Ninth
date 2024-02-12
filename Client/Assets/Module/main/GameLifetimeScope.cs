using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private AssetConfig? assetConfig;
        [SerializeField] private NameConfig? nameConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            if (assetConfig == null)
            {
                $"该类的组件 { nameof(AssetConfig) } 必须挂载".FrameError();
                return;
            }
            if (nameConfig == null)
            {
                $"该类的组件 { nameof(NameConfig) } 必须挂载".FrameError();
                return;
            }
            builder.RegisterInstance(assetConfig);
            builder.RegisterInstance(nameConfig);
            
            builder.Register<PathConfig>(Lifetime.Singleton);
            builder.Register<PlatformConfig>(Lifetime.Singleton);

            builder.Register<PlayerPrefsConfig>(Lifetime.Singleton)
                .As<IPlayerPrefsIntConfig, IPlayerPrefsFloatConfig, IPlayerPrefsStringConfig>();
            builder.Register<PlayerPrefsProxy>(Lifetime.Singleton)
                .As<IPlayerPrefsIntProxy, IPlayerPrefsFloatProxy, IPlayerPrefsStringProxy>();

            builder.Register<JsonConfig>(Lifetime.Singleton).As<IJsonConfig>();
            builder.Register<JsonProxy>(Lifetime.Singleton).As<IJsonProxy>();

            builder.Register<DownloadProxy>(Lifetime.Singleton).As<IDownloadProxy>();
            
            builder.Register<Launcher>(Lifetime.Scoped);
            builder.Register<CompareVersion>(Lifetime.Scoped);
            builder.Register<CompareDownloadConfig>(Lifetime.Scoped);
            builder.Register<IncreaseBundles>(Lifetime.Scoped);
            builder.Register<DownloadLoadConfig>(Lifetime.Scoped);
            builder.Register<UpdateConfig>(Lifetime.Scoped);
            builder.Register<ScanDecreaseBundles>(Lifetime.Scoped);
            builder.Register<StartUp>(Lifetime.Scoped);
            builder.Register<LoadDll>(Lifetime.Scoped);
            builder.Register<ProcedureProxy>(Lifetime.Scoped).As<IProcedureProxy>();
            
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<ProcedureProxy>();
                entryPoints.OnException(ex => ex.FrameError());
            });
        }
    }
}