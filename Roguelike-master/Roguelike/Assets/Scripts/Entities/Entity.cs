using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Entity : MonoBehaviour
{
    //Properties
    public string Name { get; protected set; }
    public int Speed { get; protected set; }
    public int RangeOfAggression { get; protected set; }
    public Vector3Int coordinates;

    protected Animator animator;

    //Pathfinding
    protected virtual void SetPath( Vector3Int coordinates ) { }
    protected List<Node> chain = new List<Node>();
    private Vector3 StepDestination;

    private void Awake() => animator = GetComponent<Animator>();

    private bool active = false;

    private void Update()
    {
        if ( chain.Count == 0 )
            return;

            StepFrame();
    }

    public void Teleport( Vector3Int coordinates )
    {
        this.coordinates = coordinates;
        gameObject.transform.SetPositionAndRotation( coordinates + Vector3.up * 0.75f + Vector3.right * 0.5f, Quaternion.identity );
    }

    public virtual void Action(Vector3Int playerCharacterCoordinates)
    {

    }

    /// <summary>Updates the animator and next step destination from the chain.</summary>
    /// <param name="dir">The cardinal direction the entity is moving</param>
    /// <returns>The world position the entity is moving to</returns>
    private Vector2 UpdateAnimator( Vector3Int dir )
    {
        int moveAcrossBoardSpeed = 4;

        StepDestination = chain[0].worldPosition + Vector3.up * 0.75f + Vector3.right * 0.5f;
        Vector3 positionAfterMoving = Vector3.MoveTowards( transform.position, StepDestination, moveAcrossBoardSpeed * Time.deltaTime );

        if ( dir != Vector3Int.zero )
        {
            transform.localScale = -dir.x > 0 ? new Vector3( 1.0f, 1.0f ) : new Vector3( -1.0f, 1.0f );
            animator.SetFloat( "LastMoveX", -dir.x );
            animator.SetFloat( "LastMoveY", -dir.y );
            animator.SetBool( "Moving", true );
        }

        return positionAfterMoving;
    }

    /// <summary>The unit moves at a certain framerate. Each tile cycles through the 3 or 4 frames of the animation. At the beginning of the tile move, we reset the animation and play a sound clip. We remove the tile from the chain and update the cell position.</summary>
    /// <returns>void</returns>
    private void StepFrame()
    {

        //calculate position after moving
        Vector2 positionAfterMoving = UpdateAnimator( Vector3Int.zero );

        transform.position = positionAfterMoving;

        //call it again just for the sake of accurate animation
        UpdateAnimator( coordinates - chain[0].coordinate );

        float arrivalDistance = 0.0f;
        bool unitHasArrivedAtDestination = Vector2.Distance( transform.position, StepDestination ) <= arrivalDistance;
        if ( unitHasArrivedAtDestination )
        {
            OnStep();
        }
    }

    protected virtual void OnStep()
    {
        // Unoccupy last coordinates
        Pathfind.Unoccupy( coordinates );
        //set the new coordinate at our current position
        coordinates = chain[0].coordinate;
        //let the pathfinder know this tile is now occupied
        Pathfind.Occupy( coordinates );
        Debug.Log( "Arrived at " + coordinates );
        //remove the last chain since we're not where we used to be
        chain.RemoveAt( 0 );

        //If we've arrived at our destination
        if ( chain.Count <= 0 )
        {
            animator.SetBool( "Moving", false );

            OnArrival();
        }
    }

    protected virtual void OnArrival()
    {
        Entities.Step();
    }
}