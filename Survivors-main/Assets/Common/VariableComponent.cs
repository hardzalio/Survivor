using System;
using UnityEngine;

public class VariableComponent : MonoBehaviour, ICappedVariable {
    public Eventable<float> value;
    public float maxValue;

    public float Value { get => value; set => this.value.Value = value; }
    public Action<float, float> OnValueChanged { get => value.OnValueChanged; set => this.value.OnValueChanged = value; }
    public float MaxValue {
        get => maxValue; set {
            maxValue = value;
            Value = value;
        }
    }

    private void Awake() {
        value = new Eventable<float>(maxValue);
    }
    public virtual void Subtract(float amount) {
        value.Value -= amount;
        if (value <= 0) {
            value.Value = 0f;
        }
    }
    public virtual void Add(float amount) {
        var healAmount = Mathf.Min(amount, maxValue - value);
        value.Value += healAmount;
    }
    public virtual void Reset() {
        value.Value = maxValue;
    }
    public static implicit operator float(VariableComponent component) => component.value.Value;
    public static implicit operator Eventable<float>(VariableComponent component) => component.value;
}
