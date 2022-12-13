using UnityEngine;

public class UIInputManager : MonoBehaviour
{
    public GameObject inventoryWindow;
    public GameObject characterWindow;
    public Inventory inventory;

    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.I ) )
        {
            AudioDevice.PlayGeneric( AudioDevice.Sound.WindowOpen );
            inventoryWindow.SetActive( !inventoryWindow.activeInHierarchy );
        }

        if ( Input.GetKeyDown( KeyCode.C ) )
        {
            AudioDevice.PlayGeneric( AudioDevice.Sound.WindowOpen );
            characterWindow.SetActive( !characterWindow.activeInHierarchy );
        }
    }
}
