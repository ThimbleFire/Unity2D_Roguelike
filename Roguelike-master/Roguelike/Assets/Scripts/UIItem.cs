using UnityEngine;

class UIItem : UIDraggable
{
    public Vector2Int inventoryCellPosition;
    public string Binary { get; set; }
    private bool Occupied { get { return Binary.Length > 0; } }

    public virtual void Select()
    {
        Game.InventorySelect(inventoryCellPosition, Binary);
    }
}
