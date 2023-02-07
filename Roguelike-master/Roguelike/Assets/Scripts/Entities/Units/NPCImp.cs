public class NPCImp : Navigator {
    private void Start() {
        Name = "Imp";
        Level = 1;
        
        DmgBasePhyMin = 2;
        DmgBasePhyMax = 3;
        AttackRating = 8;
        
        DefenseBase = 5;
        ChanceToBlock = 3;
        
        Experience_Current = 33; 
        Life_Current = Random.Range(7, 12); //inclusive min, inclusive max
        RangeOfAggression = 6;
        SpeedBase = 1;
    }
}
