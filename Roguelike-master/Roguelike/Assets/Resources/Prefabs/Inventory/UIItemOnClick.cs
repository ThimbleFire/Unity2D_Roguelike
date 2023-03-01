using EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AlwaysEast
{
    public class UIItemOnClick : MonoBehaviour, IPointerClickHandler
    {

        public void OnPointerClick(PointerEventData eventData)
        {
            ExecuteEvents.ExecuteHierarchy<IItemClickHandler>(gameObject, null, (x, y) => x.OnItemClick(this));
        }
    }
}