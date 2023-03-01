using UnityEngine;
using UnityEngine.EventSystems;

namespace AlwaysEast
{
    public class ItemDeselect : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            ExecuteEvents.ExecuteHierarchy<EventSystems.IItemClickOffHandler>(gameObject, null, (x, y) => x.OnItemClickOff());
        }
    }
}