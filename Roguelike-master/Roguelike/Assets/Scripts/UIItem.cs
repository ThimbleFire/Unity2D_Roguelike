using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image image;
    public string binary;
    public bool Occupied { get { return Binary.ToDecimal( binary, 0 ) > 0; } }
    public Item item = new Item();
    public int childIndex;

    private void Awake()
    {
        childIndex = transform.GetSiblingIndex();

        string savedData = PlayerPrefs.GetString( "i" + childIndex );

        if ( savedData != string.Empty )
            binary = savedData;

        if ( Occupied )
            SetItem( );
    }

    public void SetItem( )
    {
        item.Build( binary );

        if ( Occupied )
        {
            image.sprite = item.sprite;
            image.color = Color.white;
        }
    }

    public void OnBeginDrag()
    {
        Inventory.Instance.InventoryOnBeginDrag( item );
    }

    public void OnPointerUp()
    {
        Inventory.Instance.InventoryEndDrag();
    }

    public void OnDrop()
    {
        item = Inventory.Instance.itemBeingDragged;
    }

    public void OnPointerClick()
    {
        Inventory.Instance.InventorySelect( rectTransform.anchoredPosition, item );
    }
}