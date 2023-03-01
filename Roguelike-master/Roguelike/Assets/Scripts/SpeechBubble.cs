using UnityEngine;

namespace AlwaysEast
{
    public class SpeechBubble : MonoBehaviour
    {

        public enum Type
        {
            Attention, Heart, Questionmark, Talking
        }
        private static Transform Transform;
        private static Animator Animator;

        private static Vector3 offset = new Vector3(0.25f, 1.0f, 0.0f);

        private void Awake()
        {
            Transform = GetComponent<Transform>();
            Animator = GetComponent<Animator>();
        }

        public static void Show(Transform parent, Type bubbleType)
        {

            Transform.SetParent(parent);
            Transform.localPosition = offset;

            switch (bubbleType)
            {
                case Type.Attention:
                    Animator.SetTrigger("Attention");
                    break;
                case Type.Heart:
                    Animator.SetTrigger("Heart");
                    break;
                case Type.Questionmark:
                    Animator.SetTrigger("Questionmark");
                    break;
                case Type.Talking:
                    Animator.SetTrigger("Talking");
                    break;
            }
        }

        public void End() => transform.position = Vector3.zero;
    }
}