using UnityEngine;

public class NPCImpTrident : Navigator {

    private void Start() {
        Name = "Imp-Trident";
        RangeOfAggression = 6;
        Speed = 4;
        Health_Current = 5;
        Attack_Damage = 2;
    }

    public override void Action() => base.Action();

    public override void Move() => base.Move();

    protected override void OnTileChanged() => base.OnTileChanged();

    protected override void OnArrival() => base.OnArrival();
}