using System;
using UnityEngine;

public class Item
{
    public enum SubCategories
    {
        Helmet,
        Chest,
        Belt,
        Legs,
        Foot_L,
        Foot_R,
        Hand_L,
        Hand_R,
        Offhand,
        Weapon,
        Shoulder_L,
        Shoulder_R,
        Cape,
        Neck,
        Ring,
        Bracelet
    }

    public SubCategories subcategory;

    //Property arrangement determines the properties binary index
    public enum Properties
    {
        ItemType,      //greatsword
        Category,      //weapon
        Subcategory,   //sword
        Material,      //metal
        Tier,          //1 to 3, normal, exceptional, masterclass
        Prefix1,
        Prefix2,
        Prefix3,
        Suffix1,
        Suffix2,
        Suffix3,
    }

    public Sprite sprite { get; set; }

    public int[] property;

    public Item()
    {

    }

    public void Build(string binary)
    {
        property = new int[Enum.GetValues( typeof( Properties ) ).Length];

        for ( int i = 0; i < property.Length; i++ )
        {
            property[i] = Binary.ToDecimal( binary, i );
        }

        subcategory = (SubCategories)property[(byte)Properties.Subcategory];

        // to do: setup a sprite manager system.
        // organise files by <sub>[<type>]
        //replace type with index

        sprite = SpriteManager.Instance.Get( (byte)subcategory, (byte)property[0] );
    }
}
