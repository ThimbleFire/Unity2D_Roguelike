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

    public static void LoadAccount()
    {
        //gets player and inventory info from database
        //load the 28 inventory slots and however many gear slots there are.
    }

    public static void SaveState(into playerID, string v)
    {

    }
}
