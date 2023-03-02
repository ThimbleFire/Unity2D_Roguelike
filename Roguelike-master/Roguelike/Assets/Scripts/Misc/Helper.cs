using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public static bool IsNullOrDefault<T>(T value)
    {
        return Equals(value, default(T));
    }

    public const string hexMagic = "<color=#4850B8>";
    public const string hexRare = "<color=#FFFF00>";
    public const string hexGray = "<color=#8A8A8A>";
    public const string hexRed = "<color=#FF0000>";
    public const string hexUnique = "<color=#908858>";
    public const string hexWhite = "<color=#FFFFFF>";
    public const string hexEnd = "</color>";

    public static string[] ItemTypeNames = new string[12]
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

    public const string LBL_ONE_HAND_DAMAGE = "One-hand damage: ";
    public const string LBL_CHANCE_TO_BLOCK = "Chance to block: ";
    public const string LBL_DEFENSE = "Defense: ";
    public const string LBL_DURABILITY = "Durability: ";
    public const string LBL_REQUIRED_STRENGTH = "{2}Required Strength: {0}{1}";
    public const string LBL_REQUIRED_DEXTERITY = "{2}Required Dexterity: {0}{1}";
    public const string LBL_REQUIRED_INTELLIGENCE = "{2}Required Intelligence: {0}{1}";
    public const string LBL_REQUIRED_CONSTITUTION = "{2}Required Constitution: {0}{1}";
    public const string LBL_REQUIRED_LEVEL = "{2}Required Level: {0}{1}";
}