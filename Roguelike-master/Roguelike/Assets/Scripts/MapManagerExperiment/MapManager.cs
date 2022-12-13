using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : BaseMonoBehaviour
{
    //public int roomsToPlace;

    public Tilemap Curio;
    public Tilemap Walls;
    public Tilemap Ground;

    private List<Chunk> chunksInMemory = new List<Chunk>();

    [SerializeField]
    private List<Chunk> placedChunks = new List<Chunk>();

    private Vector3Int brushPosition = Vector3Int.zero;

    public int entrances = 0;

    private System.Random random;

    private void Start()
    {
        // Load all chunks to memory

        string[] filenames = Directory.GetFiles( Application.streamingAssetsPath, "*.xml" );
        for ( int i = 0; i < filenames.Length; i++ )
        {
            filenames[i] = filenames[i].Remove( 0, Application.streamingAssetsPath.Length + 1 );
            Chunk r = XMLUtility.Load<Chunk>( filenames[i] );
            chunksInMemory.Add( r );
        }

        for ( int i = 0; i < maxRooms; i++ )
        {
            random = new System.Random( System.Guid.NewGuid().GetHashCode() );
            BuildChunk( GetCompatibleChunk() );
        }
    }

    private Chunk GetCompatibleChunk( )
    {
        // If there is no parent, return a random chunk
        if ( placedChunks.Count == 0 )
        {
            Chunk root = chunksInMemory[random.Next( 0, chunksInMemory.Count )].Clone();
            entrances = root.Entrance.Count / 3;
            return root;
        }

        //// Find a parent room with an access point
        Chunk parent = GetParentRoom();
         
        //// Select an exit from that chunk
        int exitIndex = random.Next( 0, parent.Entrance.Count );
        AccessPoint.Dir parentOutputDir = parent.Entrance[exitIndex].Direction;

        //// Get the access points from parent 
        AccessPoint[] parentAP = GetAPFacing(parent, parentOutputDir);

        //// Flip the exit direction
        AccessPoint.Dir childInputDir = Flip(parentAP[0].Direction);

        //// Find a room compatible for parent
        Chunk child = GetChildRoom(childInputDir);

        entrances += child.Entrance.Count / 3;

        //// Get the accessPoints from child
        AccessPoint[] childAP = GetAPFacing(child, childInputDir);

        //// Calculate brush position so that parent and child opposite APs are adjacent, or something, idfk

        brushPosition = parent.Origin;

        switch ( parentAP[0].Direction )
        {
            case AccessPoint.Dir.LEFT:
                brushPosition += Vector3Int.left * child.Width;
                break;
            case AccessPoint.Dir.RIGHT:
                brushPosition += Vector3Int.right * parent.Width;
                break;
            case AccessPoint.Dir.UP:
                brushPosition += Vector3Int.up * parent.Height;
                break;
            case AccessPoint.Dir.DOWN:
                brushPosition += Vector3Int.down * child.Height;
                break;
        }

        for ( int i = 0; i < parentAP.Length; i++ )
        {
            parent.Entrance.Remove( parentAP[i] );
            child.Entrance.Remove( childAP[i] );
        }
        
        entrances -= 2;

        return child;
    }

    private AccessPoint.Dir Flip(AccessPoint.Dir direction)
    {
        switch ( direction )
        {
            case AccessPoint.Dir.RIGHT:
                return AccessPoint.Dir.LEFT;
            case AccessPoint.Dir.LEFT:
                return AccessPoint.Dir.RIGHT;
            case AccessPoint.Dir.DOWN:
                return AccessPoint.Dir.UP;
            case AccessPoint.Dir.UP:
                return AccessPoint.Dir.DOWN;
        }

        return AccessPoint.Dir.DOWN;
    }

    private const int maxRooms = 32;

    private Chunk GetChildRoom( AccessPoint.Dir direction )
    {
        //List<Chunk> restrictMiminum = new List<Chunk>(
        //    chunksInMemory.FindAll( x => potential > 1 ? x.Entrance.Count / 3 > 1 : x.Entrance.Count / 3 == 1 ) );

        // Filter chunks, not exceeding maximum

        List<Chunk> chunksByExits = new List<Chunk>(
            chunksInMemory.FindAll( x => x.Entrance.Count / 3 + 1 + entrances <= maxRooms )
            );

        // Filter chunks by direction of those exits

        List<Chunk> chunksByDirection = new List<Chunk>();

        foreach ( Chunk chunk in chunksByExits )
        {
            foreach ( AccessPoint accessPoint in chunk.Entrance )
            {
                if(accessPoint.Direction == direction)
                {
                    chunksByDirection.Add( chunk );
                    break;
                }
            }
        }

        if ( chunksByDirection.Count == 0 ) 
             return new Chunk();
        else return chunksByDirection[random.Next( 0, chunksByDirection.Count )].Clone();
    }
    
    private Chunk GetParentRoom()
    {
        //// Get a list of placed chunks with entrances and exits
        List<Chunk> placedChunksWithEntrances = placedChunks.FindAll( x => x.Entrance.Count > 0 );

        //// Select a random chunk from that list 
        return placedChunksWithEntrances[random.Next( 0, placedChunksWithEntrances.Count )];
    }

    private AccessPoint[] GetAPFacing(Chunk chunk, AccessPoint.Dir dir)
    {
        List<AccessPoint> accessPoints = new List<AccessPoint>();
    
        foreach(AccessPoint accessPoint in chunk.Entrance)
        {
            if(accessPoint.Direction == dir)
            {
                accessPoints.Add(accessPoint);
            }
        }
        
        return accessPoints.ToArray();
    }

    private void BuildChunk(Chunk chunk)
    {
        // If we store the chunks origin we can locate its doors by calling Origin + Entrance[index].position
        chunk.Origin = brushPosition;
    
        List<TileData> curios = chunk.Curios;
        List<TileData> walls = chunk.Walls;
        List<TileData> floors = chunk.Floors;

        foreach ( TileData tile in floors )
            Ground.SetTile( tile.position + chunk.Origin, Resources.Load<TileBase>( "Dungeon Tileset/" + tile.name ) );

        foreach ( TileData tile in walls )
            Walls.SetTile( tile.position + chunk.Origin, Resources.Load<TileBase>( "Dungeon Tileset/" + tile.name ) );

        //foreach ( TileData tile in curios )
        //    Curio.SetTile( tile.position + chunk.Origin, Resources.Load<TileBase>( "Dungeon Tileset/" + tile.name ) );

        placedChunks.Add( chunk );
        entrances++;
    }
}
