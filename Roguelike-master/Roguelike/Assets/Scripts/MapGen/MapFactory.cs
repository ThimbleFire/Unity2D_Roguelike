using System.Collections.Generic;
using UnityEngine;

public class MapFactory
{
    public static int AvailableEntrances = 0;
    public static int PlacedRooms = 0;
    
    public static void Build()
    {
        int seed = Random.Range( int.MinValue, int.MaxValue );
        Random.InitState( seed );
        bool result = false;
        MapFactory.PlacedRooms = 0;
        AvailableEntrances = 0;

        List<Room> rooms = new List<Room>() { new Room( ) };
        List<Room> prototypes = new List<Room>( rooms[0].GetPrototypes );

        int failsafe = 32;

        while ( rooms.Count < BoardManager.RoomLimit )
        {
            if ( prototypes.Count == 0 )
                break;

            int index = Random.Range( 0, prototypes.Count );
            Room prototype = prototypes[index];
            Room parent = prototype.Parent;
            Room child = new Room( parent, prototype.parentOutput, true );

            result = WillPrototypesOverlapExisting( child, rooms ); 
            if ( result ) {
                failsafe--; 
                if ( failsafe <= 0 ) {
                    Debug.Log( string.Format( "Child attempting to overlap prototypes", rooms.Count, prototypes.Count, PlacedRooms, AvailableEntrances ) );
                    break;
                }
                continue;
            }

            result = WillPrototypesOverlapExisting( child, prototypes );
            if ( result )
            {
                failsafe--;
                if ( failsafe <= 0 )
                {
                    Debug.Log( string.Format( "Child attempting to overlap prototype's prototypes", rooms.Count, prototypes.Count, PlacedRooms, AvailableEntrances ) );
                    break;
                }
                continue;
            }

            rooms.Add( child );
            prototypes.Remove( prototype );
            parent.RemoveAccessPoint( child.parentOutput.Direction );
            prototypes.AddRange( child.GetPrototypes );
            child.Build();
            child.RemoveAccessPoint( child.inputDirection );

            failsafe = 32;
        }

        Debug.Log( string.Format( "Room count: {0}, prototypes left: {1}, placed rooms: {2}, available entrances: {3}", rooms.Count, prototypes.Count, PlacedRooms, AvailableEntrances ) );

        foreach ( Room prototype in prototypes )
        {
            prototype.BuildGhost();
        }

        //If an error is found, print out the seed so we can investigate
        if(prototypes.Count > 0)
            Debug.LogError( seed );
    }

    private static bool WillPrototypesOverlapExisting(Room child, List<Room> rooms)
    {
        foreach ( Room item in child.GetPrototypes )
        {
            if ( RoomCollides( item, rooms ) == true)
            {

                Debug.Log( ( child.IsGhost ? "Prototype" : "Room" ) + ( " collides with " ) + ( item.IsGhost ? "Prototype" : "Room" ) );
                return true;
            }

            if(InBounds(item) == false)
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
                if ( item.Parent == r )
                    continue;

                return true;
            }
        }

        return false;
    }

    public static bool InBounds( Room r )
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
