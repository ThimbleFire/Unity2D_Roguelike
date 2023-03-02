using UnityEngine;

namespace AlwaysEast
{
    public class GearSlot : MonoBehaviour
    {
        public Item.Type type;
        public GameObject character;
        public ItemStats itemStats = null;

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