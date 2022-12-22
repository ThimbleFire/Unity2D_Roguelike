using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public static Tilemap tileMapGround;
    public static Tilemap tileMapWalls;
    public static Tilemap tileMapCurios;

    public static int RoomLimit { get; set; }

    private static bool ShadowsEnabled { get; set; }

    public static int Width { get; set; }
    public static int Height { get; set; }

    private void Awake()
    {
        tileMapGround = GameObject.Find( "Ground" ).GetComponent<Tilemap>();
        ;
        tileMapWalls = GameObject.Find( "Walls" ).GetComponent<Tilemap>();
        tileMapCurios = GameObject.Find( "Curio" ).GetComponent<Tilemap>();

        Application.targetFrameRate = 60;
        PlayerPrefs.DeleteAll();

        ShadowsEnabled = false;
        Width = 64;
        Height = 128;
    }

    private void Start()
    {
        RoomLimit = 64;

        Build();
    }

    public static void Build()
    {
        tileMapGround.ClearAllTiles();
        tileMapWalls.ClearAllTiles();
        tileMapCurios.ClearAllTiles();

        MapFactory.Build();

        tileMapWalls.CompressBounds();
        tileMapGround.CompressBounds();
        tileMapCurios.CompressBounds();

        ShadowCaster2DFromComposite.RebuildAll();

        ShadowsEnabled = false;

        Pathfind.Setup( tileMapGround );
    }
}