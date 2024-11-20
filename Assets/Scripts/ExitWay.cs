using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitWay : MonoBehaviour
{
    public GameManager gameManager;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            gameManager.EndGame();
        }
    }
}
