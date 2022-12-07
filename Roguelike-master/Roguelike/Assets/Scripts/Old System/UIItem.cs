using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform startParent;
    private Vector3 startPosition;
    public static GameObject itemBeingDragged;
    public Item item;

    private void Start()
    {
        string savedData = PlayerPrefs.GetString( transform.parent.gameObject.name );

        item = new Item( savedData != string.Empty ? savedData : Binary.EmptyItem );

        //image.sprite = item.Sprite;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetString( transform.parent.gameObject.name, item.BuildBinary() );
    }

    public void OnBeginDrag( PointerEventData eventData )
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        startParent.SetAsLastSibling();
    }

    public void OnDrag( PointerEventData eventData )
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag( PointerEventData eventData )
    {
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if(transform.parent == startParent)
            transform.position = startPosition;
        
    }
}
