using UnityEngine;

namespace AlwaysEast
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioDevice : MonoBehaviour
    {

        private static AudioSource s_audioSource;

        private void Awake() => s_audioSource = GetComponent<AudioSource>();

        public static void Play(AudioClip clip)
        {
            if (clip != null)
            {
                s_audioSource.PlayOneShot(clip);
            }
        }
    }
}