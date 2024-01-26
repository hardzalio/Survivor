using System;

public class Eventable<T> : IVariable<T> {
    private T _value;
    public T Value {
        get => _value;
        set {
            var oldValue = _value;
            _value = value;
            OnValueChanged?.Invoke(oldValue, _value);
        }
    }
    public Eventable(T value) {
        _value = value;
    }

    public Action<T, T> OnValueChanged { get; set; }
    public static implicit operator T(Eventable<T> eventable) => eventable.Value;
}
