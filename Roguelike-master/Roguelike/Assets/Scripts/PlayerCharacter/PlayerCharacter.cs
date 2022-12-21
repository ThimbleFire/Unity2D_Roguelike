using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter Instance;
    public Animator animator;
    public List<Interactable> collidingWith = new List<Interactable>();

    private void Awake()
    {
        Instance = this;
    }

    public float speed = 0.06f;

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

    public void SetPosition( int x, int y )
    {
        transform.position = new Vector3( x, y, 0.0f );
    }
    public void SetPosition(Vector2Int position)
    {
        transform.position = (Vector2)position;
    }

    public void Action()
    {
        if ( collidingWith.Count > 0 )
        {
            collidingWith[0].Interact();
        }
        else
        {
            animator.SetTrigger( "Attack" );
        }
    }

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.F ) )
        {
            Action();
        }

    }
}