using UnityEngine;

namespace AlwaysEast
{
    public class GearSlot : MonoBehaviour
    {
        public ItemState.Type type;
        public ItemStats itemStats = null;

        private void Start()
        {
            ItemProfile itemProfile = Game.LoadState<ItemProfile>(gameObject.name + ".east");

            if (itemProfile == null)
                return;

            if (itemProfile.ItemPath == null)
                return;

            ItemStats item = Inventory.Pickup(itemProfile);
            
            if (type == ItemState.Type.ANY)
                EquipInventory(item);
            else
                Equip(item);
            
            Inventory.OnGearChange(item, true);
        }

        public void Equip(ItemStats itemBeingSelected)
        {
            itemStats = itemBeingSelected;
            itemStats.itemLocation = ItemStats.ItemLocation.EQUIPPED;
            itemStats.transform.SetParent(transform);
        }

        public void EquipInventory(ItemStats itemBeingSelected)
        {
            itemStats = itemBeingSelected;
            itemStats.itemLocation = ItemStats.ItemLocation.INVENTORY;
            itemStats.transform.SetParent(transform);
        }

        public void Unequip()
        {
            itemStats.itemLocation = ItemStats.ItemLocation.INVENTORY;
            itemStats = null;
        }

        private void OnApplicationQuit()
        {
            ItemProfile itemProfile = new ItemProfile();

            if (itemStats != null)
            {
                itemProfile.ItemPath = itemStats.Path;
                itemProfile.Type = type;
            }
            
            Game.SaveState(itemProfile, gameObject.name + ".east");
        }
    }
}