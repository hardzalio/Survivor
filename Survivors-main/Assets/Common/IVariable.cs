using System;

public interface IVariable<T> {
    T Value { get; set; }

    Action<T, T> OnValueChanged { get; set; }
}

public interface ICappedVariable : IVariable<float> {
    float MaxValue { get; set; }
}
