using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Entity
{
    private void Start()
    {
        name = "Player Chacter";
    }

    public void Action()
    {
        Vector3Int coordinates = TileMapCursor.SelectedTileCoordinates;

        if ( collidingWith.Count > 0 )
        {
            collidingWith[0].Interact();
        }
        else
        {
            animator.SetTrigger( "Attack" );
        }
    }
}