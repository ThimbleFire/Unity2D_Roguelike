using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Animator ) )]
public class Entity : MonoBehaviour {

    public enum StatID {
        nothing = 0,
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
        Plus_Item_Find = 36,
        Plus_Attack_Rating = 37,
        Plus_Defence_Rating = 38,
        Plus_Blockrate = 39
    }
    public const int BlockRecoveryBase = 5;

    public int IncItemFind { get; set; }
    public int IncMagicFind => stats[(StatID)Item.Suffix.SType.Plus_Magic_Find] + stats[(StatID)Item.Implicit.IType.Plus_Magic_Find] + stats[(StatID)Item.Prefix.PType.Plus_Magic_Find];

    public string Name { get; set; }
    public int Level { get; set; }
    protected int SpeedBase { get; set; }
    protected int Speed => SpeedBase + (int)stats[(StatID)Item.Implicit.IType.Plus_Speed_Movement] + stats[(StatID)Item.Suffix.SType.Plus_Speed_Movement];
    protected int Life_Current { get; set; }
    protected int Life_MaxBase { get; set; }
    protected int Mana_Current { get; set; }
    protected int Mana_MaxBase { get; set; }
    protected int DmgBasePhyMin { get; set; }
    protected int DmgBasePhyMax { get; set; }
    protected float Defense { get; set; }
    public int DefenseRating { get; set; }
    public int AttackRating { get; set; }
    public int ChanceToBlock { get; set; }
    protected int RangeOfAggression { get; set; }

    protected int StrengthBase { get; set; }
    protected int DexterityBase { get; set; }
    protected int ConstitutionBase { get; set; }
    protected int IntelligenceBase { get; set; }
    public int Strength => stats[(StatID)Item.Suffix.SType.Plus_Str] + StrengthBase;
    public int Dexterity => stats[(StatID)Item.Suffix.SType.Plus_Dex] + DexterityBase;
    public int Constitution => stats[(StatID)Item.Suffix.SType.Plus_Con] + ConstitutionBase;
    public int Intelligence => stats[(StatID)Item.Suffix.SType.Plus_Int] + IntelligenceBase;

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

    public AudioClip onAttack, onHit, onMove, miss, block;

    protected List<Node> _chain = new List<Node>();

    public Vector3Int _coordinates;
    protected Animator _animator;
    protected int BlockRecoveryTurnsRemaining = 0;

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

    /// <summary>
    /// This method is used by the AI. Player actions are separate in PlayerCharacter.cs
    /// </summary>
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
            Entities.Attack( playerCharacterCoordinates, Random.Range( DmgBasePhyMin, DmgBasePhyMax + 1 ), AttackRating, Level );
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
            //if ( UnityEngine.Random.Range( 0, 10 ) == 0 )
            //    SpeechBubble.Show( transform, SpeechBubble.Type.Talking );

            _chain = Pathfind.Wander( _coordinates );
        }

        if ( spriteRenderer.isVisible )
            AudioDevice.Play( onMove );
    }

    public virtual void RecieveDamage( int incomingDamage, float attackerCombatRating, float attackerLevel ) {

        //Roll dodge
        float CRvDR = attackerCombatRating / (attackerCombatRating + DefenseRating);
        float ALvDL = attackerLevel / (attackerLevel + Level);
        float chanceToHit = 200 * CRvDR * ALvDL;
        float value = Random.Range(0.0f, 100.0f);
        if (chanceToHit < value) {
            Entities.DrawFloatingText("Miss", transform, Color.gray);
            return;
        }

        //Roll block
        if (BlockRecoveryTurnsRemaining == 0) {
            chanceToHit = 15.0f + Mathf.Ceil(150.0f * attackerLevel / ChanceToBlock);
            value = Random.Range(0.0f, 100.0f);
            if (chanceToHit < value) {
                Entities.DrawFloatingText("Blocked", transform, Color.gray);
                BlockRecoveryTurnsRemaining = BlockRecoveryBase;
                return;
            }
        }

        // reduce incoming damage by armour rating. This code desparately needs refining.
        float actualIncomingDamage = incomingDamage;
        float percentReduction = Defense / 1000 * 70;
        float percentLeftOver = 100 - percentReduction;
        actualIncomingDamage *= percentLeftOver / 100;
        
        Entities.DrawFloatingText(((int)actualIncomingDamage).ToString(), transform, Color.red);
        Life_Current -= incomingDamage;
        AudioDevice.Play( onHit );

        if ( Life_Current <= 0 ) {
            Die();
        }
    }

    public virtual void PreTurn()
    {
        //recover from blocking
        if (BlockRecoveryTurnsRemaining > 0) 
            BlockRecoveryTurnsRemaining--;
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