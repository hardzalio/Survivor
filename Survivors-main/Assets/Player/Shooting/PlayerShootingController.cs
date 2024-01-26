using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootingController : MonoBehaviour {
    public List<GunData> guns;
    [SerializeField] PlayerController controller;
    private int currentGunIndex = 0;
    AttackController attack;
    public Action<GunData> OnGunChanged;
    public GunData CurrentGun => guns[currentGunIndex];
    private void Start() {
        Debug.Assert(guns.Count > 0, "No guns found");
        attack = GetComponent<AttackController>();
        attack.currentGun = guns[currentGunIndex];
    }

    void OnEnable() {
        controller.controls.Action.Change.performed += ChangeWeapon;
    }
    void Update() {
        if (controller.controls.Action.Shoot.IsPressed()) {
            attack.TryShoot();
        }
    }

    void OnDisable() {
        controller.controls.Action.Change.performed -= ChangeWeapon;
    }
    void ChangeWeapon(InputAction.CallbackContext context) {
        var direction = context.ReadValue<float>();
        if (direction < 0) {
            currentGunIndex = (currentGunIndex + 1) % guns.Count;
        }
        else {
            currentGunIndex = (currentGunIndex - 1 + guns.Count) % guns.Count;
        }
        attack.currentGun = guns[currentGunIndex];
        OnGunChanged?.Invoke(guns[currentGunIndex]);
    }
}
