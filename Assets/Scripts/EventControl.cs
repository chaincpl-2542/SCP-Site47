using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EventDoor
{
    public List<AutoDoor> activeDoorsEvent;
    public List<AutoDoor> deactivateDoorsEvent;
}

public class EventControl : MonoBehaviour
{
    public static EventControl Instance;
    public bool isGeneratorRoom;
    public bool isDataRoom;
    public bool isControlRoom;
    public List<WanderingZone> wanderingZones;
    public Transform scp;

    [Header("Event0")]
    public bool gotTablet;

    [Header("EventMain")]
     [SerializeField] private List<EventDoor> eventDoors;

    public GameObject SCP_EventTrigger;
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

    public void ActiveEvent(int eventIndex)
    {
        var index = eventIndex;

        if(eventDoors[index].activeDoorsEvent.Count > 0)
        {
            foreach(AutoDoor door in eventDoors[index].activeDoorsEvent)
            {
                door.DoorStatus(true);
            }
        }

        if( eventDoors[index].deactivateDoorsEvent.Count > 0)
        {
            foreach(AutoDoor door in eventDoors[index].deactivateDoorsEvent)
            {
                door.DoorStatus(false);
            }
        }

        if(eventIndex >= 5)
        {
            foreach(WanderingZone zone in wanderingZones)
            {
                zone.gameObject.SetActive(true);
            }
        }
        
    }

    public void ActiveScpTrigger()
    {
        SCP_EventTrigger.SetActive(true);
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

    public void PickupTablet()
    {
        if (!gotTablet)
        {
            ActiveEvent(0);
            gotTablet = true;
        }
    }
    
    void Update()
    {
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
