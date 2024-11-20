using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScpTrigger : MonoBehaviour
{
    public EventControl eventControl;
    public Transform scp_position;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            eventControl.SpawnSCP(scp_position, true);
        }
    }
}
