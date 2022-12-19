using System;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Rect rect
    {
        get
        {
            return new Rect( position.x, position.y, width, height );
        }
    }
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
    public List<Room> GetPrototypes
    {
        get
        {
            List<Room> prototypes = new List<Room>();

            foreach ( AccessPoint accessPoint in chunk.Entrance )
            {
                if ( accessPoint.Direction == inputDirection )
                    continue;

                Room prototype = new Room( this, accessPoint );
                prototypes.Add( prototype );
            }

            return prototypes;
        }
    }

    public int left = 0;
    public int top = 0;
    public int width;
    public int height;
    public Chunk chunk = null;
    public Room Parent { get; set; }
    public AccessPoint parentOutput;
    public AccessPoint.Dir inputDirection;

    /// <summary>
    /// Start room
    /// </summary>
    public Room( )
    {
        chunk = ChunkRepository.Town;

        this.left = 0;
        this.top = 0;
        width = chunk.Width;
        height = chunk.Height;

        PlayerCharacter.Instance.SetPosition( center );

        Build();
    }

    /// <summary>
    /// Ghost room
    /// </summary>
    public Room( Room parent, AccessPoint accessPoint )
    {
        int radius_x = 3;
        int radius_y = 3;

            parentOutput = accessPoint;
            Vector2Int offset = MapFactory.GetDirVector2Int( parentOutput.Direction );
            inputDirection = AccessPoint.Flip( parentOutput.Direction );

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

    public Room(Room parent, AccessPoint accessPoint, bool t )
    {
        parentOutput = accessPoint;
        Vector2Int offset = MapFactory.GetDirVector2Int( parentOutput.Direction );
        inputDirection = AccessPoint.Flip( parentOutput.Direction );
        chunk = ChunkRepository.GetFiltered( inputDirection );

        int radius_x = chunk.radius_x;
        int radius_y = chunk.radius_y;

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

    public void RemoveAccessPoint( AccessPoint.Dir direction )
    {
        chunk.Entrance.RemoveAll( x => x.Direction == direction );
        MapFactory.AvailableEntrances--;
    }

    public bool CollidesWith( Room other )
    {
        return rect.Overlaps( other.rect );
    }

    public void Build()
    {
        foreach ( TileData data in chunk.Walls )
            BoardManager.tileMapWalls.SetTile( position + data.position, ChunkRepository.Tile[data.name] );
        foreach ( TileData data in chunk.Curios )
        {
            switch ( data.name )
            {
                //Hide curios that are triggers and build helpers
                case "Dungeon_Tileset_110":
                case "Dungeon_Tileset_111":
                case "Dungeon_Tileset_112":
                case "Dungeon_Tileset_113":
                    break;
                default:
                    BoardManager.tileMapCurios.SetTile( position + data.position, ChunkRepository.Tile[data.name] );
                    break;
            }
        }
        foreach ( TileData data in chunk.Floors )
            BoardManager.tileMapGround.SetTile( position + data.position, ChunkRepository.Tile[data.name] );

        MapFactory.AvailableEntrances += chunk.Entrance.Count;
        MapFactory.PlacedRooms++;
    }

    public void BuildGhost()
    {
        for ( int y = 0; y < height; y++ )
        {
            for ( int x = 0; x < width; x++ )
            {
                BoardManager.tileMapCurios.SetTile( new Vector3Int( left + x, top + y, 0 ), ChunkRepository.Tile["Dungeon_Tileset_78"] );
            }
        }
    }
}
