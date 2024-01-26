using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerColor{
    Red,
    Green,
    Blue
}
public class PlayerColorSwap : MonoBehaviour
{
    public Material playerMaterial;
    // Start is called before the first frame update
    void Start()
    {
        int index = PlayerPrefs.GetInt("PlayerColor", 0);
        playerMaterial = this.GetComponent<MeshRenderer>().material;
        ChangeColor((PlayerColor)index);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeColor(PlayerColor playerColor) {
        switch(playerColor){
            case PlayerColor.Red: playerMaterial.color = new Color(1,0,0);
            break;
            case PlayerColor.Green: playerMaterial.color = new Color(0,1,0);
            break;
            case PlayerColor.Blue: playerMaterial.color = new Color(0,0,1);
            break;
        }
    }
}