using System.Collections.Generic;
using UnityEngine;

public class MapFactory
{
    public static int AvailableEntrances = 0;
    public static int PlacedRooms = 0;

    public static void Build()
    {
        PlacedRooms = 0;
        AvailableEntrances = 0;

        int seed = Random.Range( int.MinValue, int.MaxValue );
        Random.InitState( seed );

        List<Room> rooms = new List<Room>() { new Room() };
        List<Room> prototypes = new List<Room>( rooms[0].GetPrototypes );

        while ( prototypes.Count > 0 )
        {
            int index = Random.Range( 0, prototypes.Count );
            Room prototype = prototypes[index];
            Room parent = prototype.Parent;
            Room child = new Room( parent, prototype.parentOutput, true );

            bool result = WillPrototypesOverlapExisting( child, rooms );
            if ( result )
                continue;

            result = WillPrototypesOverlapExisting( child, prototypes );
            if ( result )
                continue;

            rooms.Add( child );
            prototypes.Remove( prototype );
            parent.RemoveAccessPoint( child.parentOutput.Direction );
            prototypes.AddRange( child.GetPrototypes );
            child.Build();
            child.RemoveAccessPoint( child.inputDirection );
        }

        foreach ( Room prototype in prototypes )
        {
            prototype.BuildGhost();
        }
    }

    private static bool WillPrototypesOverlapExisting( Room child, List<Room> rooms )
    {
        foreach ( Room item in child.GetPrototypes )
        {
            if ( RoomCollides( item, rooms ) == true )
            {
                Debug.Log( ( child.IsGhost ? "Prototype" : "Room" ) + ( " collides with " ) + ( item.IsGhost ? "Prototype" : "Room" ) );
                return true;
            }

            if ( InBounds( item ) == false )
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
            return false;
        if ( r.top < 2 || r.top > BoardManager.Height - 2 )
            return false;
        if ( r.Right < 2 || r.Right > BoardManager.Width - 2 )
            return false;
        if ( r.Bottom < 2 || r.Bottom > BoardManager.Height - 2 )
            return false;

        return true;
    }

    public static Vector2Int GetDirVector2Int( AccessPoint.Dir direction )
    {
        switch ( direction )
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