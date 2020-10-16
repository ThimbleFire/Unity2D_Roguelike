using UnityEngine;
using UnityEngine.UI;

class UIItem : UIDraggable
{
    public Vector2Int inventoryCellPosition;

    public void Select()
    {
        Game.InventorySelect(inventoryCellPosition);
    }
}
