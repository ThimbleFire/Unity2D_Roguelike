using UnityEngine;

public class LootDropper
{
    public const BaseItemFind = 35; // percent
    
    private static float drop_uniqueChance { get { return 0.005f + ( 0.00005 * Entities.PCS.IncMagicFind ) / 2; } }
    private static float drop_magicChance { get { return 0.095f + ( 0.00095 * Entities.PCS.IncMagicFind ) / 2; } }

    public static void RollLoot(Transform _transform, int entityDifficulty)
    {
        // Are we dropping an item
        
        int roll = Random.Range(0, 100);
        
        bool itemIsDropping = roll < BaseItemFind + Entities.GetPCS.IncItemFind;
        
        if(itemIsDropping == false)
            return;
        
        // determine type of item being dropped
        
        ItemStats.Type itemType = Random.Range(0, System.Enum.GetNames(typeof(ItemStats.Type)).Length);
        
        // determine the rarity of the item being dropped
        
        float rarity = Random.Range(0.0f, 1.0f);
        
        if(rarity < drop_uniqueChance)
        {
            ItemStats itemStats = ResourceRepository.GetItem(ItemStats.Rarity.Unique, itemType, entityDifficulty);
        
            GameObject go = GameObject.Instantiate(new GameObject(), null);
            go.AddComponent<ItemStats>(itemStats);
            go.transform.position = _transform.position;
        
            return;
        }
        
        if(rarity < drop_magicChance)
        {
            ItemStats itemStats = ResourceRepository.GetItem(ItemStats.Rarity.Magic, itemType, entityDifficulty);
        
            GameObject go = GameObject.Instantiate(new GameObject(), null);
            go.AddComponent<ItemStats>(itemStats);
            go.transform.position = _transform.position;
            
            return;
        }
        
        if(rarity >= drop_uniqueChance + drop_magicChance)
        {
            ItemStats itemStats = ResourceRepository.GetItem(ItemStats.Rarity.Normal, itemType, entityDifficulty);
        
            GameObject go = GameObject.Instantiate(new GameObject(), null);
            go.AddComponent<ItemStats>(itemStats);
            go.transform.position = _transform.position;
            
            return;
        }
    }
}
