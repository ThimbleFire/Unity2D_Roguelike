using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapInput : MonoBehaviour
{
    public delegate void OnCellClickedHandler( Vector3Int coordinate );

    public static event OnCellClickedHandler OnCellClicked;

    public delegate void OnTileHoverChangeHandler( Vector3Int coordinate );

    public static event OnTileHoverChangeHandler OnTileHoverChange;

    private Grid grid;
    public Tilemap tilemap;

    private Vector3Int lastCoordinate;

    public void Awake()
    {
        grid = GetComponent<Grid>();
        lastCoordinate = Vector3Int.zero;
    }

    public void Update()
    {
        if ( UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject () ) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        Vector3Int coordinate = grid.WorldToCell( mouseWorldPos );

        if ( lastCoordinate != coordinate )
            MousedOverTileChange( coordinate );

        if ( Input.GetMouseButtonDown( 0 ) )
        {
            if ( HUDControls.InventoryOpened == false )
            {
                Debug.Log( coordinate + " clicked" );
                OnCellClicked?.Invoke( coordinate );
            }
        }
    }

    /// <summary>Sets last coordinate as coordinate, clears the tile at last coordinate, sets the tile at coordinate</summary>
    /// <param name="coordinate">The coordinate being moused over</param>
    private void MousedOverTileChange( Vector3Int coordinate )
    {
        lastCoordinate = coordinate;

        OnTileHoverChange?.Invoke( coordinate );
    }
}