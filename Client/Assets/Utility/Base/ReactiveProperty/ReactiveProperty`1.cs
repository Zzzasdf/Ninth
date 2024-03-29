using System;

namespace Ninth.Utility
{
    public class ReactiveProperty<T>: IReactiveProperty<T>
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
    }
}