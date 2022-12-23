using System;
using UnityEngine;

public class HUDControls : MonoBehaviour
{
    public GameObject inventory;
    public static GameObject mobileButton;

    public static bool InventoryOpened;

    private void Awake()
    {
        mobileButton = GameObject.Find( "Mobile Button" );
    }

    public void ToggleInventory()
    {
        // This code is really fucking dumb
        //
        // Because if inventory is disabled, we can't call the code to re-enable it
        // so we have to toggle it via HUDControls


        inventory.GetComponent<Inventory>().Hide();
        inventory.SetActive( !inventory.activeSelf );
        mobileButton.SetActive( !inventory.activeSelf );
        
        InventoryOpened = inventory.activeSelf;
    }

    public static void Show() => mobileButton.SetActive( true );
    public static void Hide() => mobileButton.SetActive( false );
}