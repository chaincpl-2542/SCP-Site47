using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForceSCPChasePlayer : MonoBehaviour
{
    public SCPController scp;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            scp.forcePlayer = true;
        }
    }
}
