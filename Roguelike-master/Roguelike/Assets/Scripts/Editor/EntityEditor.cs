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
    private const string LBL_RES_ALL = "All Elemental Resistance";    
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
        string filePath = AssetDatabase.GetAssetPath(animatorOverrideController).Substring("Assets/Resources/".Length);
        filePath = filePath.Substring(0, filePath.Length - 4);
        activeEntity.animatorOverrideControllerFileName = animatorOverrideController == null ? string.Empty : filePath;
        
        XMLUtility.Save<EntityReplacement>(activeEntity, "Entities/", activeEntity.Name);
    }
}
