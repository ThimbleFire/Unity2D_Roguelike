using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public int roomCount = 15;
    public int width = 64;
    public int height = 64;

    public Tilemap tileMapGround;
    public Tilemap tileMapWall;
    public TileBase floor;
    public TileBase wall;

    private void Awake()
    {
        //tileMapGround.BoxFill( Vector3Int.zero, wall, -25, -25, 25, 25 );

        MapFactory.Type[,] rooms = MapFactory.BuildFloor( width, height, roomCount );

        for ( int y = 0; y < height; y++ )
        {
            for ( int x = 0; x < width; x++ )
            {
                switch ( rooms[x, y] )
                {
                    case MapFactory.Type.wall:
                        tileMapGround.SetTile( new Vector3Int( x, y, 0 ), wall );
                        break;
                    case MapFactory.Type.floor:
                        tileMapGround.SetTile( new Vector3Int( x, y, 0 ), floor );
                        break;
                }
            }
        }
    }
}