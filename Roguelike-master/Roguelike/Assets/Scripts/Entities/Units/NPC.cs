public class NPC : Navigator {

    private void Start() {

    }

    protected override void Die()
    {
        LootDropper.RollLoot(transform, _base.baseStats.Level, _base.baseStats.TreasureClass);

        base.Die();
    }
}