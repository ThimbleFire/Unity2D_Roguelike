using UnityEngine;
using System.Drawing;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class BoardBuilder
{
	public class Room
	{
		public int left;
		public int top;
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

		int x = Random.Range( 0, 5 );
		int y = Random.Range( 0, 5 );

		int i = 0;

		while ( rooms.Count < roomCount )
		{
			Vector2Int dir = GetRandomDirVector2Int();

			Room r = new Room()
			{
				left = x += i == 0 ? 0 : dir.x * 5,
				top  = y += i == 0 ? 0 : dir.y * 5,
				width = 5,
				height = 5
			};

			if ( !RoomCollides( r, rooms ) )
			{
				i++;
				rooms.Add( r );
				Debug.Log( "room added" );
			}
			else
			{
				maxFails--;
				if ( maxFails <= 0 )
					break;
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
