using System;

namespace Ninth.Utility
{
    public class ReactivePropertyFunc<T> : IReactiveProperty<T>
    {
        private readonly Func<T> valueFunc;
        public T Value => valueFunc.Invoke();
        
        public ReactivePropertyFunc(Func<T> valueFuncFunc)
        {
            valueFunc = valueFuncFunc;
        }
    }
}