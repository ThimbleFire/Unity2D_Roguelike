using UnityEngine;

public class HUDControls : MonoBehaviour
{
    public GameObject inventory;
    public GameObject joystick;
    public GameObject mobileButton;

    public void ToggleInventory()
    {
        inventory.GetComponent<Inventory>().Hide();
        inventory.SetActive( !inventory.activeSelf );
        joystick.SetActive( !inventory.activeSelf );
        mobileButton.SetActive( !inventory.activeSelf );
    }
}