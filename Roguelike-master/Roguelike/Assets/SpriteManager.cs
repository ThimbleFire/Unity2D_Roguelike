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

        //string path = "UI/Inventory/Items/" + (byte)item.property[(byte)Item.Properties.Subcategory];
        //int index = item.property[(byte)Item.Properties.ItemType];

        //UI / Inventory / Items /< Subcategory >[ItemType]
        //See Item.SubCategories for a list of sub categories

        //image.sprite = Resources.LoadAll<Sprite>( path )[index];
    }

    internal Sprite Get( byte subcategory, byte v )
    {
        try
        {
            return sprites["0000000000000000"];
        }
        catch ( Exception )
        {
            Debug.LogError( string.Format( InventoryItemRoot + "{0}[{1}] not found.", subcategory, v ) );
            throw;
        }

    }
}
