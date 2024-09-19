using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPoint
{
    public GameObject spawnLocation;
    [Range(0, 100)] public float probability;
}

public class SCPTeleport : MonoBehaviour
{
    public List<SpawnPoint> spawnPoints;
    [SerializeField] private float teleportInterval = 5f;
    [SerializeField] private float timer;
    [SerializeField] private bool readyToTeleport;
    private GameObject selectedSpawnPoint;

    private void Start()
    {
        CCTVManager.Instance.changeCamera += TeleportTo;
        RandomRoom();
    }

    private void RandomRoom()
    {
        selectedSpawnPoint = GetRandomSpawnPoint();

        GameObject GetRandomSpawnPoint()
        {
            float totalProbability = 0f;

            foreach (SpawnPoint spawn in spawnPoints)
            {
                totalProbability += spawn.probability;
            }


            float randomValue = Random.Range(0, totalProbability);


            float cumulativeProbability = 0f;
            foreach (SpawnPoint spawn in spawnPoints)
            {
                cumulativeProbability += spawn.probability;
                if (randomValue <= cumulativeProbability)
                {
                    return spawn.spawnLocation;
                }
            }

            return null;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > teleportInterval)
        {
            timer = teleportInterval;
            readyToTeleport = true;
        }
        else
        {
            readyToTeleport = false;
        }

    }

    void TeleportTo()
    {
        if(readyToTeleport)
        {
            transform.position = selectedSpawnPoint.transform.position;
            Debug.Log("SCP teleported to: " + selectedSpawnPoint.name);
            RandomRoom();
            timer = 0;
        }
    }
}
