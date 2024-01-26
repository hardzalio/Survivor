using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] Transform _target;
    public Transform target {
        get => _target;
        set {
            _target = value;
            if (value != null)
                offset = transform.position - value.position;
        }
    }
    Vector3 offset;
    private void Awake() {
        if (target != null) {
            offset = transform.position - target.position;
        }
    }
    private void Start() {
        transform.parent = null;

    }
    private void LateUpdate() {
        if (target == null)
            return;
        transform.position = target.position + offset;
    }
}
