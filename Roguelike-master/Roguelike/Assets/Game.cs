using System;
using UnityEngine;

class Game
{
    public const float PPU = 16;

    public static Vector2Int selectedInventoryCellPosition;
    private static string selectedInventoryBinary = string.Empty;

    public static void InventorySelect( Vector2Int inventoryCellPosition, string binary )
    {
        selectedInventoryCellPosition = inventoryCellPosition;
        selectedInventoryBinary = binary;
    }

    //for loading data from mongo
    public static void LoadOnline()
    {

    }

    //for loading data from phone
    public static void LoadOffline()
    {

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

        SetupNewGame();
    }

    public static void SetupNewGame()
    {
        PlayerPrefs.SetInt( "firsttimesetup", 1 );
        //add startup items

        Inventory inventory = GameObject.Find( "GameManager" ).GetComponent<Inventory>();

        for ( int i = 0; i < 21; i++ )
        {
            inventory.AddItem( "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" );
        }
    }
}
