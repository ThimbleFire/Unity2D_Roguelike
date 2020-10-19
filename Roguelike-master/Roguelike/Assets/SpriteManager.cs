using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instance;

    private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    public const string InventoryItemRoot = "UI/Inventory/Items/";

    private void Awake()
    {
        Instance = this;

        string root = "UI/Inventory/Items/";

        for ( int i = 0; i < 17; i++ )
        {
            Sprite[] s = Resources.LoadAll<Sprite>( root + i.ToString() );

            for ( int j = 0; j < s.Length; j++ )
            {
                sprites.Add( i + ", " + j, s[j] );
            }
        }

        Debug.LogWarning( "sprites loaded: " + sprites.Count );
    }

    internal Sprite Get( int subcategory, int v )
    {
        try
        {
            Debug.Log( string.Format( "Fetching {0}[{1}]", subcategory, v ) );
            return sprites[subcategory + ", " + v];
        }
        catch ( Exception )
        {
            Debug.LogError( string.Format( InventoryItemRoot + "{0}[{1}] not found.", subcategory, v ) );
            throw;
        }
    }
}