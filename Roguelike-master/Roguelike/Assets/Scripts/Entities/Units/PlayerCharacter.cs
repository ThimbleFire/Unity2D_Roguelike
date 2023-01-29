using UnityEngine;

public class PlayerCharacter : Navigator {

    public Animator _primary;

    private void Start() {
        Name = "Player Chacter";
        SpeedBase = 4;
        Level = 1;
        DmgBasePhyMin = 2;
        DmgBasePhyMax = 5;
        StrengthBase = 5;
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
        Entities.Attack( TileMapCursor.SelectedTileCoordinates, Random.Range( DmgPhysMin, DmgPhysMax ), IncAttackRating, Level );

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
                Blockrate += itemStats.Blockrate;
            }
            else
            {
                stats[StatID.Def_Phys_Flat] -= itemStats.Defense;
                Blockrate -= itemStats.Blockrate;
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
}