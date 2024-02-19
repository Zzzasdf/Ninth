using System;
using System.Collections.Generic;

namespace Ninth.Editor
{
    public enum Lifetime
    {
        Transient,
        Singleton,
        Scoped,
    }
    
    public class LifetimeScope
    {
        private static List<RegistrationBuilder> builder = new();
        private static List<RegistrationBuilder> instanceBuilder = new();
        public static ContainerBuilder ContainerBuilder;

        static LifetimeScope()
        {
            ContainerBuilder = new ContainerBuilder(builder, instanceBuilder);
        }

        
        

        
    }
}