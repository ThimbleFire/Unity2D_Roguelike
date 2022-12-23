using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileMapCursor : MonoBehaviour
{
    public static Vector3Int SelectedTileCoordinates { get; set; }

    public TileBase cursorTileBase;

    private static UnityEngine.UI.Text SelectedText;

    private void Awake()
    {
        TilemapCursor = GetComponent<Tilemap>();
        SelectedText = GameObject.Find("Text_Selected").GetComponent<UnityEngine.UI.Text>();
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

        SelectedText.text = string.Empty;

        List<Entity> entities = Entities.Search(coordinate);

        if ( entities.Count == 0 )
            return;

        SelectedText.text = entities[0].Name;
    }

    public static void Hide()
    {
        SelectedText.text = string.Empty;
        TilemapCursor.SetTile( SelectedTileCoordinates, null );
    }

    private static Tilemap TilemapCursor { get; set; }

}
