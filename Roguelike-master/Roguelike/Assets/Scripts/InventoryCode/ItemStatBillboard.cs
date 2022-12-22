using UnityEngine;

public class ItemStatBillboard : MonoBehaviour
{
    private static UnityEngine.UI.Image image;
    private static UnityEngine.UI.Text textBody;
    private static UnityEngine.RectTransform rTransform;

    public static void Draw( ItemStats item )
    {
        if ( Inventory.IsItemSelected == false )
            return;

        if ( item == null )
        {
            Hide();
            return;
        }

        image.enabled = true;
        textBody.enabled = true;

        textBody.text = item.Tooltip;
    }

    public static void Hide()
    {
        image.enabled = false;
        textBody.enabled = false;
    }

    private void Awake()
    {
        image = GetComponent<UnityEngine.UI.Image>();
        textBody = transform.GetComponentInChildren<UnityEngine.UI.Text>();
        rTransform = GetComponent<RectTransform>();
    }
}