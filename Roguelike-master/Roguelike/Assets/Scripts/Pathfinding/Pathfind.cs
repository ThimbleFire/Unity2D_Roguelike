using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node {
    public Vector3Int coordinate { get; set; }
    public Vector3 worldPosition { get; set; }
    public bool walkable = true;
    public Node parent;
    public byte distance = 0;

    public int gCost;
    public int hCost;
    public int fCost { get { return gCost + hCost; } }
}

public class Pathfind {
    private static Node[,] s_nodes;

    public static void Occupy( Vector3Int coordinates ) {
        s_nodes[coordinates.x, coordinates.y].walkable = false;
        Tilemap_Occupied_Debug.SetOccupiedTile( coordinates );
    }

    public static void Unoccupy( Vector3Int coordinates ) {
        s_nodes[coordinates.x, coordinates.y].walkable = true;
        Tilemap_Occupied_Debug.SetUnoccupiedTile( coordinates );
    }

    public static void Setup( Tilemap tilemap ) {
        s_nodes = new Node[BoardManager.Width, BoardManager.Height];

        foreach ( Vector3Int cellPosition in tilemap.cellBounds.allPositionsWithin ) {
            if ( tilemap.HasTile( cellPosition ) )

                s_nodes[cellPosition.x, cellPosition.y] = new Node() {
                    walkable = true,
                    coordinate = cellPosition,
                    worldPosition = tilemap.CellToWorld( cellPosition )
                };
        }

        List<Vector3Int> entities = Entities.GetOccupied();
        foreach ( Vector3Int coordinates in entities ) {
            s_nodes[coordinates.x, coordinates.y].walkable = false;
            Occupy( coordinates );
        }
    }

    public static List<Node> Wander( Vector3Int coordinates ) {
        Node startNode = s_nodes[coordinates.x, coordinates.y];

        List<Node> neighbours = GetNeighbours(startNode, false);

        return GetPath( coordinates, neighbours[UnityEngine.Random.Range( 0, neighbours.Count )].coordinate, false );
    }

    public static List<Node> GetPath( Vector3Int start, Vector3Int destination, bool includeUnwalkable )
    {
        Node startNode = s_nodes[start.x, start.y];
        Node endNode = s_nodes[destination.x, destination.y];

        // If destination is equal to start position, forfeit turn
        if ( start == destination ) {
            Debug.LogWarning( "Attemping to move onto self" );
            return null;
        }

        int disX = Mathf.Abs( startNode.coordinate.x - endNode.coordinate.x );
        int disY = Mathf.Abs( startNode.coordinate.y - endNode.coordinate.y );
        int distance = disX + disY;

        // If destination is occupied and distance is one tile away, forfeit turn
        if ( endNode.walkable == false && distance == 1 ) {
            Debug.LogWarning( "Destination is occupied and is adjacent to player character" );
            return null;
        }
        // Now it's established we're moving tiles, establish can we move
        List<Node> startNodeNeighbours = GetNeighbours(startNode, false);

        // If we can't move because there are no unoccupied neighbours, forfeit turn
        if ( startNodeNeighbours.Count == 0 ) {
            Debug.LogWarning( "No adjacent neighbours to StartNode" );
            return null;
        }

        // If the end node is occupied, move it to an adjacent tile
        if ( endNode.walkable == false )
        {
            List<Node> endNodeNeighbours = GetNeighbours(endNode, false);
            if ( endNodeNeighbours.Count > 0 )
            {
                List<Node> neighboursSortedByDistance = SortNearest(startNode, endNodeNeighbours); // new code
                Node pathToNeighbour = GetPathToNeighbour(startNode, neighboursSortedByDistance);

                if ( pathToNeighbour != null )
                    return GetPath( startNode, pathToNeighbour, includeUnwalkable );
            }
                    
            SpeechBubble.Show( Entities.GetTurnTaker.transform, SpeechBubble.Type.Attention );

            endNodeNeighbours = GetNeighbours(endNode, true);
            if ( endNodeNeighbours.Count > 0 )
            {
                List<Node> neighboursSortedByDistance = SortNearest(startNode, endNodeNeighbours); // new code
                Node pathToNeighbour = GetPathToNeighbour(startNode, neighboursSortedByDistance);

                if ( pathToNeighbour != null )
                    return GetPath( startNode, pathToNeighbour, includeUnwalkable );
                else {
                    Debug.LogWarning( "No adjacent or diagonal neighbours to move to" );
                    return null;
                }
            }
        }

        return GetPath( startNode, endNode, includeUnwalkable );
    }
    
    private static Node GetPathToNeighbour( Node startNode, List<Node> neighboursSortedByDistance ) {
        foreach ( Node node in neighboursSortedByDistance ) {
            List<Node> path = GetPath(startNode, node, false);
            if ( path != null )
                return path[path.Count - 1];
        }

        return null;
    }

    private static List<Node> GetPath(Node startNode, Node endNode, bool includeUnwalkable )
    {
        List<Node> openSet = new List<Node>( );
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add( startNode );

        while ( openSet.Count > 0 ) {
            Node currentNode = openSet[0];

            for ( int i = 0; i < openSet.Count; i++ ) {
                if ( openSet[i].fCost < currentNode.fCost ||
                    openSet[i].fCost == currentNode.fCost &&
                    openSet[i].hCost < currentNode.hCost ) {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove( currentNode );
            closedSet.Add( currentNode );

            if ( currentNode == endNode ) {
                return RetracePath( startNode, endNode );
            }

            List<Node> neighbours = GetNeighbours( currentNode, false ); // We'll want to ignore walkables

            foreach ( Node neighbour in neighbours ) {
                if ( neighbour.walkable == false && includeUnwalkable || closedSet.Contains( neighbour ) ) {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if ( newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains( neighbour ) ) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance( neighbour, endNode );
                    neighbour.parent = currentNode;

                    if ( !openSet.Contains( neighbour ) )
                        openSet.Add( neighbour );
                }
            }
        }

        return null;
    }

    public static int GetDistance( Node a, Node b ) {
        int disX = Mathf.Abs( a.coordinate.x - b.coordinate.x );
        int disY = Mathf.Abs( a.coordinate.y - b.coordinate.y );
        return disX > disY ? 14 * disY + 10 * ( disX - disY ) : 14 * disX + 10 * ( disY - disX );
    }
    public static int GetDistance( Vector3Int a, Vector3Int b ) => GetDistance( s_nodes[a.x, a.y], s_nodes[b.x, b.y] );

    private static List<Node> RetracePath( Node startNode, Node endNode )
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while ( currentNode != startNode ) {
            path.Add( currentNode );
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private static List<Node> GetNeighbours( Node node, bool diagonal) {
        List<Node> neighbours = new List<Node>();
        Vector3Int[] offset;

        if ( !diagonal ) offset = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.right,
            Vector3Int.down,
            Vector3Int.left
        };
        else offset = new Vector3Int[]
        {
            Vector3Int.up + Vector3Int.left,
            Vector3Int.up + Vector3Int.right,
            Vector3Int.down + Vector3Int.right,
            Vector3Int.down + Vector3Int.left
        };

        for ( int i = 0; i < 4; i++ )
        {
            int checkX = node.coordinate.x + offset[i].x;
            int checkY = node.coordinate.y + offset[i].y;

            if ( s_nodes[checkX, checkY] == null )
                continue;

            if ( s_nodes[checkX, checkY].walkable == false ) 
                continue;

            bool checkXInBounds = checkX >= 0 && checkX < s_nodes.GetLength( 0 );
            bool checkYInBounds = checkY >= 0 && checkY < s_nodes.GetLength( 1 );

            if ( checkXInBounds && checkYInBounds ) {
                if ( s_nodes[checkX, checkY] != null ) {
                    neighbours.Add( s_nodes[checkX, checkY] );
                }
            }
        }

        return neighbours;
    }

    private static List<Node> SortNearest( Node startNode, List<Node> endNodes ) {
        foreach ( Node node in endNodes ) {
            node.distance = ( byte )GetDistance( startNode, node );
        }

        endNodes.Sort( ( x, y ) => x.distance.CompareTo( y.distance ) );

        return endNodes;
    }
}