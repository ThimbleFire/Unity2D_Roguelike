using UnityEngine;

public class Interactable : MonoBehaviour
{
    public BoardManager boardManager;

    public void SetPosition( Vector2Int position )
    {
        transform.position = new Vector3( position.x, position.y );
    }

    public void Interact()
    {
        boardManager.Build();
    }
}