using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter Instance;

    public Animator animator;

    Vector2 mobile = Vector2.zero;

    public List<Interactable> collidingWith = new List<Interactable>();

    private void Awake()
    {
        Instance = this;
    }

    private string binaryData;

    public float speed = 0.06f;

    private void Start()
    {
        
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        if(collision.gameObject.tag != "Wall")
            collidingWith.Add( collision.gameObject.GetComponent<Interactable>() );
    }

    private void OnCollisionExit2D( Collision2D collision )
    {
        if ( collision.gameObject.tag != "Wall" )
            collidingWith.Remove( collision.gameObject.GetComponent<Interactable>() );
    }

    public void SetPosition( int x, int y )
    {
        transform.position = new Vector3( x, y, 0.0f );
    }

    public void MobileMove(Vector2 v)
    {
        mobile = v;
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

    private void FixedUpdate()
    {
        Vector3 momentum = mobile;

        if ( Input.GetKey( KeyCode.A ) )
        {
            momentum += Vector3.left;
        }

        if ( Input.GetKey( KeyCode.D ) )
        {
            momentum += Vector3.right;
        }

        if ( Input.GetKey( KeyCode.W ) )
        {
            momentum += Vector3.up;
        }

        if ( Input.GetKey( KeyCode.S ) )
        {
            momentum += Vector3.down;
        }

        if ( Input.GetKeyDown(KeyCode.F ) )
        {
            animator.SetTrigger( "Attack" );
        }

        if ( momentum.x <= -0.1f || momentum.x >= 0.1f)
        {
            animator.SetFloat( "LastMoveX", momentum.x );
        }

        Vector3 p = transform.position + ( momentum * speed * Time.fixedDeltaTime );

        transform.position = p;

        animator.SetBool( "Moving", momentum != Vector3.zero );
        animator.SetFloat( "MoveX", momentum.x );
        animator.SetFloat( "MoveY", momentum.y );
    }
}
