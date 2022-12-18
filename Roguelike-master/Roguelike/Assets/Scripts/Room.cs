using System;
using UnityEngine;

public class Room
{
    public Vector2Int size
    {
        get { return new Vector2Int( width, height ); }
    }
    public int right
    {
        get { return left + width - 1; }
    }
    public int bottom
    {
        get { return top + height - 1; }
    }
    public int center_x
    {
        get { return left + width / 2; }
    }
    public int center_y
    {
        get { return top + height / 2; }
    }
    public int radius_x
    {
        get { return ( width - 1 ) / 2; }
    }
    public int radius_y
    {
        get { return ( height - 1 ) / 2; }
    }
    public Vector3 centerWorldSpace { get { return new Vector3( center.x, center.y, 0.0f ); } }
    public Vector2Int center
    {
        get { return new Vector2Int( center_x, center_y ); }
    }
    public Vector3Int position
    {
        get { return new Vector3Int( left, top, 0 ); }
    }
    public bool IsGhost { get { return chunk == null; } }

    public int left = 0;
    public int top = 0;
    public int width;
    public int height;
    public Chunk chunk = null;
    public Room Parent { get; set; }
    public AccessPoint parentAP;
    public AccessPoint.Dir inputDirection;
    
    /// <summary>
    /// Start room
    /// </summary>
    public Room( int left, int top )
    {
        chunk = ChunkRepository.Get( ChunkRepository.GetRandom() );

        this.left = left;
        this.top = top;
        width = chunk.Width;
        height = chunk.Height;

        PlayerCharacter.Instance.SetPosition( center );

        Build();
    }

    /// <summary>
    /// Ordinary child room
    /// </summary>
    public Room( Room parent, bool ghost = false )
    {
        int radius_x = 3;
        int radius_y = 3;

        parentAP = parent.GetRandomAccessPoint();
        Vector2Int offset = MapFactory.GetDirVector2Int( parentAP.Direction );
        inputDirection = AccessPoint.Flip( parentAP.Direction );

        if ( ghost == false )
        {
            // Filter 
            chunk = ChunkRepository.GetFiltered( inputDirection );

            radius_x = chunk.radius_x;
            radius_y = chunk.radius_y;
        }

        width = 1 + radius_x * 2;
        height = 1 + radius_y * 2;

        //set center to parent center
        top = parent.center_y - radius_y;
        left = parent.center_x - radius_x;

        //adjust center in the direction of offset
        left += offset.x * ( ( radius_x + parent.radius_x ) + 1 );
        top += offset.y * ( ( radius_y + parent.radius_y ) + 1 );
        
        Parent = parent;
    }

    public AccessPoint GetRandomAccessPoint()
    {
        return chunk.Entrance[UnityEngine.Random.Range( 0, chunk.Entrance.Count )];
    }

    public void RemoveAccessPoint(AccessPoint.Dir direction)
    {
        chunk.Entrance.RemoveAll( x => x.Direction == direction );
        MapFactory.AvailableEntrances--;
    }

    public bool CollidesWith( Room other )
    {
        if ( left > other.right )
            return false;

        if ( top > other.bottom )
            return false;

        if ( right < other.left )
            return false;

        if ( bottom < other.top )
            return false;

        return true;
    }

    public void Build()
    {
        foreach ( TileData data in chunk.Walls )
            BoardManager.tileMapWalls.SetTile( position + data.position, ChunkRepository.Tile[data.name] );
        foreach ( TileData data in chunk.Curios )
            BoardManager.tileMapCurios.SetTile( position + data.position, ChunkRepository.Tile[data.name] );
        foreach ( TileData data in chunk.Floors )
            BoardManager.tileMapGround.SetTile( position + data.position, ChunkRepository.Tile[data.name] );

        MapFactory.AvailableEntrances += chunk.Entrance.Count;
        MapFactory.PlacedRooms++;
    }
}
