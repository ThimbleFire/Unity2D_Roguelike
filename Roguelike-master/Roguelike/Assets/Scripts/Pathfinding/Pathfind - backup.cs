using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// This based on this: https://pavcreations.com/tilemap-based-a-star-algorithm-implementation-in-unity-game/

namespace Backup
{
    public class Node
    {
        public Vector3Int coordinate { get; set; }
        public Vector3 worldPosition { get; set; }
        public bool walkable = true;
        public Node parent;

        public int gCost;
        public int hCost;
        public int fCost { get { return gCost + hCost; } }
    }

    public class Pathfind
    {
        private static Node[,] nodes;

        public static void Occupy( Vector3Int coordinates ) => nodes[coordinates.x, coordinates.y].walkable = false;
        public static void Unoccupy( Vector3Int coordinates ) => nodes[coordinates.x, coordinates.y].walkable = true;

        public static void Setup( Tilemap tilemap )
        {
            nodes = new Node[BoardManager.Width, BoardManager.Height];

            foreach ( Vector3Int cellPosition in tilemap.cellBounds.allPositionsWithin )
            {
                if ( tilemap.HasTile( cellPosition ) )

                    nodes[cellPosition.x, cellPosition.y] = new Node()
                    {
                        walkable = true,
                        coordinate = cellPosition,
                        worldPosition = tilemap.CellToWorld( cellPosition )
                    };
            }
        }

        public static List<Node> Wander( Vector3Int coordinates )
        {
            UnityEngine.Random.InitState( UnityEngine.Random.Range( int.MinValue, int.MaxValue ) );

            Vector3Int destination = Vector3Int.zero;

            while ( nodes[destination.x, destination.y] == null )
            {
                switch ( UnityEngine.Random.Range( 0, 5 ) )
                {
                    case 0:
                        destination = coordinates + Vector3Int.up;
                        break;
                    case 1:
                        destination = coordinates + Vector3Int.down;
                        break;
                    case 2:
                        destination = coordinates + Vector3Int.left;
                        break;
                    case 3:
                        destination = coordinates + Vector3Int.right;
                        break;
                }
            }

            return GetPath( coordinates, destination );
        }

        public static List<Node> GetPath( Vector3Int start, Vector3Int destination )
        {
            Node startNode = nodes[start.x, start.y];
            Node endNode = nodes[destination.x, destination.y];

            List<Node> openSet = new List<Node>( );
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add( startNode );

            while ( openSet.Count > 0 )
            {
                Node currentNode = openSet[0];

                for ( int i = 0; i < openSet.Count; i++ )
                {
                    if ( openSet[i].fCost < currentNode.fCost ||
                        openSet[i].fCost == currentNode.fCost &&
                        openSet[i].hCost < currentNode.hCost )
                    {

                        currentNode = openSet[i];
                    }
                }

                openSet.Remove( currentNode );
                closedSet.Add( currentNode );

                if ( currentNode == endNode )
                {
                    return RetracePath( startNode, endNode );
                }

                List<Node> neighbours = GetNeighbours( currentNode );

                foreach ( Node neighbour in neighbours )
                {
                    if ( /*neighbour.walkable == false ||*/ closedSet.Contains( neighbour ) )
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if ( newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains( neighbour ) )
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance( neighbour, endNode );
                        neighbour.parent = currentNode;

                        if ( !openSet.Contains( neighbour ) )
                            openSet.Add( neighbour );
                    }
                }
            }

            Debug.Log( "This pathfinding sucks" );
            return null;
        }

        private static int GetDistance( Node a, Node b )
        {
            int disX = Mathf.Abs( a.coordinate.x - b.coordinate.x );
            int disY = Mathf.Abs( a.coordinate.y - b.coordinate.y );

            if ( disX > disY )
                return 14 * disY + 10 * ( disX - disY );
            else
                return 14 * disX + 10 * ( disY - disX );
        }

        private static List<Node> RetracePath( Node startNode, Node endNode )
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while ( currentNode != startNode )
            {
                path.Add( currentNode );
                currentNode = currentNode.parent;
            }

            path.Reverse();
            return path;
        }

        private static List<Node> GetNeighbours( Node node )
        {
            List<Node> neighbours = new List<Node>();

            for ( int x = -1; x <= 1; x++ )
            {
                for ( int y = -1; y <= 1; y++ )
                {
                    if ( x == 0 && y == 0 )
                        continue;

                    int checkX = node.coordinate.x + x;
                    int checkY = node.coordinate.y + y;

                    bool checkXInBounds = checkX >= 0 && checkX < nodes.GetLength( 0 );
                    bool checkYInBounds = checkY >= 0 && checkY < nodes.GetLength( 1 );

                    if ( checkXInBounds && checkYInBounds )
                    {
                        if ( nodes[checkX, checkY] != null )
                        {
                            neighbours.Add( nodes[checkX, checkY] );
                        }
                    }
                }
            }

            return neighbours;
        }
    }
}
