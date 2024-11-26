using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    public Animator doorAnim;
    public List<Animator> lightAnims;
    public AudioSource audioSource;
    public bool canOpen = true;
    [SerializeField] private bool isPlayer;

    // Start is called before the first frame update
    void Start()
    {
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
        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (canOpen)
        {
            if(other.gameObject.name == "Player")
            {
                doorAnim.CrossFade("DoorOpen",0);
                isPlayer = true;
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
            if(isPlayer)
            {
                doorAnim.CrossFade("DoorClose",0);
                isPlayer = false;
            }
        }
    }
}
