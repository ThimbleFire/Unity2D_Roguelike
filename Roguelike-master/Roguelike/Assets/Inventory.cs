using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventory;
    public GameObject mobileButton;
    public GameObject joystick;

    public UIItem[] items;

    public void Toggle()
    {
        inventory.SetActive( !inventory.activeSelf );
        mobileButton.SetActive( !mobileButton.activeSelf );
        joystick.SetActive( !joystick.activeSelf );
    }

    public void AddItem( string bin )
    {
        byte i = FindEmptyInventorySlot();

        items[i].item = Item.Build( bin );
        items[i].Binary = bin;
    }

    public void EquipItem( string bin )
    {

    }

    private byte FindEmptyInventorySlot()
    {
        byte i = 0;

        foreach ( UIItem item in items )
        {
            if ( item.Occupied == false )
                return i;
            else
                i++;
        }

        return byte.MaxValue;
    }
}