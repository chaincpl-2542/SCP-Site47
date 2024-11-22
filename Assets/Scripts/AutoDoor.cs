using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    Animator anim;
    public bool canOpen = true;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (canOpen)
        {
            if(other.gameObject.name == "Player")
            {
                anim.CrossFade("DoorOpen",0);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (other.gameObject.name == "Player")
        {
            anim.CrossFade("DoorClose",0);
        }
    }
}
