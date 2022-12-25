using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    private static Node[,] s_nodes;

    public static void Occupy( Vector3Int coordinates )
    {
        s_nodes[coordinates.x, coordinates.y].walkable = false;
        Tilemap_Occupied_Debug.SetOccupiedTile( coordinates );
    }

    public static void Unoccupy( Vector3Int coordinates )
    {
        s_nodes[coordinates.x, coordinates.y].walkable = true;
        Tilemap_Occupied_Debug.SetUnoccupiedTile( coordinates );
    }

    public static void Setup( Tilemap tilemap )
    {
        s_nodes = new Node[BoardManager.Width, BoardManager.Height];

        foreach ( Vector3Int cellPosition in tilemap.cellBounds.allPositionsWithin )
        {
            if ( tilemap.HasTile( cellPosition ) )

                s_nodes[cellPosition.x, cellPosition.y] = new Node()
                {
                    walkable = true,
                    coordinate = cellPosition,
                    worldPosition = tilemap.CellToWorld( cellPosition )
                };
        }
    }

    public static List<Node> Wander( Vector3Int coordinates )
    {
        Node startNode = s_nodes[coordinates.x, coordinates.y];

        List<Node> neighbours = GetNeighbours(startNode);

        List<Node> path = GetPath( coordinates, neighbours[UnityEngine.Random.Range(0, neighbours.Count)].coordinate, false );

        if ( path == null )
            return new List<Node>() { startNode };
        if ( path.Count == 0 )
            return new List<Node>() { startNode };

        return path;
    }

    public static List<Node> GetPath( Vector3Int start, Vector3Int destination, bool includeUnwalkable )
    {
        Node startNode = s_nodes[start.x, start.y];
        Node endNode = s_nodes[destination.x, destination.y];

        // If destination is equal to start position, forfeit turn
        if ( start == destination )
            return new List<Node>( new List<Node>() { startNode } );
        
        // Now it's established we're moving tiles, establish can we move        
        List<Node> startNodeNeighbours = GetNeighbours(startNode);
        
        // If we can't move because there are no unoccupied neighbours, forfeit turn
        if ( startNodeNeighbours.Count == 0 )
            return new List<Node>( new List<Node>() { startNode } );            

        // If the end node is occupied, move it to an adjacent tile
        if ( endNode.walkable == false )
        {
            List<Node> endNodeNeighbours = GetNeighbours(endNode);
            
            if( endNodeNeighbours.Count > 0 )
            {
                endNode = endNodeNeighbours[UnityEngine.Random.Range( 0, endNodeNeighbours.Count )];
            }
            else // If there are no adjacent tiles, search for diagonal tiles
            {
                endNodeNeighbours = GetNeighbours(endNode,  true);
            
                if( endNodeNeighbours.Count > 0 )
                {
                    endNode = endNodeNeighbours[UnityEngine.Random.Range( 0, endNodeNeighbours.Count )];
                }
                else // If there are no diagonal tiles either, forfeit turn
                {
                    return new List<Node>( new List<Node>() { startNode } );   
                }
            }
        }
        
        // Right, let's do some pathfinding!
        
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

            List<Node> neighbours = GetNeighbours( currentNode ); // We'll want to ignore walkables

            foreach ( Node neighbour in neighbours )
            {
                if ( ( neighbour.walkable == false && includeUnwalkable ) || closedSet.Contains( neighbour ) )
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

        // This could be problematic as it risks setting occupied tiles to unoccupied.
        // Maybe when setting a tile to unoccupied, check entities to see if there's still an entity there? 
        if ( includeUnwalkable == false )
        {
            return GetPath( start, destination, true );
        }

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

    private static List<Node> GetNeighbours( Node node, bool diagonal = false )
    {
        List<Node> neighbours = new List<Node>();
        Vector3Int[] offset = new Vector3Int[4];
        
        if ( diagonal == false )
        offset = new Vector3Int[] { Vector3Int.up, Vector3Int.right, Vector3Int.down, Vector3Int.left };
        if ( diagonal == true )
        offset = new Vector3Int[] { Vector3Int.up + Vector3Int.left, Vector3Int.up + Vector3Int.right,
                                    Vector3Int.down + Vector3Int.right, Vector3Int.down + Vector3Int.left };
        
        for ( int i = 0; i < 4; i++ )
        {
            int checkX = node.coordinate.x + offset[i].x;
            int checkY = node.coordinate.y + offset[i].y;

            if ( s_nodes[checkX, checkY] == null ) {
                continue;
            }

            if ( s_nodes[checkX, checkY].walkable == false ) {
                continue;
            }

            bool checkXInBounds = checkX >= 0 && checkX < s_nodes.GetLength( 0 );
            bool checkYInBounds = checkY >= 0 && checkY < s_nodes.GetLength( 1 );

            if ( checkXInBounds && checkYInBounds )
            {
                if ( s_nodes[checkX, checkY] != null )
                {
                    neighbours.Add( s_nodes[checkX, checkY] );
                }
            }
        }

        return neighbours;
    }
}
