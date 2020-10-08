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
        List<BoardBuilder.Room> rooms = BoardBuilder.Build( roomCount );

        foreach ( BoardBuilder.Room item in rooms )
        {
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