using UnityEngine;

public class PlayerCharacter : Entity
{
    private void Start()
    {
        Name = "Player Chacter";
        Speed = 4;
    }

    public override void Action( Vector3Int playerCharacterCoordinates )
    {
        int disX = Mathf.Abs( TileMapCursor.SelectedTileCoordinates.x - _coordinates.x );
        int disY = Mathf.Abs( TileMapCursor.SelectedTileCoordinates.y - _coordinates.y );
        if ( disX + disY == 1 && Entities.Search( TileMapCursor.SelectedTileCoordinates ).Count > 0 )
        {
            Debug.Log( Name + " attack!" );
            Entities.Step();
        }
        else
        {
            chain = Pathfind.GetPath( _coordinates, TileMapCursor.SelectedTileCoordinates, false );
            TileMapCursor.Hide();
            HUDControls.Hide();
            base.Action( playerCharacterCoordinates );
        }
    }

    protected override void OnStep()
    {
        base.OnStep();
    }

    protected override void OnArrival()
    {
        base.OnArrival();
    }
}