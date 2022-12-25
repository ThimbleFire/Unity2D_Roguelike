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
        
        int distance = disX + disY;
        
        // If we're in melee range and there's something there, perform a melee action
        if ( distance == 1 && Entities.Search( TileMapCursor.SelectedTileCoordinates ).Count > 0 )
        {
            Debug.Log( Name + " attack!" );
            Entities.Step();
        }
        else // Else consider it a move action
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
