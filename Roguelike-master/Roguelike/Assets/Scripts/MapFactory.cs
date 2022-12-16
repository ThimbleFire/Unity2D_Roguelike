using System.Collections.Generic;
using UnityEngine;

public class MapFactory
{
    public static int AvailableEntrances = 0;
    public static int PlacedRooms = 0;

    public static void Build( int width, int height )
    {
        AvailableEntrances = 0;

        List<Room> rooms = new List<Room>() { new Room( width / 2, height / 2 ) };

        int failsafe = 16;

        while ( rooms.Count < BoardManager.RoomLimit )
        {
            // Get all rooms with available exits
            List<Room> possibleRoot = rooms.FindAll( x => x.chunk.Entrance.Count > 0 );

            if ( possibleRoot.Count == 0 )
                return;

            // Select one of those rooms at random
            Room parent = possibleRoot[Random.Range( 0, possibleRoot.Count )];

            // Get an exit index
            AccessPoint parentAP = parent.GetRandomAccessPoint();

            // Get the direction from the parent to the new room we're about to place
            Vector2Int offset = GetDirVector2Int( parentAP.Direction );

            AccessPoint.Dir parentOutputDir = parentAP.Direction;
            AccessPoint.Dir childInputDir = AccessPoint.Flip( parentOutputDir );

            // Select one 
            Room newRoom = new Room( parent, offset, childInputDir );

            if ( !RoomCollides( newRoom, rooms ) && InBounds( newRoom, width, height ) )
            {
                rooms.Add( newRoom );
                
                newRoom.Build();

                AvailableEntrances += newRoom.chunk.Entrance.Count;

                parent.RemoveAccessPoint( parentOutputDir );
                newRoom.RemoveAccessPoint( childInputDir );

                AvailableEntrances -= 2;
                
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
        switch ( (AccessPoint.Dir)Random.Range( 0, 4 ) )
        {
            case AccessPoint.Dir.UP:
                return Vector2Int.up;

            case AccessPoint.Dir.DOWN:
                return Vector2Int.down;

            case AccessPoint.Dir.LEFT:
                return Vector2Int.left;

            case AccessPoint.Dir.RIGHT:
                return Vector2Int.right;
        }
        return Vector2Int.zero;
    }
    
    private static Vector2Int GetDirVector2Int(AccessPoint.Dir direction)
    {
        switch(direction)
        {
            case AccessPoint.Dir.UP:
                return Vector2Int.up;

            case AccessPoint.Dir.DOWN:
                return Vector2Int.down;

            case AccessPoint.Dir.LEFT:
                return Vector2Int.left;

            case AccessPoint.Dir.RIGHT:
                return Vector2Int.right;
        }

        return Vector2Int.zero;
    }
}
