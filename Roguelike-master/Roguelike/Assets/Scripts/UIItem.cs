using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image image; 
    public string binary;
    public bool Occupied { get { return Binary.ToDecimal( binary, 0 ) > 0; } }
    public Item item;

    private void Awake()
    {
        item = new Item();
    }

    public void OnBeginDrag( )
    {
        Inventory.Instance.InventoryOnBeginDrag( item );
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

    public void Setup( string bin )
    {
        binary = bin;

        item.Build( bin );

        if ( Occupied )
        {
            image.sprite = item.sprite;
            image.color = Color.white;
        }
    }
}
