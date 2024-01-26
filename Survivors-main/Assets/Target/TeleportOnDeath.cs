using UnityEngine;
[RequireComponent(typeof(HealthComponent))]
public class TeleportOnDeath : MonoBehaviour {
    public Transform teleportTarget;
    [Tooltip("How far away from the teleport target should the teleport be")]
    public float teleportRadius = 5f;
    [Tooltip("How far away from the teleport target should the teleport check for obstacles")]
    public float teleportCheckRadius = 1f;
    private HealthComponent health;
    // Start is called before the first frame update
    void Start() {
        health = GetComponent<HealthComponent>();
        health.OnDeath += OnDeath;
    }

    void OnDeath() {
        var randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0;
        var randomPosition = teleportTarget.position + (randomDirection.normalized * teleportRadius);
        //Safety check to not get stuck infinitely if there's no place to teleport to
        for (var i = 0; i < 20 && Physics.CheckSphere(randomPosition, teleportCheckRadius); i++) {
            randomDirection = Random.insideUnitSphere;
            randomDirection.y = 0;
            randomPosition = teleportTarget.position + (randomDirection.normalized * teleportRadius);
        }
        transform.position = randomPosition;
        health.Reset();
    }
}
