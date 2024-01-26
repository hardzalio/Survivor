
using UnityEngine;
using UnityEngine.UI;
public class BarController : MonoBehaviour {
    Slider bar;
    public bool faceCamera = false;
    Canvas canvas;
    [SerializeReference]
    public VariableComponent value;
    Camera cam;

    void Start() {
        bar = GetComponentInChildren<Slider>();
        canvas = GetComponentInChildren<Canvas>();
        bar.maxValue = value.MaxValue;
        UpdateBar(value.Value, value.Value);
        value.OnValueChanged += UpdateBar;
        cam = Camera.main;
    }


    private void LateUpdate() {
        if (faceCamera && cam != null) {
            var targetPos = cam.transform.position;
            targetPos.y = canvas.transform.position.y;
            canvas.transform.LookAt(targetPos);
            canvas.transform.rotation = cam.transform.rotation;
        }
    }

    private void UpdateBar(float old, float value) {
        bar.value = value;
    }
}
