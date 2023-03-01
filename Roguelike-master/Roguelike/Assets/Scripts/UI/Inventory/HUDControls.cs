using UnityEngine;

namespace AlwaysEast
{
    public class HUDControls : MonoBehaviour
    {
        public GameObject inventory_prefab;
        private GameObject inventory;
        public static GameObject mobileButton;
        public GameObject toggleInventoryButton;

        public static bool InventoryOpened;

        private void Awake()
        {
            mobileButton = GameObject.Find("Mobile Buttons");
            inventory = Instantiate(inventory_prefab, transform.parent);

        }

        public void ToggleInventory()
        {
            inventory.GetComponent<Inventory>().Hide();
            inventory.SetActive(!inventory.activeSelf);
            mobileButton.SetActive(!inventory.activeSelf);

            InventoryOpened = inventory.activeSelf;
            toggleInventoryButton.transform.SetAsLastSibling();
        }

        public static void Show() => mobileButton.SetActive(true);

        public static void Hide() => mobileButton.SetActive(false);
    }
}