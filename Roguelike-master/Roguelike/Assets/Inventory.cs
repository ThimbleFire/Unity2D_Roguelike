using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventory;
    public GameObject mobileButton;
    public GameObject joystick;

    public void Toggle()
    {
        inventory.SetActive( !inventory.activeSelf );
        mobileButton.SetActive( !mobileButton.activeSelf );
        joystick.SetActive( !joystick.activeSelf );
    }
}
