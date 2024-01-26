using UnityEngine;
[RequireComponent(typeof(HealthComponent))]
public class RestoreOnDeath : MonoBehaviour {
    HealthComponent health;
    // Start is called before the first frame update
    void Start() {
        health = GetComponent<HealthComponent>();
        health.OnDeath += OnDeath;
    }

    private void OnDeath() {
        health.Reset();
    }
}
