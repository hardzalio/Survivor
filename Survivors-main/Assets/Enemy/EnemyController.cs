using UnityEngine;

public class EnemyController : MonoBehaviour {
    public EnemyData data;
    Transform target;
    CharacterController controller;
    AttackController attackController;
    RunInterval scanForPlayer;
    Delay canAttack;
    HealthComponent health;

    void Start() {
        scanForPlayer = new RunInterval(0.5f, SelectTarget);
        canAttack = new Delay(data.attackDelay);
        controller = GetComponent<CharacterController>();
        attackController = GetComponent<AttackController>();
        if (data.type == EnemyType.Ranged && attackController != null) {
            attackController.currentGun = data.gunData;
            attackController.currentGun.damage = data.damage;
            attackController.shootDelay.Set(data.attackDelay);
            attackController.bulletLayer = LayerMask.NameToLayer("EnemyBullet");
        }
        health = GetComponent<HealthComponent>();
        health.OnDeath += OnDeath;
    }
    void SelectTarget() {
        var closestPlayer = PlayerManager.GetClosestPlayer(transform.position);
        if (closestPlayer != null) {
            target = closestPlayer.transform;
        }
    }
    void OnDeath() {
        Debug.Log("Enemy died");
        Debug.Log($"Last hit by {health.lastHitBy?.player?.gameObject?.name}");
        health.lastHitBy?.AddScore(data.score);
        Destroy(gameObject);
    }

    void Update() {
        scanForPlayer.Update(Time.deltaTime);
        canAttack.Update(Time.deltaTime);
        if (target == null) {
            return;
        }
        var moveDirection = (target.position - transform.position).normalized;

        var distance = Vector3.Distance(transform.position, target.position);
        if (data.type == EnemyType.Ranged && distance < data.attackRange) {
            if (attackController != null) {
                var offset = (target.position - transform.position).normalized * 1f;
                attackController.TryShoot(offset);
            }
            else {
                Debug.LogError("No attack controller on ranged enemy");
            }
        }
        else {
            controller.Move(data.speed * Time.deltaTime * moveDirection);
        }
    }
    private void OnTriggerStay(Collider other) {
        if (canAttack && other.gameObject.CompareTag("Player")) {
            if (other.gameObject.TryGetComponent(out HealthComponent health)) {
                canAttack.Start();
                health.Subtract(data.damage);
            }
        }
    }
}
