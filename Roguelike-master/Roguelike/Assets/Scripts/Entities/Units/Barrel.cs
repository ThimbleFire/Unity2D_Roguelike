using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Entity
{
    private void Start()
    {
        Name = "Barrel";
        RangeOfAggression = 0;
        Level = 1;
        DmgBasePhyMin = 0;
        DmgBasePhyMax = 0;
        AttackRating = 0;
        DefenseBase = 0;
        DefenseRating = 1;
        ChanceToBlock = 0;
        Life_Current = 8;
        SpeedBase = 0;
    }
    
    protected override void Die()
    {
        LootDropper.RollLoot(transform, Level);
        
        base.Die();
    }
}
