using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModeUIManager : MonoBehaviour {
    [SerializeField] GameObject attackMode;
    [SerializeField] GameObject buildingMode;
    [SerializeField] BuildingController buildingController;
    [SerializeField] PlayerShootingController attackController;
    [SerializeField] GameObject attackElementPrefab;
    [SerializeField] GameObject buildingElementPrefab;
    [SerializeField] Color baseColor = Color.white;
    [SerializeField] float inactiveAlpha = 0.1f;
    [SerializeField] float activeAlpha = 0.3f;
    Animation buildingAnimation;
    Animation attackAnimation;
    const string leftAnim = "ModeUISlideLeft";
    const string rightAnim = "ModeUISlideRight";
    Dictionary<string, (GunData gun, Image image, TextMeshProUGUI text)> guns = new();
    Dictionary<string, (GunData turretGun, Image image, TextMeshProUGUI text)> buildings = new();
    private void Start() {
        buildingAnimation = buildingMode.GetComponent<Animation>();
        attackAnimation = attackMode.GetComponent<Animation>();
        buildingController.onBuildingModeChanged += OnBuildingModeChanged;
        OnBuildingModeChanged(buildingController.IsBuilding);
        foreach (var gun in attackController.guns) {
            var element = Instantiate(attackElementPrefab, attackMode.transform);
            var image = element.GetComponentInChildren<Image>();
            var gunText = element.GetComponentInChildren<TextMeshProUGUI>();
            guns.Add(gun.name, (gun, image, gunText));
            gunText.text = gun.name;
        };
        foreach (var gun in buildingController.turretGuns) {
            var element = Instantiate(buildingElementPrefab, buildingMode.transform);
            var image = element.GetComponentInChildren<Image>();
            var gunText = element.GetComponentInChildren<TextMeshProUGUI>();
            buildings.Add(gun.name, (gun, image, gunText));
            gunText.text = gun.name;
        };
        attackController.OnGunChanged += OnGunChanged;
        buildingController.OnTurretChanged += OnTurretChanged;
        OnGunChanged(attackController.CurrentGun);
        OnTurretChanged(buildingController.CurrentTurret);
    }

    private void OnGunChanged(GunData data) {
        foreach (var (gun, image, text) in guns.Values) {
            if (gun.name == data.name) {
                image.color = new Color(baseColor.r, baseColor.g, baseColor.b, activeAlpha);
                text.fontWeight = FontWeight.Bold;
            }
            else {
                image.color = new Color(baseColor.r, baseColor.g, baseColor.b, inactiveAlpha);
                text.fontWeight = FontWeight.Medium;
            }
        }
    }
    private void OnTurretChanged(GunData data) {
        foreach (var (gun, image, text) in buildings.Values) {
            if (gun.name == data.name) {
                image.color = new Color(baseColor.r, baseColor.g, baseColor.b, activeAlpha);
                text.fontWeight = FontWeight.Bold;
            }
            else {
                image.color = new Color(baseColor.r, baseColor.g, baseColor.b, inactiveAlpha);
                text.fontWeight = FontWeight.Medium;
            }
        }
    }

    private void OnBuildingModeChanged(bool isBuilding) {
        if (isBuilding) {
            buildingAnimation.Play(rightAnim);
            attackAnimation.Play(rightAnim);
        }
        else {
            buildingAnimation.Play(leftAnim);
            attackAnimation.Play(leftAnim);
        }
    }
}
