using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceRepository : MonoBehaviour {
    private static List<Chunk> ChunksInMemory;
    public static Dictionary<string, TileBase> Tile;
    public static Chunk Town;
    public static List<EntityReplacement> AvailableEnemies;

    public static void LoadMapData(int mapIndex)
    {
        UnityEngine.Object[] objs = Resources.LoadAll("Chunks/" + mapIndex + "/");
        ChunksInMemory = new List<Chunk>();
        for ( int i = 0; i < objs.Length; i++ ) {
            Chunk r = XMLUtility.Load<Chunk>( objs[i] );

            if ( r.Name == "Town" )
                Town = r;
            else
                ChunksInMemory.Add( r );
        }

        TileBase[] t = Resources.LoadAll<TileBase>( "Dungeon Tileset/" );
        Tile = new Dictionary<string, TileBase>();
        foreach ( TileBase tile in t ) {
            Tile.Add( tile.name, tile );
        }

        Object obj = Resources.Load("Maps/" + mapIndex);
        MapData mapData = XMLUtility.Load<MapData>(obj);
        AvailableEnemies = new List<EntityReplacement>();
        foreach (string path in mapData.EnemyList)
        {
            AvailableEnemies.Add(XMLUtility.Load<EntityReplacement>(path));
        }
    }

    public static EntityReplacement GetRandomAvailableEnemy()
    {
        return (EntityReplacement)AvailableEnemies[Random.Range(0, AvailableEnemies.Count)].Clone();
    }

    public static ItemStats GetItemMatchingCriteria(Item.Type itemType, int mlvl, int TC)
    {
        if (mlvl % 3 != 0)
            mlvl = (int)(Mathf.Round(mlvl / 3) * 3);

        for (int i = mlvl * 3; i > 0; i-=3)
        {
            if (Random.Range(1, 100) < 50)
                continue;

            float roll = Random.Range(0.0f, 100.0f);

            //Item[] list = TC[mlvl/3].FindAll(x=>x.ItemType == itemType);
        }

        return null;
    }

    public static string Get( int width, int height ) {
        List<Chunk> chunksMatchingDimensions = new List<Chunk>( ChunksInMemory.FindAll( x => x.Width == width && x.Height == height ) );

        if ( chunksMatchingDimensions.Count == 0 )
            return string.Empty;

        return chunksMatchingDimensions[Random.Range( 0, chunksMatchingDimensions.Count )].Name;
    }

    public static Chunk Get( string filename ) {
        return ChunksInMemory.Find( x => x.Name == filename ).Clone();
    }

    public static string GetRandom() {
        Chunk c = ChunksInMemory[Random.Range( 0, ChunksInMemory.Count )];

        //width = c.Width;
        //height = c.Height;

        return c.Name;
    }

    public static Chunk GetFiltered( AccessPoint.Dir direction ) {
        // Filter chunks so we don't get deadends unless absolutely neccesary

        // Filter chunks so we don't exceed the maximum number of rooms
        List<Chunk> chunksByExits = new List<Chunk>(
            ChunksInMemory.FindAll( x => MapFactory.PlacedRooms + MapFactory.AvailableEntrances <= BoardManager.RoomLimit ? x.Entrance.Count > 1 : x.Entrance.Count == 1 )
            );

        if ( chunksByExits.Count == 0 ) {
            Debug.LogError( "ChunkRepository.GetFiltered. chunksByExits has zero count. Returning New Chunk, expect errors." );
            return new Chunk();
        }

        // Filter chunks by direction of exits

        List<Chunk> chunksByDirection = new List<Chunk>();

        foreach ( Chunk chunk in chunksByExits ) {
            foreach ( AccessPoint accessPoint in chunk.Entrance ) {
                if ( accessPoint.Direction == direction ) {
                    chunksByDirection.Add( chunk );
                    break;
                }
            }
        }

        if ( chunksByDirection.Count == 0 ) {
            Debug.LogError( "ChunkRepository.GetFiltered. chunksByDirection has zero count. Returning New Chunk, expect errors." );
            return new Chunk();
        }

        return chunksByDirection[Random.Range( 0, chunksByDirection.Count )].Clone();
    }

    public static Chunk GetRandomFiltered( AccessPoint.Dir direction ) {
        List<Chunk> chunksByDirection = new List<Chunk>();

        foreach ( Chunk chunk in ChunksInMemory ) {
            foreach ( AccessPoint accessPoint in chunk.Entrance ) {
                if ( accessPoint.Direction == direction ) {
                    chunksByDirection.Add( chunk );
                    break;
                }
            }
        }

        return chunksByDirection[Random.Range( 0, chunksByDirection.Count )];
    }
}