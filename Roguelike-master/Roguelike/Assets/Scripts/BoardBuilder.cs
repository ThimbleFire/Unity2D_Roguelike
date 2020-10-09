using UnityEngine;
using System.Drawing;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class BoardBuilder
{
	public class Room
	{
		public int left = 0;
		public int top = 0;
		public int width;
		public int height;

		public bool isConnected = false;

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

	public static List<Room> Build( int roomCount )
	{
		List<Room> rooms = new List<Room>();

		int maxFails = 10;

		Room r = new Room()
		{
			width = 3,
			height = 3
		};

		rooms.Add( r );

		while ( rooms.Count < roomCount )
		{
			Vector2Int dir = GetRandomDirVector2Int();

			//get a random room in the list
			int randomRoom = Random.Range( 0, rooms.Count );
			Room sample = rooms[randomRoom];

			//decide how big the room you want to make will be
			int w = 1 + Random.Range( 1, 5 ) * 2;
			int h = 1 + Random.Range( 1, 5 ) * 2;

			//calculate the distance you need to make in order to make this new room

			r = new Room()
			{
				left = dir.x < 0 ? sample.left - w : dir.x == 0 ? sample.center_x : sample.right  + 1,
				top  = dir.y < 0 ? sample.top  - h : dir.y == 0 ? sample.center_y  : sample.bottom + 1,
				width = w,
				height = h
			};

			if ( !RoomCollides( r, rooms ) )
			{
				rooms.Add( r );
			}
			else
			{
				Debug.LogWarning( "Failed to generate room" );
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
