using System;
using UnityEngine;

class Game
{
    public const float PPU = 16;

    //for loading data from mongo
    public static void LoadOnline()
    {

    }

    //for loading data from phone
    public static void LoadOffline()
    {
        Inventory inventory = GameObject.Find( "GameManager" ).GetComponent<Inventory>();

        for ( int i = 0; i < 21; i++ )
        {
            inventory.AddItem( "0000000100000000000000000000000000000000000000000000000000000000000000000000000000000000" );
        }
    }

    //for saving data to mongo
    public static void SaveStateOnline( int playerID, string v )
    {

    }


    //for saving data to the phone
    public static void SaveStateOffline( string v )
    {
        PlayerPrefs.SetString( "everything", v );
    }

    public static void Setup()
    {
        //int a = PlayerPrefs.GetInt( "firsttimesetup", 0);

        //switch ( a )
        //{
        //    case 0: SetupNewGame(); break;
        //    case 1: LoadOffline();  break;
        //}

        LoadOffline();
    }
}
