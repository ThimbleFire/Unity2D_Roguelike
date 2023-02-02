using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IItemClickHandler {
    public static bool IsItemSelected = false;

    public RectTransform selectedCell;
    private ItemStats itemBeingSelected;

    public delegate void OnEquipmentChangeHandler( ItemStats itemStats, bool adding );
    public static event OnEquipmentChangeHandler OnEquipmentChange;

    private static GameObject baseItem;
    private static InventorySlots inventorySlots;
    private static GearSlots gearSlots;

    private void Awake()
    {
        baseItem = Resources.Load("Prefabs/BaseItem") as GameObject;
        inventorySlots = GetComponentInChildren<InventorySlots>();
        gearSlots = GetComponentInChildren<GearSlots>();
    }

    public void Hide() {
        if ( IsItemSelected ) {
            ItemStatBillboard.Hide();
            selectedCell.gameObject.SetActive( false );
            IsItemSelected = false;
        }
    }

    public void OnItemClick( UIItemOnClick t ) {

        if(t.GetComponent<ItemStats>() != itemBeingSelected)
        {
            IsItemSelected = false;
        }

        // To select the item
        if ( IsItemSelected == false ) 
        {
            IsItemSelected = true;
            itemBeingSelected = t.GetComponent<ItemStats>();
            selectedCell.gameObject.SetActive( true );
            selectedCell.position = itemBeingSelected.GetComponent<RectTransform>().position;
            ItemStatBillboard.Draw( itemBeingSelected );
        }
        else 
        {
            ItemStatBillboard.Hide();
            selectedCell.gameObject.SetActive( false );
            IsItemSelected = false;

            if (itemBeingSelected.Equipped == true)
            {
                Transform emptyInventorySlotTransform = inventorySlots.GetEmpty().Unequip();
                itemBeingSelected.transform.SetParent(emptyInventorySlotTransform);
                itemBeingSelected.Equipped = false;
                OnEquipmentChange.Invoke(itemBeingSelected, false);
            }
            else if (itemBeingSelected.Equipped == false)
            {
                if (itemBeingSelected.RequirementsMetAll == false)
                    return;

                GearSlot slot = gearSlots.GetEmpty(itemBeingSelected.ItemType);
                if (slot != null)
                {
                    slot.Equip(itemBeingSelected);
                    OnEquipmentChange.Invoke(itemBeingSelected, true);
                }
            }

            itemBeingSelected = null;
        }
    }

    public static void Pickup(string itemName)
    {
        Transform inventorySlotTransform = inventorySlots.GetEmpty().transform;
        ItemStats itemStats = Instantiate(baseItem, inventorySlotTransform).GetComponent<ItemStats>();
        itemStats.Load(itemName);
    }
}

namespace UnityEngine.EventSystems {

    public interface IItemClickHandler : IEventSystemHandler {

        void OnItemClick( UIItemOnClick t );
    }
}