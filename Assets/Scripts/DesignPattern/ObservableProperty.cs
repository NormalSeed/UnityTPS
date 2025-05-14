using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DesignPattern
{
    public class ObservableProperty<T>
    {
        [SerializeField] private T _value;
        public T Value
        {
            get => _value;
            set
            {
                if (value.Equals(value)) return;
                _value = value;
                Notify();
            }
        }

        private UnityEvent<T> onValueChanged = new();

        public ObservableProperty(T value = default)
        {
            _value = value;
        }

        public void Subscribe(UnityAction<T> action)
        {
            onValueChanged.AddListener(action);
        }

        public void Unsubscribe(UnityAction<T> action)
        {
            onValueChanged.RemoveListener(action);
        }

        public void UnsubscribeAll()
        {
            onValueChanged.RemoveAllListeners();
        }

        private void Notify()
        {
            onValueChanged?.Invoke(Value);
        }
    }
}
