using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LegacyNode
{
    public bool occupied { get; set; }
    public Vector3Int cellPosition { get; set; }
    public Vector3 worldPosition { get; set; }
    public LegacyNode parent { get; set; }
    public int TeamID { get; set; }

    public int gCost;
    public int hCost;

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public LegacyNode( /*bool walkable, Vector3Int cellPosition, Vector3 worldPosition*/ )
    {
        gCost = 1;
        hCost = 0;
    }
}

public static class LegacyPathfind
{
    private static LegacyNode[,] node;

    ///<remarks>Could also take BattleOrder.cs::Units parameter to occupy tiles</remarks>
    public static void Setup( Tilemap tileMap )
    {
        node = new LegacyNode[BoardManager.Width, BoardManager.Height];

        foreach ( Vector3Int cellPosition in tileMap.cellBounds.allPositionsWithin )
        {
            Vector3 worldPosition = tileMap.CellToWorld( cellPosition );

            if ( tileMap.HasTile( cellPosition ) )

                node[cellPosition.x, cellPosition.y] = new LegacyNode()
                {
                    occupied = false,
                    cellPosition = cellPosition,
                    worldPosition = worldPosition
                };
        }
    }

    public static Queue<LegacyNode> Wander( Vector3Int coordinates )
    {
        UnityEngine.Random.InitState( UnityEngine.Random.Range( int.MinValue, int.MaxValue ) );

        Vector3Int destination = Vector3Int.zero;

        while ( node[destination.x, destination.y] == null )
        {
            switch ( UnityEngine.Random.Range(0, 5) )
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

    public static Queue<LegacyNode> GetPath( Vector3Int start, Vector3Int destination )
    {
        bool isXInBounds = destination.x >= 0 && destination.x < node.GetLength( 0 );
        if ( isXInBounds == false )
            return null;

        bool isYInBounds = destination.y >= 0 && destination.y < node.GetLength( 1 );
        if ( isYInBounds == false )
            return null;

        LegacyNode startNode = node[start.x, start.y];
        LegacyNode destinationNode = node[destination.x, destination.y];

        bool areNodesNull = startNode == null || destinationNode == null;
        if ( areNodesNull == true )
            return null;

        bool isActionForfeit = startNode.cellPosition == destinationNode.cellPosition;
        if ( isActionForfeit )
            return new Queue<LegacyNode>( new[] { node[start.x, start.y] } );

        List<LegacyNode> openSet = new List<LegacyNode>();
        HashSet<LegacyNode> closedSet = new HashSet<LegacyNode>();

        openSet.Add( startNode );

        while ( openSet.Count > 0 )
        {
            LegacyNode currentNode = openSet[0];

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

            List<LegacyNode> adjacentNodes = GetAdjacentNodes( currentNode );

            if ( adjacentNodes.Count == 0 )
            {
                Debug.Log( "There are no adjacent nodes" );
                return null;
            }

            foreach ( LegacyNode neighbour in adjacentNodes )
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

    internal static void Occupy( Vector3Int coordinates ) => node[coordinates.x, coordinates.y].occupied = true;
    internal static void Unoccupy( Vector3Int coordinates ) => node[coordinates.x, coordinates.y].occupied = false;

    private static Queue<LegacyNode> RetracePath( LegacyNode startNode, LegacyNode destinationNode )
    {
        List<LegacyNode> path = new List<LegacyNode>();
        LegacyNode currentNode = destinationNode;

        while ( currentNode != startNode )
        {
            path.Add( currentNode );
            currentNode = currentNode.parent;
        }

        path.Reverse();

        Queue<LegacyNode> chain = new Queue<LegacyNode>();

        for ( int i = 0; i < path.Count; i++ )
        {
            chain.Enqueue( path[i] );
        }

        return chain;
    }

    private static int GetDistance( LegacyNode a, LegacyNode b )
    {
        int disX = Mathf.Abs( a.cellPosition.x - b.cellPosition.x );
        int disY = Mathf.Abs( a.cellPosition.y - b.cellPosition.y );

        if ( disX > disY )
            return 14 * disY + 10 * ( disX - disY );
        else
            return 14 * disX + 10 * ( disY - disX );
    }

    private static List<LegacyNode> GetAdjacentNodes( LegacyNode n )
    {
        List<LegacyNode> neighbours = new List<LegacyNode>();

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