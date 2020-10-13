using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJoystick : Joystick
{
    public PlayerCharacter pc;

    private void Awake()
    {
        pc = GameObject.Find( "PlayerCharacter" ).GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        pc.MobileMove( input );
    }
}