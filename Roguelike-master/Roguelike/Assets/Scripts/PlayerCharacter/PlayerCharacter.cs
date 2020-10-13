using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter Instance;

    public Animator animator;

    Vector2 mobile = Vector2.zero;

    private void Awake()
    {
        Instance = this;
    }

    private string binaryData;

    public float speed = 0.06f;

    private void Start()
    {
        
    }

    public void SetPosition( int x, int y )
    {
        transform.position = new Vector3( x, y, 0.0f );
    }

    public void MobileMove(Vector2 v)
    {
        mobile = v;
    }

    public void MobileAttack()
    {
        animator.SetTrigger( "Attack" );
    }

    private void FixedUpdate()
    {
        Vector3 momentum = Vector3.zero;

        if ( Input.GetKey( KeyCode.A ) || mobile.x < -0.25f )
        {
            momentum += Vector3.left;

            animator.SetFloat( "LastMoveX", momentum.x );
        }

        if ( Input.GetKey( KeyCode.D ) || mobile.x > 0.25f )
        {
            momentum += Vector3.right;

            animator.SetFloat( "LastMoveX", momentum.x );
        }

        if ( Input.GetKey( KeyCode.W ) || mobile.y > 0.25f )
        {
            momentum += Vector3.up;
        }

        if ( Input.GetKey( KeyCode.S ) || mobile.y < -0.25f )
        {
            momentum += Vector3.down;
        }

        if ( Input.GetKeyDown(KeyCode.F ) )
        {
            animator.SetTrigger( "Attack" );
        }

        Vector3 p = transform.position + ( momentum * speed * Time.fixedDeltaTime );

        transform.position = p;

        animator.SetBool( "Moving", momentum != Vector3.zero );
        animator.SetFloat( "MoveX", momentum.x );
        animator.SetFloat( "MoveY", momentum.y );
    }
}
