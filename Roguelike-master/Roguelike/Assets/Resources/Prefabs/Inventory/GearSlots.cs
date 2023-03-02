using UnityEngine;

namespace AlwaysEast
{
    public class GearSlots : MonoBehaviour
    {
        public GearSlot[] slots;

        public GearSlot GetEmpty(ItemState.Type itemType)
        {
            foreach (GearSlot slot in slots)
            {
                if (slot.type == itemType)
                {
                    if (slot.transform.childCount == 0)
                        return slot;
                }
            }
            return null;
        }

        public GearSlot GetOccupied(ItemState.Type itemType)
        {
            foreach (GearSlot slot in slots)
            {
                if (slot.type == itemType)
                {
                    if (slot.transform.childCount == 1)
                        return slot;
                }
            }
            return null;
        }
    }
}