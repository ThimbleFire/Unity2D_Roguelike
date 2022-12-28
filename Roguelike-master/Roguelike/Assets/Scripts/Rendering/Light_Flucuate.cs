using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent( typeof( Light2D ) )]
public class Light_Flucuate : MonoBehaviour {
    private Light2D _source;

    private void Awake() => _source = GetComponent<Light2D>();

    // This works but it isn't practical
    // Lights should only fluctuate when on screen
    //void Update()
    //{
    //    _source.intensity += Random.Range( -0.02f, 0.02f );
    //    _source.intensity = Mathf.Clamp( _source.intensity, 0.9f, 1.1f );
    //}
}