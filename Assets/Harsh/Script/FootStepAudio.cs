using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip walkSFX;
    [SerializeField] AudioClip runSFX;

    public void PlayWalkSound()
    {
        PlaySound(walkSFX);
    }

    public void PlayRunSound()
    {
        PlaySound(runSFX);
    }

    public void StopSound()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource.clip == clip && audioSource.isPlaying)
            return; 

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
