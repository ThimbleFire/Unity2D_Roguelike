using UnityEngine;

namespace AlwaysEast
{
    public class GearSlot : MonoBehaviour
    {
        public ItemState.Type type;
        public ItemStats itemStats = null;

        private void Start()
        {
            ItemProfile itemProfile = Game.LoadState<ItemProfile>(type.ToString() + ".east");

            if (itemProfile == null)
                return;

            ItemStats item = Inventory.Pickup(itemProfile);
            Equip(item);
            Inventory.OnGearChange(item, true);
        }

        public void Equip(ItemStats itemBeingSelected)
        {
            itemStats = itemBeingSelected;
            itemStats.Equipped = true;
            itemStats.transform.SetParent(transform);
        }

        internal Transform Unequip()
        {
            itemStats = null;
            return transform;
        }
    }
}