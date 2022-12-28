using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Animator ) )]
public class Entity : MonoBehaviour {
    protected SpriteRenderer spriteRenderer;

    //Properties
    public string Name { get; protected set; }

    public int LootLevel { get; protected set; }

    protected int Speed { get; set; }
    protected int RangeOfAggression { get; set; }
    protected int Attack_Damage { get; set; }
    protected int Health_Current { get; set; }
    protected int Health_Maximum { get; set; }
    public bool isAggressive
    { 
        get
        {
            int disX = Mathf.Abs( Entities.GetPCCoordinates.x - _coordinates.x );
            int disY = Mathf.Abs( Entities.GetPCCoordinates.y - _coordinates.y );
            int distance = disX + disY;

            return distance <= RangeOfAggression; 
        } 
    }

    public AudioClip onAttack, onHit, onMove;

    protected List<Node> _chain = new List<Node>();

    public Vector3Int _coordinates;
    protected Animator _animator;

    private void Awake()  { 
        _animator = GetComponent<Animator>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Attack() { 
        _animator.SetTrigger( "Attack" );
        AudioDevice.Play( onAttack );
    } 
    public virtual void Move() { }
    public virtual void Interact() { }
    public virtual void Action() {
        Vector3Int playerCharacterCoordinates = Entities.GetPCCoordinates;

        // some AI shit

        int disX = Mathf.Abs( playerCharacterCoordinates.x - _coordinates.x );
        int disY = Mathf.Abs( playerCharacterCoordinates.y - _coordinates.y );

        int distance = disX + disY;

        if ( distance == 1 ) {
            Attack();
            AttackSplash.Show( playerCharacterCoordinates, AttackSplash.Type.Pierce );
            Entities.Attack( playerCharacterCoordinates, Attack_Damage );
            return;
        }

        if ( isAggressive ) {

            _chain = Pathfind.GetPath( _coordinates, playerCharacterCoordinates, false );

            if ( _chain == null ) {
                Entities.Step();
                return;
            }
            if ( _chain.Count == 0 ) {
                Entities.Step();
                return;
            }
        }
        else
        {
            _chain = Pathfind.Wander( _coordinates );
        }

        if ( spriteRenderer.isVisible )
            AudioDevice.Play( onMove );
    }

    public virtual void DealDamage( int damage ) {
        Health_Current -= damage;
        AudioDevice.Play( onHit );
        if ( Health_Current <= 0 ) {
            Die();
        }
    }

    protected virtual void Die() {
        _animator.SetTrigger( "Die" );
        Pathfind.Unoccupy( _coordinates );
        Entities.Remove( this );
    }

    public void AlertObservers( string message ) {
        if ( message.Equals( "AttackAnimationEnd" ) ) {
            Entities.Step();
        }
    }

    protected void UpdateAnimator( Vector3Int dir ) {
        if ( dir != Vector3Int.zero ) {
            transform.localScale = -dir.x > 0 ? new Vector3( 1.0f, 1.0f ) : new Vector3( -1.0f, 1.0f );
            _animator.SetBool( "Moving", true );
        }
    }
}