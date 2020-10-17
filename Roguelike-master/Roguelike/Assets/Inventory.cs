using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; set; }

    public GameObject inventory;
    public GameObject mobileButton;
    public GameObject joystick;

    public UIItem[] items;

    public GameObject itemDragHandle;

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

    public void InventoryDrag( Item item )
    {

    }

    public void InventoryEndDrag()
    {

    }

    public static Vector2Int selectedInventoryCellPosition;
    private static string selectedInventoryBinary = string.Empty;

    public void InventorySelect( Vector2 rectTransform, Item item )
    {

    }

    public void AddItem( string bin )
    {
        byte i = FindEmptyInventorySlot();

        items[i].Setup(Item.Build( bin ));
    }


    public void EquipItem( string bin )
    {

    }

    private byte FindEmptyInventorySlot()
    {
        byte i = 0;

        foreach ( UIItem item in items )
        {
            string snippet = item.Binary.Substring( i * 8, 8 );

            if ( ItemBinary.Build( snippet ) == 0 )
                return i;
            else
                i++;
        }

        return byte.MaxValue;
    }
}