public class NPCImp : Navigator {
    private void Start() {
        Name = "Imp";
        RangeOfAggression = 6;
        Level = 1;
        DmgBasePhyMin = 2;
        DmgBasePhyMax = 3;
        AttackRating = 8;
        DefenseRating = 1;
        DefenseBase = 5;
        ChanceToBlock = 3;
        Life_Current = 8;
        SpeedBase = 1;
    }

    protected override void Die()
    {
        LootDropper.RollLoot(transform, Level, TC);

        base.Die();
    }
}