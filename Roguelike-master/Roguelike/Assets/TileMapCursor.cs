using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileMapCursor : MonoBehaviour
{
    public static Vector3Int SelectedTileCoordinates { get; set; }

    public TileBase cursorTileBase;

    private void Awake()
    {
        TilemapCursor = GetComponent<Tilemap>();
        SelectedTileCoordinates = Vector3Int.zero;
    }

    private void Start()
    {
        TileMapInput.OnCellClicked += OnClickCursor;
    }
    
    private void OnClickCursor( Vector3Int coordinate )
    {
        if ( BoardManager.tileMapGround.HasTile( coordinate ) == false )
            return;

        TilemapCursor.SetTile( SelectedTileCoordinates, null );
        TilemapCursor.SetTile( coordinate, cursorTileBase );
        SelectedTileCoordinates = coordinate;

        List<Entity> entities = Entities.Search(coordinate);

        if ( entities.Count == 0 )
            return;

        aaa
    }

    private static Tilemap TilemapCursor { get; set; }

}
