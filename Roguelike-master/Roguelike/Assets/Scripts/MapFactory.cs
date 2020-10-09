using UnityEngine;
using System.Drawing;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Net.NetworkInformation;

public class MapFactory
{
	public class Room
	{
		public Room()
		{
			width = 3;
			height = 3;
		}

		public Room(Room parent, Vector2Int offset)
		{
			//decide how big the room you want to make will be
			int radius_x = Random.Range( 1, 10 );
			int radius_y = Random.Range( 1, 10 );

			width = 1 + radius_x * 2;
			height = 1 + radius_y * 2;

			//set center to parent center
			top = parent.center_y - radius_y;
			left = parent.center_x - radius_x;

			//adjust center in the direction of offset
			left += offset.x * ((radius_x+parent.radius_x)+1);
			top += offset.y * ((radius_y + parent.radius_y)+1);
		}

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

		//the left and top position
		public Vector2Int position
		{
			get { return new Vector2Int( left, top ); }
		}

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

	private enum Direction { up, down, left, right };

	public static List<Room> BuildFloor( int roomCount )
	{
		List<Room> rooms = new List<Room>();

		Room r = new Room();

		rooms.Add( r );

		int roomFailTimeout = 10;

		while ( rooms.Count < roomCount )
		{
			Vector2Int dir = GetRandomDirVector2Int();

			r = new Room( rooms[Random.Range( 0, rooms.Count )], dir );

			if ( !RoomCollides( r, rooms ) )
			{
				rooms.Add( r );
				roomFailTimeout = 10;
			}
			else
			{
				Debug.LogWarning( "Failed to generate room" );

				if ( roomFailTimeout < 1 )
					break;
				else 
					roomFailTimeout--;
			}
		}

		return rooms;
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
}
