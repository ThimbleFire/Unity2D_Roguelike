using UnityEngine;

public class LootDropper
{
    public const int BaseItemFind = 35; // percent
    
    private static float DropUniqueChance { get { return 0.005f + ( 0.00005f * Entities.GetPCS.IncMagicFind ) / 2; } }
    private static float DropMagicChance { get { return 0.095f + ( 0.00095f * Entities.GetPCS.IncMagicFind ) / 2; } }

    public static void RollLoot(Transform _transform, byte mlvl, byte TC)
    {
        // Are we dropping an item
        
        int roll = Random.Range(0, 100);
        
        bool itemIsDropping = roll < BaseItemFind + Entities.GetPCS.IncItemFind;
        
        if(itemIsDropping == false)
            return;
        
        // determine type of item being dropped
        
        Item.Type itemType = (Item.Type)Random.Range(0, System.Enum.GetNames(typeof(Item.Type)).Length);

        // determine the rarity of the item being dropped

        ItemStats itemStats = ResourceRepository.GetItemMatchingCriteria(itemType, mlvl, TC);

        //int rarity = Random.Range(0, 100) * (1 + Entities.GetPCS.IncMagicFind / 100);
                
        //ItemStats go = GameObject.Instantiate(new GameObject(), null).AddComponent<ItemStats>();
        //go = itemStats;
        //go.transform.position = _transform.position;
    }
}
