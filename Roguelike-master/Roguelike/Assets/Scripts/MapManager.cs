using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public int roomsToPlace;

    public Tilemap Curio;
    public Tilemap Walls;
    public Tilemap Ground;

    private List<Chunk> chunksInMemory = new List<Chunk>();
    private List<Chunk> placedChunks = new List<Chunk>();
    
    private Vector3Int brushPosition = Vector3Int.zero;

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

        for ( int i = 0; i < roomsToPlace; i++ )
        {
            BuildChunk( GetCompatibleChunk() );
        }
    }

    private Chunk GetCompatibleChunk( )
    {
        // If there is no parent, return a random chunk
        if(placedChunks.Count == 0)
            return chunksInMemory[Random.Range( 0, chunksInMemory.Count )];

        // Get a list rooms with entrances / exits
        List<Chunk> placedChunksWithEntrances = placedChunks.FindAll( x => x.Entrance.Count > 0 );
        
        // Select a random room from that list 
        Chunk parent = placedChunksWithEntrances[Random.Range( 0, placedChunksWithEntrances.Count )];

        // Select an exit from that room
        int exitIndex = Random.Range( 0, parent.Entrance.Count );
        AccessPoint.Dir parentOutputDir = parent.Entrance[exitIndex].direction;
        
        // Get the access points from parent 
        AccessPoint[] parentAP = GetAPFacing(parent, parentOutputDir);
        
        // Flip the exit direction
        AccessPoint.Dir childInputDir = Flip(parentAP.direction);
        
        // Find a room compatible for parent
        Chunk child = GetChildRoom(childInputDir);
        
        // Get the accessPoints from child
        AccessPoint[] childAP = GetAPFacing(child, childInputDir);
        
        // Calculate brush position so that parent and child opposite APs are adjacent, or something, idfk
        
        brushPosition = parent.Origin;
        
        switch(parentAP[0].direction)
        {
            case AccessPoint.Dir.Left:
            brushPosition += Vector3Int.Left * child.Width;
            break;
            case AccessPoint.Dir.Right:
            brushPosition += Vector3Int.Right * parent.Width);
            break;
            case AccessPoint.Dir.Up:
            brushPosition += Vector3Int.Up * parent.Height;
            break;
            case AccessPoint.Dir.Down:
            brushPosition += Vector3Int.Down * child.Height;
            break;
        }
        
        for(int i = 0; i < parentAP.Count; i++)
        {
            parent.Entrance.Remove(parentAP[i]);
            child.Entrance.Remove(childAP[i]);
        }

        // ensure changes made to parent and child are within scope

        return child;
        
        /*
        # # w w w # #       X = origin
        #           #       w = access point
        w           w       # = wall
        w           w
        w           w        
        #           #
        X # w w w # #
        */
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

    private Chunk GetChildRoom(AccessPoint.Dir direction)
    {
        foreach(Chunk chunk in chunksInMemory)
        {
            foreach(AccessPoint accessPoint in chunk.Entrance)
            {
                if(accessPoint.direction == childInputDir)
                {
                    return chunk;
                }
            }
        }    
    }
    
    private AccessPoint[] GetAPFacing(Chunk chunk, AccessPoint.Dir dir)
    {
        List<AccessPoint> accessPoints = new List<AccessPoint>();
    
        foreach(AccessPoint accessPoint in chunk.Entrance)
        {
            if(accessPoint.direction == dir)
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

        foreach ( TileData tile in curios )
            Curio.SetTile( tile.position + chunk.Origin, Resources.Load<TileBase>( "Dungeon Tileset/" + tile.name ) );

        placedChunks.Add( chunk );
    }
}
