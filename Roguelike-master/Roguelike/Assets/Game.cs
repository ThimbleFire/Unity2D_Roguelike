using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public static void LoadSession()
    {
        if(PlayerPrefs.HasKey("Main"))
        {
            int area = PlayerPrefs.GetInt("Area");
        }
    }
}
