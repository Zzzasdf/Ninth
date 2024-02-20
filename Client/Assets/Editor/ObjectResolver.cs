using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using dnlib.DotNet;

namespace Ninth.Editor
{
    public class ObjectResolver: IObjectResolver
    {
        private readonly Dictionary<Type, ObjectResolverItem> container;
        
        public ObjectResolver(List<ContainerBuilderItem> containerBuilderItems)
        {
            container = new Dictionary<Type, ObjectResolverItem>();
            foreach (var item in containerBuilderItems)
            {
                var objectResolverItem = new ObjectResolverItem(this, item);
                foreach (var type in item.Keys)
                {
                    container.Add(type, objectResolverItem);
                }
            }
        }

        T IObjectResolver.Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        object IObjectResolver.Resolve(Type type)
        {
            return Resolve(type);
        }
        
        private object Resolve(Type type)
        {
            return container[type].Resolve();
        }
    }

    public class ObjectResolverItem
    {
        private readonly Type type;
        private Func<ObjectResolver, object>? objectResolverFunc;
        private object? instance;
        private Func<object> resolveFunc;
        
        public ObjectResolverItem(ObjectResolver objectResolver, ContainerBuilderItem builderItem)
        {
            type = builderItem.Type;
            instance = builderItem.Instance;
            switch(builderItem.Lifetime)
            {
                case Lifetime.Singleton:
                {
                    if (objectResolverFunc == null)
                    {
                        resolveFunc = () =>
                        {
                            if (instance == null)
                            {
                                instance = Activator.CreateInstance(type);
                            }
                            return instance;
                        };
                    }
                    else
                    {
                        resolveFunc = () =>
                        {
                            if (instance == null)
                            {
                                instance = objectResolverFunc.Invoke(objectResolver);
                            }
                            return instance;
                        };
                    }
                } 
                break;
                case Lifetime.Transient:
                {
                    if (objectResolverFunc == null)
                    {
                        resolveFunc = () => Activator.CreateInstance(type);
                    }
                    else
                    {
                        resolveFunc = () => objectResolverFunc.Invoke(objectResolver);
                    }
                }
                break;
            }
        }

        public object Resolve()
        {
            return resolveFunc.Invoke();
        }
    }
}
