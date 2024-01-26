using UnityEngine;
public class ExplodeBulletController : BulletController {
    protected override void OnTriggerEnter(Collider other) {
        var inRange = Physics.OverlapSphere(transform.position, data.secondaryRadius);
        foreach (var collider in inRange) {
            if (collider.gameObject.TryGetComponent(out HealthComponent health)) {
                health.Subtract(data.secondaryDamage);
            }
        }
    }
}
