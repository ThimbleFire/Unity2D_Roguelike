public class Skeleton : Navigator
{

    private void Start()
    {
        Name = "Skeleton";
        SpeedBase = 3;
        Level = 1;
        DmgBasePhyMin = 1;
        DmgBasePhyMax = 3;
        RangeOfAggression = 1;
        StrengthBase = 5;
        IntelligenceBase = 1;
        ConstitutionBase = 3;
        DexterityBase = 15;
        Life_Current = Life_Max;
    }

    public override void Action() => base.Action();

    public override void Move() => base.Move();

    protected override void OnTileChanged() => base.OnTileChanged();

    protected override void OnArrival() => base.OnArrival();
}