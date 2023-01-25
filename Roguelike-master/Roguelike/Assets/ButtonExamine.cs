using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExamine : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
        TileMapCursor.OnEntitySelected += OnTargetSelected;
        TileMapCursor.OnTileSelected += OnTileSelected;
    }

    private void OnTileSelected(Vector3Int coordinate)
    {
        gameObject.SetActive(false);
    }

    private void OnTargetSelected(Vector3Int coordinate, string name)
    {
        gameObject.SetActive(true);
    }
}
