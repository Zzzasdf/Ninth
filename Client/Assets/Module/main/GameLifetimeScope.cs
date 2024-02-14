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
            builder.RegisterInstance(assetConfig).As<IAssetConfig>();
            builder.RegisterInstance(nameConfig).As<INameConfig>();
            
            builder.Register<PlayerSettings>(Lifetime.Singleton).As<IPlayerSettings>();
            builder.Register<VersionPath>(Lifetime.Singleton).As<IVersionPath>();
            builder.Register<ConfigPath>(Lifetime.Singleton).As<IConfigPath>(); 
            builder.Register<BundlePath>(Lifetime.Singleton).As<IBundlePath>();
            builder.Register<PathProxy>(Lifetime.Singleton).As<IPathProxy>();
            
            builder.Register<JsonConfig>(Lifetime.Singleton).As<IJsonConfig>();
            builder.Register<JsonProxy>(Lifetime.Singleton).As<IJsonProxy>();

            builder.Register<PlayerPrefsConfig>(Lifetime.Singleton).As<IPlayerPrefsIntConfig, IPlayerPrefsFloatConfig, IPlayerPrefsStringConfig>();
            builder.Register<PlayerPrefsProxy>(Lifetime.Singleton).As<IPlayerPrefsIntProxy, IPlayerPrefsFloatProxy, IPlayerPrefsStringProxy>();

            builder.Register<DownloadProxy>(Lifetime.Singleton).As<IDownloadProxy>();
            builder.Register<AssetBundleProxy>(Lifetime.Transient).As<IAssetBundleProxy>();
            
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
            builder.Register<MessageBox>(Lifetime.Scoped); // 接口，下载专用
            
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<ProcedureProxy>();
                entryPoints.OnException(ex => ex.FrameError());
            });
        }
    }
}