using System;
using System.Collections.Generic;

namespace Ninth.Editor
{
    public enum Lifetime
    {
        Transient,
        Singleton,
    }
    
    public class LifetimeScope
    {
        public static IContainerBuilder ContainerBuilder { get; private set; }
        public static IObjectResolver IObjectResolver { get; private set; }

        static LifetimeScope()
        {
            var builder = new List<ContainerBuilderItem>();
            ContainerBuilder = new ContainerBuilder(AssemblyResolver, builder);
        }

        private static void AssemblyResolver(List<ContainerBuilderItem> builderItems)
        {
            IObjectResolver = new ObjectResolver(builderItems);
        }
    }
}