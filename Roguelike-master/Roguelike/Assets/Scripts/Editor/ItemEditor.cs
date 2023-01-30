using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class ItemEditor : EditorWindow
{
    ItemStats itemStats;

    [MenuItem("Window/Editor/Items")]
    private static void ShowWindow()
    {
        GetWindow(typeof(ItemEditor));
    }

    private void OnGUI()
    {
        /*
        /   Item name
        /   UI sprite
        /   EquippedSpritesheet
        /   IsUnique
        /   
        /   ItemType
        /   value1 (rename this to damageMin)
        /   value2 (rename this to damageMax)
        /   value3 (call this defense)
        /   value4 (call this chance to block)
        /   durability  (if ItemType != consumable)
        /   
        /   implicit
        /   prefix  (if IsUnique)  
        /   suffix  (if IsUnique)
        */
    }

    private void SaveOnAndroid(string itemName)
    {
        XMLUtility.Save<ItemStats>(itemStats, "Items/"+itemName);
        Debug.Log("Saved");
    }
}
