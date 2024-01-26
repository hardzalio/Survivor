using UnityEngine;

public class BulletController : MonoBehaviour {
    public GunData data;
    public BulletSource source;
    public virtual void Start() {
        Destroy(gameObject, data.bulletLifetime);
        transform.localScale = Vector3.one * data.bulletSize;
    }
    // Update is called once per frame
    protected virtual void Update() {
        transform.position += data.bulletSpeed * Time.deltaTime * transform.forward;
    }
    protected virtual void OnTriggerEnter(Collider other) {
        if (other.gameObject.TryGetComponent(out HealthComponent health)) {
            health.lastHitBy = source;
            health.Subtract(data.damage);
            Destroy(gameObject);
        }
    }
}
