using System;

namespace Ninth.Editor
{
    public interface IObjectResolver
    {
        T Resolve<T>();

        protected object Resolve(Type type);
    }
}