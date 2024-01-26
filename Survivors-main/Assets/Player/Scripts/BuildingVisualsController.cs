using UnityEngine;

public class BuildingVisualsController : MonoBehaviour {
    public float checkRadius = .5f;
    public GameObject prefab;
    public Material material;
    public LayerMask collisionLayers;
    public BuildingController buildingController;
    Collider[] colliders = new Collider[1];
    GameObject preview;
    Renderer[] previewRenderers;

    private void Start() {
        preview = Instantiate(prefab, transform);
        previewRenderers = preview.GetComponentsInChildren<Renderer>();
        foreach (var pr in previewRenderers) {
            pr.material = material;
        }
        var scripts = preview.GetComponentsInChildren<MonoBehaviour>();
        foreach (var script in scripts) {
            script.enabled = false;
        }
        preview.layer = LayerMask.NameToLayer("VFX");
        buildingController.onBuildingModeChanged += OnBuildingModeChanged;
        preview.SetActive(false);
    }
    private void OnBuildingModeChanged(bool isBuilding) {
        preview.SetActive(isBuilding);
    }
    private void Update() {
        var collisions = Physics.OverlapSphereNonAlloc(transform.position, checkRadius, colliders, collisionLayers);
        material.SetInt("_IsAvaliable", collisions <= 0 ? 1 : 0);
    }
}
