using System;

class Item
{
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

    public int[] property;

    public Item()
    {
        property = new int[Enum.GetValues( typeof( Properties ) ).Length];
    }

    public static Item Build(string binary)
    {
        Item item = new Item();

        for ( int i = 0; i < item.property.Length; i++ )
        {
            string snippet = binary.Substring( i * 8, 1 + i * 8 );
            item.property[i] = ItemBinary.Build( snippet );
        }

        return item;
    }
}
