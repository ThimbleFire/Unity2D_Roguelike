using UnityEngine;

[RequireComponent( typeof( AudioSource ) )]
public static class AudioDevice
{
    private static AudioSource s_audioSource;
    private static AudioClip[] s_generics;

    public enum Sound
    {
        Button,
        Pickup,
        WindowOpen
    }

    public static void Setup( AudioSource source, AudioClip[] _generics )
    {
        s_generics = _generics;
        s_audioSource = source;
    }

    public static void Play( AudioClip clip )
    {
        if ( clip != null )
        {
            s_audioSource.PlayOneShot( clip );
        }
    }

    public static void PlayGeneric( Sound sound )
    {
        switch ( sound )
        {
            case Sound.Button:
                Play( s_generics[0] );
                break;

            case Sound.Pickup:
                Play( s_generics[1] );
                break;

            case Sound.WindowOpen:
                Play( s_generics[2] );
                break;
        }
    }
}