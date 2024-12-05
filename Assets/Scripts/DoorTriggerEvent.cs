using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerEvent : MonoBehaviour
{
    public enum TriggerEventType
    {
        None,
        Open,
        Malfunction
    }

    public AutoDoor doorTarget;
    public TriggerEventType eventType;
    
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            if(eventType == TriggerEventType.Open)
            {
                doorTarget.DoorForceOpen();
                gameObject.SetActive(false);
            }
            if(eventType == TriggerEventType.Malfunction)
            {
                doorTarget.DoorMalfunction();
                gameObject.SetActive(false);
            }
        }
    }
     
}
