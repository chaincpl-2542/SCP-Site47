using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSoundControl : MonoBehaviour
{
    public AudioSource detectionSound; // Sound when SCP detects the player
    public AudioSource chaseMusic; // Background music during a chase
    public float fadeSpeed = 1f; // Speed of fading in/out the chase music

    private bool isChasing = false;

    public void PlayDetectionSound()
    {
        if (detectionSound && !detectionSound.isPlaying)
        {
            detectionSound.Play();
        }
    }

    public void StartChaseMusic()
    {
        isChasing = true;
        if (chaseMusic && !chaseMusic.isPlaying)
        {
            chaseMusic.Play();
        }
    }

    public void StopChaseMusic()
    {
        isChasing = false;
    }

    private void Update()
    {
        if (chaseMusic)
        {
            if (isChasing)
            {
                if (chaseMusic.volume < 1f)
                {
                    chaseMusic.volume = Mathf.MoveTowards(chaseMusic.volume, 1f, fadeSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (chaseMusic.volume > 0f)
                {
                    chaseMusic.volume = Mathf.MoveTowards(chaseMusic.volume, 0f, fadeSpeed * Time.deltaTime);
                }
                else if (chaseMusic.isPlaying)
                {
                    chaseMusic.Stop(); // Stop playing only when fully faded out
                }
            }
        }
    }
}
