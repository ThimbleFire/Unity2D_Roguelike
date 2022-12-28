using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IItemClickHandler {
    public static bool IsItemSelected = false;

    public RectTransform selectedCell;
    public GearSlot[] gearSlots;
    public GearSlot[] inventorySlots;

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

            if ( itemBeingSelected.Equipped == true ) {
                foreach ( GearSlot iSlot in inventorySlots ) {
                    if ( iSlot.itemStats == null ) {
                        iSlot.Unequip( itemBeingSelected );
                        break;
                    }
                }
            }
            else if ( itemBeingSelected.Equipped == false ) {
                foreach ( GearSlot gSlot in gearSlots ) {
                    if ( gSlot.type == itemBeingSelected.type ) {
                        gSlot.Equip( itemBeingSelected );
                        break;
                    }
                }
            }

            itemBeingSelected = null;
        }
    }
}

namespace UnityEngine.EventSystems {

    public interface IItemClickHandler : IEventSystemHandler {

        void OnItemClick( UIItemOnClick t );
    }
}