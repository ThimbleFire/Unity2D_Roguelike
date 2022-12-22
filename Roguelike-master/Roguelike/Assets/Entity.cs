using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Entity : BaseMonoBehaviour
{
    public Vector3Int coordinates;

    protected string Name = "Undefined";
    protected float MovementSpeed = 0.06f;
    protected Animator animator;
    protected virtual void SetPath( Vector3Int coordinates ) { }
    protected List<Interactable> collidingWith = new List<Interactable>();

    private void Awake() => animator = GetComponent<Animator>();

    private void Update()
    {

    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.gameObject.tag != "Wall" )
            collidingWith.Add( collision.gameObject.GetComponent<Interactable>() );
    }

    private void OnTriggerExit2D( Collider2D collision )
    {
        if ( collision.gameObject.tag != "Wall" )
            collidingWith.Remove( collision.gameObject.GetComponent<Interactable>() );
    }

    protected void Teleport( Vector3Int coordinates )
    {
        this.coordinates = coordinates;
        gameObject.transform.SetPositionAndRotation( coordinates, Quaternion.identity );
    }
}