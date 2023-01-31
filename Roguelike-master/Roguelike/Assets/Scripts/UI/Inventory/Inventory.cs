using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IItemClickHandler {
    public static bool IsItemSelected = false;

    public RectTransform selectedCell;

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
        ItemStats itemBeingSelected = t.GetComponent<ItemStats>();

        // To select the item
        if ( IsItemSelected == false ) {
            IsItemSelected = true;
            selectedCell.gameObject.SetActive( true );
            selectedCell.position = itemBeingSelected.GetComponent<RectTransform>().position;
            ItemStatBillboard.Draw( itemBeingSelected );
        }

        //To click the item while it's already selected
        else {
            ItemStatBillboard.Hide();
            selectedCell.gameObject.SetActive( false );
            IsItemSelected = false;

            if (itemBeingSelected.Equipped == true)
            {
                foreach (GearSlot iSlot in inventorySlots.slots)
                {
                    if (iSlot.itemStats == null)
                    {
                        OnEquipmentChange.Invoke(itemBeingSelected, false);
                        iSlot.Unequip(itemBeingSelected);
                        break;
                    }
                }
            }
            else if (itemBeingSelected.Equipped == false)
            {
                foreach (GearSlot gSlot in gearSlots.slots)
                {
                    if (itemBeingSelected.ItemType == gSlot.type)
                    {
                        OnEquipmentChange.Invoke(itemBeingSelected, true);
                        gSlot.Equip(itemBeingSelected);
                        break;
                    }
                }
            }

            itemBeingSelected = null;
        }
    }

    public static void Pickup(string itemName)
    {
        Transform inventorySlotTransform = inventorySlots.GetEmptyTransform();

        ItemStats itemStats = Instantiate(baseItem, inventorySlotTransform).GetComponent<ItemStats>();
        itemStats.Load(itemName);
    }
}

namespace UnityEngine.EventSystems {

    public interface IItemClickHandler : IEventSystemHandler {

        void OnItemClick( UIItemOnClick t );
    }
}