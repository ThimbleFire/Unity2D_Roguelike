using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapInput : MonoBehaviour
{
    public delegate void OnCellClickedHandler( Vector3Int coordinate );

    public static event OnCellClickedHandler OnCellClicked;

    public delegate void OnTileHoverChangeHandler( Vector3Int coordinate );

    public static event OnTileHoverChangeHandler OnTileHoverChange;

    private Grid grid;

    private Vector3Int lastCoordinate;

    public Tilemap selectedTileMap;
    public TileBase selectedTileBase;

    public void Awake()
    {
        grid = GetComponent<Grid>();
        lastCoordinate = Vector3Int.zero;

        OnCellClicked += TileMapInput_OnCellClicked;
    }

    public void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        Vector3Int coordinate = grid.WorldToCell( mouseWorldPos );

        if ( lastCoordinate != coordinate )
            MousedOverTileChange( coordinate );

        if ( Input.GetMouseButtonDown( 0 ) )
            OnCellClicked?.Invoke( coordinate );
    }

    /// <summary>Sets last coordinate as coordinate, clears the tile at last coordinate, sets the tile at coordinate</summary>
    /// <param name="coordinate">The coordinate being moused over</param>
    private void MousedOverTileChange( Vector3Int coordinate )
    {
        //selectedTileMap.SetTile( lastCoordinate, null );
        //selectedTileMap.SetTile( coordinate, selectedTileBase );

        lastCoordinate = coordinate;

        OnTileHoverChange?.Invoke( coordinate );
    }

    Vector3Int last = Vector3Int.zero;

    private void TileMapInput_OnCellClicked( Vector3Int coordinate )
    {
        selectedTileMap.SetTile( last, null );
        selectedTileMap.SetTile( coordinate, selectedTileBase );
        last = coordinate;
        Debug.Log( coordinate );
    }

}