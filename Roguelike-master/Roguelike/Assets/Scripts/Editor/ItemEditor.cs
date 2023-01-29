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
        
    }

    private void SaveOnAndroid(string itemName)
    {
        XMLUtility.Save<ItemStats>(itemStats, "Items/"+itemName);
        Debug.Log("Saved");
    }
}
