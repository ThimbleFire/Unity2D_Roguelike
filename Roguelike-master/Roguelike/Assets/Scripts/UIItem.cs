using UnityEngine;
using UnityEngine.UI;

class UIItem : UIDraggable
{
    public Vector2Int inventoryCellPosition;
    public string Binary { get; set; }
    private bool Occupied { get { return Binary.Empty; } }

    public virtul void Select()
    {
        Game.InventorySelect(inventoryCellPosition, Binary);
    }
}
