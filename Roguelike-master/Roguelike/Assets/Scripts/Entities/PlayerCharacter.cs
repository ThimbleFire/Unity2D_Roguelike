using UnityEngine;

public class PlayerCharacter : Entity
{
    private void Start()
    {
        Name = "Player Chacter";
        Speed = 4;
    }

    public override void Move()
    {    
        int disX = Mathf.Abs( TileMapCursor.SelectedTileCoordinates.x - _coordinates.x );
        int disY = Mathf.Abs( TileMapCursor.SelectedTileCoordinates.y - _coordinates.y );
        int distance = disX + disY;
        
        // If we're at the location then we don't need to move
        if ( distance <= 0 )
            return;
        
        chain = Pathfind.GetPath( _coordinates, TileMapCursor.SelectedTileCoordinates, false );
        TileMapCursor.Hide();
        HUDControls.Hide();
        base.Move();
    }

    public override void Attack()
    {
        // If there are no enemies, return
        if(Entities.Search( TileMapCursor.SelectedTileCoordinates ).Count <= 0)
            return;
    
        int disX = Mathf.Abs( TileMapCursor.SelectedTileCoordinates.x - _coordinates.x );
        int disY = Mathf.Abs( TileMapCursor.SelectedTileCoordinates.y - _coordinates.y );        
        int distance = disX + disY;
        
        //If we're not in melee range, return
        if ( distance != 1 )
            return;
            
        Debug.Log( Name + " attack!" );
        Entities.Step();
            
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
