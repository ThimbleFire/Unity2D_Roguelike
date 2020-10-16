using System;
using UnityEngine;

/*  Short sword, arming sword, great sword, small axe, battle axe, great axe, hammer, warhammer, maul,
    Cloth, leather, studded, plate
    Parrying dagger, targe, kite, wall
    Ring/amulet of life, stamina, regen, strength, crit, light, fools, horses
*/

class ItemBinary
{
    string binary;

    public ItemBinary(string binary)
    {
        this.binary = binary;
    }

    public static int Build(string b)
    {
        return Convert.ToInt32(b, 2); 
    }

    public void Save()
    {
        //int value = 8; 
        //string b = Convert.ToString(value, 2);
        //binary += b;
    }
}
