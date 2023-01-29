using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceRepository : MonoBehaviour {
    private static List<Chunk> ChunksInMemory;
    public static Dictionary<string, TileBase> Tile;
    public static Dictionary<string, GameObject> Prefab;
    public static Chunk Town;

    private void Awake() {
        // load maps
        UnityEngine.Object[] objs = Resources.LoadAll("Chunks/");

        ChunksInMemory = new List<Chunk>();
        for ( int i = 0; i < objs.Length; i++ ) {
            Chunk r = XMLUtility.Load<Chunk>( objs[i] );

            if ( r.Name == "Town" )
                Town = r;
            else
                ChunksInMemory.Add( r );
        }

        // load tiles

        TileBase[] t = Resources.LoadAll<TileBase>( "Dungeon Tileset/" );
        Tile = new Dictionary<string, TileBase>();
        foreach ( TileBase tile in t ) {
            Tile.Add( tile.name, tile );
        }

        GameObject[] entityPrefabs = Resources.LoadAll<GameObject>("Prefabs/Entities/NPCs/");

        Prefab = new Dictionary<string, GameObject>();
        foreach ( GameObject item in entityPrefabs ) {
            Prefab.Add( item.name, item );
        }
    }

    public static ItemStats GetItemMatchingCriteria(int v, ItemStats.Type itemType, int entityDifficulty)
    {
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

    public static GameObject GetUnit( string prefabName ) {
        return Prefab[prefabName];
    }

    public static GameObject GetRandomEnemyByLevel(int difficulty)
    {
        int num;
        string prefabName = string.Empty;

        switch (difficulty)
        {
            case 0:
                num = Random.Range(0, System.Enum.GetNames(typeof(Entities.ENEMY_LEVEL_0)).Length);
                prefabName = System.Enum.ToObject(typeof(Entities.ENEMY_LEVEL_0), num).ToString();
                break;
            case 1:
                num = Random.Range(0, System.Enum.GetNames(typeof(Entities.ENEMY_LEVEL_1)).Length);
                prefabName = System.Enum.ToObject(typeof(Entities.ENEMY_LEVEL_1), num).ToString();
                break;
            case 2:
                num = Random.Range(0, System.Enum.GetNames(typeof(Entities.ENEMY_LEVEL_2)).Length);
                prefabName = System.Enum.ToObject(typeof(Entities.ENEMY_LEVEL_2), num).ToString();
                break;
        }
        
        return Prefab[prefabName];
    }
}