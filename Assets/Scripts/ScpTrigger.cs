using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScpTrigger : MonoBehaviour
{
    public EventControl eventControl;
    public Transform scp_position;
    public bool isHideScp;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            if(!isHideScp)
            {
                eventControl.SpawnSCP(scp_position, true);
                gameObject.SetActive(false);
            }
            else
            {
                eventControl.HideSCP();
                gameObject.SetActive(false);
            }
        }
    }


}
