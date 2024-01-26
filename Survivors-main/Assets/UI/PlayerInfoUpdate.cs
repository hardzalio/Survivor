using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;

public class PlayerInfoUpdate : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public TextMeshProUGUI label;
    public string template = "{0}";
    // Start is called before the first frame update
    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }
    public string InfoToText() {
        return toggleGroup.ActiveToggles().FirstOrDefault().ToString();
    }

    public void OnGroupChange() {
        label.text = String.Format(template, InfoToText());
        /*if (rename) {
            label.text = String.Format(template, names[(int) (sliderValue-slider.minValue)]);
        }*/
    }
}
