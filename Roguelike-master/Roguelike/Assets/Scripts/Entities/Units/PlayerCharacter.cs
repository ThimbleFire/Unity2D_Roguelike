using UnityEngine;

public class PlayerCharacter : Navigator {

    public Animator _primary;


    protected int Life_Max => Life_MaxBase + Constitution * 5 + stats[(StatID)GearStats.Suffix.Plus_Life];
    protected int Mana_Max => Mana_MaxBase + Intelligence * 5 + stats[(StatID)GearStats.Prefix.Plus_Mana];
    protected int DmgPhysMin => DmgBasePhyMin + Strength / 10 * DmgBasePhyMin + stats[(StatID)GearStats.Suffix.Dmg_Phys_Min];
    protected int DmgPhysMax => DmgBasePhyMax + Strength / 10 * DmgBasePhyMax + stats[(StatID)GearStats.Suffix.Dmg_Phys_Max];
    protected int DmgEleFireMin => stats[(StatID)GearStats.Prefix.Dmg_Ele_Fire] + stats[(StatID)GearStats.Suffix.Dmg_Ele_Fire];
    protected int DmgEleFireMax => stats[(StatID)GearStats.Prefix.Dmg_Ele_Fire] + stats[(StatID)GearStats.Suffix.Dmg_Ele_Fire];
    protected int DmgEleColdMin => stats[(StatID)GearStats.Prefix.Dmg_Ele_Cold] + stats[(StatID)GearStats.Suffix.Dmg_Ele_Cold];
    protected int DmgEleColdMax => stats[(StatID)GearStats.Prefix.Dmg_Ele_Cold] + stats[(StatID)GearStats.Suffix.Dmg_Ele_Cold];
    protected int DmgEleLightningMin => stats[(StatID)GearStats.Prefix.Dmg_Ele_Lightning] + stats[(StatID)GearStats.Suffix.Dmg_Ele_Lightning];
    protected int DmgEleLightningMax => stats[(StatID)GearStats.Prefix.Dmg_Ele_Lightning] + stats[(StatID)GearStats.Suffix.Dmg_Ele_Lightning];
    protected int DmgElePoisonMin => stats[(StatID)GearStats.Prefix.Dmg_Ele_Poison] + stats[(StatID)GearStats.Suffix.Dmg_Ele_Poison];
    protected int DmgElePoisonMax => stats[(StatID)GearStats.Prefix.Dmg_Ele_Poison] + stats[(StatID)GearStats.Suffix.Dmg_Ele_Poison];
    protected int DefDmgReductionPhys => stats[(StatID)GearStats.Prefix.Def_Dmg_Reduction_Phys] + stats[(StatID)GearStats.Suffix.Def_Dmg_Reduction_All] + stats[(StatID)GearStats.Implicit.Def_Dmg_Reduction_All];
    protected int DefDmgReductionMagic => stats[(StatID)GearStats.Prefix.Def_Dmg_Reduction_Magic] + stats[(StatID)GearStats.Suffix.Def_Dmg_Reduction_All] + stats[(StatID)GearStats.Implicit.Def_Dmg_Reduction_All];
    protected int DefResFire => stats[(StatID)GearStats.Prefix.Def_Ele_Res_All] + stats[(StatID)GearStats.Prefix.Def_Ele_Res_Fire];
    protected int DefResCold => stats[(StatID)GearStats.Prefix.Def_Ele_Res_All] + stats[(StatID)GearStats.Prefix.Def_Ele_Res_Cold];
    protected int DefResLightning => stats[(StatID)GearStats.Prefix.Def_Ele_Res_All] + stats[(StatID)GearStats.Prefix.Def_Ele_Res_Lightning];
    protected int DefResPoison => stats[(StatID)GearStats.Prefix.Def_Ele_Res_All] + stats[(StatID)GearStats.Prefix.Def_Ele_Res_Poison];
    protected float Defense => Dexterity / 2 + base.Defense + stats[StatID.Def_Phys_Flat];
    protected int OnHitLife => stats[(StatID)GearStats.Prefix.On_Hit_Life];
    protected int OnKillLife => stats[(StatID)GearStats.Prefix.On_Kill_Life];
    protected int OnHitMana => stats[(StatID)GearStats.Suffix.On_Hit_Mana];
    protected int OnKillMana => stats[(StatID)GearStats.Suffix.On_Kill_Mana];
    protected int RegenLife => stats[(StatID)GearStats.Implicit.Plus_Regen_Life] + stats[(StatID)GearStats.Suffix.Plus_Regen_Life];
    protected int RegenMana => stats[(StatID)GearStats.Implicit.Plus_Regen_Mana] + stats[(StatID)GearStats.Prefix.Plus_Regen_Mana];
    protected int IncPhysSpeed => stats[(StatID)GearStats.Suffix.Plus_Speed_Phys] + stats[(StatID)GearStats.Implicit.Plus_Speed_Phys];
    protected int IncMagicSpeed => stats[(StatID)GearStats.Suffix.Plus_Speed_Magic] + stats[(StatID)GearStats.Implicit.Plus_Speed_Magic];
    protected int IncMoveSpeed => stats[(StatID)GearStats.Suffix.Plus_Speed_Movement] + stats[(StatID)GearStats.Implicit.Plus_Speed_Movement];
    protected int IncBlockRecovery => stats[(StatID)GearStats.Suffix.Plus_Block_Recovery] + stats[(StatID)GearStats.Implicit.Plus_Block_Recovery];
    protected int IncStaggerRecovery => stats[(StatID)GearStats.Suffix.Plus_Stagger_Recovery] + stats[(StatID)GearStats.Implicit.Plus_Stagger_Recovery];
    public float IncBlockRate => stats[(StatID)GearStats.Suffix.Plus_Blockrate] + stats[(StatID)GearStats.Implicit.Plus_Blockrate];
    public float IncAttackRating => Dexterity / 2 + stats[(StatID)GearStats.Prefix.Plus_Attack_Rating];
    public float IncDefenseRating => Dexterity / 4 + DefenseRating + stats[(StatID)GearStats.Suffix.Plus_Defence_Rating];

    private void Start() {
        Name = "Player Chacter";
        SpeedBase = 4;
        Level = 1;
        DmgBasePhyMin = 2;
        DmgBasePhyMax = 3;
        StrengthBase = 5;
        AttackRating = 5;
        IntelligenceBase = 5;
        ConstitutionBase = 5;
        DexterityBase = 5;
        Life_Current = Life_Max;

        Inventory.OnEquipmentChange += Inventory_OnEquipmentChange;
    }

    public override void Move() {
        int disX = Mathf.Abs( TileMapCursor.SelectedTileCoordinates.x - _coordinates.x );
        int disY = Mathf.Abs( TileMapCursor.SelectedTileCoordinates.y - _coordinates.y );
        int distance = disX + disY;

        // If we're at the location then we don't need to move
        if ( distance <= 0 )
            return;

        _chain = Pathfind.GetPath( _coordinates, TileMapCursor.SelectedTileCoordinates, false );

        if ( _chain == null )
            return;

        if ( _chain.Count == 0 )
            return;

        if(_primary != null)
            _primary.SetBool( "Moving", true );
        
        AudioDevice.Play( onMove );

        TileMapCursor.Hide();
        HUDControls.Hide();
        base.Move();
    }

    protected override void OnArrival()
    {
        if (_primary != null)
            _primary.SetBool( "Moving", false );

        base.OnArrival();
    }

    public override void Attack() {
        // If there are no enemies, return
        if ( Entities.Search( TileMapCursor.SelectedTileCoordinates ).Count <= 0 )
            return;

        int disX = Mathf.Abs( TileMapCursor.SelectedTileCoordinates.x - _coordinates.x );
        int disY = Mathf.Abs( TileMapCursor.SelectedTileCoordinates.y - _coordinates.y );
        int distance = disX + disY;

        //If we're not in melee range, return
        if ( distance != 1 )
            return;

        HUDControls.Hide();

        AttackSplash.Show( TileMapCursor.SelectedTileCoordinates, AttackSplash.Type.Slash );
        Entities.Attack( TileMapCursor.SelectedTileCoordinates, Random.Range( DmgPhysMin, DmgPhysMax ), AttackRating + IncAttackRating, Level );

        if (_primary != null)
            _primary.SetTrigger( "Attack" );

        base.Attack();
    }

    private void Inventory_OnEquipmentChange( ItemStats itemStats, bool adding )
    {
        if (itemStats.type == ItemStats.Type.PRIMARY)
        {
            if (adding)
            {
                DmgBasePhyMin += itemStats.MinDamage;
                DmgBasePhyMax += itemStats.MaxDamage;
            }
            else
            {
                DmgBasePhyMin -= itemStats.MinDamage;
                DmgBasePhyMax -= itemStats.MaxDamage;
            }
        }
        else
        {
            if (adding)
            {
                stats[StatID.Def_Phys_Flat] += itemStats.Defense;
                ChanceToBlock += itemStats.Blockrate;
            }
            else
            {
                stats[StatID.Def_Phys_Flat] -= itemStats.Defense;
                ChanceToBlock -= itemStats.Blockrate;
            }
        }
        foreach ( ItemStats.Prefix item in itemStats.prefixes )
        {
            if ( adding )
                stats[( StatID )item.type] += item.value;
            else
                stats[( StatID )item.type] -= item.value;
        }
        foreach ( ItemStats.Suffix item in itemStats.suffixes ) 
        {
            if ( adding )
                stats[( StatID )item.type] += item.value;
            else
                stats[( StatID )item.type] -= item.value;
        }
        foreach ( ItemStats.Implicit item in itemStats.implicits )
        {
            if ( adding )
                stats[( StatID )item.type] += item.value;
            else
                stats[( StatID )item.type] -= item.value;
        }
    }

    public override void PreTurn()
    {
        //regen life
        if (Life_Current < Life_Max && RegenLife > 0)
        {
            Life_Current = Mathf.Clamp(Life_Current + RegenLife, 0, Life_Max);
            Entities.DrawFloatingText(RegenLife.ToString(), transform, Color.green);
        }

        base.PreTurn();
    }

    public override void RecieveDamage(int incomingDamage, float attackerCombatRating, float attackerLevel)
    {
        //Roll dodge
        float CRvDR = attackerCombatRating / (attackerCombatRating + IncDefenseRating);
        float ALvDL = attackerLevel / (attackerLevel + Level);
        float chanceToHit = 200 * CRvDR * ALvDL;
        float value = Random.Range(0.0f, 100.0f);
        if (chanceToHit < value)
        {
            Entities.DrawFloatingText("Miss", transform, Color.gray);
            return;
        }

        //Roll block
        if (BlockRecoveryTurnsRemaining == 0)
        {
            chanceToHit = 15.0f + Mathf.Ceil(150.0f * attackerLevel / (ChanceToBlock + IncBlockRate));
            value = Random.Range(0.0f, 100.0f);
            if (chanceToHit < value)
            {
                Entities.DrawFloatingText("Blocked", transform, Color.gray);
                BlockRecoveryTurnsRemaining = BlockRecoveryBase - IncBlockRecovery;
                return;
            }
        }

        // reduce incoming damage by this entities flat damage reduction
        incomingDamage -= DefDmgReductionPhys;

        // reduce incoming damage by armour rating. This code desparately needs refining.
        float actualIncomingDamage = incomingDamage;
        float percentReduction = Defense / 1000 * 70;
        float percentLeftOver = 100 - percentReduction;
        actualIncomingDamage *= percentLeftOver / 100;
        incomingDamage = (int)actualIncomingDamage;

        Entities.DrawFloatingText(incomingDamage.ToString(), transform, Color.red);
        Life_Current -= incomingDamage;
        AudioDevice.Play(onHit);

        if (Life_Current <= 0)
        {
            Die();
        }
    }
}