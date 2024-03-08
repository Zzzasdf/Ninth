using System;

namespace Ninth.Utility
{
    public class ReactivePropertyFunc<T>
    {
        private readonly Func<T> valueFunc;
        public T Value => valueFunc.Invoke();
        
        public ReactivePropertyFunc(Func<T> valueFuncFunc)
        {
            valueFunc = valueFuncFunc;
        }

        // public ReactivePropertyFunc<TResult> AsEnum<TResult>() where TResult : struct, Enum
        // {
        //     if (valueFunc is Func<int> intValueFunc
        //         && Enum.IsDefined(typeof(TResult), intValueFunc.Invoke()))  
        //     {
        //         return new ReactivePropertyFunc<TResult>((Func<TResult>)(object)intValueFunc);
        //     }  
        //     else
        //     {
        //         throw new InvalidOperationException($"无法转换，类型 {typeof(T)} 无法转换到 {typeof(TResult)}");
        //     } 
        // }
    }
}