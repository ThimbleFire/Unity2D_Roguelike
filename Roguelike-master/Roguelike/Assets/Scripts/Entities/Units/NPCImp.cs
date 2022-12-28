using UnityEngine;

public class NPCImp : Navigator {

    private void Start() {
        Name = "Imp";
        RangeOfAggression = 6;
        Speed = 3;
        Health_Current = 6;
        Attack_Damage = 1;
    }

    public override void Action() => base.Action();

    public override void Move() => base.Move();

    protected override void OnTileChanged() => base.OnTileChanged();

    protected override void OnArrival() => base.OnArrival();
}