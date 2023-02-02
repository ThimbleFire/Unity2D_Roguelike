public class NPCImpTrident : Navigator {
    private void Start() {
        Name = "Imp-Trident";
        RangeOfAggression = 6;
        Level = 3;
        DmgBasePhyMin = 3;
        DmgBasePhyMax = 4;
        AttackRating = 16;
        DefenseRating = 2;
        DefenseBase = 5;
        ChanceToBlock = 3;
        Life_Current = 16;
        SpeedBase = 1;
    }
}