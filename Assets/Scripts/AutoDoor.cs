using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    public Animator doorAnim;
    public List<Animator> lightAnims;
    public AudioSource audioSource;
    public bool canOpen = true;
    public bool isMalfunction;
    public bool isAuto = true;
    public AudioClip openClip;
    public AudioClip closeClip;
    [SerializeField] private bool isPlayer;

    // Start is called before the first frame update
    void Start()
    {
        isAuto = true;
        doorAnim = gameObject.GetComponent<Animator>();
        if(lightAnims.Count > 0)
        {
            foreach(Animator lightAnim in lightAnims)
            {
                lightAnim.SetBool("Active",false);
            }
        }
    }

    public void DoorStatus(bool isUnlock)
    {
        if(isUnlock)
        {
            canOpen = true;
            if(lightAnims.Count > 0)
            {
                foreach(Animator lightAnim in lightAnims)
                {
                    lightAnim.SetBool("Active",true);
                }
            }
        }
        else
        {
            canOpen = false;
            if(lightAnims.Count > 0)
            {
                foreach(Animator lightAnim in lightAnims)
                {
                    lightAnim.SetBool("Active",false);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(isAuto)
        {
            if (canOpen)
            {
                if(other.gameObject.name == "Player")
                {
                    if (audioSource != null)
                    {
                        audioSource.PlayOneShot(openClip);
                    }
                    doorAnim.CrossFade("DoorOpen",0);
                    isPlayer = true;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {  
        if(isAuto)
        {
            if (other.gameObject.name == "Player")
            {
                if(isPlayer)
                {
                    if (audioSource != null)
                    {
                        audioSource.PlayOneShot(closeClip);
                    }
                    doorAnim.CrossFade("DoorClose",0);
                    isPlayer = false;
                }
            }
        }
    }

    public void DoorMalfunction()
    {
        DoorStatus(false);
        doorAnim.CrossFade("DoorMulfunction",0);
        if (audioSource != null)
        {
            audioSource.PlayOneShot(closeClip);
        }
        isPlayer = false;
    }

    public void DoorForceOpen()
    {
        DoorStatus(true);
        isAuto = true;
        doorAnim.CrossFade("DoorOpen",0);
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
