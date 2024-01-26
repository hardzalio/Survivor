using System;
using UnityEngine;

public class EnergyManager : VariableComponent {
    public float energyRegen = 1f;
    private void Update() {
        Add(energyRegen * Time.deltaTime);
    }
    public bool UseEnergy(float amount) {
        if (value.Value >= amount) {
            value.Value -= amount;
            return true;
        }
        return false;
    }
}
