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

		public bool CollidesWith( Room other )
		{
			if ( left - 2 > other.right + 2 )
				return false;

			if ( top - 2 > other.bottom + 2 )
				return false;

			if ( right + 2 < other.left - 2 )
				return false;

			if ( bottom + 2 < other.top - 2 )
				return false;

			return true;
		}
	}

	private enum Direction { up, down, left, right };

	public static List<Room> Build( int roomCount )
	{
		List<Room> rooms = new List<Room>();

		Room r = new Room();
		r.left = Random.Range( 0, 5 );
		r.top = Random.Range(  0, 5 );
		r.width = 5;
		r.height = 5;

		rooms.Add( r );

		for ( int i = 0; i < roomCount; i++ )
		{
			Vector2Int dir = GetRandomDirVector2Int();

			//randomize next room parameters
			r.left += (dir.x * 5);
			r.top += (dir.y * 5);

			rooms.Add( r );
		}

		return rooms;
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

	private static Direction GetRandomDir()
	{
		return (Direction)UnityEngine.Random.Range( 0, 4 );
	}
}
