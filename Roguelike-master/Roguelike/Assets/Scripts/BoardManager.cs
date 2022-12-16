using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        
        PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        tileMapGround.ClearAllTiles();
        tileMapWalls.ClearAllTiles();
        tileMapCurios.ClearAllTiles();

        int roomCount = 15;
        int width = 128;
        int height = 128;

        MapFactory.Build( width, height, roomCount );
        
        tileMapWalls.CompressBounds();
        tileMapGround.CompressBounds();
        tileMapCurios.CompressBounds();
    }
}
