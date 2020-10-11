using UnityEngine;
using System.Drawing;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Net.NetworkInformation;

public class MapFactory
{
	public class Room
	{
		public Room(int left, int top)
		{
			width = 3;
			height = 3;
			this.left = left;
			this.top = top;
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
			left += offset.x * ( ( radius_x + parent.radius_x ) + 1 );
			top += offset.y * ( ( radius_y + parent.radius_y ) + 1 );
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
	public enum Type { empty, floor, wall };
	private static Type[,] mapData;

	public static Type[,] BuildFloor( int width, int height, int roomCount )
	{
		// Design rooms

		List<Room> rooms = new List<Room>() { new Room(width / 2, height / 2) };

		int failsafe = 16;

		while ( rooms.Count < roomCount )
		{
			Vector2Int dir = GetRandomDirVector2Int();

			Room r = new Room( rooms[Random.Range( 0, rooms.Count )], dir );

			if ( !RoomCollides( r, rooms ) && InBounds( r, width, height ) )
			{
				rooms.Add( r );
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

		mapData = new Type[width, height];

		// Make rooms

		foreach ( Room room in rooms )
		{
			for ( int x = 0; x < room.width; x++ )
			{
				for ( int y = 0; y < room.height; y++ )
				{
					try
					{
						mapData[room.left + x, room.top + y] = Type.floor;
					}
					catch ( System.Exception )
					{

						throw;
					}
					
				}
			}
		}

		// Make walls

		for ( int x = 0; x < width; x++ )
		{
			for ( int y = 0; y < height; y++ )
			{
				if ( mapData[x, y] == Type.empty && HasAdjacentFloor( x, y, width, height ) )
				{
					mapData[x, y] = Type.wall;
				}
			}
		}

		for ( int x = 0; x < width; x++ )
		{
			for ( int y = 0; y < height; y++ )
			{
				if ( mapData[x, y] == Type.wall && mapData[x - 1, y] == Type.floor && mapData[x + 1, y] == Type.floor )
				{
					mapData[x, y] = Type.floor;
				}

				if ( mapData[x, y] == Type.wall && mapData[x, y - 1] == Type.floor && mapData[x, y + 1] == Type.floor )
				{
					mapData[x, y] = Type.floor;
				}
			}
		}

		return mapData;
	}

	private static bool HasAdjacentFloor( int x, int y, int width, int height )
	{
		if ( x > 0 && mapData[x - 1, y] == Type.floor )
			return true;
		if ( x < width - 1 && mapData[x + 1, y] == Type.floor )
			return true;
		if ( y > 0 && mapData[x, y - 1] == Type.floor )
			return true;
		if ( y < height - 1 && mapData[x, y + 1] == Type.floor )
			return true;
		if ( x > 0 && y > 0 && mapData[x - 1, y - 1] == Type.floor )
			return true;
		if ( x < width - 1 && y > 0 && mapData[x + 1, y - 1] == Type.floor )
			return true;
		if ( x > 0 && y < height - 1 && mapData[x - 1, y + 1] == Type.floor )
			return true;
		if ( x < width - 1 && y < height - 1 && mapData[x + 1, y + 1] == Type.floor )
			return true;

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

	private static bool InBounds(Room r, int width, int height)
	{
		if ( r.left < 2 || r.left > width - 2)
		{
			return false;
		}

		if(r.top < 2 || r.top > height - 2)
		{
			return false;
		}

		if ( r.right < 2 || r.right > width - 2)
		{
			return false;
		}

		if ( r.bottom < 2 || r.bottom > height - 2)
		{
			return false;
		}

		return true;
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
