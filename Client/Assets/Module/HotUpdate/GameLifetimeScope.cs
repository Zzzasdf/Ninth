using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.HotUpdate
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private HelloScreen helloScreen;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<HotUpdateMain>(Lifetime.Singleton);
            
            builder.Register<GamePresenter>(Lifetime.Singleton);
            builder.Register<HelloWorldService>(Lifetime.Singleton);
            builder.Register<HelloScreen>(container =>
            {
                return container.Instantiate(helloScreen, GameObject.Find("Canvas").transform);
            }, Lifetime.Scoped);

            
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<HotUpdateMain>();
                entryPoints.Add<GamePresenter>();
            });
        }
    }
}

