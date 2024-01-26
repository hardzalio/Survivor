using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour {
    public Slider hue;
    public Slider saturation;
    public Slider value;
    public Image colorDisplay;
    public Action<Color> onColorChanged;
    private void Start() {
        hue.onValueChanged.AddListener(UpdateColor);
        saturation.onValueChanged.AddListener(UpdateColor);
        value.onValueChanged.AddListener(UpdateColor);
        UpdateColor(0);
    }
    void UpdateColor(float _v) {
        colorDisplay.color = GetColor();
        onColorChanged?.Invoke(colorDisplay.color);
    }
    public void SetColor(Color color) {
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);
        hue.value = h;
        saturation.value = s;
        value.value = v;
        UpdateColor(0);
    }
    public Color GetColor() {
        return Color.HSVToRGB(hue.value, saturation.value, value.value);
    }
}

