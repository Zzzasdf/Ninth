using UnityEngine;
using Zenject;

namespace Ninth.HotUpdate
{
    public class ConstructorInjectionInstaller : MonoInstaller<ConstructorInjectionInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IFoo>().To<Foo>().AsSingle();
            Container.Bind<Foo2>().AsTransient();

            // IFoo ifoo = Container.Resolve<IFoo>();
            // ifoo.Increase();
            // ifoo.Dump();
            Container.Resolve<IFoo>().Increase().Dump();
            Container.Resolve<Foo>().Increase().Dump();
            Container.Resolve<Foo2>().Increase().Dump();
        }
    }

    

    public interface IFoo
    {
        IFoo Increase();
        void Dump();
    }

    public class Foo: IFoo 
    {
        private int cnt;
        public IFoo Increase()
        {
            cnt++;
            return this;
        }

        public void Dump()
        {
            Debug.Log($"{GetType()}: {cnt}");
        }
    }

    public class Foo2 : IFoo
    {
        private int cnt;
        
        public IFoo Increase()
        {
            cnt++;
            return this;
        }

        public void Dump()
        {
            Debug.Log($"{GetType()}: {cnt * 100_000}");
        }

    }
}
