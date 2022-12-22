using UnityEngine;

public class Setup : MonoBehaviour
{
    public AudioClip[] generics;

    private void Awake()
    {
        AudioDevice.Setup( GetComponent<AudioSource>(), generics );
    }
}