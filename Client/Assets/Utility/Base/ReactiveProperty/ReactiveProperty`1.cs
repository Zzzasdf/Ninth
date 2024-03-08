using System;

namespace Ninth.Utility
{
    public class ReactiveProperty<T>
    {
        private T latestValue;
        private Action<T>? triggerEvents;
        public T Value
        {
            get => latestValue;
            set
            {
                latestValue = value;
                triggerEvents?.Invoke(value);
            }
        }
        
        public ReactiveProperty(T value) => latestValue = value;

        public ReactiveProperty<T> AsSetEvent(Action<T> triggerEvent)
        {
            triggerEvents += triggerEvent;
            return this;
        }

        public ReactiveProperty<TResult> AsEnum<TResult>() where TResult: struct, Enum
        {
            if (latestValue is int intValue
                && Enum.IsDefined(typeof(TResult), intValue))  
            {  
                var reactiveProperty = new ReactiveProperty<TResult>((TResult)(object)intValue);
                if (triggerEvents is Action<int> intTriggerEvents)
                {
                    reactiveProperty.AsSetEvent(e =>
                    {
                        intTriggerEvents.Invoke((int)(object)e);
                    });
                }
                return reactiveProperty;
            }  
            else
            {
                throw new InvalidOperationException($"无法转换，类型 {typeof(T)} 无法转换到 {typeof(TResult)}");
            } 
        }
    }
}