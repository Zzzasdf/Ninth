using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ninth.HotUpdate
{
    public class GameLifetimeScope : LifetimeScope
    {
        public HelloScreen helloScreen;
        protected override void Configure(IContainerBuilder builder)
        {
            // builder.Register<HotUpdateMain>(Lifetime.Singleton);
            // 可以解析出一张 UI 路径表 .. 妙！！！
            builder.RegisterComponentInNewPrefab(objectResolver => Resources.Load<HelloScreen>("Test/VContainer/HelloScreen").Log() , Lifetime.Singleton);
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