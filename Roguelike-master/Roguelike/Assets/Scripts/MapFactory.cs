using System.Collections.Generic;
using UnityEngine;

public class MapFactory
{
    public static int AvailableEntrances = 0;
    public static int PlacedRooms = 0;
    
    public static void Build()
    {
        AvailableEntrances = 0;

        List<Room> rooms = new List<Room>() { new Room( BoardManager.Width / 2, BoardManager.Height / 2 ) };
        List<Room> prototypes = new List<Room>( rooms[0].GetPrototypes );

        int failsafe = 32;

        while ( rooms.Count < BoardManager.RoomLimit )
        {
            List<Room> possibleRoot = rooms.FindAll( x => x.chunk.Entrance.Count > 0 );

            if ( possibleRoot.Count <= 0 )
                return;

            Room parent = possibleRoot[Random.Range( 0, possibleRoot.Count )];
            Room child = new Room( parent );

            bool 
            result = WillPrototypesOverlapExisting( child, rooms );

            if ( result )
            {
                continue;
            }

            result = RoomCollides( child, rooms ) == true || InBounds( child ) == false;

            if ( result )
            {
                failsafe--;

                if ( failsafe <= 0 )
                    break;

                continue;
            }

            rooms.Add( child );
            child.Build();

            parent.RemoveAccessPoint( child.parentAP.Direction );
            child.RemoveAccessPoint( child.inputDirection );

            failsafe = 32;
        }

        GameObject stairs = GameObject.Find( "Ladder(Clone)" );
        if ( stairs == null )
            GameObject.Instantiate( Resources.Load<GameObject>( "Prefabs/Ladder" ), rooms[Random.Range( 0, rooms.Count )].centerWorldSpace, Quaternion.identity );
        else
            stairs.transform.position = rooms[Random.Range( 0, rooms.Count )].centerWorldSpace;

        Debug.Log( "room count: " + rooms.Count );
        MapFactory.PlacedRooms = 0;
    }

    private static bool WillPrototypesOverlapExisting(Room child, List<Room> rooms)
    {
        foreach ( var item in child.GetPrototypes )
        {
            if ( RoomCollides( item, rooms ) == true || InBounds( item ) == false )
            {
                return true;
            }
        }

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

    private static bool InBounds( Room r )
    {
        if ( r.left < 2 || r.left > BoardManager.Width - 2 )
        {
            return false;
        }

        if ( r.top < 2 || r.top > BoardManager.Height - 2 )
        {
            return false;
        }

        if ( r.right < 2 || r.right > BoardManager.Width - 2 )
        {
            return false;
        }

        if ( r.bottom < 2 || r.bottom > BoardManager.Height - 2 )
        {
            return false;
        }

        return true;
    }
    
    public static Vector2Int GetDirVector2Int(AccessPoint.Dir direction)
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
