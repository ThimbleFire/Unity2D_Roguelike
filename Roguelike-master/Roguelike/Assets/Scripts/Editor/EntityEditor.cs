using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntityEditor : EditorBase
{
    Vector2 scrollView;

    EntityReplacement activeEntity;
    TextAsset obj;
    UnityEngine.AnimatorOverrideController animatorOverrideController;

    [MenuItem("Window/Editor/Entities")]
    private static void ShowWindow()
    {
        GetWindow(typeof(EntityEditor));
    }

    private void Awake()
    {
        activeEntity = new EntityReplacement();
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
            PaintTextField(ref activeEntity.Name, "Entity Name");
            activeEntity.ItemType = (Item.Type)PaintPopup(ItemStats.Type_Text, (int)activeEntity.ItemType, "Item Type");
            PaintSpriteField(ref SpriteUI);
            animatorOverrideController = PaintAnimationOverrideControllerLookup(animatorOverrideController);
            PaintIntField(ref activeEntity.ItemLevel, "Entity Level");
        }
        EditorGUILayout.EndScrollView();
        
        base.MainWindow();
    }

    protected override void ResetProperties()
    {

    }

    protected override void LoadProperties(TextAsset textAsset)
    {
        activeEntity = XMLUtility.Load<EntityReplacement>(textAsset);
    }

    protected override void CreationWindow()
    {
        base.CreationWindow();  
    }

    private void Save()
    {
//        string filePath = AssetDatabase.GetAssetPath(SpriteUI).Substring("Assets/Resources/".Length);
//        filePath = filePath.Substring(0, filePath.Length - 4);
//        activeEntity.SpriteUIFilename = SpriteUI == null ? string.Empty : filePath;
//
//        filePath = AssetDatabase.GetAssetPath(animatorOverrideController).Substring("Assets/Resources/".Length);
//        activeEntity.animationName = animatorOverrideController == null ? string.Empty : filePath;
//        XMLUtility.Save<Item>(activeEntity, "Items/", activeEntity.Name);
    }
}
