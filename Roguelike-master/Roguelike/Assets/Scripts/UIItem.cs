using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image image; 
    [SerializeField]
    private string binary;
    public string Binary { get { return binary; } set { binary = value; } }
    public bool Occupied { get { return Binary.Length > 0; } }
    public Item item;

    public void OnBeginDrag( )
    {
        Inventory.Instance.InventoryDrag( item );
    }

    public void OnPointerUp( )
    {
        Inventory.Instance.InventoryEndDrag();
    }

    public void OnDrop( )
    {

    }

    public void OnPointerClick( )
    {
        Inventory.Instance.InventorySelect( rectTransform.anchoredPosition, item);
    }

    public void Setup( Item item )
    {
        this.item = item;
        image.sprite = Resources.LoadAll<Sprite>( "UI/Inventory/Items/spritesheet" )[(byte)item.property[0]];
        image.color = Color.white;
    }
}
