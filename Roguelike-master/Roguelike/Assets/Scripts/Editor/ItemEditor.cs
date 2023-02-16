using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemEditor : EditorBase
{
    Vector2 scrollView;

    Item activeItem;
    TextAsset obj;
    public List<Item.Implicit> Implicits = new List<Item.Implicit>();
    public List<Item.Prefix> Prefixes = new List<Item.Prefix>();
    public List<Item.Suffix> Suffixes = new List<Item.Suffix>();
    public UnityEngine.Sprite SpriteUI;
    public UnityEngine.AnimatorOverrideController animatorOverrideController;


    [MenuItem("Window/Editor/Items")]
    private static void ShowWindow()
    {
        GetWindow(typeof(ItemEditor));
    }

    private void Awake()
    {
        so = new SerializedObject(this);
        activeItem = new Item();
        Implicits = new List<Item.Implicit>();
        Prefixes = new List<Item.Prefix>();
        Suffixes = new List<Item.Suffix>();
    }

    protected override void MainWindow()
    {
        scrollView = EditorGUILayout.BeginScrollView(scrollView, false, true, GUILayout.Width(position.width));
        {
            obj = PaintXMLLookup(obj, "Resource File", true);
            if (PaintButton("Save"))
            {
                Save();
            }
            PaintTextField(ref activeItem.Name, "Item Name");
            activeItem.ItemType = (Item.Type)PaintPopup(ItemStats.Type_Text, (int)activeItem.ItemType, "Item Type");
            PaintSpriteField(ref SpriteUI);
            animatorOverrideController = PaintAnimationOverrideControllerLookup(animatorOverrideController);
            PaintIntField(ref activeItem.qlvl, "Quality Level");
            PaintIntField(ref activeItem.DmgMin, "Min Damage");
            PaintIntField(ref activeItem.DmgMax, "Max Damage");
            PaintIntField(ref activeItem.DefMin, "Min Defense");
            PaintIntField(ref activeItem.DefMax, "Max Defense");
            PaintIntField(ref activeItem.Blockrate, "Chance to Block");
            PaintIntField(ref activeItem.Durability, "Durability");
            PaintTextField(ref activeItem.Description, "Item Description");
            PaintIntSlider(ref activeItem.ReqStr, 0, 255, "Str Requirement");
            PaintIntSlider(ref activeItem.ReqDex, 0, 255, "Dex Requirement");
            PaintIntSlider(ref activeItem.ReqInt, 0, 255, "Int Requirement");
            PaintIntSlider(ref activeItem.ReqCons, 0, 255, "Con Requirement");
            PaintIntSlider(ref activeItem.ReqLvl, 0, 60, "Lvl Requirement"); 
            if (Checkbox(ref activeItem.Unique, "Unique"))
            {
                PaintList<Item.Prefix>("Prefixes");
                PaintList<Item.Suffix>("Suffixes");
            }
            PaintList<Item.Implicit>("Implicits");
        }
        EditorGUILayout.EndScrollView();

        base.MainWindow();
    }

    protected override void ResetProperties()
    {

    }

    protected override void LoadProperties(TextAsset textAsset)
    {
        activeItem = XMLUtility.Load<Item>(textAsset);

        Implicits = activeItem.Implicits;
        Prefixes = activeItem.Prefixes;
        Suffixes = activeItem.Suffixes;

        SpriteUI = Resources.Load<Sprite>(activeItem.SpriteUIFilename);
    }

    protected override void CreationWindow()
    {
        base.CreationWindow();  
    }

    private void Save()
    {
        activeItem.Implicits = Implicits;
        activeItem.Prefixes = Prefixes;
        activeItem.Suffixes = Suffixes;

        string filePath = string.Empty;

        // UI Sprite
        if (SpriteUI != null)
        {
            filePath = AssetDatabase.GetAssetPath(SpriteUI).Substring(S_RESOURCE_DIR_LENGTH);
            filePath = filePath.Substring(0, filePath.Length - S_PNG_EXTENSION_LENGTH);
            activeItem.SpriteUIFilename = SpriteUI == null ? string.Empty : filePath;
        }

        // Animation
        if (animatorOverrideController != null)
        {
            filePath = AssetDatabase.GetAssetPath(animatorOverrideController).Substring(S_RESOURCE_DIR_LENGTH);
            filePath = filePath.Substring(0, filePath.Length - S_OVERRIDECONTROLLER_LENGTH);
            activeItem.animationName = animatorOverrideController == null ? string.Empty : filePath;
        }

        System.Text.StringBuilder t = new System.Text.StringBuilder(S_ITEMS_DIR);
        t.Append(activeItem.ItemType);
        t.Append("/");
        t.Append(activeItem.qlvl);
        t.Append("/");

        XMLUtility.Save<Item>(activeItem, t.ToString(), activeItem.Name);
    }    
}
