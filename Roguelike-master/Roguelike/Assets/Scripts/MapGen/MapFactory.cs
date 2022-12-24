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
            // Get a random prototype
            int index = Random.Range( 0, prototypes.Count );
            Room prototype = prototypes[index];

            // Get that prototypes parent
            Room parent = prototype.Parent;

            // Build a room based on the protoype
            Room child = new Room( parent, prototype.parentOutput, true );

            // Is the child room in bounds
            bool
            result = IsInBounds(child);
            if ( !result )
                continue;

            List<Room> childPrototypes = new List<Room>(child.GetPrototypes);

            // Are the child's prototype rooms within bounds
            result = IsInBounds( childPrototypes );
            if ( !result )
                continue;

            // Does the child collide with other rooms
            result = RoomsCollide( child, rooms );
            if ( result )
                continue;

            // Does the child's prototype rooms collide with other rooms
            result = RoomsCollide( childPrototypes, rooms );
            if ( result )
                continue;

            // Does the child's prototoype rooms collide with other prototype rooms
            result = RoomsCollide( childPrototypes, prototypes );
            if ( result )
                continue;

            prototypes.Remove( prototype );

            // Does the child collide with prototype rooms
            result = RoomsCollide( child, prototypes );
            if ( result )
            {
                prototypes.Add( prototype );
                continue;
            }

            rooms.Add( child );
            parent.RemoveAccessPoint( child.parentOutput.Direction );
            prototypes.AddRange( child.GetPrototypes );
            child.Build();
            child.RemoveAccessPoint( child.inputDirection );
        }
    }

    private static bool RoomsCollide( List<Room> children, List<Room> rooms )
    {
        foreach ( Room room in children )
        {
            if ( RoomsCollide( room, rooms ) == true )
            {
                return true;
            }
        }

        return false;
    }

    private static bool RoomsCollide( Room room, List<Room> rooms )
    {
        foreach ( Room item in rooms )
        {
            if ( room.CollidesWith( item ) )
            {
                if ( item.Parent == room )
                    continue;

                return true;
            }
        }

        return false;
    }

    public static bool IsInBounds( Room r )
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

    public static bool IsInBounds( List<Room> r )
    {
        foreach ( var item in r )
        {
            if ( item.left < 2 || item.left > BoardManager.Width - 2 )
                return false;
            if ( item.top < 2 || item.top > BoardManager.Height - 2 )
                return false;
            if ( item.Right < 2 || item.Right > BoardManager.Width - 2 )
                return false;
            if ( item.Bottom < 2 || item.Bottom > BoardManager.Height - 2 )
                return false;
        }

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