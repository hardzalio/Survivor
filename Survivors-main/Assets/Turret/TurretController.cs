using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackController))]
public class TurretController : MonoBehaviour {
    public Transform platform;
    Transform target;
    List<Transform> targets = new List<Transform>();
    AttackController attackController;
    public float range = 10f;
    public float turnSpeed = 10f;
    public LayerMask targetable;

    RunInterval scanInterval;
    void Start() {
        attackController = GetComponent<AttackController>();
        scanInterval = new RunInterval(1f, Scan);
    }

    // Update is called once per frame
    void Update() {
        scanInterval.Update(Time.deltaTime);
        if (targets.Count > 0) {
            target = targets[0];
            if (target == null) {
                targets.RemoveAt(0);
                return;
            }
            var direction = target.position - transform.position;
            direction.y = platform.position.y;
            platform.forward = Vector3.Lerp(platform.forward, direction, Time.deltaTime * turnSpeed);
            // var rotation = Quaternion.LookRotation(direction);
            // platform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
            attackController.TryShoot();
        }
    }

    void Scan() {
        targets.Clear();
        var colliders = Physics.OverlapSphere(transform.position, range, targetable);
        foreach (var collider in colliders) {
            targets.Add(collider.transform);
        }
        targets.Sort((a, b) => {
            var aDistance = Vector3.Distance(transform.position, a.position);
            var bDistance = Vector3.Distance(transform.position, b.position);
            return aDistance.CompareTo(bDistance);
        });
    }
}
