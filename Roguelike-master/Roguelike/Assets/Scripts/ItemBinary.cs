#using UnityEngine

public Enum weapons
{
Short sword, arming sword, great sword,
Small axe, battle axe, great axe,
Hammer, warhammer, maul,
}

public Enum Armour
{
Cloth, leather, studded leather, iron plate, steel plate
}

public Enum Offhand
{
Parrying dagger, targe, kite, wall
}

class ItemBinary
{
    string binary;

    public ItemBinary(string binary)
    {
        this.binary = binary;
    }

    public void Build()
    {
        //substring to find region
        //Convert.ToInt32("1001101", 2).ToString(); 
    }

    public void Save()
    {
        //int value = 8; 
        //string b = Convert.ToString(value, 2);
        //binary += b;
    }
}
