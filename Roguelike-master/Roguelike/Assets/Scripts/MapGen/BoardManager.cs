using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour {

    public static Tilemap tileMapGround;
    public static Tilemap tileMapWalls;
    public static Tilemap tileMapCurios;

    public static int RoomLimit { get; set; }

    public static int Width { get; set; }
    public static int Height { get; set; }

    private static SaveState saveState;

    private void Awake() {
        tileMapGround = GameObject.Find( "Ground" ).GetComponent<Tilemap>();
        tileMapWalls = GameObject.Find( "Walls" ).GetComponent<Tilemap>();
        tileMapCurios = GameObject.Find( "Curio" ).GetComponent<Tilemap>();

        Application.targetFrameRate = 60;
        Width = 64;
        Height = 64;

        saveState = Game.LoadState();
        ResourceRepository.LoadMapData(saveState.MapIndex);
    }

    private void Start() {

        RoomLimit = 32;
        Build();
    }

    public static void Build() {

        // Establish what area level we're building
        // Tell the resource loader to load enemies belonging to this area

        tileMapGround.ClearAllTiles();
        tileMapWalls.ClearAllTiles();
        tileMapCurios.ClearAllTiles();

        MapFactory.Build(saveState.MapSeed);

        tileMapWalls.CompressBounds();
        tileMapGround.CompressBounds();
        tileMapCurios.CompressBounds();

        ShadowCaster2DFromComposite.RebuildAll();

        Pathfind.Setup( tileMapGround );
    }
}