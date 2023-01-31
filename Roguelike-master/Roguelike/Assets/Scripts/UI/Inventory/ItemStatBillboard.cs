using UnityEngine;

public class ItemStatBillboard : MonoBehaviour {
    private static UnityEngine.UI.Image s_image;
    private static UnityEngine.UI.Text s_textBody;
    private static UnityEngine.RectTransform s_rTransform;

    public static void Draw( ItemStats item ) {
        if ( Inventory.IsItemSelected == false )
            return;

        if ( item == null ) {
            Hide();
            return;
        }

        s_image.enabled = true;
        s_textBody.enabled = true;

        s_textBody.text = item.Tooltip;
    }

    public static void Hide() {
        s_image.enabled = false;
        s_textBody.enabled = false;
    }

    private void Awake() {
        s_image = GetComponent<UnityEngine.UI.Image>();
        s_textBody = transform.GetComponentInChildren<UnityEngine.UI.Text>();
        s_rTransform = GetComponent<RectTransform>();
    }
}