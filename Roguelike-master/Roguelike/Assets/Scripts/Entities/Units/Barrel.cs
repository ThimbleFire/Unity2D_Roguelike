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
        Level = 1;
    }
    
    protected override void Die()
    {
        LootDropper.RollLoot(transform, Level);
        
        base.Die();
    }
}
