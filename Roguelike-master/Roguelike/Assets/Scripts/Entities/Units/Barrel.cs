using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Entity
{
    private void Start()
    {
        Name = "Barrel";
        SpeedBase = 0;
        Life_Current = 5;
    }
    
    protected override void OnDeath()
    {
        LootDropper.Roll(transform, entityDifficulty);
    
        Base.OnDeath();
    }
}
