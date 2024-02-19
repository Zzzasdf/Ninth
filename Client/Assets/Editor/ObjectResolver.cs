using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ninth.Editor
{
    public class ObjectResolver
    {
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            return null;
        }
    }
}
