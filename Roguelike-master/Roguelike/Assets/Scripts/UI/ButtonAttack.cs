using UnityEngine;

namespace AlwaysEast
{
    public class ButtonAttack : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
            TileMapCursor.OnTargetInMeleeRange += OnTargetSelected;
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
}