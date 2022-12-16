using System.Collections.Generic;
using UnityEngine;

public class MapFactory
{
    private enum Direction { up, down, left, right };

    public enum Type { empty, floor, wall, ladder, chest, enemy, currency, item, destructable };

    private static Type[,] mapData;

    public static Type[,] BuildFloor( int width, int height, int roomCount )
    {
        // Design rooms

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

        mapData = new Type[width, height];

        // Make rooms

        foreach ( Room room in rooms )
        {
            for ( int x = 0; x < room.width; x++ )
            {
                for ( int y = 0; y < room.height; y++ )
                {
                    try
                    {
                        mapData[room.left + x, room.top + y] = Type.floor;
                    }
                    catch ( System.Exception )
                    {
                        throw;
                    }
                }
            }
        }    

        // Make walls

        for ( int x = 0; x < width; x++ )
        {
            for ( int y = 0; y < height; y++ )
            {
                if ( mapData[x, y] == Type.empty && HasAdjacentFloor( x, y, width, height ) )
                {
                    mapData[x, y] = Type.wall;
                }
            }
        }

        // Remove thin walls

        for ( int x = 0; x < width; x++ )
        {
            for ( int y = 0; y < height; y++ )
            {
                if ( mapData[x, y] == Type.wall && mapData[x - 1, y] == Type.floor && mapData[x + 1, y] == Type.floor )
                {
                    mapData[x, y] = Type.floor;
                }

                if ( mapData[x, y] == Type.wall && mapData[x, y - 1] == Type.floor && mapData[x, y + 1] == Type.floor )
                {
                    mapData[x, y] = Type.floor;
                }
            }
        }

        return mapData;
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
        switch ( GetRandomDir() )
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

    private static Direction GetRandomDir( /*Direction lastDir*/ )
    {
        int rand = Random.Range( 0, 4 );

        //while ( (int)lastDir == rand )
        //{
        //	rand = Random.Range( 0, 4 );
        //}

        return (Direction)rand;
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
