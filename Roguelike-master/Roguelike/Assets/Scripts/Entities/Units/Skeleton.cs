public class Skeleton : Navigator {
    private void Start() {
        Name = "Skeleton";
        RangeOfAggression = 1;
        Level = 2;
        DmgBasePhyMin = 1;
        DmgBasePhyMax = 3;
        AttackRating = 8;
        DefenseRating = 1;
        DefenseBase = 5;
        ChanceToBlock = 3;
        Life_Current = 12;
        SpeedBase = 1;
    }

    protected override void Die()
    {
        LootDropper.RollLoot(transform, Level, TC);

        base.Die();
    }
}