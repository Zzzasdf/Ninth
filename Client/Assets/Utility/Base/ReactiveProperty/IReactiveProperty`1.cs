using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninth.Utility
{
    public interface IReactiveProperty<out T>
    {
        public T Value { get; }
    }
}