using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node
{
    public bool occupied { get; set; }
    public Vector3Int cellPosition { get; set; }
    public Vector3 worldPosition { get; set; }
    public Node parent { get; set; }
    public int TeamID { get; set; }

    public int gCost;
    public int hCost;

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public Node( /*bool walkable, Vector3Int cellPosition, Vector3 worldPosition*/ )
    {
        gCost = 1;
        hCost = 0;
    }
}

public static class Pathfind
{
    private static Node[,] node;

    ///<remarks>Could also take BattleOrder.cs::Units parameter to occupy tiles</remarks>
    public static void Setup( Tilemap tileMap )
    {
        node = new Node[BoardManager.Width, BoardManager.Height];

        foreach ( Vector3Int cellPosition in tileMap.cellBounds.allPositionsWithin )
        {
            Vector3 worldPosition = tileMap.CellToWorld( cellPosition );

            if ( tileMap.HasTile( cellPosition ) )

                node[cellPosition.x, cellPosition.y] = new Node()
                {
                    occupied = false,
                    cellPosition = cellPosition,
                    worldPosition = worldPosition
                };
        }
    }

    public static void SetTileOccupied( Vector3Int position, int teamID )
    {
        node[position.x, position.y].occupied = true;
        node[position.x, position.y].TeamID = teamID;
    }

    public static void SetTileUnoccupied( Vector3Int position, int teamID )
    {
        node[position.x, position.y].occupied = false;
        node[position.x, position.y].TeamID = int.MinValue;
    }

    public static Queue<Node> GetPath( Vector3Int start, Vector3Int destination )
    {
        bool isXInBounds = destination.x >= 0 && destination.x < node.GetLength( 0 );
        if ( isXInBounds == false )
            return null;

        bool isYInBounds = destination.y >= 0 && destination.y < node.GetLength( 1 );
        if ( isYInBounds == false )
            return null;

        Node startNode = node[start.x, start.y];
        Node destinationNode = node[destination.x, destination.y];

        bool areNodesNull = startNode == null || destinationNode == null;
        if ( areNodesNull == true )
            return null;

        bool isActionForfeit = startNode.cellPosition == destinationNode.cellPosition;
        if ( isActionForfeit )
            return new Queue<Node>( new[] { node[start.x, start.y] } );

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add( startNode );

        while ( openSet.Count > 0 )
        {
            Node currentNode = openSet[0];

            for ( int i = 1; i < openSet.Count; i++ )
            {
                if ( openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost )
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove( currentNode );
            closedSet.Add( currentNode );

            if ( currentNode == destinationNode )
            {
                return RetracePath( startNode, destinationNode );
            }

            foreach ( Node neighbour in GetAdjacentNodes( currentNode ) )
            {
                if ( closedSet.Contains( neighbour ) )
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance( currentNode, neighbour );

                if ( newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains( neighbour ) )
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance( neighbour, destinationNode );
                    neighbour.parent = currentNode;

                    if ( !openSet.Contains( neighbour ) )
                    {
                        openSet.Add( neighbour );
                    }
                }
            }
        }

        return null;
    }

    private static Queue<Node> RetracePath( Node startNode, Node destinationNode )
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        while ( currentNode != startNode )
        {
            path.Add( currentNode );
            currentNode = currentNode.parent;
        }

        path.Reverse();

        Queue<Node> chain = new Queue<Node>();

        for ( int i = 0; i < path.Count; i++ )
        {
            chain.Enqueue( path[i] );
        }

        return chain;
    }

    private static int GetDistance( Node a, Node b )
    {
        int disX = Mathf.Abs( a.cellPosition.x - b.cellPosition.x );
        int disY = Mathf.Abs( a.cellPosition.y - b.cellPosition.y );

        if ( disX > disY )
            return 14 * disY + 10 * ( disX - disY );
        else
            return 14 * disX + 10 * ( disY - disX );
    }

    private static List<Node> GetAdjacentNodes( Node n )
    {
        List<Node> neighbours = new List<Node>();

        for ( int x = -1; x <= 1; x++ )
        {
            for ( int y = -1; y <= 1; y++ )
            {
                if ( x == 0 && y == 0 )
                    continue;

                int checkX = n.cellPosition.x + x;
                int checkY = n.cellPosition.y + y;

                bool checkXInBounds = checkX >= 0 && checkX < node.GetLength( 0 );
                bool checkYInBounds = checkY >= 0 && checkY < node.GetLength( 1 );

                if ( checkXInBounds && checkYInBounds )
                {
                    if ( node[checkX, checkY] != null )
                    {
                        neighbours.Add( node[checkX, checkY] );
                    }
                }
            }
        }

        return neighbours;
    }
}