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

    public List<AccessPoint> unconnectedDoors = new List<AccessPoint>();
    private List<Chunk> chunksInMemory = new List<Chunk>();

    private Vector3Int brushPosition = Vector3Int.zero;
    private int roomsPlaced = 0;

    private void Awake()
    {
        // Load all chunks to memory

        string[] filenames = Directory.GetFiles( Application.streamingAssetsPath, "*.xml" );
        for ( int i = 0; i < filenames.Length; i++ )
        {
            filenames[i] = filenames[i].Remove( 0, Application.streamingAssetsPath.Length + 1 );
            Chunk r = XMLUtility.Load<Chunk>( filenames[i] );
            chunksInMemory.Add( r );
        }

        // Select the initial room at random

        int roomID = Random.Range( 0, filenames.Length );

        // Build the room

        BuildChunk( chunksInMemory[roomID] ); //bug here, we call delete room we're placing when there is not room.
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            int randomDoor = Random.Range( 0, unconnectedDoors.Count );
            AccessPoint d = unconnectedDoors[randomDoor];
            Chunk c = GetRoomThatConnectsTo( d.Direction );

            brushPosition += GetCardinal(d.Direction) * 7;

            foreach ( AccessPoint item in c.Entrance )
            {
                item.position += brushPosition;
            }

            BuildChunk( c );

            DeleteConnectedPoints( d, true);
            todelete.Clear();
        }
    }

    List<AccessPoint> todelete = new List<AccessPoint>();

    private void DeleteConnectedPoints( AccessPoint d, bool delete )
    {
        //problem must be here!!!!!!!!!!!!!!!

        foreach ( AccessPoint item in unconnectedDoors )
        {
            if ( item.Direction == d.Direction )
            {
                if ( (
                   d.position + Vector3Int.left == item.position ||
                   d.position + Vector3Int.right == item.position ||
                   d.position + Vector3Int.up == item.position ||
                   d.position + Vector3Int.down == item.position ) &&
                   todelete.Contains( item ) == false &&
                   d != item) {

                    todelete.Add( item );

                    DeleteConnectedPoints( item, false );
                }
            }
        }

        if ( delete )
        {
            foreach ( AccessPoint item in todelete )
            {
                Debug.Log( item );
                Curio.SetTile( item.position, null );
                unconnectedDoors.Remove( item );
            }
        }
    }

    private Chunk GetRoomThatConnectsTo(AccessPoint.Dir direction)
    {
        foreach ( Chunk chunk in chunksInMemory )
        {
            foreach ( AccessPoint point in chunk.Entrance )
            {
                if(point.Direction == GetOpposite(direction))
                {
                    DeleteConnectedPoints( point, true );

                    return chunk;
                }
            }
        }

        return null;
    }

    private AccessPoint.Dir GetOpposite(AccessPoint.Dir dir)
    {
        switch ( dir )
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

        return AccessPoint.Dir.UP;
    }

    private Vector3Int GetCardinal( AccessPoint.Dir dir )
    {
        switch ( dir )
        {
            case AccessPoint.Dir.RIGHT:
                return Vector3Int.right;
            case AccessPoint.Dir.LEFT:
                return Vector3Int.left;
            case AccessPoint.Dir.DOWN:
                return Vector3Int.down;
            case AccessPoint.Dir.UP:
                return Vector3Int.up;
            default:
                return Vector3Int.zero;
        }
    }

    private void BuildChunk(Chunk chunk)
    {
        Debug.Log( brushPosition );

        List<TileData> curios = chunk.Curios;
        List<TileData> walls = chunk.Walls;
        List<TileData> floors = chunk.Floors;

        foreach ( TileData tile in floors )
            Ground.SetTile( tile.position + brushPosition, Resources.Load<TileBase>( "Dungeon Tileset/" + tile.name ) );

        foreach ( TileData tile in walls )
            Walls.SetTile( tile.position + brushPosition, Resources.Load<TileBase>( "Dungeon Tileset/" + tile.name ) );

        foreach ( TileData tile in curios )
            Curio.SetTile( tile.position + brushPosition, Resources.Load<TileBase>( "Dungeon Tileset/" + tile.name ) );

        foreach ( AccessPoint item in chunk.Entrance )
            unconnectedDoors.Add( new AccessPoint { position = item.position + brushPosition, Direction = item.Direction } );
            
    }
}
