using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public static Tilemap tileMapGround;
    public static Tilemap tileMapWalls;
    public static Tilemap tileMapCurios;

    public static int RoomLimit { get; set; }

    private void Awake()
    {
        tileMapGround = GameObject.Find( "Ground" ).GetComponent<Tilemap>(); ;
        tileMapWalls = GameObject.Find( "Walls" ).GetComponent<Tilemap>(); ;
        tileMapCurios = GameObject.Find( "Curio" ).GetComponent<Tilemap>();

        Application.targetFrameRate = 60;
        PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        RoomLimit = 9;

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
    }

    float timer = 0.0f;
    float interval = 1f;

    private void Update()
    {
        timer += Time.smoothDeltaTime;
        if ( timer >= interval ) {

            Build();
            timer = 0.0f;
        }
    }
}
