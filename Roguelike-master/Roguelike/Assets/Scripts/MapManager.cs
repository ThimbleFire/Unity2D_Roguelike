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
        // If there is no root, return a random chunk

        if(placedChunks.Count == 0)
            return chunksInMemory[Random.Range( 0, chunksInMemory.Count )];

        // Get a random root with a valid point of entry

        List<Chunk> placedChunksWithEntrances = placedChunks.FindAll( x => x.Entrance.Count > 0 );
        Chunk root = placedChunksWithEntrances[Random.Range( 0, placedChunksWithEntrances.Count )];

        // Get an exit from room

        int exit = Random.Range( 0, root.Entrance.Count );

        return null;
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

    private void BuildChunk(Chunk chunk)
    {
        List<TileData> curios = chunk.Curios;
        List<TileData> walls = chunk.Walls;
        List<TileData> floors = chunk.Floors;

        foreach ( TileData tile in floors )
            Ground.SetTile( tile.position, Resources.Load<TileBase>( "Dungeon Tileset/" + tile.name ) );

        foreach ( TileData tile in walls )
            Walls.SetTile( tile.position, Resources.Load<TileBase>( "Dungeon Tileset/" + tile.name ) );

        foreach ( TileData tile in curios )
            Curio.SetTile( tile.position, Resources.Load<TileBase>( "Dungeon Tileset/" + tile.name ) );

        placedChunks.Add( chunk );
    }
}
