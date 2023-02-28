using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlots : MonoBehaviour
{
    public GearSlot[] slots;

    public GearSlot GetEmpty()
    {
        foreach (GearSlot item in slots)
        {
            if (item.transform.childCount == 0)
                return item;
        }

        return null;
    }
}
