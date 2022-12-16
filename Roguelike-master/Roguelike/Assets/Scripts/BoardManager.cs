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

        int width = 128;
        int height = 128;

        MapFactory.Build( width, height );

        tileMapWalls.CompressBounds();
        tileMapGround.CompressBounds();
        tileMapCurios.CompressBounds();
    }
}
