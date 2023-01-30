using UnityEngine;

public class TileMapInput : MonoBehaviour {

    public delegate void OnCellClickedHandler( Vector3Int coordinate );
    public static event OnCellClickedHandler OnCellClicked;

    public delegate void OnTileHoverChangeHandler( Vector3Int coordinate );
    public static event OnTileHoverChangeHandler OnTileHoverChange;

    private Grid grid;

    private Vector3Int lastCoordinate;

    public void Awake() {
        grid = GetComponent<Grid>();
        lastCoordinate = Vector3Int.zero;
    }

    public void Update() {
        
        Vector3 mouseWorldPos = Vector3.Zero;
        
        #if UNITY_STANDABLE_WIN
        if ( UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() ) return;
        mouseWorldPos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        #endif

        #if UNITY_ANDROIOD
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if ( UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
            Vector2 touchPosition = Input.touches[0].position;
            mouseWorldPos = Camera.main.ScreenToWorldPoint( touchPosition.x, touchPosition.y, camera.nearClipPlane );            
        }
        #endif
    
        Vector3Int coordinate = grid.WorldToCell( mouseWorldPos );
        if ( lastCoordinate != coordinate )
            MousedOverTileChange( coordinate );

        if ( Input.GetMouseButtonDown( 0 ) ) {
            if ( HUDControls.InventoryOpened == false ) {
                OnCellClicked?.Invoke( coordinate );
            }
        }
    }

    /// <summary>Sets last coordinate as coordinate, clears the tile at last coordinate, sets the tile at coordinate</summary>
    /// <param name="coordinate">The coordinate being moused over</param>
    private void MousedOverTileChange( Vector3Int coordinate ) {
        lastCoordinate = coordinate;

        OnTileHoverChange?.Invoke( coordinate );
    }
}
