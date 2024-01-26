using System.Collections;
using UnityEngine;

public class Detach : MonoBehaviour {
    public float delay = 0f;
    Vector3 position;
    Quaternion rotation;
    private void Awake() {
        position = transform.position;
        rotation = transform.rotation;
    }
    private void Start() {
        StartCoroutine(DetachRoutine());
    }
    private IEnumerator DetachRoutine() {
        yield return new WaitForSeconds(delay);
        transform.parent = null;
        transform.SetPositionAndRotation(position, rotation);
    }
}
