using System.Collections;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] footstepClips;
    public CharacterController characterController;

    public float stepInterval = 0.3f; // Interval between footsteps in seconds
    private float stepTimer;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        stepTimer = stepInterval;

        // Test sound on Start
        PlayFootstep();
    }


    void Update()
    {
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.01f)
        {
            Debug.Log("Player is moving and grounded");
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0)
            {
                PlayFootstep();
                stepTimer = stepInterval; // Reset timer
            }
        }
        else
        {
            Debug.Log("Player is not moving or not grounded");
            stepTimer = stepInterval; // Reset timer if the player stops moving
        }
    }


    void PlayFootstep()
    {
        if (footstepClips.Length > 0)
        {
            int randomIndex = Random.Range(0, footstepClips.Length);
            audioSource.PlayOneShot(footstepClips[randomIndex]);
        }
    }
}
