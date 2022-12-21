using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum OnInteraction
    {
        PICK_UP,
        GO_DOWN_STAIRS,
        OPEN_CHEST

    } public OnInteraction onInteract;

    public void SetPosition( Vector2Int position )
    {
        transform.position = new Vector3( position.x, position.y );
    }

    public void Interact()
    {
        switch ( onInteract )
        {
            case OnInteraction.PICK_UP:

                break;
            case OnInteraction.GO_DOWN_STAIRS:
                BoardManager.Build();
                break;
            case OnInteraction.OPEN_CHEST:
                break;
        }
    }
}