using UnityEngine;

namespace AlwaysEast
{
    public class PlayerHealthBar : MonoBehaviour
    {
        private static RectTransform value;
        private static RectTransform limiter;

        private static int MaximumLife { get; set; }
        private static int CurrentLife { get; set; }

        private void Awake()
        {
            value = transform.Find("Value").GetComponent<RectTransform>();
            limiter = value.Find("Limiter").GetComponent<RectTransform>();
        }

        public static void SetMaximumLife(int m)
        {
            MaximumLife = m;

            limiter.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MaximumLife * 1);
        }

        public static void SetCurrentLife(int c)
        {
            CurrentLife = c;

            value.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CurrentLife * 1);
        }
    }
}