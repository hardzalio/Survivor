using System;
using UnityEngine;

public class HitBlock : HealthComponent {
    public int blocks = 1;





    public override void Subtract(float amount) {
        if (blocks > 0)
            blocks--;
        else
            base.Subtract(amount);
    }

    public override string ToString() {
        return base.ToString();
    }
}
