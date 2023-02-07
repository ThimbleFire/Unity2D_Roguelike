public class Skeleton : Navigator {
    private void Start() {
        Name = "Skeleton";
        Level = 1;
        
        DmgBasePhyMin = 1;
        DmgBasePhyMax = 2;
        AttackRating = 8;
        
        DefenseBase = 5;
        ChanceToBlock = 9;
        
        Experience_Current = 18; 
        Life_Current = Random.Range(1, 4); //inclusive min, inclusive max
        RangeOfAggression = 6;
        SpeedBase = 4;
    }
}
