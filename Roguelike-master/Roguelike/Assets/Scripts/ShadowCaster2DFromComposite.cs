using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

[RequireComponent( typeof( CompositeShadowCaster2D ) )]
public class ShadowCaster2DFromComposite : MonoBehaviour
{
    public bool castsShadows = true;
    public bool selfShadows = false;

    private static readonly FieldInfo _meshField;
    private static readonly FieldInfo _shapePathField;
    private static readonly MethodInfo _generateShadowMeshMethod;

    private ShadowCaster2D[] _shadowCasters;

    private Tilemap _tilemap;
    private TilemapCollider2D _tilemapCollider2D;
    private CompositeCollider2D _compositeCollider;
    private List<Vector2> _compositeVerts = new List<Vector2>();

    /// <summary>
    /// Using reflection to expose required properties in ShadowCaster2D
    /// </summary>
    static ShadowCaster2DFromComposite()
    {
        _meshField = typeof( ShadowCaster2D ).GetField( "m_Mesh", BindingFlags.NonPublic | BindingFlags.Instance );
        _shapePathField = typeof( ShadowCaster2D ).GetField( "m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance );

        _generateShadowMeshMethod = typeof( ShadowCaster2D )
                                    .Assembly
                                    .GetType( "UnityEngine.Experimental.Rendering.Universal.ShadowUtility" )
                                    .GetMethod( "GenerateShadowMesh", BindingFlags.Public | BindingFlags.Static );
    }

    public static void RebuildAll()
    {
        foreach ( var item in FindObjectsOfType<ShadowCaster2DFromComposite>() )
        {
            item.Rebuild();
        }
    }

    private void OnEnable()
    {
        _tilemap = GetComponent<Tilemap>();
        _tilemapCollider2D = GetComponent<TilemapCollider2D>();
    }

    /// <summary>
    /// Rebuilds this specific ShadowCaster2DFromComposite
    /// </summary>
    public void Rebuild()
    {
        _compositeCollider = GetComponent<CompositeCollider2D>();
        _tilemapCollider2D.ProcessTilemapChanges();
        _compositeCollider.GenerateGeometry();
        CreateShadowGameObjects();
        _shadowCasters = GetComponentsInChildren<ShadowCaster2D>();

        GetCompositeVerts();
    }

    /// <summary>
    /// Automatically creates holder gameobjects for each needed ShadowCaster2D, depending on complexity of tilemap
    /// </summary>
    private void CreateShadowGameObjects()
    {
        //Delete all old objects
        for ( int i = transform.childCount - 1; i >= 0; i-- )
        {
            if ( transform.GetChild( i ).name.Contains( "ShadowCaster" ) )
                DestroyImmediate( transform.GetChild( i ).gameObject );
        }
        //Generate new ones
        GameObject newShadowCaster = new GameObject( "ShadowCaster" );
        newShadowCaster.transform.parent = transform;
        ShadowCaster2D caster = newShadowCaster.AddComponent<ShadowCaster2D>();

        int[] layers = new int[] {
          SortingLayer.NameToID("Default"),
          SortingLayer.NameToID("ShadowCaster"),
          SortingLayer.NameToID("Walls"),
          SortingLayer.NameToID("Floor"),
        };

        FieldInfo fieldInfo = caster.GetType().GetField( "m_ApplyToSortingLayers", BindingFlags.NonPublic | BindingFlags.Instance );
        fieldInfo.SetValue( caster, layers );
    }

    /// <summary>
    /// Gathers all the verts of a given path shape in a CompositeCollider2D
    /// </summary>
    /// <param name="path">The path index to fetch verts from</param>
    private void GetCompositeVerts()
    {
        _compositeVerts = new List<Vector2>();

        Vector2[] pathVerts = new Vector2[_compositeCollider.GetPathPointCount( 0 )];
        Vector2[] pathVerts2 = new Vector2[_compositeCollider.GetPathPointCount( 1 )];

        _compositeCollider.GetPath( 0, pathVerts );
        _compositeCollider.GetPath( 1, pathVerts2 );

        _compositeVerts.AddRange( pathVerts );
        _compositeVerts.Add( pathVerts[0] );
        _compositeVerts.AddRange( pathVerts2 );
        _compositeVerts.Add( pathVerts2[0] );

        UpdateCompositeShadow( _shadowCasters[0] );
    }

    /// <summary>
    /// Sets the verts of each ShadowCaster2D to match their corresponding
    /// verts in CompositeCollider2D and then generates the mesh
    /// </summary>
    /// <param name="caster"></param>
    private void UpdateCompositeShadow( ShadowCaster2D caster )
    {
        caster.castsShadows = castsShadows;
        caster.selfShadows = selfShadows;

        Vector2[] points = _compositeVerts.ToArray();
        var threes = ConvertArray( points );

        _shapePathField.SetValue( caster, threes );
        _meshField.SetValue( caster, new Mesh() );
        _generateShadowMeshMethod.Invoke( caster,
            new object[] { _meshField.GetValue(caster),
                _shapePathField.GetValue(caster) } );
    }

    //Quick method for converting a Vector2 array into a Vector3 array
    private Vector3[] ConvertArray( Vector2[] v2 )
    {
        Vector3[] v3 = new Vector3[v2.Length];
        for ( int i = 0; i < v3.Length; i++ )
        {
            Vector2 tempV2 = v2[i];
            v3[i] = new Vector3( tempV2.x, tempV2.y );
        }
        return v3;
    }
}