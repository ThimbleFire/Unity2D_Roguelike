using System.Collections.Generic;
using UnityEngine;

public class MapFactory
{
    public static int AvailableEntrances = 0;
    public static int PlacedRooms = 0;

    // Problems
    //
    // We have an issue where we place a room at a location where another room leads to, but the room being placed does not link up to this other room.
    //
    // Solution
    //
    // Have rooms store their parents.
    // Instead of building rooms from exits, (possibleRoot list), we build them from prototypes.
    
    public static void Build( int width, int height )
    {
        AvailableEntrances = 0;

        List<Room> rooms = new List<Room>() { new Room( width / 2, height / 2 ) };
        List<Room> prototypes = new List<Room>();

        int failsafe = 32;

        while ( rooms.Count < BoardManager.RoomLimit )
        {
            // Get all rooms with available exits
            List<Room> possibleRoot = rooms.FindAll( x => x.chunk.Entrance.Count > 0 );

            if ( possibleRoot.Count == 0 )
            {
                Debug.LogWarning( "possible root count is zero, breaking from MapFactory.Build early." );
                break;
            }
            // Select one of those rooms at random
            Room parent = possibleRoot[Random.Range( 0, possibleRoot.Count )];

            // Get an exit index
            AccessPoint parentAP = parent.GetRandomAccessPoint();

            // Get the direction from the parent to the new room we're about to place
            Vector2Int offset = GetDirVector2Int( parentAP.Direction );

            AccessPoint.Dir parentOutputDir = parentAP.Direction;
            AccessPoint.Dir childInputDir = AccessPoint.Flip( parentOutputDir );

            // Select one
            Room child = new Room( parent, offset, childInputDir );

            //Ensure exits aren't leading into walls

            bool exitLeadsToWall = false;

            foreach ( var item in child.chunk.Entrance )
            {
                if ( item.Direction == childInputDir )
                    continue;

                Vector2Int prototypeOffset = GetDirVector2Int( item.Direction );
                Room prototype = new Room( child, prototypeOffset );
                if ( RoomCollides( prototype, rooms ) == true || InBounds( prototype, width, height ) == false )
                {
                    exitLeadsToWall = true;
                }
            }

            if ( exitLeadsToWall )
                continue;
            
            //Something like this...
            if ( ( RoomCollides( child, rooms ) == false && child.Parent == parent ) && InBounds( child, width, height ) == true )
            {   
                rooms.Add( child );
                
                child.Build();

                AvailableEntrances += child.chunk.Entrance.Count;

                parent.RemoveAccessPoint( parentOutputDir );
                child.RemoveAccessPoint( childInputDir );

                AvailableEntrances -= 2;
                
                // trying to stop spawning rooms in position where other rooms lead. This might result in issues.
                foreach(AccessPoint accessPoint in child.chunk.Entrance)
                {
                    Vector2Int prototypeOffset = GetDirVector2Int( accessPoint.Direction );
                    prototypes.Add(new Room(child, prototypeOffset));
                }
                
                failsafe = 32;
            }
            else
            {
                failsafe--;

                if ( failsafe <= 0 )
                    break;
            }
        }

        GameObject stairs = GameObject.Find( "Ladder(Clone)" );
        if ( stairs == null )
            GameObject.Instantiate( Resources.Load<GameObject>( "Prefabs/Ladder" ), rooms[Random.Range( 0, rooms.Count )].centerWorldSpace, Quaternion.identity );
        else
            stairs.transform.position = rooms[Random.Range( 0, rooms.Count )].centerWorldSpace;

        Debug.Log( "room count: " + rooms.Count );
        MapFactory.PlacedRooms = 0;
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
