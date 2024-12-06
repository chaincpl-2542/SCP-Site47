using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCPDesk : MonoBehaviour, IInteractable
{
    public Animator animGate;
    public AudioSource gateSound;
    public AudioClip sound;
    public GameObject scpWandering;
    public void OnInteract()
    {
        EventControl.Instance.ActiveEvent(5);
        animGate.SetBool("Open",true);
        GetComponent<AudioSource>().PlayOneShot(sound);
        gateSound.PlayOneShot(gateSound.clip);
        GetComponent<Animator>().SetBool("Open",true);

        gameObject.layer = 0;
        GetComponent<BoxCollider>().enabled = false;
        scpWandering .SetActive(true);
    }
}
