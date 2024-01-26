using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager Instance;

    public float spawnDistanceAwayFromPlayer = 10f;
    public List<WaveData> waves = new();
    public LayerMask spawnPreventLayer;
    Collider[] overlapResults = new Collider[3];
    RunInterval nextWaveInterval;
    List<GameObject> enemies = new();
    int currentWaveIndex = 0;
    float currentStrength = 0f;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    private void Start() {
        nextWaveInterval = new RunInterval(10f, NextWave).Delayed();
    }
    private WaveData currentWave => waves[currentWaveIndex];
    private void Update() {
        nextWaveInterval.Update(Time.deltaTime);
        // Add an enemy every frame until strength is matched
        if (currentStrength < currentWave.strength) {
            SpawnEnemy();
        }
    }
    private bool CheckPosition(Vector3 position, PlayerController player) {
        var viewportPosition = player.camera.WorldToViewportPoint(position);
        if ((viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1)) {
            return false;
        }
        if (Physics.OverlapSphereNonAlloc(position, 1f, overlapResults, spawnPreventLayer) > 0
        ) {
            return false;
        }
        return true;
    }
    private (bool, Vector3) GetSpawnPosition() {
        if (PlayerManager.Instance.players.Count == 0) {
            return (false, Vector3.zero);
        }
        var randomPlayer = PlayerManager.Players[Random.Range(0, PlayerManager.Instance.players.Count)];

        // Limiting the number of tries, if can't find a position we'll retry next frame
        for (var i = 0; i < 20; i++) {
            var offsetPosition = Random.insideUnitCircle.normalized * spawnDistanceAwayFromPlayer;
            var position = randomPlayer.transform.position + new Vector3(offsetPosition.x, 0, offsetPosition.y);
            if (CheckPosition(position, randomPlayer)) {
                return (true, position);
            }
        };
        return (false, Vector3.zero);
    }
    private void SpawnEnemy() {
        var enemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Count)];
        var (foundPosition, randomPosition) = GetSpawnPosition();
        if (!foundPosition)
            return;
        var enemyObject = Instantiate(enemy.prefab, randomPosition, Quaternion.identity);
        var enemyController = enemyObject.GetComponent<EnemyController>();
        var enemyHealth = enemyObject.GetComponent<HealthComponent>();
        enemyHealth.MaxValue = enemy.maxHealth;
        enemyController.data = enemy;
        currentStrength += enemy.strength;
        enemies.Add(enemyObject);
        enemyHealth.OnDeath += () => currentStrength -= enemy.strength;
    }

    private void NextWave() {
        currentWaveIndex++;
        if (currentWaveIndex >= waves.Count) {
            currentWaveIndex = 0;
            Debug.Log("Restarting waves");
        }
    }
    public void Restart() {
        currentWaveIndex = 0;
        currentStrength = 0f;
        enemies.ForEach(Destroy);
        enemies.Clear();
        nextWaveInterval.Restart();
    }
}
