using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; set; }

    public GameObject inventory;
    public GameObject mobileButton;
    public GameObject joystick;

    public UIItem[] items;

    public RectTransform itemDragHandle;

    public Item itemBeingDragged;

    private bool dragging = false;

    private void Awake()
    {
        Instance = this;
    }

    public void Toggle()
    {
        inventory.SetActive( !inventory.activeSelf );
        mobileButton.SetActive( !mobileButton.activeSelf );
        joystick.SetActive( !joystick.activeSelf );
    }

    public void InventoryOnBeginDrag( Item item )
    {
        itemBeingDragged = item;
        itemDragHandle.gameObject.SetActive( true );
        itemDragHandle.GetComponent<Image>().sprite = item.sprite;
        dragging = true;
    }

    public void InventoryEndDrag()
    {
        itemDragHandle.gameObject.SetActive( false );
        dragging = false;
    }

    private void Update()
    {
        if(dragging)
        {
            itemDragHandle.position = Input.mousePosition;
        }
    }

    public static Vector2Int selectedInventoryCellPosition;
    private static string selectedInventoryBinary = string.Empty;

    public void InventorySelect( Vector2 rectTransform, Item item )
    {
        //d
    }

    public void AddItem( string bin )
    {
        byte i = FindEmptyInventorySlot();

        items[i].Setup( );
    }

    public void EquipItem( string bin )
    {
    }

    private byte FindEmptyInventorySlot()
    {
        byte i = 0;

        foreach ( UIItem item in items )
        {
            if ( Binary.ToDecimal( item.binary, i ) == 0 )
                return i;
            else
                i++;
        }

        return byte.MaxValue;
    }
}