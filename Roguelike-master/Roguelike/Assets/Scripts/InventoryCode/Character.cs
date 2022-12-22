using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour, IEquipped
{
    public static byte Level = 1;

    public static int DmgAccuracy
    { get { return stats[( StatID )GearStats.Prefix.Plus_Accuracy] + Dexterity * 5; } }

    private static int dmgPhyMin, dmgPhyMax = 0;

    public static int DmgPhysMin
    { get { return dmgPhyMin + ( Strength / 100 ) * dmgPhyMin; } }

    public static int DmgPhysMax
    { get { return dmgPhyMax + ( Strength / 100 ) * dmgPhyMax; } }

    /*
        // NEW VALUE
    public static byte DmgCritRate = 0;

    */

    public static int DmgEleFireMin
    { get { return stats[( StatID )GearStats.Prefix.Dmg_Ele_Fire] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Fire]; } }

    public static int DmgEleFireMax
    { get { return stats[( StatID )GearStats.Prefix.Dmg_Ele_Fire] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Fire]; } }

    public static int DmgEleColdMin
    { get { return stats[( StatID )GearStats.Prefix.Dmg_Ele_Cold] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Cold]; } }

    public static int DmgEleColdMax
    { get { return stats[( StatID )GearStats.Prefix.Dmg_Ele_Cold] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Cold]; } }

    public static int DmgEleLightningMin
    { get { return stats[( StatID )GearStats.Prefix.Dmg_Ele_Lightning] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Lightning]; } }

    public static int DmgEleLightningMax
    { get { return stats[( StatID )GearStats.Prefix.Dmg_Ele_Lightning] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Lightning]; } }

    public static int DmgElePoisonMin
    { get { return stats[( StatID )GearStats.Prefix.Dmg_Ele_Poison] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Poison]; } }

    public static int DmgElePoisonMax
    { get { return stats[( StatID )GearStats.Prefix.Dmg_Ele_Poison] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Poison]; } }

    public static int Armour
    { get { return stats[StatID.Def_Phys_Flat]; } }

    public static int Defense
    { get { return Dexterity / 2 + Armour; } }

    public static int DefDmgReductionPhys
    { get { return stats[( StatID )GearStats.Prefix.Def_Dmg_Reduction_Phys] + stats[( StatID )GearStats.Suffix.Def_Dmg_Reduction_All] + stats[( StatID )GearStats.Implicit.Def_Dmg_Reduction_All]; } }

    public static int DefDmgReductionMagic
    { get { return stats[( StatID )GearStats.Prefix.Def_Dmg_Reduction_Magic] + stats[( StatID )GearStats.Suffix.Def_Dmg_Reduction_All] + stats[( StatID )GearStats.Implicit.Def_Dmg_Reduction_All]; } }

    public static int DefResFire
    { get { return stats[( StatID )GearStats.Prefix.Def_Ele_Res_All] + stats[( StatID )GearStats.Prefix.Def_Ele_Res_Fire]; } }

    public static int DefResCold
    { get { return stats[( StatID )GearStats.Prefix.Def_Ele_Res_All] + stats[( StatID )GearStats.Prefix.Def_Ele_Res_Cold]; } }

    public static int DefResLightning
    { get { return stats[( StatID )GearStats.Prefix.Def_Ele_Res_All] + stats[( StatID )GearStats.Prefix.Def_Ele_Res_Lightning]; } }

    public static int DefResPoison
    { get { return stats[( StatID )GearStats.Prefix.Def_Ele_Res_All] + stats[( StatID )GearStats.Prefix.Def_Ele_Res_Poison]; } }

    public static int OnHitLife
    { get { return stats[( StatID )GearStats.Prefix.On_Hit_Life]; } }

    public static int OnKillLife
    { get { return stats[( StatID )GearStats.Prefix.On_Kill_Life]; } }

    public static int OnHitMana
    { get { return stats[( StatID )GearStats.Suffix.On_Hit_Mana]; } }

    public static int OnKillMana
    { get { return stats[( StatID )GearStats.Suffix.On_Kill_Mana]; } }

    public static int RegenLife
    { get { return stats[( StatID )GearStats.Implicit.Plus_Regen_Life] + stats[( StatID )GearStats.Suffix.Plus_Regen_Life] + Life_Max / 100; } }

    public static int RegenMana
    { get { return stats[( StatID )GearStats.Implicit.Plus_Regen_Mana] + stats[( StatID )GearStats.Prefix.Plus_Regen_Mana] + Mana_Max / 100; } }

    public static int Strength
    { get { return stats[( StatID )GearStats.Suffix.Plus_Str]; } }

    public static int Dexterity
    { get { return stats[( StatID )GearStats.Suffix.Plus_Dex]; } }

    public static int Constitution
    { get { return stats[( StatID )GearStats.Suffix.Plus_Con]; } }

    public static int Intelligence
    { get { return stats[( StatID )GearStats.Suffix.Plus_Int]; } }

    public static int Life_Max
    { get { return Constitution * 5 + stats[( StatID )GearStats.Suffix.Plus_Life]; } }

    public static int Life_Current = 0;

    public static int Mana_Max
    { get { return Intelligence * 5 + stats[( StatID )GearStats.Prefix.Plus_Mana]; } }

    public static int Mana_Current = 0;

    public static int IncPhysSpeed
    { get { return stats[( StatID )GearStats.Suffix.Plus_Speed_Phys] + stats[( StatID )GearStats.Implicit.Plus_Speed_Phys]; } }

    public static int IncMagicSpeed
    { get { return stats[( StatID )GearStats.Suffix.Plus_Speed_Magic] + stats[( StatID )GearStats.Implicit.Plus_Speed_Magic]; } }

    public static int IncMoveSpeed
    { get { return stats[( StatID )GearStats.Suffix.Plus_Speed_Movement] + stats[( StatID )GearStats.Implicit.Plus_Speed_Movement]; } }

    public static int IncBlockRecovery
    { get { return stats[( StatID )GearStats.Suffix.Plus_Block_Recovery] + stats[( StatID )GearStats.Implicit.Plus_Block_Recovery]; } }

    public static int IncStaggerRecovery
    { get { return stats[( StatID )GearStats.Suffix.Plus_Stagger_Recovery] + stats[( StatID )GearStats.Implicit.Plus_Stagger_Recovery]; } }

    public static int IncMagicFind
    { get { return stats[( StatID )GearStats.Suffix.Plus_Magic_Find] + stats[( StatID )GearStats.Implicit.Plus_Magic_Find] + stats[( StatID )GearStats.Prefix.Plus_Magic_Find]; } }

    public enum StatID
    {
        Plus_Accuracy = 0,

        Dmg_Phys_Min = 1,
        Dmg_Phys_Max = 2,
        Dmg_Phys_Percent = 3,
        Dmg_Ele_Fire = 4,
        Dmg_Ele_Cold = 5,
        Dmg_Ele_Lightning = 6,
        Dmg_Ele_Poison = 7,

        Def_Phys_Flat = 8,
        Def_Phys_Percent = 9,
        Def_Dmg_Reduction_Phys = 10,
        Def_Dmg_Reduction_Magic = 11,
        Def_Dmg_Reduction_All = 12,
        Def_Ele_Res_Fire = 13,
        Def_Ele_Res_Cold = 14,
        Def_Ele_Res_Lightning = 15,
        Def_Ele_Res_Poison = 16,
        Def_Ele_Res_All = 17,

        On_Hit_Life = 18,
        On_Kill_Life = 19,
        On_Kill_Mana = 20,
        On_Hit_Mana = 21,

        Plus_Life = 22,
        Plus_Mana = 23,
        Plus_Regen_Life = 24,
        Plus_Regen_Mana = 25,
        Plus_Str = 26,
        Plus_Dex = 27,
        Plus_Con = 28,
        Plus_Int = 29,
        Plus_Speed_Phys = 30,
        Plus_Speed_Magic = 31,
        Plus_Speed_Movement = 32,
        Plus_Block_Recovery = 33,
        Plus_Stagger_Recovery = 34,
        Plus_Magic_Find = 35,
        Plus_Durability = 36
    }

    public static Dictionary<StatID, int> stats = new Dictionary<StatID, int>();

    private void Awake()
    {
        for ( int i = 0; i < Enum.GetNames( typeof( StatID ) ).Length; i++ )
        {
            stats.Add( ( StatID )i, 0 );
        }
    }

    public void GearEquipped( ItemStats _stats, bool added )
    {
        if ( _stats.type == ItemStats.Type.PRIMARY )
        {
            if ( added )
            {
                dmgPhyMin += _stats.MinDamage;
                dmgPhyMax += _stats.MaxDamage;
            }
            else
            {
                dmgPhyMin -= _stats.MinDamage;
                dmgPhyMax -= _stats.MaxDamage;
            }
        }
        else
        {
            if ( added )
                stats[StatID.Def_Phys_Flat] += _stats.Defense;
            else
                stats[StatID.Def_Phys_Flat] -= _stats.Defense;
        }

        foreach ( ItemStats.Prefix item in _stats.prefixes )
        {
            if ( added )
                stats[( StatID )item.type] += item.value;
            else
                stats[( StatID )item.type] -= item.value;
        }
        foreach ( ItemStats.Suffix item in _stats.suffixes )
        {
            if ( added )
                stats[( StatID )item.type] += item.value;
            else
                stats[( StatID )item.type] -= item.value;
        }
        foreach ( ItemStats.Implicit item in _stats.implicits )
        {
            if ( added )
                stats[( StatID )item.type] += item.value;
            else
                stats[( StatID )item.type] -= item.value;
        }

        CharacterWindow.Instance.UpdateDisplay();
    }
}

namespace UnityEngine.EventSystems
{
    public interface IEquipped : IEventSystemHandler
    {
        void GearEquipped( ItemStats stats, bool added );
    }
}