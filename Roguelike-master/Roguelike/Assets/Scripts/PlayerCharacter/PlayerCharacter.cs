using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter Instance;
    public Animator animator;
    public Joystick joystick;
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
        //mobile = new Vector2( v.x < 0 ? -1.0f : v.x > 0.0f ?  1.0f : 0.0f,
        //                      v.y > 0 ?  1.0f : v.x < 0.0f ? -1.0f : 0.0f );

        //mobile = v;
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
        if ( Input.GetKeyDown(KeyCode.F ) )
        {
            Action();
        }

        Vector3 momentum = joystick.input.normalized;

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

        if ( momentum.x <= -0.01f || momentum.x >= 0.01f)
        {
            animator.SetFloat( "LastMoveX", momentum.x );
        }

        animator.SetBool( "Moving", momentum != Vector3.zero );
        animator.SetFloat( "MoveX", momentum.x );
        animator.SetFloat( "MoveY", momentum.y );

        if ( momentum == Vector3.zero )
            return;

        Vector2 newPosition = transform.position + ( momentum * speed * Time.smoothDeltaTime );
        float nextX = Mathf.Round( Game.PPU * newPosition.x );
        float nextY = Mathf.Round( Game.PPU * newPosition.y );

        transform.position = new Vector3( nextX / Game.PPU, nextY / Game.PPU, 0.0f );
    }
}
