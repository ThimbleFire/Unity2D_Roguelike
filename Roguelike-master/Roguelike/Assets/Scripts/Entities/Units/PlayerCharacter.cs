using UnityEngine;

public class PlayerCharacter : Navigator {

    public Animator _primary;

    private const int UNARMED_DMG_PHYS_MIN = 2;
    private const int UNARMED_DMG_PHYS_MAX = 3;

    private void Start() {
        Name = "Player Chacter";
        SpeedBase = 4;
        Level = 1;
        DmgBasePhyMin = 2;
        DmgBasePhyMax = 3;
        AttackRating = 5;
        StrengthBase = 25;
        ConstitutionBase = 25;
        DexterityBase = 20;
        IntelligenceBase = 15;
        Life_MaxBase = 55;
        Life_Current = Life_MaxBase;

        PlayerHealthBar.SetMaximumLife(Life_MaxBase);
        PlayerHealthBar.SetCurrentLife(Life_Current);
        Inventory.RefreshCharacterStats(this);

        Inventory.OnEquipmentChange += Inventory_OnEquipmentChange;

        Inventory.Pickup("Long Sword");
        Inventory.Pickup("Animal Skin");
        Inventory.Pickup("Rotton Twine");
        Inventory.Pickup("Shield of Debugging");
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
        if (itemStats.MinDamage > 0)
        {
            if (adding)
            {
                DmgBasePhyMin = itemStats.MinDamage;
            }
            else
            {
                DmgBasePhyMin = UNARMED_DMG_PHYS_MIN;
            }
        }
        if (itemStats.MaxDamage > 0)
        {
            if (adding)
            {
                DmgBasePhyMax = itemStats.MaxDamage;
            }
            else
            {
                DmgBasePhyMax = UNARMED_DMG_PHYS_MAX;
            }
        }
        if (itemStats.Defense > 0)
        {
            if (adding)
            {
                stats[StatID.Def_Phys_Flat] += itemStats.Defense;
            }
            else
            {
                stats[StatID.Def_Phys_Flat] -= itemStats.Defense;
            }
        }
        if (itemStats.Blockrate > 0)
        {
            if (adding)
            {
                ChanceToBlock += itemStats.Blockrate;
            }
            else
            {
                ChanceToBlock -= itemStats.Blockrate;
            }
        }

        foreach ( Item.Prefix item in itemStats.Prefixes )
        {
            if (adding)
            {
                stats[(StatID)item.type] += item.value;
            }
            else
            {
                stats[(StatID)item.type] -= item.value;
            }
        }
        foreach (Item.Suffix item in itemStats.Suffixes)
        {
            if (adding)
            {
                stats[(StatID)item.type] += item.value;
            }
            else
            {
                stats[(StatID)item.type] -= item.value;
            }
        }
        foreach (Item.Implicit item in itemStats.Implicits)
        {
            if (adding)
            {
                stats[(StatID)item.type] += item.value;
            }
            else
            {
                stats[(StatID)item.type] -= item.value;
            }
        }
        PlayerHealthBar.SetMaximumLife(Life_MaxBase);
        Inventory.RefreshCharacterStats(this);
    }

    public override void PreTurn()
    {
        //regen life
        if (Life_Current < Life_Max && RegenLife > 0)
        {
            Life_Current = Mathf.Clamp(Life_Current + RegenLife, 0, Life_MaxBase);
            Entities.DrawFloatingText(RegenLife.ToString(), transform, Color.green);
        }

        base.PreTurn();
    }
}
