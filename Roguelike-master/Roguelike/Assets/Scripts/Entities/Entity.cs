using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Animator ) )]
public class Entity : MonoBehaviour {

    public enum StatID {
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

    public string Name { get; set; }
    protected int Life_Current { get; set; }
    protected int Mana_Current { get; set; }
    protected int RangeOfAggression { get; set; }
    public int Level { get; set; }
    protected int DmgBasePhyMin { get; set; }
    protected int DmgBasePhyMax { get; set; }
    protected int StrengthBase { get; set; }
    protected int IntelligenceBase { get; set; }
    protected int ConstitutionBase { get; set; }
    protected int DexterityBase { get; set; }
    protected int SpeedBase { get; set; }

    protected int Speed => SpeedBase + stats[( StatID )GearStats.Implicit.Plus_Speed_Movement] + stats[( StatID )GearStats.Suffix.Plus_Speed_Movement];
    protected int Life_Max => Constitution * 5 + stats[( StatID )GearStats.Suffix.Plus_Life];
    protected int Mana_Max => Intelligence * 5 + stats[( StatID )GearStats.Prefix.Plus_Mana];
    protected int DmgPhysMin => DmgBasePhyMin + ( Strength / 100 ) * DmgBasePhyMin + stats[(StatID)GearStats.Suffix.Dmg_Phys_Min];
    protected int DmgPhysMax => DmgBasePhyMax + ( Strength / 100 ) * DmgBasePhyMax + stats[( StatID )GearStats.Suffix.Dmg_Phys_Max];
    protected int DmgAccuracy => stats[( StatID )GearStats.Prefix.Plus_Accuracy] + Dexterity * 5;
    protected int DmgEleFireMin => stats[( StatID )GearStats.Prefix.Dmg_Ele_Fire] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Fire];
    protected int DmgEleFireMax => stats[( StatID )GearStats.Prefix.Dmg_Ele_Fire] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Fire];
    protected int DmgEleColdMin => stats[( StatID )GearStats.Prefix.Dmg_Ele_Cold] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Cold];
    protected int DmgEleColdMax => stats[( StatID )GearStats.Prefix.Dmg_Ele_Cold] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Cold];
    protected int DmgEleLightningMin => stats[( StatID )GearStats.Prefix.Dmg_Ele_Lightning] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Lightning];
    protected int DmgEleLightningMax => stats[( StatID )GearStats.Prefix.Dmg_Ele_Lightning] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Lightning];
    protected int DmgElePoisonMin => stats[( StatID )GearStats.Prefix.Dmg_Ele_Poison] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Poison];
    protected int DmgElePoisonMax => stats[( StatID )GearStats.Prefix.Dmg_Ele_Poison] + stats[( StatID )GearStats.Suffix.Dmg_Ele_Poison];
    protected int Defense => Dexterity / 2 + stats[StatID.Def_Phys_Flat];
    protected int DefDmgReductionPhys => stats[( StatID )GearStats.Prefix.Def_Dmg_Reduction_Phys] + stats[( StatID )GearStats.Suffix.Def_Dmg_Reduction_All] + stats[( StatID )GearStats.Implicit.Def_Dmg_Reduction_All];
    protected int DefDmgReductionMagic => stats[( StatID )GearStats.Prefix.Def_Dmg_Reduction_Magic] + stats[( StatID )GearStats.Suffix.Def_Dmg_Reduction_All] + stats[( StatID )GearStats.Implicit.Def_Dmg_Reduction_All];
    protected int DefResFire => stats[( StatID )GearStats.Prefix.Def_Ele_Res_All] + stats[( StatID )GearStats.Prefix.Def_Ele_Res_Fire];
    protected int DefResCold => stats[( StatID )GearStats.Prefix.Def_Ele_Res_All] + stats[( StatID )GearStats.Prefix.Def_Ele_Res_Cold];
    protected int DefResLightning => stats[( StatID )GearStats.Prefix.Def_Ele_Res_All] + stats[( StatID )GearStats.Prefix.Def_Ele_Res_Lightning];
    protected int DefResPoison => stats[( StatID )GearStats.Prefix.Def_Ele_Res_All] + stats[( StatID )GearStats.Prefix.Def_Ele_Res_Poison];
    protected int OnHitLife => stats[( StatID )GearStats.Prefix.On_Hit_Life];
    protected int OnKillLife => stats[( StatID )GearStats.Prefix.On_Kill_Life];
    protected int OnHitMana => stats[( StatID )GearStats.Suffix.On_Hit_Mana];
    protected int OnKillMana => stats[( StatID )GearStats.Suffix.On_Kill_Mana];
    protected int RegenLife => stats[( StatID )GearStats.Implicit.Plus_Regen_Life] + stats[( StatID )GearStats.Suffix.Plus_Regen_Life] + Life_Max / 100;
    protected int RegenMana => stats[( StatID )GearStats.Implicit.Plus_Regen_Mana] + stats[( StatID )GearStats.Prefix.Plus_Regen_Mana] + Mana_Max / 100;
    public int Strength => stats[( StatID )GearStats.Suffix.Plus_Str] + StrengthBase;
    public int Dexterity => stats[( StatID )GearStats.Suffix.Plus_Dex] + DexterityBase;
    public int Constitution => stats[( StatID )GearStats.Suffix.Plus_Con] + ConstitutionBase;
    public int Intelligence => stats[( StatID )GearStats.Suffix.Plus_Int] + IntelligenceBase;
    protected int IncPhysSpeed => stats[( StatID )GearStats.Suffix.Plus_Speed_Phys] + stats[( StatID )GearStats.Implicit.Plus_Speed_Phys];
    protected int IncMagicSpeed => stats[( StatID )GearStats.Suffix.Plus_Speed_Magic] + stats[( StatID )GearStats.Implicit.Plus_Speed_Magic];
    protected int IncMoveSpeed => stats[( StatID )GearStats.Suffix.Plus_Speed_Movement] + stats[( StatID )GearStats.Implicit.Plus_Speed_Movement];
    protected int IncBlockRecovery => stats[( StatID )GearStats.Suffix.Plus_Block_Recovery] + stats[( StatID )GearStats.Implicit.Plus_Block_Recovery];
    protected int IncStaggerRecovery => stats[( StatID )GearStats.Suffix.Plus_Stagger_Recovery] + stats[( StatID )GearStats.Implicit.Plus_Stagger_Recovery];
    protected int IncMagicFind => stats[( StatID )GearStats.Suffix.Plus_Magic_Find] + stats[( StatID )GearStats.Implicit.Plus_Magic_Find] + stats[( StatID )GearStats.Prefix.Plus_Magic_Find];

    protected Dictionary<StatID, int> stats = new Dictionary<StatID, int>();
    protected SpriteRenderer spriteRenderer;

    public bool isAggressive {
        get {
            int disX = Mathf.Abs( Entities.GetPCS._coordinates.x - _coordinates.x );
            int disY = Mathf.Abs( Entities.GetPCS._coordinates.y - _coordinates.y );
            int distance = disX + disY;

            return distance <= RangeOfAggression;
        }
    }

    public AudioClip onAttack, onHit, onMove;

    protected List<Node> _chain = new List<Node>();

    public Vector3Int _coordinates;
    protected Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        for ( int i = 0; i < System.Enum.GetNames( typeof( StatID ) ).Length; i++ ) {
            stats.Add( ( StatID )i, 0 );
        }
    }

    public virtual void Attack() {
        _animator.SetTrigger( "Attack" );
        AudioDevice.Play( onAttack );
    }

    public virtual void Move() {
    }

    public virtual void Interact() {
    }

    public virtual void Action() {

        if(SpeedBase == 0)
        {
            Entities.Step(false);
            return;
        }

        Vector3Int playerCharacterCoordinates = Entities.GetPCS._coordinates;

        // some AI shit

        int disX = Mathf.Abs( playerCharacterCoordinates.x - _coordinates.x );
        int disY = Mathf.Abs( playerCharacterCoordinates.y - _coordinates.y );

        int distance = disX + disY;

        if ( distance == 1 ) {
            Attack();
            AttackSplash.Show( playerCharacterCoordinates, AttackSplash.Type.Pierce );
            Entities.Attack( playerCharacterCoordinates, Random.Range( DmgPhysMin, DmgPhysMax + 1 ), Name );
            return;
        }

        if ( isAggressive ) {
            _chain = Pathfind.GetPath( _coordinates, playerCharacterCoordinates, false );

            if ( _chain == null ) {
                Entities.Step( spriteRenderer.isVisible );
                return;
            }
            if ( _chain.Count == 0 ) {
                Entities.Step( spriteRenderer.isVisible );
                return;
            }
        }
        else {
            //There's a 1 in 10 chance idling NPCs will talk
            if ( UnityEngine.Random.Range( 0, 10 ) == 0 )
                SpeechBubble.Show( transform, SpeechBubble.Type.Talking );

            _chain = Pathfind.Wander( _coordinates );
        }

        if ( spriteRenderer.isVisible )
            AudioDevice.Play( onMove );
    }

    public virtual void RecieveDamage( int incomingDamage, string attacker ) {

        // reduce incoming damage by flat damage reduction
        incomingDamage -= DefDmgReductionPhys;

        float actualIncomingDamage = incomingDamage;

        // reduce incoming damage by armour rating. This code desparately needs refining.
        float percentReduction = ((float)Defense / 1000) * 70;
        float percentLeftOver = 100 - percentReduction;
        actualIncomingDamage *= percentLeftOver / 100;
        incomingDamage = (int)actualIncomingDamage;

        Entities.DrawFloatingText(incomingDamage, transform, Color.red);
        Life_Current -= incomingDamage;
        AudioDevice.Play( onHit );
        if ( Life_Current <= 0 ) {
            Die();
        }
    }

    protected virtual void Die() {
        TextLog.Print( string.Format("<color=#FF0000>{0}</color> is slain", Name ) );
        _animator.SetTrigger( "Die" );
        Pathfind.Unoccupy( _coordinates );
        Entities.Remove( this );
        TileMapCursor.Hide();
    }

    public void DestroyAfterDeathAnimation()
    {
        Destroy(gameObject);
    }

    public void AlertObservers( string message ) {
        if ( message.Equals( "AttackAnimationEnd" ) ) {
            Entities.Step( spriteRenderer.isVisible );
        }
    }

    protected void UpdateAnimator( Vector3Int dir ) {
        if ( dir != Vector3Int.zero ) {
            transform.localScale = -dir.x > 0 ? new Vector3( 1.0f, 1.0f ) : new Vector3( -1.0f, 1.0f );
            _animator.SetBool( "Moving", true );
        }
    }
}