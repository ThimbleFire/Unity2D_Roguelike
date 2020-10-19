using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform startParent;
    private Vector3 startPosition;
    public static GameObject itemBeingDragged;

    private void Start()
    {
        //childIndex = transform.GetSiblingIndex();

        //string savedData = PlayerPrefs.GetString( childIndex.ToString() );

        //item = new Item( savedData != string.Empty ? savedData : Binary.EmptyItem );
        //item.Build();

        //image.sprite = item.Sprite;
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