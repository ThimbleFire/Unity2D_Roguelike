using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GearSlot : MonoBehaviour
{
    public ItemStats.Type type;
    public GameObject character;
    public ItemStats itemStats = null;

    public void Equip( ItemStats itemBeingSelected )
    {
        this.itemStats = itemBeingSelected;
        this.itemStats.Equipped = true;
        this.itemStats.transform.SetParent( transform );
    }

    internal void Unequip( ItemStats itemBeingSelected )
    {
        itemBeingSelected.transform.SetParent( transform );
        itemBeingSelected.Equipped = false;
        this.itemStats = null;
    }
}
