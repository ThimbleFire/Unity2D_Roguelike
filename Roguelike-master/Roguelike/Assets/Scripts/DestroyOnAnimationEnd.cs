using UnityEngine;

namespace AlwaysEast
{
    public class DestroyOnAnimationEnd : MonoBehaviour
    {
        public void DestroyParent()
        {
            GameObject parent = gameObject.transform.parent.gameObject;

            Destroy(parent);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}