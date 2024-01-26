using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour {
    [SerializeField] private Transform firePoint;
    public Delay shootDelay = new(1f);
    [SerializeField] GunData _currentGun;
    public LayerMask bulletLayer;
    [SerializeField] BulletSource bulletSource;
    public GunData currentGun {
        get => _currentGun;
        set {
            _currentGun = value;
            shootDelay.Set(value.fireDelay);
        }
    }

    void Start() {
        shootDelay = new Delay(currentGun?.fireDelay ?? 1f);
    }
    void Shoot(Vector3 offset = default) {
        var bullet = Instantiate(currentGun.bulletPrefab, firePoint.position + offset, Quaternion.identity);
        bullet.layer = bulletLayer;
        var bulletController = bullet.GetComponent<BulletController>();
        bulletController.data = currentGun;
        if (bulletSource != null) {
            bulletController.source = bulletSource;
        }
        else {
            Debug.Log("No controller");
        }
        if (offset != default) {
            bullet.transform.forward = offset;
        }
        else {
            bullet.transform.forward = firePoint.forward;
        }
    }
    public bool TryShoot(Vector3 offset = default) {
        if (shootDelay.IsReady()) {
            Shoot(offset);
            shootDelay.Start();
            return true;
        }
        return false;
    }
    private void Update() {
        shootDelay.Update(Time.deltaTime);
    }


}
