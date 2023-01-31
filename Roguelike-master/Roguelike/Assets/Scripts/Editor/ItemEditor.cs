using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class ItemEditor : EditorBase
{
    Item activeItem;
    TextAsset obj;
    AnimatorOverrideController animationOverrideController;
    public List<ItemStats.Implicit> Implicits = new List<ItemStats.Implicit>();
    public List<ItemStats.Prefix> Prefixes = new List<ItemStats.Prefix>();
    public List<ItemStats.Suffix> Suffixes = new List<ItemStats.Suffix>();


    [MenuItem("Window/Editor/Items")]
    private static void ShowWindow()
    {
        GetWindow(typeof(ItemEditor));
    }

    private void Awake()
    {
        activeItem = new Item();
    }

    protected override void MainWindow()
    {
        obj = PaintXMLLookup(obj);
        PaintTextField(ref activeItem.Name, "Item Name");
        activeItem.ItemType = (ItemStats.Type)PaintPopup(ItemStats.Type_Text, (int)activeItem.ItemType, "Item Type");
        PaintSpriteField(ref activeItem.SpriteUI, ref activeItem.SpriteUIFilename);
        animationOverrideController = PaintAnimationOverrideControllerLookup(animationOverrideController);
        PaintIntField(ref activeItem.ItemLevel, "Item Level");
        PaintIntField(ref activeItem.DmgMin, "Minimum Damage");
        PaintIntField(ref activeItem.DmgMax, "Maximum Damage");
        PaintIntField(ref activeItem.DefMin, "Defense Damage");
        PaintIntField(ref activeItem.DefMax, "Defense Damage");
        PaintIntField(ref activeItem.Blockrate, "Chance to block");
        PaintTextField(ref activeItem.Description, "Item Description");
        PaintIntSlider(ref activeItem.ReqStr, 0, 255, "Strength Requirement");
        PaintIntSlider(ref activeItem.ReqDex, 0, 255, "Dexterity Requirement");
        PaintIntSlider(ref activeItem.ReqInt, 0, 255, "Intelligence Requirement");
        PaintIntSlider(ref activeItem.ReqCons, 0, 255, "Constitution Requirement");
        PaintIntSlider(ref activeItem.ReqLvl, 0, 60, "Level Requirement");
        Checkbox(ref activeItem.Unique, "Unique");
        PaintList<ItemStats.Implicit>("Implicits");
        PaintList<ItemStats.Prefix>("Prefixes");
        PaintList<ItemStats.Suffix>("Suffixes");
        

        base.MainWindow();
    }

    protected override void ResetProperties()
    {

    }

    protected override void LoadProperties()
    {

    }

    protected override void CreationWindow()
    {
        base.CreationWindow();  
    }

    protected override void OnClick_SaveButton()
    {

    }
}
