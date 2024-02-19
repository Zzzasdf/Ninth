using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Ninth.Editor
{
    public class ContainerBuilder
    {
        private readonly List<RegistrationBuilder> builder;
        private readonly List<RegistrationBuilder> instanceBuilder;

        public ContainerBuilder(List<RegistrationBuilder> builder, List<RegistrationBuilder> instanceBuilder)
        {
            this.builder = builder;
            this.instanceBuilder = instanceBuilder;
        }

        public RegistrationBuilder Register<T>(Lifetime lifetime)
        {
            var register = new RegistrationBuilder(typeof(T), lifetime);
            builder.Add(register);
            return register;
        }

        public RegistrationBuilder RegisterInstance<TInterface>(TInterface instance)
        {
            var register = new RegistrationBuilder(typeof(TInterface), instance);
            instanceBuilder.Add(register);
            return register;
        }

        public void UseEntryPoints(Lifetime lifetime) //, Action<EntryPointsBuilder> configuration)
        {
            builder.Count.Log();
        }
        // {
        //     EntryPointsBuilder.EnsureDispatcherRegistered(builder);
        //     configuration(new EntryPointsBuilder(builder, lifetime));
        // }
    }

    public class RegistrationBuilder
    {
        public Type Type { get; private set; }
        public object instance { get; set; }
        public Lifetime Lifetime { get; private set; }
        private List<Type>? keys;

        public ReadOnlyCollection<Type> Keys
        {
            get
            {
                if (keys == null)
                {
                    return new ReadOnlyCollection<Type>(new List<Type> { Type });
                }

                return new ReadOnlyCollection<Type>(Keys);
            }
        }

        public RegistrationBuilder(Type type, Lifetime lifetime)
        {
            Type = type;
            Lifetime = lifetime;
        }

        public RegistrationBuilder(Type type, object instance)
        {
            Type = type;
            Lifetime = Lifetime.Singleton;
            this.instance = instance;
        }

        public RegistrationBuilder As<T>()
        {
            var type = typeof(T);
            keys ??= new List<Type>();
            keys.Add(type);
            return this;
        }

        public RegistrationBuilder AsSelf()
        {
            keys ??= new List<Type>();
            keys.Add(Type);
            return this;
        }
    }
}