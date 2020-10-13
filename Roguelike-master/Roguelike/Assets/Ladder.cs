using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public static Ladder Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetPosition( int x, int y )
    {
        transform.position = new Vector3( x, y, 0.0f );
    }

    public void Interact()
    {
        // next level
    }
}
