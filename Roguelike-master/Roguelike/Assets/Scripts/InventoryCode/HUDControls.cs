using UnityEngine;

public class HUDControls : MonoBehaviour
{
    public GameObject inventory;
    public GameObject mobileButton;

    public static bool InventoryOpened;

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
}