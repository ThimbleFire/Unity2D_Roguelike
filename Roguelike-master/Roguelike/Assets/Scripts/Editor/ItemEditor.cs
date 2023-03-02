using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AlwaysEast
{
    public class ItemEditor : EditorBase
    {
        Vector2 scrollView;

        ItemState activeItem;
        TextAsset obj;
        public List<ItemState.Implicit> Implicits = new List<ItemState.Implicit>();
        public List<ItemState.Prefix> Prefixes = new List<ItemState.Prefix>();
        public List<ItemState.Suffix> Suffixes = new List<ItemState.Suffix>();
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
            activeItem = new ItemState();
            Implicits = new List<ItemState.Implicit>();
            Prefixes = new List<ItemState.Prefix>();
            Suffixes = new List<ItemState.Suffix>();
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
                activeItem.ItemType = (ItemState.Type)PaintPopup(Helper.ItemTypeNames, (int)activeItem.ItemType, "Item Type");
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
                    PaintList<ItemState.Prefix>("Prefixes");
                    PaintList<ItemState.Suffix>("Suffixes");
                }
                PaintList<ItemState.Implicit>("Implicits");
            }
            EditorGUILayout.EndScrollView();

            base.MainWindow();
        }

        protected override void ResetProperties()
        {

        }

        protected override void LoadProperties(TextAsset textAsset)
        {
            activeItem = XMLUtility.Load<ItemState>(textAsset);

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

            XMLUtility.Save<ItemState>(activeItem, t.ToString(), activeItem.Name);
        }
    }
}