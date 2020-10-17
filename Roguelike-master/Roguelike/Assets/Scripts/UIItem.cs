using UnityEngine;

public class UIItem : UIDraggable
{
    [SerializeField]
    private string binary;
    public string Binary { get { return binary; } set { binary = value; } }
    public bool Occupied { get { return Binary.Length > 0; } }
    public Vector2Int inventoryCellPosition;
    public Item item;

    public virtual void Select()
    {
        Game.InventorySelect(inventoryCellPosition, Binary);
    }
}
