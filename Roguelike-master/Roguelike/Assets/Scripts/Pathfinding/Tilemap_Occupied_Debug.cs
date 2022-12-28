using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class Tilemap_Occupied_Debug : MonoBehaviour {

    private static Tilemap TilemapOccupied { get; set; }
    private static TileBase tileOccupied;

    void Awake()
    { 
        tileOccupied = Resources.Load<TileBase>( "Dungeon Tileset/Dungeon_Tileset_120" ); 
        TilemapOccupied = GetComponent<Tilemap>();
    }
    
    public static void SetOccupiedTile( Vector3Int position ) => TilemapOccupied.SetTile( position, tileOccupied );
    public static void SetUnoccupiedTile( Vector3Int position ) => TilemapOccupied.SetTile( position, null );
}
