using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDraggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform rectTransform;

    //we may want to remove OnPointerDown
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //OnDrag(eventData);
        Debug.Log( name + " OnPointerDown" );
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        //if (Camera.main.renderMode == RenderMode.ScreenSpaceCamera)
        //    cam = canvas.worldCamera;

        Vector2 position = RectTransformUtility.WorldToScreenPoint(Camera.main, rectTransform.position);
        /*Vector2 radius = background.sizeDelta / 2;
        input = (eventData.position - position) / (radius * canvas.scaleFactor);
        FormatInput();
        HandleInput(input.magnitude, input.normalized, radius, cam);
        handle.anchoredPosition = input * radius * handleRange;*/
        Debug.Log( name + " OnDrag" );
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        //input = Vector2.zero;
        //handle.anchoredPosition = Vector2.zero;
        Debug.Log( name + " OnPointerUp" );
    }
}
