using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Rect Rect { get { return new Rect( Position.x, Position.y, width, height ); } }
    public int Right { get { return left + width - 1; } }
    public int Bottom { get { return top + height - 1; } }
    public int Center_x { get { return left + width / 2; } }
    public int Center_y { get { return top + height / 2; } }
    public int Radius_x { get { return ( width - 1 ) / 2; } }
    public int Radius_y { get { return ( height - 1 ) / 2; } }
    public Vector3 CenterWorldSpace { get { return new Vector3( Center.x, Center.y, 0.0f ); } }
    public Vector2Int Center { get { return new Vector2Int( Center_x, Center_y ); } }
    public Vector3Int Position { get { return new Vector3Int( left, top, 0 ); } }
    public bool IsGhost { get { return chunk == null; } }

    public List<Room> GetPrototypes {
        get {
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
    public Room()
    {
        chunk = ChunkRepository.Town;

        this.left = BoardManager.Width / 2;
        this.top = BoardManager.Height / 2;
        width = chunk.Width;
        height = chunk.Height;

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
        top = parent.Center_y - radius_y;
        left = parent.Center_x - radius_x;

        //adjust center in the direction of offset
        left += offset.x * ( ( radius_x + parent.Radius_x ) + 1 );
        top += offset.y * ( ( radius_y + parent.Radius_y ) + 1 );
        Parent = parent;
    }

    /// <summary>
    /// Turn ghost room into child
    /// </summary>
    public Room( Room parent, AccessPoint accessPoint, bool t )
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
        top = parent.Center_y - radius_y;
        left = parent.Center_x - radius_x;

        //adjust center in the direction of offset
        left += offset.x * ( ( radius_x + parent.Radius_x ) + 1 );
        top += offset.y * ( ( radius_y + parent.Radius_y ) + 1 );

        Parent = parent;
    }

    public AccessPoint GetRandomAccessPoint() => chunk.Entrance[Random.Range( 0, chunk.Entrance.Count )];

    public bool CollidesWith( Room other ) => Rect.Overlaps( other.Rect );

    public void RemoveAccessPoint( AccessPoint.Dir direction )
    {
        chunk.Entrance.RemoveAll( x => x.Direction == direction );
        MapFactory.AvailableEntrances--;
    }

    public void Build()
    {
        foreach ( TileData data in chunk.Walls )
        {
            BoardManager.tileMapWalls.SetTile( Position + data.position, ChunkRepository.Tile[data.name] );
        }
        foreach ( TileData data in chunk.Curios )
        {
            switch ( data.name )
            {
                case "Dungeon_Tileset_91":
                case "Dungeon_Tileset_90":
                case "Dungeon_Tileset_93":
                case "Dungeon_Tileset_95":
                    BoardManager.Instantiate( Resources.Load<GameObject>( "Prefabs/Light - Wall Light" ), Position + data.position, Quaternion.identity );
                    BoardManager.tileMapCurios.SetTile( Position + data.position, ChunkRepository.Tile[data.name] );
                    break;
                //Hide curios that are triggers and build helpers
                case "Dungeon_Tileset_110":
                case "Dungeon_Tileset_111":
                case "Dungeon_Tileset_112":
                case "Dungeon_Tileset_113":
                    break;

                default:
                    BoardManager.tileMapCurios.SetTile( Position + data.position, ChunkRepository.Tile[data.name] );
                    break;
            }
        }
        foreach ( TileData data in chunk.Floors )
            BoardManager.tileMapGround.SetTile( Position + data.position, ChunkRepository.Tile[data.name] );

        MapFactory.AvailableEntrances += chunk.Entrance.Count;
        MapFactory.PlacedRooms++;
    }
}