using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorInitializer : MonoBehaviour {
    public Renderer[] renderers;

    public void SetColor(Color color) {
        foreach (var r in renderers) {
            r.material.color = color;
        }
    }
}
