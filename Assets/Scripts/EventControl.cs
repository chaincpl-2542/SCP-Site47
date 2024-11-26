using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EventControl : MonoBehaviour
{
    public static EventControl Instance;
    public bool isGeneratorRoom;
    public bool isDataRoom;
    public bool isControlRoom;
    public Transform scp;

    [Header("Event0")]
    public bool gotTablet;
    public AutoDoor startRoomDoor;

    [Header("Event1")]
    public Animator door01Anim;

    [Header("Event2")]
    public GameObject showDoor;
    public GameObject hideDoor;
    public GameObject SCP_EventTrigger;

    [Header("Event3")]
    public GameObject showDoor2;
    public GameObject hideDoor2;
    public GameObject exitWay;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ActiveEvent1()
    {
        if(isGeneratorRoom)
        {
            door01Anim.CrossFade("DoorOpen",0);
        }
    }

    public void ActiveEvent2()
    {
        showDoor.SetActive(true);
        hideDoor.SetActive(false);
        SCP_EventTrigger.SetActive(true);
    }

    public void ActiveEvent3()
    {
        showDoor2.SetActive(true);
        hideDoor2.SetActive(false);
        exitWay.SetActive(true);
    }

    public void SpawnSCP(Transform spawnPosition,bool isForcePlayer)
    {
        StartCoroutine(scp.GetComponent<SCPController>().BlinkRandomly());
        scp.transform.position = spawnPosition.position;
        scp.transform.rotation = spawnPosition.rotation;

        if(isForcePlayer)
        {
            ScpForcePlayer();
        }
        scp.gameObject.SetActive(true);
    }

    public void HideSCP()
    {
        scp.gameObject.SetActive(false);
    }

    public void ScpForcePlayer()
    {
        scp.GetComponent<SCPController>().forcePlayer = true;
    }

    void Update()
    {
        if(gotTablet)
        {
            startRoomDoor.DoorStatus(true);
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            HideSCP();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            SpawnSCP(scp.transform,false);
        }
    }
}
