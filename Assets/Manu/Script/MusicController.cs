using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip ambianceMusic;    
    [SerializeField] AudioClip eventMusic; 

    public void PlayAmbianceMusic()
    {
        PlayMusic(ambianceMusic);
    }

    public void PlayEventMusic()
    {
        PlayMusic(eventMusic);
    }
    public void StopMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
    void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
