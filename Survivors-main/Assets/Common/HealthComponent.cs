using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HealthComponent : VariableComponent {
    public Action OnDeath { get; set; }
    public BulletSource lastHitBy;

    public override void Subtract(float amount) {
        base.Subtract(amount);
        if (Value <= 0) {
            OnDeath?.Invoke();
        }
    }

    public void Heal(float amount) {
        Add(amount);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(HealthComponent))]
public class HealthComponentEditor : Editor {
    HealthComponent healthComponent;
    void OnEnable() {
        healthComponent = (HealthComponent)target;
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (UnityEngine.GUILayout.Button("Heal")) {
            healthComponent.Reset();
        }
    }
}
#endif
