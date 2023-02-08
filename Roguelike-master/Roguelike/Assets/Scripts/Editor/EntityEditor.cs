using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntityEditor : EditorBase
{
    Vector2 scrollView;

    EntityReplacement activeEntity;
    TextAsset obj;
    UnityEngine.AnimatorOverrideController animatorOverrideController;
    string animatorOverrideControllerPath;
    string animatorOverrideControllerFileName;

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
            
            animatorOverrideController = PaintAnimationOverrideControllerLookup(animatorOverrideController);
            
            PaintTextField(ref activeEntity.entityBaseStats.Name, "Entity Name");
            PaintIntField(ref activeEntity.entityBaseStats.Level, "Entity Level");
            PaintIntField(ref activeEntity.entityBaseStats.Speed, "Entity Speed");
            
            PaintIntField(ref activeEntity.entityBaseStats.LifeMax, "Entity Max Life");
            PaintIntField(ref activeEntity.entityBaseStats.ManaMax, "Entity Max Mana");
            
            PaintIntField(ref activeEntity.entityBaseStats.RangeOfAggression, "Entity Aggression Range");
            PaintIntField(ref activeEntity.entityBaseStats.Experience, "Entity Experience On Death");
            
            PaintIntField(ref activeEntity.entityBaseStats.ItemFind, "Entity ItemFind");
            PaintIntField(ref activeEntity.entityBaseStats.MagicFind, "Entity MagicFind");
            
            PaintIntField(ref activeEntity.entityBaseStats.ResFire, "Entity ResFire");
            PaintIntField(ref activeEntity.entityBaseStats.ResCold, "Entity ResCold");
            PaintIntField(ref activeEntity.entityBaseStats.ResLight, "Entity ResLight");
            PaintIntField(ref activeEntity.entityBaseStats.ResPoison, "Entity ResPoison");
            PaintIntField(ref activeEntity.entityBaseStats.ResAll, "Entity ResAll");
            
            PaintIntField(ref activeEntity.entityBaseStats.AttackRating, "Entity AttackRating");
            PaintIntField(ref activeEntity.entityBaseStats.ChanceToBlock, "Entity ChanceToBlock");
            PaintIntField(ref activeEntity.entityBaseStats.Defense, "Entity Defense");
            
            PaintIntField(ref activeEntity.entityBaseStats.DmgPhyMin, "Entity DmgPhyMin");
            PaintIntField(ref activeEntity.entityBaseStats.DmgPhyMax, "Entity DmgPhyMax");
            
            PaintIntField(ref activeEntity.entityBaseStats.DmgFireMin, "Entity DmgFireMin");
            PaintIntField(ref activeEntity.entityBaseStats.DmgColdMin, "Entity DmgColdMin");
            PaintIntField(ref activeEntity.entityBaseStats.DmgLightMin, "Entity DmgLightMin");
            PaintIntField(ref activeEntity.entityBaseStats.DmgPoisonMin, "Entity DmgPoisonMin");
            PaintIntField(ref activeEntity.entityBaseStats.DmgEleAllMin, "Entity DmgEleAllMin");
            
            PaintIntField(ref activeEntity.entityBaseStats.DmgFireMax, "Entity DmgFireMax");
            PaintIntField(ref activeEntity.entityBaseStats.DmgColdMax, "Entity DmgColdMax");
            PaintIntField(ref activeEntity.entityBaseStats.DmgLightMax, "Entity DmgLightMax");
            PaintIntField(ref activeEntity.entityBaseStats.DmgPoisonMax, "Entity DmgPoisonMax");
            PaintIntField(ref activeEntity.entityBaseStats.DmgEleAllMax, "Entity DmgEleAllMax");
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
