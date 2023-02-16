using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public static bool SessionExists { get { return PlayerPrefs.HasKey("Main"); } }

    public static void LoadSession()
    {
        if(PlayerPrefs.HasKey("Main"))
        {
            int area = PlayerPrefs.GetInt("Area");
        }
    }

    public static void NewSession(PlayerCharacter.Class startingClass)
    {
        PlayerPrefs.SetInt("Main", 1);
        PlayerPrefs.SetInt("Area", 0);

        switch (startingClass)
        {
            case PlayerCharacter.Class.Melee:
                PlayerPrefs.SetInt("Str", 25);
                PlayerPrefs.SetInt("Dex", 20);
                PlayerPrefs.SetInt("Int", 15);
                PlayerPrefs.SetInt("Con", 25);
                PlayerPrefs.SetString("Primary", "Items/PRIMARY/1/Short Sword");
                PlayerPrefs.SetString("Secondary", "Items/SECONDARY/1/Buckler");
                break;
            case PlayerCharacter.Class.Ranged:
                PlayerPrefs.SetInt("Str", 20);
                PlayerPrefs.SetInt("Dex", 25);
                PlayerPrefs.SetInt("Int", 15);
                PlayerPrefs.SetInt("Con", 20);
                PlayerPrefs.SetString("Primary", "Items/PRIMARY/1/Short Wooden Bow");
                break;
            case PlayerCharacter.Class.Magic:
                PlayerPrefs.SetInt("Str", 10);
                PlayerPrefs.SetInt("Dex", 25);
                PlayerPrefs.SetInt("Int", 35);
                PlayerPrefs.SetInt("Con", 10);
                PlayerPrefs.SetString("Primary", "Items/PRIMARY/1/Short Staff");
                break;
        }
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
