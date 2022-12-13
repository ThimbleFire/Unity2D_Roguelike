using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public static class AudioDevice
{
    private static AudioSource audioSource;
    private static AudioClip[] generics;

    public enum Sound
    {
        Button,
        Pickup,
        WindowOpen
    }

    public static void Setup(AudioSource source, AudioClip[] _generics)
    {
        generics = _generics;
        audioSource = source;
    }

    public static void Play( AudioClip clip )
    {
        if ( clip != null )
        {
            audioSource.PlayOneShot( clip );
        }
    }

    public static void PlayGeneric(Sound sound)
    {
        switch ( sound )
        {
            case Sound.Button: 
                Play( generics[0] );
                break;
            case Sound.Pickup:
                Play( generics[1] );
                break;
            case Sound.WindowOpen:
                Play( generics[2] );
                break;
        }
    }
}
