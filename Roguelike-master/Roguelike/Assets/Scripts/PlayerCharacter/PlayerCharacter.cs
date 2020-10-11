using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter Instance;

    public Animator animator;

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
        transform.position = new Vector3( x * 0.16f, y * 0.16f, 0.0f );
    }

    private void Update()
    {
        Vector3 momentum = Vector3.zero;

        if ( Input.GetKey(KeyCode.A) )
        {
            momentum += Vector3.left;
        }

        if ( Input.GetKey(KeyCode.D) )
        {
            momentum += Vector3.right;
        }

        if ( Input.GetKey(KeyCode.W) )
        {
            momentum += Vector3.up;
        }

        if ( Input.GetKey(KeyCode.S) )
        {
            momentum += Vector3.down;
        }

        transform.Translate( momentum * speed * Time.smoothDeltaTime );

        animator.SetBool( "Moving", momentum != Vector3.zero );
        animator.SetFloat( "MoveX", momentum.x );
        animator.SetFloat( "MoveY", momentum.y );
    }
}
