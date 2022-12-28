using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Animator ) )]
public class Entity : MonoBehaviour {

    //Properties
    public string Name { get; protected set; }

    public int LootLevel { get; protected set; }

    protected int Speed { get; set; }
    protected int RangeOfAggression { get; set; }
    protected int Attack_Damage { get; set; }
    protected int Health_Current { get; set; }
    protected int Health_Maximum { get; set; }

    protected List<Node> _chain = new List<Node>();

    public Vector3Int _coordinates;
    protected Animator _animator;

    private void Awake() => _animator = GetComponent<Animator>();

    public virtual void Attack() => _animator.SetTrigger( "Attack" );
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

        if ( distance <= RangeOfAggression ) {
            
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
        else _chain = Pathfind.Wander( _coordinates );
        
    }

    public virtual void DealDamage( int damage ) {
        Health_Current -= damage;
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