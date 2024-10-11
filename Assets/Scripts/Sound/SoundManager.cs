using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // Singleton pattern

    public AudioSource floatingSound;
    public AudioSource glitchSound;
    public AudioSource jumpscareSound;

    void Awake()
    {
        // Singleton instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play floating sound (looping)
    public void PlayFloatingSound()
    {
        if (!floatingSound.isPlaying)
        {
            floatingSound.loop = true;
            floatingSound.Play();
        }
    }

    // Stop floating sound
    public void StopFloatingSound()
    {
        if (floatingSound.isPlaying)
        {
            floatingSound.Stop();
        }
    }

    // Play glitch sound (one-shot)
    public void PlayGlitchSound()
    {
        glitchSound.PlayOneShot(glitchSound.clip);
    }

    // Play jumpscare sound (one-shot)
    public void PlayJumpscareSound()
    {
        jumpscareSound.PlayOneShot(jumpscareSound.clip);
    }
}
