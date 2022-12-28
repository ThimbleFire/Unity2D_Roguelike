public class NPCImpTrident : Navigator {

    private void Start() {
        Name = "Imp-Trident";
        Speed = 4;
        Level = 2;
        DmgBasePhyMin = 1;
        DmgBasePhyMax = 3;
        RangeOfAggression = 6;
        StrengthBase = 5;
        IntelligenceBase = 5;
        ConstitutionBase = 5;
        DexterityBase = 5;
        Life_Current = Life_Max;
    }

    public override void Action() => base.Action();

    public override void Move() => base.Move();

    protected override void OnTileChanged() => base.OnTileChanged();

    protected override void OnArrival() => base.OnArrival();
}