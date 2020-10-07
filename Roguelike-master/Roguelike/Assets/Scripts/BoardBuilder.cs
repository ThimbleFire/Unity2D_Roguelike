using UnityEngine;
using System.Collections.Generic;

public class BoardBuilder
{
	protected class DRoom
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

		public bool CollidesWith( DRoom other )
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

	int size_x;
	int size_y;

	int[,] map_data;

	List<DRoom> rooms;

	public enum Type
	{
		unknown,
		floor,
		wall,
		stone
	};

	public BoardBuilder( int size_x, int size_y, int roomCount )
	{
		DRoom r;
		this.size_x = size_x;
		this.size_y = size_y;

		map_data = new int[size_x, size_y];

		for ( int x = 0; x < size_x; x++ )
		{
			for ( int y = 0; y < size_y; y++ )
			{
				map_data[x, y] = 3;
			}
		}

		rooms = new List<DRoom>();

		int maxFails = 10;

		while ( rooms.Count < roomCount )
		{
			int rsx = Random.Range( 5, 5 );
			int rsy = Random.Range( 5, 5 );

			r = new DRoom();
			r.left = Random.Range( 0, size_x - rsx );
			r.top = Random.Range( 0, size_y - rsy );
			r.width = rsx;
			r.height = rsy;

			if ( !RoomCollides( r ) )
			{
				rooms.Add( r );
			}
			else
			{
				maxFails--;
				if ( maxFails <= 0 )
					break;
			}

		}

		foreach ( DRoom r2 in rooms )
		{
			MakeRoom( r2 );
		}


		for ( int i = 0; i < rooms.Count; i++ )
		{
			if ( !rooms[i].isConnected )
			{
				int j = Random.Range( 1, rooms.Count );
				MakeCorridor( rooms[i], rooms[( i + j ) % rooms.Count] );
			}
		}

		MakeWalls();

		//ExtrudeWalls();
	}

	bool RoomCollides( DRoom r )
	{
		foreach ( DRoom r2 in rooms )
		{
			if ( r.CollidesWith( r2 ) )
			{
				return true;
			}
		}

		return false;
	}

	public int GetTileAt( int x, int y )
	{
		return map_data[x, y];
	}

	public Type GetTileTypeAt(int x, int y)
	{
		return (Type)map_data[x, y];
	}

	void MakeRoom( DRoom r )
	{

		for ( int x = 0; x < r.width; x++ )
		{
			for ( int y = 0; y < r.height; y++ )
			{
				if ( x == 0 || x == r.width - 1 || y == 0 || y == r.height - 1 )
				{
					map_data[r.left + x, r.top + y] = 2;
				}
				else
				{
					map_data[r.left + x, r.top + y] = 1;
				}
			}
		}

	}

	void MakeCorridor( DRoom r1, DRoom r2 )
	{
		int x = r1.center_x;
		int y = r1.center_y;

		while ( x != r2.center_x )
		{
			map_data[x, y] = 1;

			x += x < r2.center_x ? 1 : -1;
		}

		while ( y != r2.center_y )
		{
			map_data[x, y] = 1;

			y += y < r2.center_y ? 1 : -1;
		}

		r1.isConnected = true;
		r2.isConnected = true;

	}

	void MakeWalls()
	{
		for ( int x = 0; x < size_x; x++ )
		{
			for ( int y = 0; y < size_y; y++ )
			{
				if ( map_data[x, y] == 3 && HasAdjacentFloor( x, y ) )
				{
					map_data[x, y] = 2;
				}
			}
		}
	}

	void ExtrudeWalls()
	{
		for ( int x = 0; x < size_x; x++ )
		{
			for ( int y = size_y; y > 0; y-- )
			{
				if ( HasWallBelow( x, y ) && map_data[x, y] == 3)
				{
					map_data[x, y] = 2;
				}
			}
		}
	}

	bool HasAdjacentFloor( int x, int y )
	{
		if ( x > 0 && map_data[x - 1, y] == 1 )
			return true;
		if ( x < size_x - 1 && map_data[x + 1, y] == 1 )
			return true;
		if ( y > 0 && map_data[x, y - 1] == 1 )
			return true;
		if ( y < size_y - 1 && map_data[x, y + 1] == 1 )
			return true;

		if ( x > 0 && y > 0 && map_data[x - 1, y - 1] == 1 )
			return true;
		if ( x < size_x - 1 && y > 0 && map_data[x + 1, y - 1] == 1 )
			return true;

		if ( x > 0 && y < size_y - 1 && map_data[x - 1, y + 1] == 1 )
			return true;
		if ( x < size_x - 1 && y < size_y - 1 && map_data[x + 1, y + 1] == 1 )
			return true;


		return false;
	}

	bool HasWallBelow(int x, int y)
	{
		if ( y > 1 && y < size_y - 1 && map_data[x, y - 1] == 2 )
			return true;

		return false;
	}

	bool HasFloorBelowBelow(int x, int y)
	{
		if ( y > 2 && y < size_y - 2 && map_data[x, y - 2] == 1 )
			return true;

		return false;
	}
}
