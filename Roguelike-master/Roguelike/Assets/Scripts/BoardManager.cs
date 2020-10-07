using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager :MonoBehaviour
{
    public int roomCount = 15;
    public int width = 32;
    public int height = 32;

    public Tilemap tileMapGround;
    public Tilemap tileMapWall;
    public TileBase floor;
    public TileBase wall;

    private void Awake()
    {
        BoardBuilder b = new BoardBuilder( width, height, roomCount );

        for ( int y = 0; y < height; y++ )
        {
            for ( int x = 0; x < width; x++ )
            {
                switch ( b.GetTileTypeAt(x, y))
                {
                    case BoardBuilder.Type.unknown:
                        break;
                    case BoardBuilder.Type.floor:
                        tileMapGround.SetTile( new Vector3Int( x, y, 0 ), floor );
                        break;
                    case BoardBuilder.Type.wall:
                        tileMapWall.SetTile( new Vector3Int( x, y, 0 ), wall );
                        break;
                    case BoardBuilder.Type.stone:
                        break;
                }                    


                //south wall

                //if ( b.GetTileAt( x, y ) == 1 )


            }
        }
    }
}
