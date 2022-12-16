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

        if ( momentum.x <= -0.01f || momentum.x >= 0.01f )
        {
            animator.SetFloat( "LastMoveX", momentum.x );
        }

        animator.SetBool( "Moving", momentum != Vector3.zero );
        animator.SetFloat( "MoveX", momentum.x );
        animator.SetFloat( "MoveY", momentum.y );

        if ( momentum == Vector3.zero )
            return;

#if PLATFORM_ANDROID
        Vector3 velocity = momentum * speed * Time.smoothDeltaTime * 1.5f;
#else
        Vector3 velocity = momentum * speed * Time.smoothDeltaTime;
#endif

        Vector2 newPosition = transform.position + velocity;
        float nextX = Mathf.Round( Game.PPU * newPosition.x );
        float nextY = Mathf.Round( Game.PPU * newPosition.y );

        transform.position = new Vector3( nextX / Game.PPU, nextY / Game.PPU, 0.0f );
    }
}