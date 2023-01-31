using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class TextLog : MonoBehaviour
    {
        private static Text[] textElement;
        private void Awake() => textElement = GetComponentsInChildren<Text>();
        private static List<string> messages = new List<string>();

        public static void Print(string message)
        {
            if ( messages.Count < textElement.Length )
                messages.Add( message );
            else {
                messages.RemoveAt( 0 );
                messages.Add( message );
            }

            Refresh();
        }

        private static void Refresh() {

            for ( int i = 0; i < messages.Count; i++ )
            {
                textElement[i].text = messages[i];
            }
        }
}