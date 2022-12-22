using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkRepository : MonoBehaviour
{
    private static List<Chunk> chunksInMemory;
    public static Dictionary<string, TileBase> Tile;
    public static Chunk Town;

    private void Awake()
    {
        // load maps

        chunksInMemory = new List<Chunk>();
        Tile = new Dictionary<string, TileBase>();

        string[] filenames = Directory.GetFiles( Application.streamingAssetsPath, "*.xml" );
        for ( int i = 0; i < filenames.Length; i++ )
        {
            filenames[i] = filenames[i].Remove( 0, Application.streamingAssetsPath.Length + 1 );
            Chunk r = XMLUtility.Load<Chunk>( filenames[i] );

            if ( filenames[i] == "Town.xml" )
                Town = r;
            else
                chunksInMemory.Add( r );
        }

        // load tiles

        TileBase[] t = Resources.LoadAll<TileBase>( "Dungeon Tileset/" );

        foreach ( TileBase tile in t )
        {
            Tile.Add( tile.name, tile );
        }
    }

    public static string Get( int width, int height )
    {
        List<Chunk> chunksMatchingDimensions = new List<Chunk>( chunksInMemory.FindAll( x => x.Width == width && x.Height == height ) );

        if ( chunksMatchingDimensions.Count == 0 )
            return string.Empty;

        return chunksMatchingDimensions[Random.Range( 0, chunksMatchingDimensions.Count )].Name;
    }

    public static Chunk Get( string filename )
    {
        return chunksInMemory.Find( x => x.Name == filename ).Clone();
    }

    public static string GetRandom()
    {
        Chunk c = chunksInMemory[Random.Range( 0, chunksInMemory.Count )];

        //width = c.Width;
        //height = c.Height;

        return c.Name;
    }

    public static Chunk GetFiltered( AccessPoint.Dir direction )
    {
        // Filter chunks so we don't exceed the maximum number of rooms
        List<Chunk> chunksByExits = new List<Chunk>(
            chunksInMemory.FindAll( x => x.Entrance.Count + MapFactory.PlacedRooms + MapFactory.AvailableEntrances <= BoardManager.RoomLimit )
            );

        if ( chunksByExits.Count == 0 )
        {
            Debug.LogError( "ChunkRepository.GetFiltered. chunksByExits has zero count. Returning New Chunk, expect errors." );
            return new Chunk();
        }

        // Filter chunks by direction of exits

        List<Chunk> chunksByDirection = new List<Chunk>();

        foreach ( Chunk chunk in chunksByExits )
        {
            foreach ( AccessPoint accessPoint in chunk.Entrance )
            {
                if ( accessPoint.Direction == direction )
                {
                    chunksByDirection.Add( chunk );
                    break;
                }
            }
        }

        if ( chunksByDirection.Count == 0 )
        {
            Debug.LogError( "ChunkRepository.GetFiltered. chunksByDirection has zero count. Returning New Chunk, expect errors." );
            return new Chunk();
        }

        return chunksByDirection[Random.Range( 0, chunksByDirection.Count )].Clone();
    }

    public static Chunk GetRandomFiltered( AccessPoint.Dir direction )
    {
        List<Chunk> chunksByDirection = new List<Chunk>();

        foreach ( Chunk chunk in chunksInMemory )
        {
            foreach ( AccessPoint accessPoint in chunk.Entrance )
            {
                if ( accessPoint.Direction == direction )
                {
                    chunksByDirection.Add( chunk );
                    break;
                }
            }
        }

        return chunksByDirection[Random.Range( 0, chunksByDirection.Count )];
    }
}