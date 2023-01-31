using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileMapCursor : MonoBehaviour {

    public delegate void OnEntitySelectedHandler(Vector3Int coordinate, string name);
    public static event OnEntitySelectedHandler OnEntitySelected;

    public delegate void OnTileSelectedHandler(Vector3Int coordinate);
    public static event OnTileSelectedHandler OnTileSelected;

    public delegate void OnTargetInMeleeRangeHandler(Vector3Int coordinate, string name);
    public static event OnTargetInMeleeRangeHandler OnTargetInMeleeRange;

    public static Vector3Int SelectedTileCoordinates { get; set; }
    private static Tilemap TilemapCursor { get; set; }

    public TileBase cursorTileBase;

    private static UnityEngine.UI.Text s_SelectedText;
    private PlayerCharacter playerCharacter;

    public AudioClip changeTileSoundClip;

    private void Awake() {
        TilemapCursor = GetComponent<Tilemap>();
        s_SelectedText = GameObject.Find( "Text_Selected" ).GetComponent<UnityEngine.UI.Text>();
        SelectedTileCoordinates = Vector3Int.zero;
        TileMapInput.OnCellClicked += OnClickCursor;
    }

    private void Start()
    {
        playerCharacter = GameObject.Find("PlayerCharacter(Clone)").GetComponent<PlayerCharacter>();
    }

    private void OnClickCursor( Vector3Int coordinate ) {
        if ( BoardManager.tileMapGround.HasTile( coordinate ) == false )
            return;

        TilemapCursor.SetTile( SelectedTileCoordinates, null );
        TilemapCursor.SetTile( coordinate, cursorTileBase );
        SelectedTileCoordinates = coordinate;

        AudioDevice.Play(changeTileSoundClip);

        s_SelectedText.text = string.Empty;

        List<Entity> entities = Entities.Search(coordinate);

        if ( entities.Count == 0 )
        {
            OnTileSelected.Invoke(coordinate);
            return;
        }
        else
        {
            s_SelectedText.text = entities[0].Name;

            int distance = Pathfind.GetDistance(playerCharacter._coordinates, coordinate);

            if(distance == 10 || distance == 14)
                OnTargetInMeleeRange.Invoke(coordinate, entities[0].Name);

            OnEntitySelected.Invoke(coordinate, entities[0].Name);
        }
    }

    public static void Hide() {
        s_SelectedText.text = string.Empty;
        TilemapCursor.SetTile( SelectedTileCoordinates, null );
    }
}