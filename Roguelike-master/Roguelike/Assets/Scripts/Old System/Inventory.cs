using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IHasChanged
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

    public static void Pickup(Item item)
    {

    }

    public void HasChanged()
    {

    }
}

namespace UnityEngine.EventSystems
{
    public interface IHasChanged : IEventSystemHandler
    {
        void HasChanged();
    }
}