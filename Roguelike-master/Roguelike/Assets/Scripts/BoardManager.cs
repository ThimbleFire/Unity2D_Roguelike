using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public int roomCount = 15;

    public Tilemap tileMapGround;
    public Tilemap tileMapWall;
    public TileBase floor;
    public TileBase wall;

    private void Awake()
    {
        //tileMapGround.BoxFill( Vector3Int.zero, wall, -25, -25, 25, 25 );

        List<MapFactory.Room> rooms = MapFactory.BuildFloor( roomCount );

        int left = 0;
        int right = 1;
        int top = 1;
        int bottom = 0;

        foreach ( MapFactory.Room item in rooms )
        {
            left    = item.left     < left      ? item.left     : left;
            right   = item.right    > right     ? item.right    : right;
            top     = item.top      > top       ? item.top      : top;
            bottom  = item.bottom   < bottom    ? item.bottom   : bottom;

            for ( int x = 0; x < item.width; x++ )
            {
                for ( int y = 0; y < item.height; y++ )
                {
                    tileMapGround.SetTile( new Vector3Int( item.left + x, item.top + y, 0 ), floor );
                }
            }
        }
    }
}