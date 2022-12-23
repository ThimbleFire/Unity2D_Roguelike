using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Entity
{
    private void Start()
    {
        Name = "Player Chacter";
        Speed = 4;
    }

    public override void Action()
    {
        chain = Pathfind.GetPath( coordinates, TileMapCursor.SelectedTileCoordinates );
        TileMapCursor.Hide();
        HUDControls.Hide();
    }

    protected override void OnStep()
    {

    }

    protected override void OnArrival()
    {
        HUDControls.Show();
    }
}