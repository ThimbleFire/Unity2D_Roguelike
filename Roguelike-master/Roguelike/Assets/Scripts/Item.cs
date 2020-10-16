class Item
{
    //Property arrangement determines the properties binary index
    public Enum Properties
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

    public static Item Build(string binary)
    {
        Item item = new Item();

        byte itemType = binary.SubString(0, 8);
    }
}
