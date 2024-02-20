using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth.Editor
{
    public class ContainerBuilder: IContainerBuilder
    {
        private readonly Action<List<ContainerBuilderItem>> assemblyResolver;
        
        private readonly List<ContainerBuilderItem> builder;

        public ContainerBuilder(Action<List<ContainerBuilderItem>> assemblyResolver, List<ContainerBuilderItem> builder)
        {
            this.assemblyResolver = assemblyResolver;
            this.builder = builder;
        }

        ContainerBuilderItem IContainerBuilder.Register<T>(Lifetime lifetime)
        {
            var register = new ContainerBuilderItem(typeof(T), lifetime);
            builder.Add(register);
            return register;
        }

        ContainerBuilderItem IContainerBuilder.Register<T>(Func<IObjectResolver, object> objectResolver, Lifetime lifetime)
        {
            var register = new ContainerBuilderItem(typeof(T), objectResolver, lifetime);
            builder.Add(register);
            return register;
        }

        ContainerBuilderItem IContainerBuilder.RegisterInstance<TInterface>(TInterface instance)
        {
            var register = new ContainerBuilderItem(typeof(TInterface), instance);
            builder.Add(register);
            return register;
        }

        void IContainerBuilder.CompleteConfiguration()
        {
            assemblyResolver.Invoke(builder);
        }
    }

    public class ContainerBuilderItem
    {
        public Type Type { get; }
        public Lifetime Lifetime { get; }
        public Func<ObjectResolver, object> ObjectResolverFunc { get; }
        public object Instance { get; }
        
        private List<Type>? keys;
        public ReadOnlyCollection<Type> Keys
        {
            get
            {
                if (keys == null)
                {
                    return new ReadOnlyCollection<Type>(new List<Type> { Type });
                }
                return new ReadOnlyCollection<Type>(keys);
            }
        }

        public ContainerBuilderItem(Type type, Lifetime lifetime)
        {
            Type = type;
            Lifetime = lifetime;
        }

        public ContainerBuilderItem(Type type, Func<ObjectResolver, object> objectResolverFunc, Lifetime  lifetime)
        {
            Type = type;
            ObjectResolverFunc = objectResolverFunc;
            this.Lifetime = lifetime;
        }

        public ContainerBuilderItem(Type type, object instance)
        {
            Type = type;
            Lifetime = Lifetime.Singleton;
            this.Instance = instance;
        }

        public ContainerBuilderItem As<T>()
        {
            var type = typeof(T);
            keys ??= new List<Type>();
            keys.Add(type);
            return this;
        }

        public ContainerBuilderItem AsSelf()
        {
            keys ??= new List<Type>();
            keys.Add(Type);
            return this;
        }
    }
}