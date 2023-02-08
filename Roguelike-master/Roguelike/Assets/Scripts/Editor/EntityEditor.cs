using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntityEditor : EditorBase
{
    private const string LBL_NAME = "Name";
    private const string LBL_LEVEL = "Level";
    private const string LBL_SPEED = "Speed";    
    private const string LBL_LIFE_MAX = "Maximum Life";
    private const string LBL_LIFE_CURRENT = "Current Life";
    private const string LBL_MANA_MAX = "Maximum Mana";
    private const string LBL_MANA_CURRENT = "Current Mana";    
    private const string LBL_RANGE_OF_AGGRESSION = "Range of Aggression";
    private const string LBL_EXPERIENCE = "Experience Reward" ;    
    private const string LBL_ITEM_FIND = "Item Find";
    private const string LBL_MAGIC_FIND = "Magic Find";    
    private const string LBL_RES_FIRE = "Fire Resistance";
    private const string LBL_RES_COLD = "Cold Resistance";
    private const string LBL_RES_LIGHT = "Lightning Resistance" ;
    private const string LBL_RES_POISON = "Poison Resistance" ;
    private const string LBL_RES_ELEMENTAL_ALL = "All Elemental Resistance";    
    private const string LBL_ATTACK_RATING = "Attack Rating" ;
    private const string LBL_CHANCE_TO_BLOCK = "Chance to Block" ;
    private const string LBL_DEFENSE = "Defense" ;    
    private const string LBL_DMG_PHYSICAL_MIN = "Minimum Physical Damage" ;
    private const string LBL_DMG_FIRE_MIN = "Minimum Fire Damage" ;
    private const string LBL_DMG_COLD_MIN ="Minimum Cold Damage" ;
    private const string LBL_DMG_LIGHT_MIN = "Minimum Lightning Damage" ;
    private const string LBL_DMG_POISON_MIN = "Minimum Poison Damage" ;
    private const string LBL_DMG_ELEMENTAL_ALL_MIN ="Minimum Elemental Damage" ;
    private const string LBL_DMG_PHYSICAL_MAX = "Maximum Physical Damage" ;
    private const string LBL_DMG_FIRE_MAX =  "Maximum Fire Damage" ;
    private const string LBL_DMG_COLD_MAX = "Maximum Cold Damage" ;
    private const string LBL_DMG_LIGHT_MAX =  "Maximum Lightning Damage" ;
    private const string LBL_DMG_POISON_MAX =  "Maximum Poison Damage" ;
    private const string LBL_DMG_ELEMENTAL_ALL_MAX = "Maximum Elemental Damage" ;
    
    Vector2 scrollView;

    EntityReplacement activeEntity;
    TextAsset obj;
    UnityEngine.AnimatorOverrideController animatorOverrideController;
    string animatorOverrideControllerPath;

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
            
            PaintTextField(ref activeEntity.entityBaseStats.Name, LBL_NAME);
            PaintIntField(ref activeEntity.entityBaseStats.Level, LBL_LEVEL);
            PaintIntField(ref activeEntity.entityBaseStats.Speed, LBL_SPEED);
            
            PaintIntField(ref activeEntity.entityBaseStats.LifeMax, LBL_LIFE_MAX);
            PaintIntField(ref activeEntity.entityBaseStats.ManaMax, LBL_MANA_MAX);
            
            PaintIntField(ref activeEntity.entityBaseStats.RangeOfAggression, LBL_RANGE_OF_AGGRESSION);
            PaintIntField(ref activeEntity.entityBaseStats.Experience, LBL_EXPERIENCE);
            
            PaintIntField(ref activeEntity.entityBaseStats.ItemFind, LBL_ITEM_FIND);
            PaintIntField(ref activeEntity.entityBaseStats.MagicFind, LBL_MAGIC_FIND);
            
            PaintIntField(ref activeEntity.entityBaseStats.ResFire, LBL_RES_FIRE);
            PaintIntField(ref activeEntity.entityBaseStats.ResCold, LBL_RES_COLD);
            PaintIntField(ref activeEntity.entityBaseStats.ResLight, LBL_RES_LIGHT);
            PaintIntField(ref activeEntity.entityBaseStats.ResPoison, LBL_RES_POISON);
            PaintIntField(ref activeEntity.entityBaseStats.ResAll, LBL_RES_ELEMENTAL_ALL);
            
            PaintIntField(ref activeEntity.entityBaseStats.AttackRating, LBL_ATTACK_RATING);
            PaintIntField(ref activeEntity.entityBaseStats.ChanceToBlock, LBL_CHANCE_TO_BLOCK);
            PaintIntField(ref activeEntity.entityBaseStats.Defense, LBL_DEFENSE);
            
            PaintIntField(ref activeEntity.entityBaseStats.DmgPhyMin, LBL_DMG_PHYSICAL_MIN);            
            PaintIntField(ref activeEntity.entityBaseStats.DmgFireMin, LBL_DMG_FIRE_MIN);
            PaintIntField(ref activeEntity.entityBaseStats.DmgColdMin, LBL_DMG_COLD_MIN);
            PaintIntField(ref activeEntity.entityBaseStats.DmgLightMin, LBL_DMG_LIGHT_MIN);
            PaintIntField(ref activeEntity.entityBaseStats.DmgPoisonMin, LBL_DMG_POISON_MIN);
            PaintIntField(ref activeEntity.entityBaseStats.DmgEleAllMin, LBL_DMG_ELEMENTAL_MIN);
            
            PaintIntField(ref activeEntity.entityBaseStats.DmgPhyMax, LBL_DMG_PHYSICAL_MAX);
            PaintIntField(ref activeEntity.entityBaseStats.DmgFireMax, LBL_DMG_FIRE_MAX);
            PaintIntField(ref activeEntity.entityBaseStats.DmgColdMax, LBL_DMG_COLD_MAX);
            PaintIntField(ref activeEntity.entityBaseStats.DmgLightMax, LBL_DMG_LIGHT_MAX);
            PaintIntField(ref activeEntity.entityBaseStats.DmgPoisonMax, LBL_DMG_POISON_MAX);
            PaintIntField(ref activeEntity.entityBaseStats.DmgEleAllMax, LBL_DMG_ELEMENTAL_MAX);
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
        string filePath = AssetDatabase.GetAssetPath(animatorOverrideController).Substring("Assets/Resources/".Length);
        filePath = filePath.Substring(0, filePath.Length - 4);
        activeEntity.animatorOverrideControllerFileName = animatorOverrideController == null ? string.Empty : filePath;
        
        XMLUtility.Save<EntityReplacement>(activeEntity, "Entities/", activeEntity.Name);
    }
}
