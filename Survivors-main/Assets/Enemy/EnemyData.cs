using UnityEngine;
public enum EnemyType {
    Melee,
    Ranged,
    Shield,
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "SO/Enemy/EnemyData", order = 0)]
public class EnemyData : ScriptableObject {
    public new string name = "Enemy";
    public float speed = 5f;
    public float damage = 10f;
    public float attackDelay = 0.5f;
    public float strength = 1f;
    public float maxHealth = 30f;
    public int score = 1;

    public GameObject prefab;
    public EnemyType type;

    [Header("Only for ranged enemies")]
    public float attackRange = 5f;
    public GunData gunData;

    public static EnemyData operator *(EnemyData data, float multiplier) {
        data.damage *= multiplier;
        data.maxHealth *= multiplier;
        data.score = (int)(data.score * multiplier);
        return data;
    }
}
