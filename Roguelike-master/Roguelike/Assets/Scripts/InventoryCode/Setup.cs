using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour
{
    public AudioClip[] generics;

    private void Awake()
    {
        AudioDevice.Setup( GetComponent<AudioSource>(), generics );
    }
}
