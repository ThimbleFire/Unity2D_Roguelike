using UnityEngine;

public class NPCImp : Navigator {

    public AudioClip teleport;

    private void Start() {
        Name = "Imp";
        SpeedBase = 2;
        Level = 1;
        DmgBasePhyMin = 1;
        DmgBasePhyMax = 2;
        RangeOfAggression = 6;
        StrengthBase = 5;
        IntelligenceBase = 5;
        ConstitutionBase = 5;
        DexterityBase = 15;
        Life_Current = Life_Max;
    }

    public override void Action()
    {
        bool willTeleport = Random.Range(0, 2) == 1;

        if (willTeleport && isAggressive)
        {
            Entities.DrawTeleport(transform);
            AudioDevice.Play(teleport);

            Pathfind.Unoccupy(_coordinates);
            MoveUnitTo(Pathfind.GetRandomTile());
            Pathfind.Occupy(_coordinates);
        }
        base.Action();
    }

    public override void Move() => base.Move();

    protected override void OnTileChanged() => base.OnTileChanged();

    protected override void OnArrival() => base.OnArrival();
}