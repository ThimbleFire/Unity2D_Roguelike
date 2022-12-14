using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class ChunkEditor : EditorWindow
{
    Tilemap Floor;
    Tilemap Walls;
    Tilemap Curios;

    private bool Loaded = false;

    private int popupIndex = 0;
    private string[] popupOptions;
    private string newSceneName = string.Empty;

    [MenuItem( "Window/Editor" )]
    private static void ShowWindow()
    {
        GetWindow( typeof( ChunkEditor ) );
    }

    private void OnGUI()
    {
        if ( Loaded == false )
        {
            if ( GUILayout.Button( "Load Tilemaps" ) )
            {
                GameObject

                c = GameObject.Find( "Ground" );
                Floor = c.GetComponent<Tilemap>();

                c = GameObject.Find( "Walls" );
                Walls = c.GetComponent<Tilemap>();

                c = GameObject.Find( "Curio" );
                Curios = c.GetComponent<Tilemap>();

                RefreshChunkList();

                Loaded = true;
            }
        }
        else
        {
            if ( popupOptions != null )
            {
                int temp = popupIndex;
                popupIndex = EditorGUI.Popup( new Rect( 3, 4, position.width - 6, 20 ), popupIndex, popupOptions );
                if ( temp != popupIndex )
                {
                    LoadScene();
                }
            }
            EditorGUILayout.Space( 22 );
            if ( GUILayout.Button( "Paint over 0, 0, 0 with a curio" ) )
                Curios.SetTile( Vector3Int.zero, Resources.Load<TileBase>( "Dungeon Tileset/Dungeon_Tileset_86" ) );
            if ( GUILayout.Button( "Clear Scene" ) )
                ClearScene();
            newSceneName = GUILayout.TextArea( newSceneName );
            EditorGUI.BeginDisabledGroup( newSceneName == string.Empty );
            {
                if ( GUILayout.Button( "Save New Scene" ) )
                    SaveScene( newSceneName );
            }
            EditorGUI.EndDisabledGroup();
            if ( GUILayout.Button( "Overwrite Scene" ) )
                SaveScene( popupOptions[popupIndex] );
        }
    }

    private void ClearScene()
    {
        Floor.ClearAllTiles();
        Walls.ClearAllTiles();
        Curios.ClearAllTiles();
    }

    private void SaveScene( string name )
    {
        Chunk chunk = new Chunk
        {
            Name = name,
            Walls = GetTileMapTilesByName( "Walls" ),
            Curios = GetTileMapTilesByName( "Curio" ),
            Floors = GetTileMapTilesByName( "Ground" ),
            Entrance = GetAccessPoints(),
            //this assumes walls extends to the far north-east-south and west side of the map.
            Width = Floor.size.x,
            Height = Floor.size.y
        };

        XMLUtility.Save( chunk, chunk.Name );
        RefreshChunkList();
        newSceneName = string.Empty;
    }

    private void LoadScene()
    {
        ClearScene();

        Chunk data = XMLUtility.Load<Chunk>( popupOptions[popupIndex] );

        List<TileData> curios = data.Curios;
        List<TileData> walls = data.Walls;
        List<TileData> floors = data.Floors;

        //Loading each tilebase over and over is extremely inefficient, consider making an asset dictionary.

        foreach ( TileData tile in floors )
            Floor.SetTile( tile.position, Resources.Load<TileBase>( "Dungeon Tileset/" + tile.name ) );
        
        foreach ( TileData tile in walls )
            Walls.SetTile( tile.position, Resources.Load<TileBase>( "Dungeon Tileset/" + tile.name ) );
        
        foreach ( TileData tile in curios )
            Curios.SetTile( tile.position, Resources.Load<TileBase>( "Dungeon Tileset/" + tile.name ) );
        
    }

    private List<TileData> GetTileMapTilesByName(string name)
    {
        List<TileData> r = new List<TileData>();

        GameObject c = GameObject.Find( name );
        Tilemap t = c.GetComponent<Tilemap>();
        t.CompressBounds();

        TileBase[] tiles = t.GetTilesBlock( t.cellBounds );

        for ( int y = t.cellBounds.yMin; y < t.cellBounds.yMax; y++ )
        {
            for ( int x = t.cellBounds.xMin; x < t.cellBounds.xMax; x++ )
            {
                if ( t.GetTile( new Vector3Int( x, y, 0 ) ) == null )
                    continue;

                TileData f = new TileData
                {
                    name = t.GetTile( new Vector3Int( x, y, 0 ) ).name,
                    position = new Vector3Int( x, y, 0 )
                };

                r.Add( f );
            }
        }

        return r;
    }

    private List<AccessPoint> GetAccessPoints()
    {
        List<AccessPoint> accesspoints = new List<AccessPoint>();

        TileBase arrowRight = Resources.Load<TileBase>( "Dungeon Tileset/Dungeon_Tileset_110" );
        TileBase arrowLeft = Resources.Load<TileBase>( "Dungeon Tileset/Dungeon_Tileset_111" );
        TileBase arrowDown = Resources.Load<TileBase>( "Dungeon Tileset/Dungeon_Tileset_112" );
        TileBase arrowUp = Resources.Load<TileBase>( "Dungeon Tileset/Dungeon_Tileset_113" );

        if ( Curios.ContainsTile( arrowRight ) || Curios.ContainsTile( arrowLeft ) || Curios.ContainsTile( arrowDown ) || Curios.ContainsTile( arrowUp )  )
        {
            for ( int y = Curios.cellBounds.yMin; y < Curios.cellBounds.yMax; y++ )
            {
                for ( int x = Curios.cellBounds.xMin; x < Curios.cellBounds.xMax; x++ )
                {
                    Vector3Int position = new Vector3Int( x, y, 0 );

                    if ( Curios.GetTile( position ) == arrowRight )
                        accesspoints.Add( new AccessPoint { position = position, Direction = AccessPoint.Dir.RIGHT } );

                    if ( Curios.GetTile( position ) == arrowLeft )
                        accesspoints.Add( new AccessPoint { position = position, Direction = AccessPoint.Dir.LEFT } );

                    if ( Curios.GetTile( position ) == arrowDown )
                        accesspoints.Add( new AccessPoint { position = position, Direction = AccessPoint.Dir.DOWN } );

                    if ( Curios.GetTile( position ) == arrowUp )
                        accesspoints.Add( new AccessPoint { position = position, Direction = AccessPoint.Dir.UP } );
                }
            }
        }

        return accesspoints;
    }

    private void RefreshChunkList()
    {
        popupOptions = Directory.GetFiles( Application.streamingAssetsPath, "*.xml" );
        for ( int i = 0; i < popupOptions.Length; i++ )
            popupOptions[i] = popupOptions[i].Remove( 0, Application.streamingAssetsPath.Length + 1 );
    }
}
