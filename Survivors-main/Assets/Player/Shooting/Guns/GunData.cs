using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "SO/Player/GunData")]
public class GunData : ScriptableObject {
    public new string name;
    public float damage = 10f;
    public float fireDelay = 0.5f;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 5f;
    public float bulletSize = 0.2f;
    public GameObject bulletPrefab;
    public float secondaryDamage = 10f;
    public float secondaryRadius = 1f;
}
