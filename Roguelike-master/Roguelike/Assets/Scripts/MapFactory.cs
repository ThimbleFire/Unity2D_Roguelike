using System.Collections.Generic;
using UnityEngine;

public class MapFactory
{
    public static void Build( int width, int height, int roomCount )
    {
        List<Room> rooms = new List<Room>() { new Room( width / 2, height / 2 ) };

        int failsafe = 16;
        int counter = 1;

        while ( rooms.Count < roomCount )
        {
            // Get all rooms with available exits
            List<Room> possibleRoot = rooms.FindAll( x => x.availableExits > 0 );

            // Select one of those rooms at random
            Room root = possibleRoot[Random.Range( 0, possibleRoot.Count )];
            
            // Get an entrance index
            int index = Random.Range(0, root.chunk.Entrances.Count);
            
            // Get the direction of an exit door from that room
            Vector2Int dir = GetDirVector2Int( root.chunk.Entrances[index].Direction );
            
            // Select one 
            Room r = new Room( root, dir, counter++ );

            if ( !RoomCollides( r, rooms ) && InBounds( r, width, height ) )
            {
                rooms.Add( r );
                root.chunk.Entrances.Remove(root.chunk.entrances[index]);
                root.size --;
                failsafe = 16;
            }
            else
            {
                failsafe--;

                if ( failsafe <= 0 )
                    break;
            }
        }

        Debug.Log( "room count: " + rooms.Count );
    }

    private static bool HasAdjacentFloor( int x, int y, int width, int height )
    {
        if ( x > 0 && mapData[x - 1, y] == Type.floor )
            return true;
        if ( x < width - 1 && mapData[x + 1, y] == Type.floor )
            return true;
        if ( y > 0 && mapData[x, y - 1] == Type.floor )
            return true;
        if ( y < height - 1 && mapData[x, y + 1] == Type.floor )
            return true;
        if ( x > 0 && y > 0 && mapData[x - 1, y - 1] == Type.floor )
            return true;
        if ( x < width - 1 && y > 0 && mapData[x + 1, y - 1] == Type.floor )
            return true;
        if ( x > 0 && y < height - 1 && mapData[x - 1, y + 1] == Type.floor )
            return true;
        if ( x < width - 1 && y < height - 1 && mapData[x + 1, y + 1] == Type.floor )
            return true;

        return false;
    }

    private static bool RoomCollides( Room r, List<Room> rooms )
    {
        foreach ( Room item in rooms )
        {
            if ( r.CollidesWith( item ) )
            {
                return true;
            }
        }

        return false;
    }

    private static bool InBounds( Room r, int width, int height )
    {
        if ( r.left < 2 || r.left > width - 2 )
        {
            return false;
        }

        if ( r.top < 2 || r.top > height - 2 )
        {
            return false;
        }

        if ( r.right < 2 || r.right > width - 2 )
        {
            return false;
        }

        if ( r.bottom < 2 || r.bottom > height - 2 )
        {
            return false;
        }

        return true;
    }

    private static Vector2Int GetRandomDirVector2Int()
    {
        switch ( (AccessPoint.Dir)Random.Range( 0, 4 ); )
        {
            case Direction.up:
                return Vector2Int.up;

            case Direction.down:
                return Vector2Int.down;

            case Direction.left:
                return Vector2Int.left;

            case Direction.right:
                return Vector2Int.right;
        }
        return Vector2Int.zero;
    }
    
    private static Vector2Int GetDirVector2Int(AccessPoint.Dir direction)
    {
        switch(direction)
        {
            case Direction.up:
                return Vector2Int.up;

            case Direction.down:
                return Vector2Int.down;

            case Direction.left:
                return Vector2Int.left;

            case Direction.right:
                return Vector2Int.right;
        }
        
        return Vector2Int.zero;
    }
}
