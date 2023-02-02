using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public bool Equipped = false;
    public const string hexMagic = "<color=#4850B8>";
    public const string hexRare = "<color=#FFFF00>";
    public const string hexGray = "<color=#8A8A8A>";
    public const string hexRed = "<color=#FF0000>";
    public const string hexUnique = "<color=#908858>";
    public const string hexWhite = "<color=#FFFFFF>";
    public const string hexEnd = "</color>";

    private Item item;

    public Item.Type ItemType { get { return item.ItemType; } }
    public List<Item.Implicit> Implicits { get { return item.Implicits; } }
    public List<Item.Prefix> Prefixes { get { return item.Prefixes; } }
    public List<Item.Suffix> Suffixes { get { return item.Suffixes; } }

    public byte Rarity { get { return ( byte )( item.Prefixes.Count + item.Suffixes.Count ); } }

    public int MinDamage
    {
        get {
            Item.Suffix s = item.Suffixes.Find( x => x.type == Item.Suffix.SType.Dmg_Phys_Min );
            Item.Prefix p = item.Prefixes.Find( x => x.type == Item.Prefix.PType.Dmg_Phys_Percent );

            if ( s != null && p != null ) return ( int )( (item.DmgMin + s.value ) * ( p.value / 100.0f + 1 ) );
            if ( s != null ) return item.DmgMin + s.value;
            if ( p != null ) return ( int )(item.DmgMin * ( p.value / 100.0f + 1 ) );
            
            return item.DmgMin;
        }
    }
    public int MaxDamage
    {
        get {
            Item.Suffix s = item.Suffixes.Find( x => x.type == Item.Suffix.SType.Dmg_Phys_Max );
            Item.Prefix p = item.Prefixes.Find( x => x.type == Item.Prefix.PType.Dmg_Phys_Percent );

            if ( s != null && p != null ) return ( int )( (item.DmgMax + s.value ) * ( p.value / 100.0f + 1 ) );
            if ( s != null ) return item.DmgMax + s.value;
            if ( p != null ) return ( int )(item.DmgMax * ( p.value / 100.0f + 1 ) );
            
            return item.DmgMax;
        }
    }
    public int Defense
    {
        get {
            Item.Suffix s = item.Suffixes.Find( x => x.type == Item.Suffix.SType.Def_Phys_Flat );
            Item.Prefix p = item.Prefixes.Find( x => x.type == Item.Prefix.PType.Def_Phys_Percent );

            if ( s != null && p != null ) return ( int )( ( item.DefMin + s.value ) * ( p.value / 100.0f + 1 ) );
            if ( s != null ) return item.DefMin + s.value;
            if ( p != null ) return ( int )(item.DefMin * ( p.value / 100.0f + 1 ) );
            
            return item.DefMin;
        }
    }
    public int Blockrate
    {
        get
        {
            Item.Suffix s = item.Suffixes.Find(x => x.type == Item.Suffix.SType.Plus_Blockrate);
            Item.Implicit i = item.Implicits.Find(x => x.type == Item.Implicit.IType.Plus_Blockrate);

            if (s != null && i != null) return item.Blockrate + s.value + i.value;
            if (s != null) return item.Blockrate + s.value;
            if (i != null) return item.Blockrate + i.value;

            return item.Blockrate;
        }
    }

    public AudioClip soundEndDrag;

    public string Tooltip {
        get {

            System.Text.StringBuilder t = new System.Text.StringBuilder(
                item.Unique ? hexUnique : hexWhite);
            t.Append(item.Name);
            t.Append(hexEnd);
            
            t.Append( "\n" + Type_Text[( byte )item.ItemType] );
            
            if (item.DmgMin > 0)
                t.Append( "\n" + hexGray + "One-hand damage: " + hexEnd + hexMagic + MinDamage + " to " + MaxDamage + hexEnd );

            if (item.Blockrate > 0)
                t.Append("\n" + hexGray + "Chance to block: " + hexEnd + hexMagic + Blockrate + hexEnd);

            if ( item.DefMin > 0)
                t.Append( "\n" + hexGray + "Defense: " + hexEnd + hexMagic + Defense + hexEnd );
            
            if ( item.Durability > 0 )
                t.Append( "\n" + hexGray + "Durability: " + hexEnd + item.Durability );

            if (item.ReqStr > 0)
            {
                if (Entities.GetPCS.Strength < item.ReqStr)
                    t.Append(hexRed);
                else t.Append(hexGray);
                t.Append(string.Format("\nRequired Strength: {0}{1}", item.ReqStr, hexEnd));
            }
            if (item.ReqDex > 0)
            {
                if (Entities.GetPCS.Dexterity < item.ReqDex)
                    t.Append(hexRed);
                else t.Append(hexGray);
                t.Append(string.Format("\nRequired Dexterity: {0}{1}", item.ReqDex, hexEnd));
            }
            if (item.ReqInt > 0)
            {
                if (Entities.GetPCS.Intelligence < item.ReqInt)
                    t.Append(hexRed);
                else t.Append(hexGray);
                t.Append(string.Format("\nRequired Intelligence: {0}{1}", item.ReqInt, hexEnd));
            }
            if (item.ReqCons > 0)
            {
                if (Entities.GetPCS.Constitution < item.ReqCons)
                    t.Append(hexRed);
                else t.Append(hexGray);
                t.Append(string.Format("\nRequired Constitution: {0}{1}", item.ReqCons, hexEnd));
            }
            if (item.ReqLvl > 0)
            {
                if (Entities.GetPCS.Level < item.ReqLvl)
                    t.Append(hexRed);
                else t.Append(hexGray);
                t.Append(string.Format("\nRequired Level: {0}{1}", item.ReqLvl, hexEnd));
            }

            foreach ( var implicitMod in item.Implicits ) 
                t.Append( "\n" + hexMagic + string.Format( GearStats.Affix_Text[( byte )implicitMod.type], implicitMod.value ) + hexEnd );
            foreach ( var prefix in item.Prefixes ) 
                t.Append( "\n" + hexMagic + string.Format( GearStats.Affix_Text[( byte )prefix.type], prefix.value ) + hexEnd );
            foreach ( var suffix in item.Suffixes ) 
                t.Append( "\n" + hexMagic + string.Format( GearStats.Affix_Text[( byte )suffix.type], suffix.value ) + hexEnd );
            

            if ( item.Description != string.Empty )
                t.Append( "\n\n<i>" + item.Description + "</i>" );

            return t.ToString();
        }
    }

    public static string[] Type_Text = new string[12]
    { 
        "Any", 
        "Helmet", 
        "Chest", 
        "Gloves", 
        "Legs", 
        "Feet", 
        "Weapon", 
        "Offhand", 
        "Ring", 
        "Amulet",
        "Consumable", 
        "Belt"
    };

    public void Load(string itemName)
    {
        item = XMLUtility.Load<Item>("Items/" + itemName);
        GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>(item.SpriteUIFilename);
    }
}
