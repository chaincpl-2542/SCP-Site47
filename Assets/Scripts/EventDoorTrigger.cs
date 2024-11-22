using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDoorTrigger : MonoBehaviour
{
    public EventControl eventControl;
    public Animator doorAnim;
    void OnTriggerEnter(Collider other)
    {
        if(eventControl.isGeneratorRoom && eventControl.isControlRoom)
        {
            if(other.gameObject.name == "Player")
            {
                doorAnim.CrossFade("DoorStuck",0);
                eventControl.ActiveEvent3();
            }
        }
    }
}
