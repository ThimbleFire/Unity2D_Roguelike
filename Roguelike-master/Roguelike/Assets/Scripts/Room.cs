using UnityEngine;

public class Room
{
    public Room( int left, int top )
    {
        width = 3;
        height = 3;
        this.left = left;
        this.top = top;

        PlayerCharacter.Instance.SetPosition( left + 1, top + 1 );
        occupied = true;
    }

    public Room( Room parent, Vector2Int offset )
    {
        int radius_x = 1;
        int radius_y = 1;

        //decide how big the room you want to make will be
        radius_x = Random.Range( 1, 10 );
        radius_y = Random.Range( 1, 10 );

        width = 1 + radius_x * 2;
        height = 1 + radius_y * 2;

        //set center to parent center
        top = parent.center_y - radius_y;
        left = parent.center_x - radius_x;

        //adjust center in the direction of offset
        left += offset.x * ( ( radius_x + parent.radius_x ) + 1 );
        top += offset.y * ( ( radius_y + parent.radius_y ) + 1 );
    }

    public bool IsPrefab { get { return filename != string.Empty; } }
    public string filename = string.Empty;
    public int left = 0;
    public int top = 0;
    public int width;
    public int height;

    public Vector2Int size
    {
        get { return new Vector2Int( width, height ); }
    }

    public int right
    {
        get { return left + width - 1; }
    }

    public int bottom
    {
        get { return top + height - 1; }
    }

    public int center_x
    {
        get { return left + width / 2; }
    }

    public int center_y
    {
        get { return top + height / 2; }
    }

    public int radius_x
    {
        get { return ( width - 1 ) / 2; }
    }

    public int radius_y
    {
        get { return ( height - 1 ) / 2; }
    }

    public Vector2Int center
    {
        get { return new Vector2Int( center_x, center_y ); }
    }

    public Vector2Int position
    {
        get { return new Vector2Int( left, top ); }
    }

    // whether the room has something in it or not
    public bool occupied = false;

    public bool CollidesWith( Room other )
    {
        if ( left > other.right )
            return false;

        if ( top > other.bottom )
            return false;

        if ( right < other.left )
            return false;

        if ( bottom < other.top )
            return false;

        return true;
    }
}