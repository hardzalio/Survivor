using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class BuildingController : MonoBehaviour {
    public bool IsBuilding { get; private set; } = false;
    [SerializeField] PlayerController controller;
    private EnergyManager energy;
    public Action<bool> onBuildingModeChanged;
    public float buildEnergyCost = 50f;
    [SerializeField] Transform buildVisuals;
    public GameObject buildPrefab;
    public List<GunData> turretGuns;
    public Action<GunData> OnTurretChanged;
    public List<GameObject> turrets;
    int currentTurretIndex = 0;
    public GunData CurrentTurret => turretGuns[currentTurretIndex];
    private void Start() {
        energy = GetComponent<EnergyManager>();
        controller.playerDeath.onPlayerDeath += OnDeath;
    }
    void OnDeath() {
        foreach (var turret in turrets) {
            Destroy(turret);
        }
        turrets.Clear();
    }
    public void OnToggleBuildMode(InputAction.CallbackContext context) {
        IsBuilding = !IsBuilding;
        ApplyBuilding();
    }
    private void ApplyBuilding() {
        if (IsBuilding) {
            controller.controls.Building.Enable();
            controller.controls.Action.Disable();
        }
        else {
            controller.controls.Building.Disable();
            controller.controls.Action.Enable();
        }
        onBuildingModeChanged?.Invoke(IsBuilding);
    }
    private void OnBuild(InputAction.CallbackContext context) {
        if (energy.UseEnergy(buildEnergyCost)) {
            Debug.Log("Building");
            var turret = Instantiate(buildPrefab, buildVisuals.position, Quaternion.identity);
            var attackController = turret.GetComponent<AttackController>();
            attackController.currentGun = turretGuns[currentTurretIndex];
            turrets.Add(turret);
        }
        else {
            Debug.Log("Not enough energy");
        }
    }

    private void OnEnable() {
        controller.controls.Movement.ToggleBuildMode.performed += OnToggleBuildMode;
        controller.controls.Building.Build.performed += OnBuild;
        controller.controls.Building.Change.performed += OnBuildingChange;
        ApplyBuilding();
    }

    private void OnBuildingChange(InputAction.CallbackContext context) {
        var direction = context.ReadValue<float>();
        if (direction < 0) {
            currentTurretIndex = (currentTurretIndex + 1) % turretGuns.Count;
        }
        else {
            currentTurretIndex = (currentTurretIndex - 1 + turretGuns.Count) % turretGuns.Count;
        }
        OnTurretChanged?.Invoke(turretGuns[currentTurretIndex]);
    }

    private void OnDisable() {
        controller.controls.Movement.ToggleBuildMode.performed -= OnToggleBuildMode;
        controller.controls.Building.Build.performed -= OnBuild;
        controller.controls.Building.Change.performed -= OnBuildingChange;
    }
}
