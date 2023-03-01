using UnityEngine;

namespace AlwaysEast
{
    public class ButtonMove : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);

            TileMapCursor.OnEntitySelected += OnTargetSelected;
            TileMapCursor.OnTileSelected += OnTileSelected;
        }

        private void OnTileSelected(Vector3Int coordinate)
        {
            gameObject.SetActive(true);
        }

        private void OnTargetSelected(Vector3Int coordinate, string name)
        {
            gameObject.SetActive(false);
        }
    }
}