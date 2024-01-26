
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class RadioButtonSystem : MonoBehaviour
{
    private ToggleGroup toggleGroup;
    private Toggle []toggleArray;
    public Dictionary<PlayerColor,Toggle> togglesDict = new Dictionary<PlayerColor, Toggle>();
    void Start()
    {
        int index = PlayerPrefs.GetInt("PlayerColor", 0);
        toggleGroup = GetComponent<ToggleGroup>();
        toggleGroup.transform.GetChild(index).GetComponent<Toggle>().isOn = true;
        toggleArray = GetComponentsInChildren<Toggle>();
    }

    public void Submit() {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        Debug.Log(toggle.name + " _" + toggle.GetComponentInChildren<Text>().text);
        int index = Array.IndexOf(toggleArray, toggle);
        PlayerPrefs.SetInt("PlayerColor", index);
    }
}