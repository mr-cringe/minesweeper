using System;
using System.Collections.Generic;

namespace Minesweeper.Utils
{
    public class ReactiveProperty<T>
    {
        private T _value;
        private HashSet<Action<T>> _subscribers = new HashSet<Action<T>>();

        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value))
                {
                    return;
                }

                _value = value;
                Notify();
            }
        }

        private void Notify()
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber?.Invoke(_value);
            }

            _subscribers.RemoveWhere(x => x == null);
        }

        public void Subscribe(Action<T> action)
        {
            if (_subscribers.Add(action))
            {
                action?.Invoke(_value);
            }
        }

        public void Unsubscribe(Action<T> action)
        {
            _subscribers.Remove(action);
        }
    }
}