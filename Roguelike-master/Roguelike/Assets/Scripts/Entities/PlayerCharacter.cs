using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Entity
{
    private void Start()
    {
        name = "Player Chacter";
    }

    public override void Action()
    {
        chain = Pathfind.GetPath( coordinates, TileMapCursor.SelectedTileCoordinates );

        TileMapCursor.Hide();
    }
}