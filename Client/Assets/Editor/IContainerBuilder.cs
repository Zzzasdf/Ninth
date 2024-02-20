using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Editor
{
    public interface IContainerBuilder
    {
        ContainerBuilderItem Register<T>(Lifetime lifetime);
        ContainerBuilderItem Register<T>(Func<IObjectResolver, object> objectResolver, Lifetime lifetime);
        ContainerBuilderItem RegisterInstance<TInterface>(TInterface instance);
        void CompleteConfiguration();
    }
}
