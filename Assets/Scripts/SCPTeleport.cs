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
    public float teleportInterval = 5f;

    private void Start()
    {
        StartCoroutine(TeleportRoutine());


        IEnumerator TeleportRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(teleportInterval);

                GameObject selectedSpawnPoint = GetRandomSpawnPoint();
                if (selectedSpawnPoint != null)
                {
                    TeleportTo(selectedSpawnPoint);
                }
            }
        }


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


        void TeleportTo(GameObject spawnPoint)
        {
            transform.position = spawnPoint.transform.position;
            Debug.Log("SCP teleported to: " + spawnPoint.name);
        }
    }
}
