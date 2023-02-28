using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDeselect : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        ExecuteEvents.ExecuteHierarchy<IItemClickOffHandler>(gameObject, null, (x, y) => x.OnItemClickOff());
    }
}