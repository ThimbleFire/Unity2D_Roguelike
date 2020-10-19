using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform startParent;
    private Vector3 startPosition;
    public static GameObject itemBeingDragged;
    public binary;
    public Item item;

    private void Start()
    {
        string savedData = PlayerPrefs.GetString( gameObject.name );

        item = new Item( savedData != string.Empty ? savedData : Binary.EmptyItem );
        item.Build();

        image.sprite = item.Sprite;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetString( gameObject.name, binary );
    }

    public void OnBeginDrag( PointerEventData eventData )
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        Debug.Log( gameObject.name + " on begin drag" );
        startParent.SetAsLastSibling();
    }

    public void OnDrag( PointerEventData eventData )
    {
        transform.position = Input.mousePosition;
        Debug.Log( gameObject.name + " on drag" );
    }

    public void OnEndDrag( PointerEventData eventData )
    {
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if(transform.parent == startParent)
        {
            transform.position = startPosition;
        }
        Debug.Log( gameObject.name + " on end drag" );

    }

}
