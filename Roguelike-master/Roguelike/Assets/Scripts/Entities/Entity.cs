using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Animator ) )]
public class Entity : MonoBehaviour
{
    //Properties
    public string Name { get; protected set; }
    public int Speed { get; protected set; }
    public int RangeOfAggression { get; protected set; }
    public Vector3Int _coordinates;

    protected Animator _animator;

    //Pathfinding
    //protected virtual void SetPath( Vector3Int coordinates ) { }

    protected List<Action> _operations = new List<Action>();

    protected List<Node> chain = new List<Node>();
    private Vector3 _stepDestination;
    private int stepsTaken = 0;

    private void Awake() => _animator = GetComponent<Animator>();

    private void Update()
    {
        if ( chain.Count == 0 )
            return;

        StepFrame();
    }

    public void Teleport( Vector3Int coordinates )
    {
        this._coordinates = coordinates;
        gameObject.transform.SetPositionAndRotation( coordinates + Vector3.up * 0.75f + Vector3.right * 0.5f, Quaternion.identity );
    }

    public virtual void Action()
    {
        // override for performing actions. We need to figure out how we want this to work.
    }
    
    protected virtual void Move()
    {
    
    }
    
    protected virtual void Attack()
    {
        // attack options for player character and creatures
    }
    
    protected virtual void Interact()
    {
        // interact option for entities that want to open chests / equip armour / take torches off the wall
    }

    /// <summary>Updates the animator and next step destination from the chain.</summary>
    /// <param name="dir">The cardinal direction the entity is moving</param>
    /// <returns>The world position the entity is moving to</returns>
    private Vector2 UpdateAnimator( Vector3Int dir )
    {
        int moveAcrossBoardSpeed = 4;

        _stepDestination = chain[0].worldPosition + Vector3.up * 0.75f + Vector3.right * 0.5f;
        Vector3 positionAfterMoving = Vector3.MoveTowards( transform.position, _stepDestination, moveAcrossBoardSpeed * Time.deltaTime );

        if ( dir != Vector3Int.zero )
        {
            transform.localScale = -dir.x > 0 ? new Vector3( 1.0f, 1.0f ) : new Vector3( -1.0f, 1.0f );
            _animator.SetFloat( "LastMoveX", -dir.x );
            _animator.SetFloat( "LastMoveY", -dir.y );
            _animator.SetBool( "Moving", true );
        }

        return positionAfterMoving;
    }

    /// <summary>The unit moves at a certain framerate. Each tile cycles through the 3 or 4 frames of the animation. At the beginning of the tile move, we reset the animation and play a sound clip. We remove the tile from the chain and update the cell position.</summary>
    /// <returns>void</returns>
    private void StepFrame()
    {
        // if the entity is not on screen, instantly move the unit
        if(!GetComponent<Renderer2D>().isVisible){
            transform.position = chain[0].worldPosition + Vector3.up * 0.75f + Vector3.right * 0.5f;
            OnStep();
            return;
        }
    
        //calculate position after moving
        Vector2 positionAfterMoving = UpdateAnimator( Vector3Int.zero );
        transform.position = positionAfterMoving;
        //call it again just for the sake of accurate animation
        UpdateAnimator( _coordinates - chain[0].coordinate );

        float arrivalDistance = 0.0f;
        bool unitHasArrivedAtDestination = Vector2.Distance( transform.position, _stepDestination ) <= arrivalDistance;
        if ( unitHasArrivedAtDestination )
        {
            OnStep();
        }
    }

    protected virtual void OnStep()
    {
        // Unoccupy last coordinates
        Pathfind.Unoccupy( _coordinates );
        //set the new coordinate at our current position
        _coordinates = chain[0].coordinate;
        //let the pathfinder know this tile is now occupied
        Pathfind.Occupy( _coordinates );
        //remove the last chain since we're not where we used to be
        chain.RemoveAt( 0 );
        //If we've arrived at our destination
        if ( chain.Count <= 0 ) {
            OnArrival();
            return;
        }
        //Once it's moved its all the tiles it can, it calls on arrival.
        if(stepsTaken++ == Speed)
            OnArrival();
        
    }

    protected virtual void OnArrival()
    {
        chain.Clear();
        
        stepsTaken = 0;
        
        _animator.SetBool( "Moving", false );
        
        Entities.Step();
    }
}
