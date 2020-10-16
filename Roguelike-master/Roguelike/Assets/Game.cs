using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
}