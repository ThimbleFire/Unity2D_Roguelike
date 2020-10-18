using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public int roomCount = 15;
    public int width = 64;
    public int height = 64;

    public Tilemap tileMapWalls;
    public Tilemap tileMapGround;
    public List<Interactable> interactables;

    public TileBase[] floor;
    public TileBase[] north_walls;
    public TileBase[] east_walls;
    public TileBase[] south_walls;
    public TileBase[] west_walls;
    public TileBase[] corners;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        Build();
    }

    public void Build()
    {
        tileMapGround.ClearAllTiles();
        tileMapWalls.ClearAllTiles();

        MapFactory.Type[,] rooms = MapFactory.BuildFloor( width, height, roomCount, interactables );

        for ( int y = 0; y < height; y++ )
        {
            for ( int x = 0; x < width; x++ )
            {
                switch ( rooms[x, y] )
                {
                    case MapFactory.Type.wall:

                        if ( rooms[x + 1, y] == MapFactory.Type.wall && rooms[x, y - 1] == MapFactory.Type.wall )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), west_walls[Random.Range( 0, west_walls.Length )] );

                        if ( rooms[x - 1, y] == MapFactory.Type.wall && rooms[x, y - 1] == MapFactory.Type.wall )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), east_walls[Random.Range( 0, east_walls.Length )] );

                        if ( rooms[x - 1, y] == MapFactory.Type.wall && rooms[x, y - 1] == MapFactory.Type.wall &&
                             rooms[x + 1, y - 1] == MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), west_walls[Random.Range( 0, west_walls.Length )] );

                        if ( rooms[x + 1, y] == MapFactory.Type.wall && rooms[x, y - 1] == MapFactory.Type.wall &&
                             rooms[x - 1, y - 1] == MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), east_walls[Random.Range( 0, east_walls.Length )] );

                        if ( rooms[x, y - 1] == MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), north_walls[Random.Range( 0, north_walls.Length )] );

                        if ( rooms[x, y + 1] == MapFactory.Type.floor && rooms[x, y - 1] != MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), south_walls[Random.Range( 0, south_walls.Length )] );

                        if ( rooms[x + 1, y] == MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), west_walls[Random.Range( 0, west_walls.Length )] );

                        if ( rooms[x - 1, y] == MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), east_walls[Random.Range( 0, east_walls.Length )] );

                        if ( rooms[x - 1, y] == MapFactory.Type.floor && rooms[x, y - 1] == MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), north_walls[Random.Range( 0, north_walls.Length )] );

                        if ( rooms[x + 1, y] == MapFactory.Type.floor && rooms[x, y - 1] == MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), north_walls[Random.Range( 0, north_walls.Length )] );

                        if ( rooms[x + 1, y] == MapFactory.Type.wall && rooms[x, y + 1] == MapFactory.Type.wall &&
                             rooms[x + 1, y + 1] == MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), corners[0] );

                        if ( rooms[x - 1, y] == MapFactory.Type.wall && rooms[x, y + 1] == MapFactory.Type.wall &&
                             rooms[x - 1, y + 1] == MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), corners[1] );

                        if ( rooms[x + 1, y] == MapFactory.Type.wall && rooms[x, y - 1] == MapFactory.Type.wall &&
                             rooms[x, y + 1] == MapFactory.Type.floor && rooms[x - 1, y] == MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), corners[2] );

                        if ( rooms[x - 1, y] == MapFactory.Type.wall && rooms[x, y - 1] == MapFactory.Type.wall &&
                             rooms[x, y + 1] == MapFactory.Type.floor && rooms[x + 1, y] == MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), corners[3] );

                        if ( rooms[x - 1, y] == MapFactory.Type.floor && rooms[x + 1, y] == MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), corners[6] );

                        if ( rooms[x - 1, y] == MapFactory.Type.floor && rooms[x + 1, y] == MapFactory.Type.floor &&
                             rooms[x, y + 1] == MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), corners[7] );

                        if ( rooms[x - 1, y] == MapFactory.Type.floor && rooms[x + 1, y] == MapFactory.Type.floor &&
                             rooms[x, y - 1] == MapFactory.Type.floor )
                            tileMapWalls.SetTile( new Vector3Int( x, y, 0 ), north_walls[Random.Range( 0, north_walls.Length )] );

                        break;

                    case MapFactory.Type.floor:

                        tileMapGround.SetTile( new Vector3Int( x, y, 0 ), floor[Random.Range( 0, floor.Length )] );

                        break;
                }
            }
        }

        tileMapWalls.CompressBounds();
        tileMapGround.CompressBounds();
    }
}