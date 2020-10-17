using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instance;

    Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    public const string InventoryItemRoot = "UI/Inventory/Items/";

    private void Awake()
    {
        Instance = this;

        string root = "UI/Inventory/Items/";

        int i = 0;

        while ( true )
        {
            try
            {
                Sprite[] s = Resources.LoadAll<Sprite>( root + i.ToString() );

                for ( int j = 0; j < s.Length; j++ )
                {
                    sprites.Add( i + ", " + j, s[j] );
                }
            }
            catch ( Exception )
            {
                Debug.LogWarning( "Sprites Loaded: " + sprites.Count );
                break;
            }
        }
    }

    internal Sprite Get( byte subcategory, byte v )
    {
        try
        {
            return sprites[subcategory + ", " + v];
        }
        catch ( Exception )
        {
            Debug.LogError( string.Format( InventoryItemRoot + "{0}[{1}] not found.", subcategory, v ) );
            throw;
        }

    }
}
