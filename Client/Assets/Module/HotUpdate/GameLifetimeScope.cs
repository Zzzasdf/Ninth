using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.HotUpdate
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private AssetConfig assetConfig;
        [SerializeField] private NameConfig nameConfig;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(assetConfig);
            builder.RegisterInstance(nameConfig);

            builder.Register<PathConfig>(Lifetime.Singleton);
            builder.Register<PlatformConfig>(Lifetime.Singleton);

            builder.Register<AssetProxy>(Lifetime.Singleton).As<IAssetProxy>();

            builder.Register<HelloWorldService>(Lifetime.Singleton);
            builder.Register<GamePresenter>(Lifetime.Singleton);
            
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                // entryPoints.Add<HotUpdateMain>();
                entryPoints.Add<GamePresenter>();
                // entryPoints.OnException(ex => ex.Log());
            });
        }
    }
}