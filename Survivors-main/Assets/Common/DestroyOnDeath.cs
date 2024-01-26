using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class DestroyOnDeath : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        var health = GetComponent<HealthComponent>();
        health.OnDeath += OnDeath;
    }
    void OnDeath() {
        Destroy(gameObject);
    }
}
